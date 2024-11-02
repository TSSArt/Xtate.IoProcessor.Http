// Copyright © 2019-2024 Sergii Artemenko
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

using Xtate.Core;

namespace Xtate;

internal class IncomingEvent : IIncomingEvent
{
	public IncomingEvent(EventName name,
					   FullUri? origin,
					   FullUri originType,
					   DataModelValue data)
	{
		Name = name;
		Origin = origin;
		OriginType = originType;
		Data = data.AsConstant();
	}

	public IncomingEvent(string eventName,
					   FullUri? origin,
					   FullUri originType,
					   DataModelValue data)
	{
		Name = (EventName) eventName;
		Origin = origin;
		OriginType = originType;
		Data = data.AsConstant();
	}

#region Interface IIncomingEvent

	public DataModelValue Data { get; }

	public InvokeId? InvokeId => null;

	public EventName Name { get; }

	public FullUri? Origin { get; }

	public FullUri OriginType { get; }

	public SendId? SendId => null;

	public EventType Type => EventType.External;

#endregion
}