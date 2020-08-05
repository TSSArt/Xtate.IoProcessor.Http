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

using System;
using System.Xml;
using Xtate.Annotations;

namespace Xtate
{
	[PublicAPI]
	public static class ErrorProcessorExtensions
	{
		public static void AddError<T>(this IErrorProcessor errorProcessor, object? entity, string message, Exception? exception = null) =>
				AddError(errorProcessor, typeof(T), entity, message, exception);

		public static void AddError(this IErrorProcessor errorProcessor, Type source, object? entity, string message, Exception? exception = null)
		{
			if (errorProcessor == null) throw new ArgumentNullException(nameof(errorProcessor));
			if (source == null) throw new ArgumentNullException(nameof(source));
			if (message == null) throw new ArgumentNullException(nameof(message));

			if (errorProcessor.LineInfoRequired)
			{
				if (entity != null && entity.Is<IXmlLineInfo>(out var xmlLineInfo) && xmlLineInfo.HasLineInfo())
				{
					errorProcessor.AddError(new ErrorItem(source, message, exception, xmlLineInfo.LineNumber, xmlLineInfo.LinePosition));

					return;
				}

				if (exception is XmlException xmlException && xmlException.LineNumber > 0)
				{
					errorProcessor.AddError(new ErrorItem(source, message, exception, xmlException.LineNumber, xmlException.LinePosition));

					return;
				}
			}

			errorProcessor.AddError(new ErrorItem(source, message, exception));
		}
	}
}