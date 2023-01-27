namespace Aegis.Core.Events.AuditEvents.IdentityProvider
{
	using Aegis.Enums.AuditEvents;

	/// <summary>
	/// Forgot Password Succeeded Audit Event
	/// </summary>
	/// <seealso cref="Aegis.Core.Events.AuditEvents.AuditEvent" />
	/// <seealso cref="Aegis.Core.Contracts.Application.Events.IAuditEvent" />
	/// <seealso cref="MediatR.INotification" />
	/// <seealso cref="System.IEquatable&lt;Aegis.Core.Events.AuditEvents.AuditEvent&gt;" />
	/// <seealso cref="System.IEquatable&lt;Aegis.Core.Events.AuditEvents.IdentityProvider.SendForgotPasswordSucceededAuditEvent&gt;" />
	public sealed record SendForgotPasswordSucceededAuditEvent : AuditEvent
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SendForgotPasswordSucceededAuditEvent"/> class.
		/// </summary>
		/// <param name="subjectId">The subject identifier.</param>
		/// <param name="summary">The summary.</param>
		/// <param name="oldValues">The old values.</param>
		/// <param name="newValues">The new values.</param>
		public SendForgotPasswordSucceededAuditEvent(Guid subjectId, string summary, string? oldValues = null, string? newValues = null)
			: base(true, AuditModule.IdentityProvider, AuditAction.Update, AuditSubject.User, subjectId, summary, false, oldValues, newValues)
		{
		}
	}
}
