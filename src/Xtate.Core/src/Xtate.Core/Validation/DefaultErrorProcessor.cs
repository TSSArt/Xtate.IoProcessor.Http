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

namespace Xtate
{
	public sealed class DefaultErrorProcessor : IErrorProcessor
	{
		public static readonly IErrorProcessor Instance = new DefaultErrorProcessor();

		private DefaultErrorProcessor() { }

	#region Interface IErrorProcessor

		public void ThrowIfErrors() { }

		void IErrorProcessor.AddError(ErrorItem errorItem)
		{
			if (errorItem == null) throw new ArgumentNullException(nameof(errorItem));

			throw new StateMachineValidationException(ImmutableArray.Create(errorItem));
		}

		bool IErrorProcessor.LineInfoRequired => false;

	#endregion
	}
}