namespace Aegis.Application.Events.Audit.IdentityProvider
{
	using Aegis.Application.Contracts.Application.Events;
	using Aegis.Application.Events.AuditEvents;
	using Aegis.Enums.AuditEvents;

	/// <summary>
	/// Role Assignment Failed Audit Event
	/// </summary>
	/// <seealso cref="IAuditEvent" />
	public sealed record AssignRoleFailedAuditEvent : AuditEvent
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="AssignRoleFailedAuditEvent"/> class.
		/// </summary>
		/// <param name="subjectId">The subject identifier.</param>
		/// <param name="summary">The summary.</param>
		/// <param name="oldValues">The old values.</param>
		/// <param name="newValues">The new values.</param>
		public AssignRoleFailedAuditEvent(Guid subjectId, string summary, string? oldValues = null, string? newValues = null)
			: base(false, AuditModule.IdentityProvider, AuditAction.Update, AuditSubject.User, subjectId, summary, false, oldValues, newValues)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="AssignRoleFailedAuditEvent"/> class.
		/// </summary>
		/// <param name="subjectId">The subject identifier.</param>
		/// <param name="summary">The summary.</param>
		/// <param name="serviceInitiated">if set to <c>true</c> [service initiated].</param>
		/// <param name="oldValues">The old values.</param>
		/// <param name="newValues">The new values.</param>
		public AssignRoleFailedAuditEvent(Guid subjectId, string summary, bool serviceInitiated, string? oldValues = null, string? newValues = null)
			: base(false, AuditModule.IdentityProvider, AuditAction.Update, AuditSubject.User, subjectId, summary, serviceInitiated, oldValues, newValues)
		{
		}
	}
}
