﻿#region Copyright © 2019-2020 Sergii Artemenko
// This file is part of the Xtate project. <http://xtate.net>
// Copyright © 2019-2020 Sergii Artemenko
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

using System.Collections.Immutable;

namespace Xtate
{
	public struct ForEachEntity : IForEach, IVisitorEntity<ForEachEntity, IForEach>, IAncestorProvider
	{
		internal object? Ancestor;

	#region Interface IAncestorProvider

		object? IAncestorProvider.Ancestor => Ancestor;

	#endregion

	#region Interface IForEach

		public ImmutableArray<IExecutableEntity> Action { get; set; }
		public IValueExpression?                 Array  { get; set; }
		public ILocationExpression?              Index  { get; set; }
		public ILocationExpression?              Item   { get; set; }

	#endregion

	#region Interface IVisitorEntity<ForEachEntity,IForEach>

		void IVisitorEntity<ForEachEntity, IForEach>.Init(IForEach source)
		{
			Ancestor = source;
			Action = source.Action;
			Array = source.Array;
			Index = source.Index;
			Item = source.Item;
		}

		bool IVisitorEntity<ForEachEntity, IForEach>.RefEquals(ref ForEachEntity other) =>
				Action == other.Action &&
				ReferenceEquals(Array, other.Array) &&
				ReferenceEquals(Index, other.Index) &&
				ReferenceEquals(Item, other.Item);

	#endregion
	}
}