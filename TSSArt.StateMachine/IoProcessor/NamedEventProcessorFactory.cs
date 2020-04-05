﻿using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace TSSArt.StateMachine
{
	[PublicAPI]
	public sealed class NamedEventProcessorFactory : IEventProcessorFactory
	{
		private const int FreeSlotsCount = 2;

		private static readonly string HostName = GetHostName();

		private readonly string _host;
		private readonly string _name;

		public NamedEventProcessorFactory(string name)
		{
			if (string.IsNullOrEmpty(name)) throw new ArgumentException(Resources.Exception_ValueCannotBeNullOrEmpty, nameof(name));

			_name = name;
			_host = HostName;
		}

		public NamedEventProcessorFactory(string host, string name)
		{
			if (string.IsNullOrEmpty(host)) throw new ArgumentException(Resources.Exception_ValueCannotBeNullOrEmpty, nameof(host));
			if (string.IsNullOrEmpty(name)) throw new ArgumentException(Resources.Exception_ValueCannotBeNullOrEmpty, nameof(name));

			_host = host;
			_name = name;
		}

	#region Interface IEventProcessorFactory

		public async ValueTask<IEventProcessor> Create(IEventConsumer eventConsumer, CancellationToken token)
		{
			if (eventConsumer == null) throw new ArgumentNullException(nameof(eventConsumer));

			var processor = new NamedEventProcessor(eventConsumer, _host, _name);

			for (var i = 0; i < FreeSlotsCount; i ++)
			{
				processor.StartListener().Forget();
			}

			await processor.CheckPipeline(token).ConfigureAwait(false);

			return processor;
		}

	#endregion

		private static string GetHostName()
		{
			try
			{
				return Dns.GetHostName();
			}
			catch
			{
				return ".";
			}
		}
	}
}