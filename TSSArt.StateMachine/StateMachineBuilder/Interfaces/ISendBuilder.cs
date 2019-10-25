﻿using System;
using System.Collections.Generic;

namespace TSSArt.StateMachine
{
	public interface ISendBuilder
	{
		ISend Build();

		void SetEvent(string @event);
		void SetEventExpression(IValueExpression eventExpression);
		void SetTarget(Uri target);
		void SetTargetExpression(IValueExpression targetExpression);
		void SetType(Uri type);
		void SetTypeExpression(IValueExpression typeExpression);
		void SetId(string id);
		void SetIdLocation(ILocationExpression idLocation);
		void SetDelay(int delay);
		void SetDelayExpression(IValueExpression delayExpression);
		void SetNameList(IReadOnlyList<ILocationExpression> nameList);
		void AddParameter(IParam param);
		void SetContent(IContent content);
	}
}