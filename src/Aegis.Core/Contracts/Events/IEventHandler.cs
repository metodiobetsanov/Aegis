#region copyright
//----------------------------------------------------------------------
// Copyright 2023 MNB Software
// Licensed under the Apache License, Version 2.0
// You may obtain a copy at http://www.apache.org/licenses/LICENSE-2.0
//----------------------------------------------------------------------
#endregion

namespace Aegis.Core.Contracts.Events
{
	using MediatR;

	/// <summary>
	/// Event Handler Interface
	/// </summary>
	/// <typeparam name="TEvent">The type of the event.</typeparam>
	/// <seealso cref="MediatR.INotificationHandler&lt;TEvent&gt;" />
	public interface IEventHandler<in TEvent> : INotificationHandler<TEvent>
		where TEvent : IEvent
	{
	}
}
