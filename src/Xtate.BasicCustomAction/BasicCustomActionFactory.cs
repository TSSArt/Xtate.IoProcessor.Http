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

namespace Xtate.CustomAction
{
	[CustomActionProvider("http://xtate.net/scxml/customaction/basic")]
	public class BasicCustomActionFactory : CustomActionFactoryBase
	{
		public static readonly ICustomActionFactory Instance = new BasicCustomActionFactory();

		private BasicCustomActionFactory()
		{
			Register(name: "base64decode", (xmlReader, context) => new Base64DecodeAction(xmlReader, context));
			Register(name: "parseUrl", (xmlReader, context) => new ParseUrlAction(xmlReader, context));
			Register(name: "format", (xmlReader, context) => new FormatAction(xmlReader, context));
			Register(name: "operation", (xmlReader, context) => new OperationAction(xmlReader, context));
		}
	}
}