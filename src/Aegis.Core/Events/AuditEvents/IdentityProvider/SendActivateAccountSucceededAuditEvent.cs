namespace Aegis.Core.Events.AuditEvents.IdentityProvider
{
	using Aegis.Enums.AuditEvents;

	/// <summary>
	/// Send Activate Account Succeeded Audit Event
	/// </summary>
	/// <seealso cref="Aegis.Core.Events.AuditEvents.AuditEvent" />
	/// <seealso cref="Aegis.Core.Contracts.Application.Events.IAuditEvent" />
	/// <seealso cref="MediatR.INotification" />
	/// <seealso cref="System.IEquatable&lt;Aegis.Core.Events.AuditEvents.AuditEvent&gt;" />
	/// <seealso cref="System.IEquatable&lt;Aegis.Core.Events.AuditEvents.IdentityProvider.SendActivateAccountSucceededAuditEvent&gt;" />
	public sealed record SendActivateAccountSucceededAuditEvent : AuditEvent
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SendActivateAccountSucceededAuditEvent"/> class.
		/// </summary>
		/// <param name="subjectId">The subject identifier.</param>
		/// <param name="summary">The summary.</param>
		/// <param name="oldValues">The old values.</param>
		/// <param name="newValues">The new values.</param>
		public SendActivateAccountSucceededAuditEvent(Guid subjectId, string summary, string? oldValues = null, string? newValues = null)
			: base(true, AuditModule.IdentityProvider, AuditAction.Update, AuditSubject.User, subjectId, summary, false, oldValues, newValues)
		{
		}
	}
}
