using System;
using TSSArt.StateMachine.Services;

namespace TSSArt.StateMachine
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