namespace Aegis.Application.Contracts.Events
{
	using MediatR;

	/// <summary>
	/// Event Interface
	/// </summary>
	/// <seealso cref="MediatR.INotification" />
	public interface IEvent : INotification
	{
	}
}
