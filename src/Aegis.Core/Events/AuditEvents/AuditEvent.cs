#region copyright
//----------------------------------------------------------------------
// Copyright 2023 MNB Software
// Licensed under the Apache License, Version 2.0
// You may obtain a copy at http://www.apache.org/licenses/LICENSE-2.0
//----------------------------------------------------------------------
#endregion

namespace Aegis.Core.Events.AuditEvents
{
	using Aegis.Core.Contracts.Application.Events;
	using Aegis.Enums.AuditEvents;

	public record AuditEvent : IAuditEvent
	{
		/// <summary>
		/// Gets a value indicating whether this <see cref="AuditLog"/> is succeeded.
		/// </summary>
		/// <value>
		///  <c>true</c> if succeeded; otherwise, <c>false</c>.
		/// </value>
		public bool Succeeded { get; init; }

		/// <summary>
		/// Gets or sets the module.
		/// </summary>
		/// <value>
		/// The module.
		/// </value>
		public AuditModule Module { get; init; }

		/// <summary>
		/// Gets the action.
		/// </summary>
		/// <value>
		/// The action.
		/// </value>
		public AuditAction Action { get; init; }

		/// <summary>
		/// Gets the type of the subject.
		/// </summary>
		/// <value>
		/// The type of the subject.
		/// </value>
		public AuditSubject Subject { get; init; }

		/// <summary>
		/// Gets the subject identifier.
		/// </summary>
		/// <value>
		/// The subject identifier.
		/// </value>
		public Guid SubjectId { get; init; }

		/// <summary>
		/// Gets the summary.
		/// </summary>
		/// <value>
		/// The summary.
		/// </value>
		public string Summary { get; init; }

		/// <summary>
		/// Gets or sets the old values.
		/// </summary>
		/// <value>
		/// The old values.
		/// </value>
		public string? OldValues { get; init; }

		/// <summary>
		/// Creates new values.
		/// </summary>
		/// <value>
		/// The new values.
		/// </value>
		public string? NewValues { get; init; }

		/// <summary>
		/// Gets a value indicating whether [service initiated].
		/// </summary>
		/// <value>
		///  <c>true</c> if [service initiated]; otherwise, <c>false</c>.
		/// </value>
		public bool ServiceInitiated { get; init; }

		/// <summary>
		/// Initializes a new instance of the <see cref="AuditEvent"/> class.
		/// </summary>
		/// <param name="succeeded">if set to <c>true</c> [succeeded].</param>
		/// <param name="module">The module.</param>
		/// <param name="action">The action.</param>
		/// <param name="subject">The subject.</param>
		/// <param name="subjectId">The subject identifier.</param>
		/// <param name="summary">The summary.</param>
		/// <param name="serviceInitiated">if set to <c>true</c> [service initiated].</param>
		/// <param name="oldValues">The old values.</param>
		/// <param name="newValues">The new values.</param>
		protected AuditEvent(
			bool succeeded,
			AuditModule module,
			AuditAction action,
			AuditSubject subject,
			Guid subjectId,
			string summary,
			bool serviceInitiated = false,
			string? oldValues = null,
			string? newValues = null)
		{
			this.Succeeded = succeeded;
			this.Module = module;
			this.Action = action;
			this.Subject = subject;
			this.SubjectId = subjectId;
			this.Summary = summary;
			this.ServiceInitiated = serviceInitiated;
			this.OldValues = oldValues;
			this.NewValues = newValues;
		}
	}
}
