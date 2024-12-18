﻿// Copyright © 2019-2024 Sergii Artemenko
// 
// This file is part of the Xtate project. <https://xtate.net/>
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as published
// by the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
// 
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using Xtate.Core;
using Exception = System.Exception;

namespace Xtate.IoProcessor;

public sealed class HttpIoProcessor : IoProcessorBase, IAsyncDisposable
{
	private static readonly FullUri Id = new(@"http://www.w3.org/TR/scxml/#BasicHTTPEventProcessor");

	private static readonly FullUri Alias = new(@"http");

	private const string ErrorSuffix = @"HttpIoProcessor";

	private const string MediaTypeTextPlain = @"text/plain";

	private const string MediaTypeApplicationJson = @"application/json";

	private const string MediaTypeApplicationFormUrlEncoded = @"application/x-www-form-urlencoded";

	private const string EventNameParameterName = @"_scxmleventname";

	private static readonly ConcurrentDictionary<IPEndPoint, Host> Hosts = new();

	private static readonly ImmutableArray<IPAddress> Interfaces = GetInterfaces();

	private readonly Uri _baseUri;

	private readonly PathString _path;

	private IPEndPoint _ipEndPoint;

	public HttpIoProcessor(IEventConsumer eventConsumer, Uri baseUri, IPEndPoint ipEndPoint) : base(Id, Alias)
	{
		_baseUri = baseUri ?? throw new ArgumentNullException(nameof(baseUri));
		_path = PathString.FromUriComponent(baseUri);
		_ipEndPoint = ipEndPoint;
	}

#region Interface IAsyncDisposable

	public async ValueTask DisposeAsync()
	{
		if (Hosts.TryGetValue(_ipEndPoint, out var host))
		{
			await host.RemoveProcessor(this, token: default).ConfigureAwait(false);
		}
	}

#endregion

	private static ImmutableArray<IPAddress> GetInterfaces()
	{
		var result = ImmutableArray.CreateBuilder<IPAddress>();

		foreach (var networkInterface in NetworkInterface.GetAllNetworkInterfaces())
		{
			var ipInterfaceProperties = networkInterface.GetIPProperties();

			foreach (var ipInterfaceProperty in ipInterfaceProperties.UnicastAddresses)
			{
				result.Add(ipInterfaceProperty.Address);
			}
		}

		return result.ToImmutable();
	}

	public async ValueTask Start(CancellationToken token)
	{
		if (_ipEndPoint.Address.Equals(IPAddress.None) && _ipEndPoint.Port == 0)
		{
			_ipEndPoint = await FromUri(_baseUri).ConfigureAwait(false);
		}

		var host = Hosts.GetOrAdd(_ipEndPoint, Host.Create);

		await host.AddProcessor(this, token).ConfigureAwait(false);
	}

	private static async ValueTask<IPEndPoint> FromUri(Uri uri)
	{
		if (uri.IsLoopback)
		{
			return new IPEndPoint(IPAddress.Loopback, uri.Port);
		}

		var hostEntry = await Dns.GetHostEntryAsync(uri.DnsSafeHost).ConfigureAwait(false);

		IPAddress? listenAddress = null;

		foreach (var address in hostEntry.AddressList)
		{
			if (Interfaces.IndexOf(address) >= 0)
			{
				if (listenAddress is not null)
				{
					throw new ProcessorException(Resources.Exception_FoundMoreThenOneInterfaceToListen);
				}

				listenAddress = address;
			}
		}

		if (listenAddress is null)
		{
			throw new ProcessorException(Resources.Exception_CantMatchNetworkInterfaceToListen);
		}

		return new IPEndPoint(listenAddress, uri.Port);
	}

	protected override FullUri GetTarget(ServiceId serviceId)
	{
		if (serviceId is null) throw new ArgumentNullException(nameof(serviceId));

		return new FullUri(_baseUri, serviceId.Value);
	}

	protected override ValueTask OutgoingEvent(IRouterEvent routerEvent, CancellationToken token) => throw new NotImplementedException(); //TODO:

	protected async ValueTask OutgoingEvent(ServiceId serviceId, IOutgoingEvent evt, CancellationToken token)
	{
		if (evt is null) throw new ArgumentNullException(nameof(evt));

		using var client = new HttpClient();

		if (evt.Target is null)
		{
			throw new ArgumentException(Resources.Exception_TargetIsNotDefined, nameof(evt));
		}

		var targetUri = evt.Target.ToString();

		var content = GetContent(evt, out var eventNameInContent);

		if (!evt.Name.IsDefault && !eventNameInContent)
		{
			targetUri = QueryHelpers.AddQueryString(targetUri, EventNameParameterName, evt.Name.ToString());
		}

		using var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, targetUri)
									   {
										   Content = content,
										   Headers = { { @"Origin", GetTarget(serviceId).ToString() } }
									   };

