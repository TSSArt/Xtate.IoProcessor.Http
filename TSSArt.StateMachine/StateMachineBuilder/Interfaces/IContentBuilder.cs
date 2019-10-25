﻿namespace TSSArt.StateMachine
{
	public interface IContentBuilder
	{
		IContent Build();

		void SetExpression(IValueExpression expression);
		void SetBody(string body);
	}
}