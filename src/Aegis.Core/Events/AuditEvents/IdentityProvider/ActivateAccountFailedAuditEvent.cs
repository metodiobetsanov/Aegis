#region copyright
//----------------------------------------------------------------------
// Copyright 2023 MNB Software
// Licensed under the Apache License, Version 2.0
// You may obtain a copy at http://www.apache.org/licenses/LICENSE-2.0
//----------------------------------------------------------------------
#endregion

namespace Aegis.Core.Events.AuditEvents.IdentityProvider
{
	using Aegis.Enums.AuditEvents;

	/// <summary>
	/// Activate Account Failed Audit Event
	/// </summary>
	/// <seealso cref="Aegis.Core.Events.AuditEvents.AuditEvent" />
	/// <seealso cref="Aegis.Core.Contracts.Application.Events.IAuditEvent" />
	/// <seealso cref="MediatR.INotification" />
	/// <seealso cref="System.IEquatable&lt;Aegis.Core.Events.AuditEvents.AuditEvent&gt;" />
	/// <seealso cref="System.IEquatable&lt;Aegis.Core.Events.AuditEvents.IdentityProvider.ActivateAccountFailedAuditEvent&gt;" />
	public sealed record ActivateAccountFailedAuditEvent : AuditEvent
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ActivateAccountFailedAuditEvent"/> class.
		/// </summary>
		/// <param name="subjectId">The subject identifier.</param>
		/// <param name="summary">The summary.</param>
		/// <param name="oldValues">The old values.</param>
		/// <param name="newValues">The new values.</param>
		public ActivateAccountFailedAuditEvent(Guid subjectId, string summary, string? oldValues = null, string? newValues = null)
			: base(false, AuditModule.IdentityProvider, AuditAction.Update, AuditSubject.User, subjectId, summary, false, oldValues, newValues)
		{
		}
	}
}
