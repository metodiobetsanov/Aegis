namespace Aegis.Core.Events.Audit.IdentityProvider
{
	using Aegis.Core.Events.AuditEvents;
	using Aegis.Enums.AuditEvents;

	/// <summary>
	/// Assign Role Succeeded Audit Event
	/// </summary>
	/// <seealso cref="AuditEvent" />
	public sealed record AssignRoleSucceededAuditEvent : AuditEvent
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="AssignRoleSucceededAuditEvent"/> class.
		/// </summary>
		/// <param name="subjectId">The subject identifier.</param>
		/// <param name="summary">The summary.</param>
		/// <param name="oldValues">The old values.</param>
		/// <param name="newValues">The new values.</param>
		public AssignRoleSucceededAuditEvent(Guid subjectId, string summary, string? oldValues = null, string? newValues = null)
			: base(true, AuditModule.IdentityProvider, AuditAction.Update, AuditSubject.User, subjectId, summary, false, oldValues, newValues)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="AssignRoleSucceededAuditEvent"/> class.
		/// </summary>
		/// <param name="subjectId">The subject identifier.</param>
		/// <param name="summary">The summary.</param>
		/// <param name="serviceInitiated">if set to <c>true</c> [service initiated].</param>
		/// <param name="oldValues">The old values.</param>
		/// <param name="newValues">The new values.</param>
		public AssignRoleSucceededAuditEvent(Guid subjectId, string summary, bool serviceInitiated, string? oldValues = null, string? newValues = null)
			: base(true, AuditModule.IdentityProvider, AuditAction.Update, AuditSubject.User, subjectId, summary, serviceInitiated, oldValues, newValues)
		{
		}
	}
}
