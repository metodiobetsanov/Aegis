#region copyright
//----------------------------------------------------------------------
// Copyright 2023 MNB Software
// Licensed under the Apache License, Version 2.0
// You may obtain a copy at http://www.apache.org/licenses/LICENSE-2.0
//----------------------------------------------------------------------
#endregion

namespace Aegis.Core.Contracts.Application.Events
{
	using MediatR;

	/// <summary>
	/// Audit Event Handler Interface
	/// </summary>
	/// <typeparam name="TEvent">The type of the event.</typeparam>
	/// <seealso cref="INotificationHandler&lt;TEvent&gt;" />
	public interface IAuditEventHandler<in TEvent> : INotificationHandler<TEvent>
		where TEvent : IAuditEvent
	{
	}
}
