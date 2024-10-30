﻿// Copyright © 2019-2024 Sergii Artemenko
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

using System;

namespace Xtate;

internal class EventObject : IEvent
{
	public EventObject(EventName name,
					   Uri? origin,
					   Uri originType,
					   DataModelValue data)
	{
		Name = name;
		Origin = origin;
		OriginType = originType;
		Data = data.AsConstant();
	}

	public EventObject(string eventName,
					   Uri? origin,
					   Uri originType,
					   DataModelValue data)
	{
		Name = (EventName)eventName;
		Origin = origin;
		OriginType = originType;
		Data = data.AsConstant();
	}

#region Interface IEvent

	public DataModelValue Data { get; }

	public InvokeId? InvokeId => null;

	public EventName Name { get; }

	public Uri? Origin { get; }

	public Uri OriginType { get; }

	public SendId? SendId => null;

	public EventType Type => EventType.External;

#endregion
}