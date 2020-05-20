﻿using System;
using System.Runtime.Serialization;

namespace TSSArt.StateMachine
{
	[Serializable]
	public class StateMachineDestroyedException : StateMachineException
	{
		public StateMachineDestroyedException() { }

		public StateMachineDestroyedException(string? message) : base(message) { }

		public StateMachineDestroyedException(string? message, Exception? inner) : base(message, inner) { }

		protected StateMachineDestroyedException(SerializationInfo info, StreamingContext context) : base(info, context) { }
	}
}