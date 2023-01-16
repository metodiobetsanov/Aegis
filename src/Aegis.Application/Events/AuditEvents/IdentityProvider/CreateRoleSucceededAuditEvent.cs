namespace Aegis.Application.Events.Audit.IdentityProvider
{
	using Aegis.Application.Contracts.Application.Events;
	using Aegis.Application.Events.AuditEvents;
	using Aegis.Enums.AuditEvents;

	/// <summary>
	/// Create Role Succeeded Audit Event
	/// </summary>
	/// <seealso cref="IAuditEvent" />
	public sealed record CreateRoleSucceededAuditEvent : AuditEvent
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CreateRoleSucceededAuditEvent"/> class.
		/// </summary>
		/// <param name="subjectId">The subject identifier.</param>
		/// <param name="summary">The summary.</param>
		/// <param name="oldValues">The old values.</param>
		/// <param name="newValues">The new values.</param>
		public CreateRoleSucceededAuditEvent(Guid subjectId, string summary, string? oldValues = null, string? newValues = null)
			: base(true, AuditModule.IdentityProvider, AuditAction.Create, AuditSubject.Role, subjectId, summary, false, oldValues, newValues)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CreateRoleSucceededAuditEvent"/> class.
		/// </summary>
		/// <param name="subjectId">The subject identifier.</param>
		/// <param name="summary">The summary.</param>
		/// <param name="serviceInitiated">if set to <c>true</c> [service initiated].</param>
		/// <param name="oldValues">The old values.</param>
		/// <param name="newValues">The new values.</param>
		public CreateRoleSucceededAuditEvent(Guid subjectId, string summary, bool serviceInitiated, string? oldValues = null, string? newValues = null)
			: base(true, AuditModule.IdentityProvider, AuditAction.Create, AuditSubject.Role, subjectId, summary, serviceInitiated, oldValues, newValues)
		{
		}
	}
}
