namespace Aegis.Core.Events.AuditLog.Handlers
{
	using System.Threading.Tasks;

	using Aegis.Core.Constants;
	using Aegis.Core.Contracts.Application.Events;
	using Aegis.Models.Shared;
	using Aegis.Persistence.Contracts;
	using Aegis.Persistence.Entities.Application;

	using Microsoft.AspNetCore.Http;
	using Microsoft.Extensions.Logging;

	/// <summary>
	/// Audit Event Handler
	/// </summary>
	/// <typeparam name="TEvent">The type of the event.</typeparam>
	/// <seealso cref="Aegis.Core.Contracts.Application.Events.IAuditEventHandler&lt;TEvent&gt;" />
	public class AuditEventHandler<TEvent> : IAuditEventHandler<TEvent>
		where TEvent : IAuditEvent
	{
		/// <summary>
		/// The logger
		/// </summary>
		private readonly ILogger<AuditEventHandler<TEvent>> _logger;

		/// <summary>
		/// The HTTP context
		/// </summary>
		private readonly IHttpContextAccessor _httpContext;

		/// <summary>
		/// The repository
		/// </summary>
		private readonly IAegisContext _context;

		/// <summary>
		/// Initializes a new instance of the <see cref="AuditEventHandler{TEvent}"/> class.
		/// </summary>
		/// <param name="logger">The logger.</param>
		/// <param name="httpContext">The HTTP context.</param>
		/// <param name="context">The context.</param>
		public AuditEventHandler(ILogger<AuditEventHandler<TEvent>> logger, IHttpContextAccessor httpContext, IAegisContext context)
		{
			_logger = logger;
			_httpContext = httpContext;
			_context = context;
		}

		/// <summary>
		/// Handles the specified notification.
		/// </summary>
		/// <param name="notification">The audit event.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns></returns>
		/// <exception cref="System.NotImplementedException"></exception>
		public Task Handle(TEvent notification, CancellationToken cancellationToken)
		{
			try
			{
				_logger.LogDebug("Received Audit Event '{Name}'.", typeof(TEvent).Name);
				LoggedUser user = this.GetAuditEventUser(notification);

				AuditLog log = new AuditLog
				{
					EventName = notification.GetType().Name.Replace("AuditEvent", ""),
					Succeeded = notification.Succeeded,
					Module = (int)notification.Module,
					Action = (int)notification.Action,
					Subject = (int)notification.Subject,
					SubjectId = notification.SubjectId,
					UserId = user.UserId,
					UserName = user.UserName,
					UserIp = user.UserIp,
					UserAgent = user.UserAgent,
					Summary = notification.Summary,
					OldValues = notification.OldValues,
					NewValues = notification.NewValues
				};

				_context.AuditLogs.Create(log);
				_context.SaveChanges();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "AuditEvent handler Error: {Message}", ex.Message);
			}

			return Task.CompletedTask;
		}

		/// <summary>
		/// Gets the audit event user.
		/// </summary>
		/// <returns></returns>
		private LoggedUser GetAuditEventUser(TEvent notification)
		{
			Guid userId = Guid.Empty;
			string username = ApplicationConstants.ApplicationName.ToLower().Replace(' ', '_');
			string? userIp = null;
			string? userAgent = null;

			if (!notification.ServiceInitiated)
			{
				userId = Guid.Parse(_httpContext.HttpContext!.User.Claims.First(x => x.Type == "sub").Value);
				username = _httpContext.HttpContext!.User.Claims.First(x => x.Type == "name").Value;
				userIp = _httpContext.HttpContext!.Connection?.RemoteIpAddress?.ToString();
				userAgent = _httpContext.HttpContext!.Request.Headers.FirstOrDefault(x => x.Key == "User-Agent").Value;
			}

			return new LoggedUser(userId, username, userIp, userAgent);
		}
	}
}
