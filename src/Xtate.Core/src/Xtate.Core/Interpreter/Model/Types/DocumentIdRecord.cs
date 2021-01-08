﻿#region Copyright © 2019-2020 Sergii Artemenko

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

using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Xtate.Core
{
	internal struct DocumentIdRecord
	{
		private LinkedListNode<int>? _node;
		private int                  _value;

		public DocumentIdRecord(LinkedList<int> list)
		{
			_node = list.AddLast(-1);
			_value = -1;
		}

		public int Value
		{
			get
			{
				if (_node is { } node)
				{
					var value = node.Value;

					if (value >= 0)
					{
						_node = null;
						_value = value;
					}

					return value;
				}

				return _value;
			}
		}

		[Pure]
		public DocumentIdRecord After()
		{
			Infrastructure.NotNull(_node);

			var list = _node.List;
			Infrastructure.NotNull(list);

			return new DocumentIdRecord
				   {
						   _node = list.AddAfter(_node, value: -1),
						   _value = -1
				   };
		}
	}
}