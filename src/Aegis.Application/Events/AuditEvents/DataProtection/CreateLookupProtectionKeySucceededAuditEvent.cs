namespace Aegis.Application.Events.Audit.DataProtection
{
	using Aegis.Application.Contracts.Application.Events;
	using Aegis.Application.Events.AuditEvents;
	using Aegis.Enums.AuditEvents;

	/// <summary>
	/// Create Lookup Protection Key Succeeded Audit Event
	/// </summary>
	/// <seealso cref="IAuditEvent" />
	public sealed record CreateLookupProtectionKeySucceededAuditEvent : AuditEvent
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CreateLookupProtectionKeySucceededAuditEvent"/> class.
		/// </summary>
		/// <param name="subjectId">The subject identifier.</param>
		/// <param name="summary">The summary.</param>
		/// <param name="oldValues">The old values.</param>
		/// <param name="newValues">The new values.</param>
		public CreateLookupProtectionKeySucceededAuditEvent(Guid subjectId, string summary, string? oldValues = null, string? newValues = null)
			: base(true, AuditModule.Application, AuditAction.Create, AuditSubject.ProtectionKey, subjectId, summary, false, oldValues, newValues)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CreateLookupProtectionKeySucceededAuditEvent"/> class.
		/// </summary>
		/// <param name="subjectId">The subject identifier.</param>
		/// <param name="summary">The summary.</param>
		/// <param name="serviceInitiated">if set to <c>true</c> [service initiated].</param>
		/// <param name="oldValues">The old values.</param>
		/// <param name="newValues">The new values.</param>
		public CreateLookupProtectionKeySucceededAuditEvent(Guid subjectId, string summary, bool serviceInitiated, string? oldValues = null, string? newValues = null)
			: base(true, AuditModule.Application, AuditAction.Create, AuditSubject.ProtectionKey, subjectId, summary, serviceInitiated, oldValues, newValues)
		{
		}
	}
}
