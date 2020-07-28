﻿#region Copyright © 2019-2020 Sergii Artemenko
// 
// This file is part of the Xtate project. <http://xtate.net>
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
// 
#endregion

using System.Collections.Immutable;

namespace Xtate.Builder
{
	public class TransitionBuilder : BuilderBase, ITransitionBuilder
	{
		private ImmutableArray<IExecutableEntity>.Builder? _actions;
		private IExecutableEntity?                         _condition;
		private ImmutableArray<IEventDescriptor>           _eventDescriptors;
		private ImmutableArray<IIdentifier>                _target;
		private TransitionType                             _type;

		public TransitionBuilder(IErrorProcessor errorProcessor, object? ancestor) : base(errorProcessor, ancestor) { }

	#region Interface ITransitionBuilder

		public ITransition Build() =>
				new TransitionEntity { Ancestor = Ancestor, EventDescriptors = _eventDescriptors, Condition = _condition, Target = _target, Type = _type, Action = _actions?.ToImmutable() ?? default };

		public void SetCondition(IExecutableEntity condition) => _condition = condition;

		public void SetTarget(ImmutableArray<IIdentifier> target) => _target = target;

		public void SetType(TransitionType type) => _type = type;

		public void SetEvent(ImmutableArray<IEventDescriptor> eventDescriptors) => _eventDescriptors = eventDescriptors;

		public void AddAction(IExecutableEntity action) => (_actions ??= ImmutableArray.CreateBuilder<IExecutableEntity>()).Add(action);

	#endregion
	}
}