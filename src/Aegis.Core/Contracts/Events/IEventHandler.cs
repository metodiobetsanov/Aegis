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
