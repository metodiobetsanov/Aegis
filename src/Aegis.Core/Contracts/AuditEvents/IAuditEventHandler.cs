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
