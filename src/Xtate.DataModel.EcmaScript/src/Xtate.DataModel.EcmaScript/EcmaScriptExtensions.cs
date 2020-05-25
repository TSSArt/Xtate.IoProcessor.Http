﻿using System;

namespace Xtate.EcmaScript
{
	public static class EcmaScriptExtensions
	{
		public static StateMachineHostBuilder AddEcmaScript(this StateMachineHostBuilder builder)
		{
			if (builder == null) throw new ArgumentNullException(nameof(builder));

			builder.AddDataModelHandlerFactory(EcmaScriptDataModelHandler.Factory);

			return builder;
		}
	}
}