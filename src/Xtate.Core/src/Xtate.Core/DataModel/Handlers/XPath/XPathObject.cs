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

using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace Xtate.DataModel.XPath
{
	internal class XPathObject : IObject
	{
		private readonly object _value;

		public XPathObject(object value)
		{
			_value = value switch
			{
					XPathObject xPathObject => xPathObject._value,
					XPathNodeIterator _ => value,
					string _ => value,
					int _ => value,
					bool _ => value,
					_ => Infrastructure.UnexpectedValue<object>()
			};
		}

		public XPathObjectType Type =>
				_value switch
				{
						XPathNodeIterator _ => XPathObjectType.NodeSet,
						int _ => XPathObjectType.Integer,
						string _ => XPathObjectType.String,
						bool _ => XPathObjectType.Boolean,
						_ => Infrastructure.UnexpectedValue<XPathObjectType>()
				};

	#region Interface IObject

		public object? ToObject() =>
				_value switch
				{
						XPathNodeIterator iterator => ToObject(iterator),
						_ => _value
				};

	#endregion

		private static string GetFirstStringValue(XPathNodeIterator iterator)
		{
			iterator = iterator.Clone();

			if (iterator.MoveNext() && iterator.Current is { } first)
			{
				return first.Value ?? string.Empty;
			}

			return string.Empty;
		}

		public int AsInteger() =>
				_value switch
				{
						XPathNodeIterator iterator => XmlConvert.ToInt32(GetFirstStringValue(iterator)),
						string val => XmlConvert.ToInt32(val),
						int val => val,
						bool val => val ? 1 : 0,
						_ => Infrastructure.UnexpectedValue<int>()
				};

		public string AsString() =>
				_value switch
				{
						XPathNodeIterator iterator => GetFirstStringValue(iterator),
						string val => val,
						int val => XmlConvert.ToString(val),
						bool val => XmlConvert.ToString(val),
						_ => Infrastructure.UnexpectedValue<string>()
				};

		public bool AsBoolean() =>
				_value switch
				{
						XPathNodeIterator iterator => XmlConvert.ToBoolean(GetFirstStringValue(iterator)),
						string val => XmlConvert.ToBoolean(val),
						int val => val != 0,
						bool val => val,
						_ => Infrastructure.UnexpectedValue<bool>()
				};

		public XPathNodeIterator AsIterator() => ((XPathNodeIterator) _value).Clone();

		private static object? ToObject(XPathNodeIterator iterator)
		{
			var length = 0;
			var count = 0;
			string? result = null;

			foreach (DataModelXPathNavigator navigator in iterator)
			{
				switch (navigator.NodeType)
				{
					case XPathNodeType.Element:
						return ToDataModelObject(iterator);

					case XPathNodeType.Text:
						count ++;
						if (navigator.DataModelValue.AsStringOrDefault() is { } str)
						{
							length += str.Length;
							result = str;
						}

						break;

					default:
						return Infrastructure.UnexpectedValue<DataModelObject>();
				}
			}

			if (count == 0)
			{
				return null;
			}

			if (length == 0)
			{
				return string.Empty;
			}

			Infrastructure.NotNull(result);

			if (result.Length == length)
			{
				return result;
			}

			var sb = new StringBuilder(length);

			foreach (DataModelXPathNavigator navigator in iterator)
			{
				if (navigator.DataModelValue.AsStringOrDefault() is { } str)
				{
					sb.Append(str);
				}
			}

			return sb.ToString();
		}

		private static DataModelObject ToDataModelObject(XPathNodeIterator iterator)
		{
			var obj = new DataModelObject();

			foreach (DataModelXPathNavigator navigator in iterator)
			{
				switch (navigator.NodeType)
				{
					case XPathNodeType.Element:
						obj.Add(navigator.LocalName, navigator.DataModelValue.CloneAsWritable(), navigator.Metadata?.DeepClone(DataModelAccess.Writable));
						break;

					case XPathNodeType.Text:
						obj.Add(key: default, navigator.DataModelValue.CloneAsWritable(), metadata: default);
						break;

					default:
						return Infrastructure.UnexpectedValue<DataModelObject>();
				}
			}

			return obj;
		}

		public static string ToString(object obj) =>
				obj switch
				{
						XPathNodeIterator iterator => ToString(iterator),
						int val => XmlConvert.ToString(val),
						string val => val,
						bool val => XmlConvert.ToString(val),
						_ => Infrastructure.UnexpectedValue<string>()
				};

		private static string ToString(XPathNodeIterator iterator)
		{
			var stringBuilder = new StringBuilder();

			foreach (XPathNavigator navigator in iterator)
			{
				stringBuilder.Append(navigator.Value);
			}

			return stringBuilder.ToString();
		}
	}
}