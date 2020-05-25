﻿using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Xtate.Services
{
	[SimpleService("http://xtate.net/scxml/service/#Input", Alias = "input")]
	public class InputService : SimpleServiceBase
	{
		public static readonly IServiceFactory Factory = SimpleServiceFactory<InputService>.Instance;

		protected override ValueTask<DataModelValue> Execute()
		{
			var task = Task.Factory.StartNew(Show, StopToken, TaskCreationOptions.LongRunning, TaskScheduler.Current);

			return new ValueTask<DataModelValue>(task);
		}

		[SuppressMessage(category: "ReSharper", checkId: "AccessToDisposedClosure", Justification = "Form closed by external event")]
		private DataModelValue Show()
		{
			var controls = Content.AsObjectOrEmpty()["controls"].AsArrayOrEmpty();

			using var form = new InputForm();

			foreach (var control in controls)
			{
				var fieldObj = control.AsObjectOrEmpty();
				var name = fieldObj["name"].AsStringOrDefault();
				var location = fieldObj["location"].AsStringOrDefault();
				var type = fieldObj["type"].AsStringOrDefault();

				form.AddInput(name, location, type);
			}

			using var registration = StopToken.Register(() => form.Close(DialogResult.Abort, result: default));

			form.Closed += (sender, args) => Application.ExitThread();

			Application.Run(form);

			var result = new DataModelObject(capacity: 2);

			if (form.DialogResult == DialogResult.OK)
			{
				result["status"] = "ok";

				if (form.Result != null)
				{
					var parameters = new DataModelObject(capacity: form.Result.Count);

					foreach (var pair in form.Result)
					{
						parameters[pair.Key] = pair.Value;
					}

					result["parameters"] = parameters;
				}
				else
				{
					result["parameters"] = DataModelObject.Empty;
				}
			}
			else
			{
				result["status"] = "cancel";
			}

			return result;
		}
	}
}