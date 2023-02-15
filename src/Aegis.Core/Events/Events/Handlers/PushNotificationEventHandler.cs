namespace Aegis.Core.Events.Events.Handlers
{
	using System.Threading;
	using System.Threading.Tasks;

	using Aegis.Core.Contracts.Events;
	using Aegis.Core.Events.Events;
	using Aegis.Core.Hubs;

	using Microsoft.AspNetCore.SignalR;
	using Microsoft.Extensions.Logging;

	/// <summary>
	/// Push Notification Event Handler
	/// </summary>
	/// <seealso cref="Aegis.Core.Contracts.Events.IEventHandler&lt;PushNotificationEvent&gt;" />
	public sealed class PushNotificationEventHandler : IEventHandler<PushNotificationEvent>
	{
		/// <summary>
		/// The logger
		/// </summary>
		private readonly ILogger<PushNotificationEventHandler> _logger;

		/// <summary>
		/// The hub
		/// </summary>
		private readonly IHubContext<AegisHub> _hub;

		public PushNotificationEventHandler(ILogger<PushNotificationEventHandler> logger, IHubContext<AegisHub> hub)
		{
			_logger = logger;
			_hub = hub;
		}

		/// <summary>
		/// Handles the specified notification.
		/// </summary>
		/// <param name="notification">The notification.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns></returns>
		public Task Handle(PushNotificationEvent notification, CancellationToken cancellationToken)
		{
			switch (notification.AuditoryType)
			{
				case Models.Enums.Events.PushNotificationEventAuditoryType.User:
					this.SendPushNotificationToUser(notification.EventType.ToString(), notification.Auditory, notification.Message);
					break;
				case Models.Enums.Events.PushNotificationEventAuditoryType.Room:
					this.SendPushNotificationToRoom(notification.EventType.ToString(), notification.Auditory, notification.Message);
					break;
				case Models.Enums.Events.PushNotificationEventAuditoryType.All:
					this.SendPushNotificationToAll(notification.EventType.ToString(), notification.Message);
					break;
				default:
					break;
			}

			return Task.CompletedTask;
		}

		/// <summary>
		/// Sends the push notification to user.
		/// </summary>
		/// <param name="notificationType">Type of the notification.</param>
		/// <param name="user">The user.</param>
		/// <param name="message">The message.</param>
		/// <returns></returns>
		public Task SendPushNotificationToUser(string notificationType, string user, string message)
			=> _hub.Clients.User(user).SendAsync(notificationType, message);

		/// <summary>
		/// Sends the push notification to room.
		/// </summary>
		/// <param name="notificationType">Type of the notification.</param>
		/// <param name="room">The room.</param>
		/// <param name="message">The message.</param>
		/// <returns></returns>
		public Task SendPushNotificationToRoom(string notificationType, string room, string message)
			=> _hub.Clients.Group(room).SendAsync(notificationType, message);

		/// <summary>
		/// Sends the push notification to room.
		/// </summary>
		/// <param name="notificationType">Type of the notification.</param>
		/// <param name="message">The message.</param>
		/// <returns></returns>
		public Task SendPushNotificationToAll(string notificationType, string message)
			=> _hub.Clients.All.SendAsync(notificationType, message);
	}
}
