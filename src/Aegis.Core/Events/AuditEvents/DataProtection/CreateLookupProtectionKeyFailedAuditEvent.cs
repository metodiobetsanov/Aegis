﻿#region copyright
//----------------------------------------------------------------------
// Copyright 2023 MNB Software
// Licensed under the Apache License, Version 2.0
// You may obtain a copy at http://www.apache.org/licenses/LICENSE-2.0
//----------------------------------------------------------------------
#endregion

namespace Aegis.Core.Events.Audit.DataProtection
{
	using Aegis.Core.Contracts.Application.Events;
	using Aegis.Core.Events.AuditEvents;
	using Aegis.Enums.AuditEvents;

	/// <summary>
	/// Create Lookup Protection Key Failed Audit Event
	/// </summary>
	/// <seealso cref="IAuditEvent" />
	public sealed record CreateLookupProtectionKeyFailedAuditEvent : AuditEvent
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CreateLookupProtectionKeyFailedAuditEvent"/> class.
		/// </summary>
		/// <param name="subjectId">The subject identifier.</param>
		/// <param name="summary">The summary.</param>
		/// <param name="oldValues">The old values.</param>
		/// <param name="newValues">The new values.</param>
		public CreateLookupProtectionKeyFailedAuditEvent(Guid subjectId, string summary, string? oldValues = null, string? newValues = null)
			: base(false, AuditModule.Application, AuditAction.Create, AuditSubject.ProtectionKey, subjectId, summary, false, oldValues, newValues)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CreateLookupProtectionKeyFailedAuditEvent"/> class.
		/// </summary>
		/// <param name="subjectId">The subject identifier.</param>
		/// <param name="summary">The summary.</param>
		/// <param name="serviceInitiated">if set to <c>true</c> [service initiated].</param>
		/// <param name="oldValues">The old values.</param>
		/// <param name="newValues">The new values.</param>
		public CreateLookupProtectionKeyFailedAuditEvent(Guid subjectId, string summary, bool serviceInitiated, string? oldValues = null, string? newValues = null)
			: base(false, AuditModule.Application, AuditAction.Create, AuditSubject.ProtectionKey, subjectId, summary, serviceInitiated, oldValues, newValues)
		{
		}
	}
}
