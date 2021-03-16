﻿#region Copyright © 2019-2021 Sergii Artemenko

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

#endregion

using System;
using Xtate.Core;

namespace Xtate.Persistence
{
	internal sealed class StateMachineHostClusterContext : StateMachineHostContext
	{
		private readonly StateMachineHost _stateMachineHost;

		public StateMachineHostClusterContext(StateMachineHost stateMachineHost, StateMachineHostOptions options) : base(stateMachineHost, options, new PersistedEventSchedulerFactory(options)) =>
				_stateMachineHost = stateMachineHost;

		protected override StateMachineControllerBase CreateStateMachineController(SessionId sessionId, IStateMachine? stateMachine, IStateMachineOptions? stateMachineOptions,
																				   Uri? stateMachineLocation, InterpreterOptions defaultOptions, SecurityContext securityContext,
																				   DeferredFinalizer finalizer) =>
				new StateMachineSingleMacroStepController(sessionId, stateMachineOptions, stateMachine, stateMachineLocation, _stateMachineHost, defaultOptions, securityContext, finalizer);
	}
}