		var httpResponseMessage = await client.SendAsync(httpRequestMessage, token).ConfigureAwait(false);
		httpResponseMessage.EnsureSuccessStatusCode();
	}

	private static HttpContent? GetContent(IOutgoingEvent evt, out bool eventNameInContent)
	{
		var data = evt.Data;
		var dataType = data.Type;

		switch (dataType)
		{
			case DataModelValueType.Undefined:
			case DataModelValueType.Null:
				eventNameInContent = !evt.Name.IsDefault;

				return eventNameInContent ? new FormUrlEncodedContent(GetParameters(evt.Name, dataModelList: null)) : null;

			case DataModelValueType.String:
				eventNameInContent = false;

				return new StringContent(data.AsString(), Encoding.UTF8, MediaTypeTextPlain);

			case DataModelValueType.List:
			{
				var dataModelList = data.AsList();

				if (IsStringDictionary(dataModelList))
				{
					eventNameInContent = true;

					return new FormUrlEncodedContent(GetParameters(evt.Name, dataModelList));
				}

				eventNameInContent = false;

				return new StringContent(DataModelConverter.ToJson(data), Encoding.UTF8, MediaTypeApplicationJson);
			}

			default:
				throw new NotSupportedException(Resources.Exception_DataFormatNotSupported);
		}
	}

	private static bool IsStringDictionary(DataModelList dataModelList)
	{
		foreach (var (key, value) in dataModelList.KeyValues)
		{
			if (key is null)
			{
				return false;
			}

			switch (value.Type)
			{
				case DataModelValueType.List:
				case DataModelValueType.Number:
				case DataModelValueType.DateTime:
				case DataModelValueType.Boolean:
					return false;

				case DataModelValueType.Undefined:
				case DataModelValueType.Null:
				case DataModelValueType.String:
					break;

				default:
					throw new InvalidOperationException();
			}
		}

		return true;
	}

	private static IEnumerable<KeyValuePair<string?, string?>> GetParameters(EventName eventName, DataModelList? dataModelList)
	{
		if (!eventName.IsDefault)
		{
			yield return new KeyValuePair<string?, string?>(EventNameParameterName, eventName.ToString());
		}

		if (dataModelList is not null)
		{
			foreach (var (key, value) in dataModelList.KeyValues)
			{
				yield return new KeyValuePair<string?, string?>(key, Convert.ToString(value, CultureInfo.InvariantCulture));
			}
		}
	}

	private async ValueTask<bool> Handle(HttpRequest request, CancellationToken token)
	{
		SessionId sessionId;

		if (_path == @"/")
		{
			sessionId = ExtractSessionId(request.Path);
		}
		else if (request.Path.StartsWithSegments(_path, StringComparison.OrdinalIgnoreCase, out var sessionIdPathString))
		{
			sessionId = ExtractSessionId(sessionIdPathString);
		}
		else
		{
			return false;
		}

		IEventDispatcher eventDispatcher = null; // LinkedList await TryGetEventDispatcher(sessionId, token).ConfigureAwait(false);//TODO


		if (eventDispatcher is null)
		{
			return false;
		}

		IIncomingEvent? evt;

		try
		{
			evt = await CreateEvent(request, token).ConfigureAwait(false);
		}
		catch (Exception ex)
		{
			evt = CreateErrorEvent(request, ex);
		}

		await eventDispatcher.Dispatch(evt, token).ConfigureAwait(false);

		return true;
	}

	private static SessionId ExtractSessionId(PathString pathString)
	{
		var unescapedString = pathString.Value;

		if (unescapedString.Length > 0 && unescapedString[0] == '/')
		{
			return SessionId.FromString(unescapedString[1..]);
		}

		return SessionId.FromString(unescapedString);
	}

	private IIncomingEvent CreateErrorEvent(HttpRequest request, Exception exception)
	{
		var requestData = new DataModelList
						  {
							  { @"remoteIp", request.HttpContext.Connection.RemoteIpAddress.ToString() },
							  { @"method", request.Method },
							  { @"contentType", request.ContentType },
							  { @"contentLength", (int?) request.ContentLength ?? -1 },
							  { @"path", request.Path.ToString() },
							  { @"query", request.QueryString.ToString() }
						  };

		var exceptionData = new DataModelList
							{
								{ @"message", exception.Message },
								{ @"typeName", exception.GetType().Name },
								{ @"source", exception.Source },
								{ @"typeFullName", exception.GetType().FullName },
								{ @"stackTrace", exception.StackTrace },
								{ @"text", exception.ToString() }
							};

		var data = new DataModelList
				   {
					   { @"request", requestData },
					   { @"exception", exceptionData }
				   };

		exceptionData.MakeDeepConstant();

		return new IncomingEvent(EventName.GetErrorPlatform(ErrorSuffix), origin: default, Id, data);
	}

	private async ValueTask<IIncomingEvent> CreateEvent(HttpRequest request, CancellationToken token)
	{
		var contentType = request.ContentType is not null ? new ContentType(request.ContentType) : new ContentType();
		var encoding = contentType.CharSet is not null ? Encoding.GetEncoding(contentType.CharSet) : Encoding.ASCII;

		string body;

		using (var streamReader = new StreamReader(request.Body.InjectCancellationToken(token), encoding))
		{
			body = await streamReader.ReadToEndAsync().ConfigureAwait(false);
		}

		FullUri.TryCreate(request.Headers[@"Origin"].ToString(), out var origin);

		var data = CreateData(contentType.MediaType, body, out var eventNameInContent);

		var eventNameInQueryString = request.Query[EventNameParameterName];
		var eventName = !StringValues.IsNullOrEmpty(eventNameInQueryString) ? eventNameInQueryString[0] : null;

		eventName ??= eventNameInContent ?? request.Method;

		return new IncomingEvent(eventName, origin, Id, data);
	}

	private static DataModelValue CreateData(string mediaType, string body, out string? eventName)
	{
		eventName = null;

		if (mediaType == MediaTypeTextPlain)
		{
			return body;
		}

		if (mediaType == MediaTypeApplicationFormUrlEncoded)
		{
			var pairs = QueryHelpers.ParseQuery(body);
			var list = new DataModelList();

			foreach (var (key, value) in pairs)
			{
				if (key == EventNameParameterName)
				{
					eventName = value[0];
				}
				else
				{
					foreach (var stringValue in value)
					{
						list.Add(key, stringValue);
					}
				}
			}

			return list;
		}

		if (mediaType == MediaTypeApplicationJson)
		{
			return DataModelConverter.FromJson(body);
		}

		return default;
	}

	private class Host
	{
		private readonly IWebHost _webHost;

		private ImmutableList<HttpIoProcessor> _processors = ImmutableList<HttpIoProcessor>.Empty;

		private Host(IPEndPoint ipEndPoint)
		{
			_webHost = new WebHostBuilder()
					   .Configure(builder => builder.Run(HandleRequest))
					   .UseKestrel(ConfigureOptions)
					   .Build();

			void ConfigureOptions(KestrelServerOptions options)
			{
				options.AllowSynchronousIO = false;

				if (ipEndPoint.Address.Equals(IPAddress.Any) || ipEndPoint.Address.Equals(IPAddress.IPv6Any))
				{
					options.ListenAnyIP(ipEndPoint.Port);
				}
				else if (IPAddress.IsLoopback(ipEndPoint.Address))
				{
					options.ListenLocalhost(ipEndPoint.Port);
				}
				else
				{
					options.Listen(ipEndPoint);
				}
			}
		}

		public static Host Create(IPEndPoint ipEndPoint) => new(ipEndPoint);

		private async Task HandleRequest(HttpContext context)
		{
			foreach (var httpIoProcessor in _processors)
			{
				if (await httpIoProcessor.Handle(context.Request, context.RequestAborted).ConfigureAwait(false))
				{
					context.Response.StatusCode = (int) HttpStatusCode.NoContent;

					return;
				}
			}

			context.Response.StatusCode = (int) HttpStatusCode.NotFound;
		}

		public async ValueTask AddProcessor(HttpIoProcessor httpIoProcessor, CancellationToken token)
		{
			ImmutableList<HttpIoProcessor> preVal, newVal;

			do
			{
				preVal = _processors;
				newVal = preVal.Add(httpIoProcessor);
			}
			while (Interlocked.CompareExchange(ref _processors, newVal, preVal) != preVal);

			if (preVal.Count == 0)
			{
				await _webHost.StartAsync(token).ConfigureAwait(false);
			}
		}

		public async ValueTask RemoveProcessor(HttpIoProcessor httpIoProcessor, CancellationToken token)
		{
			ImmutableList<HttpIoProcessor> preVal, newVal;

			do
			{
				preVal = _processors;
				newVal = preVal.Remove(httpIoProcessor);
			}
			while (Interlocked.CompareExchange(ref _processors, newVal, preVal) != preVal);

			if (newVal.Count == 0)
			{
				await _webHost.StopAsync(token).ConfigureAwait(false);
			}
		}
	}
}