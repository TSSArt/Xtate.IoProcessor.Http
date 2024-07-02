﻿#region Copyright © 2019-2023 Sergii Artemenko

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

using System.Xml;
using Xtate.Builder;
using Xtate.IoC;
using Xtate.XInclude;

namespace Xtate.Scxml;

public static class ScxmlExtensions
{
	public static void RegisterScxml(this IServiceCollection services)
	{
		if (services.IsRegistered<IScxmlSerializer>())
		{
			return;
		}

		services.RegisterErrorProcessor();
		services.RegisterStateMachineBuilder();
		services.RegisterResourceLoaders();
		services.RegisterNameTable();

		services.AddType<ScxmlSerializerWriter, XmlWriter>();
		services.AddImplementation<ScxmlSerializer>().For<IScxmlSerializer>();
		services.AddType<ScxmlDirector, XmlReader>();
		services.AddTypeSync<XmlBaseReader, XmlReader>();
		services.AddTypeSync<XIncludeReader, XmlReader>();
		services.AddImplementationSync<RedirectXmlResolver>().For<ScxmlXmlResolver>().For<XmlResolver>().For<IXIncludeXmlResolver>();
		services.AddImplementation<ScxmlDeserializer>().For<IScxmlDeserializer>();
	}
}