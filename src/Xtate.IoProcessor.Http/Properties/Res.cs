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

using System.Diagnostics.CodeAnalysis;
using Xtate.Annotations;

namespace Xtate
{
	[PublicAPI]
	[ExcludeFromCodeCoverage]
	internal static class Res
	{
		public static string Format(string format, object arg)                            => string.Format(Resources.Culture, format, arg);
		public static string Format(string format, object arg0, object arg1)              => string.Format(Resources.Culture, format, arg0, arg1);
		public static string Format(string format, object arg0, object arg1, object arg2) => string.Format(Resources.Culture, format, arg0, arg1, arg2);
		public static string Format(string format, params object[] args)                  => string.Format(Resources.Culture, format, args);
	}
}