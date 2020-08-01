﻿#region Copyright © 2019-2020 Sergii Artemenko
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
// 
#endregion

using System;
using System.Collections.Immutable;

namespace Xtate.Builder
{
	public class ForEachBuilder : BuilderBase, IForEachBuilder
	{
		private ImmutableArray<IExecutableEntity>.Builder? _actions;
		private IValueExpression?                          _array;
		private ILocationExpression?                       _index;
		private ILocationExpression?                       _item;

		public ForEachBuilder(IErrorProcessor errorProcessor, object? ancestor) : base(errorProcessor, ancestor) { }

	#region Interface IForEachBuilder

		public IForEach Build() => new ForEachEntity { Ancestor = Ancestor, Array = _array, Item = _item, Index = _index, Action = _actions?.ToImmutable() ?? default };

		public void SetArray(IValueExpression array) => _array = array ?? throw new ArgumentNullException(nameof(array));

		public void SetItem(ILocationExpression item)
		{
			_item = item ?? throw new ArgumentNullException(nameof(item));
		}

		public void SetIndex(ILocationExpression index)
		{
			_index = index ?? throw new ArgumentNullException(nameof(index));
		}

		public void AddAction(IExecutableEntity action)
		{
			if (action == null) throw new ArgumentNullException(nameof(action));

			(_actions ??= ImmutableArray.CreateBuilder<IExecutableEntity>()).Add(action);
		}

	#endregion
	}
}