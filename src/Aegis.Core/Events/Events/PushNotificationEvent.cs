namespace Aegis.Core.Events.Events
{
	using Aegis.Core.Contracts.Events;
	using Aegis.Models.Enums.Events;

	/// <summary>
	/// Push Notification Event
	/// </summary>
	/// <seealso cref="IEvent" />
	/// <seealso cref="MediatR.INotification" />
	/// <seealso cref="IEquatable&lt;PushNotificationEvent&gt;" />
	public sealed record PushNotificationEvent : IEvent
	{
		/// <summary>
		/// Gets the type of the event.
		/// </summary>
		/// <value>
		/// The type of the event.
		/// </value>
		public PushNotificationEventType EventType { get; init; }

		/// <summary>
		/// Gets the auditory.
		/// </summary>
		/// <value>
		/// The auditory.
		/// </value>
		public PushNotificationEventAuditoryType AuditoryType { get; init; }

		/// <summary>
		/// Gets the user.
		/// </summary>
		/// <value>
		/// The user.
		/// </value>
		public string Auditory { get; init; }

		/// <summary>
		/// Gets the message.
		/// </summary>
		/// <value>
		/// The message.
		/// </value>
		public string Message { get; init; }

		/// <summary>
		/// Initializes a new instance of the <see cref="PushNotificationEvent"/> class.
		/// </summary>
		/// <param name="eventType">Type of the event.</param>
		/// <param name="auditoryType">Type of the auditory.</param>
		/// <param name="auditory">The auditory.</param>
		/// <param name="message">The message.</param>
		public PushNotificationEvent(
			PushNotificationEventType eventType,
			PushNotificationEventAuditoryType auditoryType,
			string auditory,
			string message)
		{
			this.EventType = eventType;
			this.AuditoryType = auditoryType;
			this.Auditory = auditory;
			this.Message = message;
		}
	}
}
