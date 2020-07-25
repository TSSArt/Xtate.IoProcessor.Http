using System;
using Xtate.Service;

namespace Xtate
{
	public static class UserInteractionExtensions
	{
		public static StateMachineHostBuilder AddUserInteraction(this StateMachineHostBuilder builder)
		{
			if (builder == null) throw new ArgumentNullException(nameof(builder));

			builder.AddServiceFactory(InputService.Factory);

			return builder;
		}
	}
}