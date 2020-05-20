﻿using System.Threading.Tasks;

namespace TSSArt.StateMachine
{
	public interface INotifyStateChanged
	{
		ValueTask OnChanged(StateMachineInterpreterState state);
	}
}