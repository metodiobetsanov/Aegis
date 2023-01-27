namespace Aegis.Core.Contracts.Application.Events
{
	using Aegis.Enums.AuditEvents;

	using MediatR;

	/// <summary>
	/// Audit Event Interface
	/// </summary>
	/// <seealso cref="INotification" />
	public interface IAuditEvent : INotification
	{
		/// <summary>
		/// Gets a value indicating whether this <see cref="AuditLog"/> is succeeded.
		/// </summary>
		/// <value>
		///   <c>true</c> if succeeded; otherwise, <c>false</c>.
		/// </value>
		bool Succeeded { get; init; }

		/// <summary>
		/// Gets or sets the module.
		/// </summary>
		/// <value>
		/// The module.
		/// </value>
		AuditModule Module { get; init; }

		/// <summary>
		/// Gets the action.
		/// </summary>
		/// <value>
		/// The action.
		/// </value>
		AuditAction Action { get; init; }

		/// <summary>
		/// Gets the type of the subject.
		/// </summary>
		/// <value>
		/// The type of the subject.
		/// </value>
		AuditSubject Subject { get; init; }

		/// <summary>
		/// Gets the subject identifier.
		/// </summary>
		/// <value>
		/// The subject identifier.
		/// </value>
		Guid SubjectId { get; init; }

		/// <summary>
		/// Gets the summary.
		/// </summary>
		/// <value>
		/// The summary.
		/// </value>
		string Summary { get; init; }

		/// <summary>
		/// Gets or sets the old values.
		/// </summary>
		/// <value>
		/// The old values.
		/// </value>
		string? OldValues { get; init; }

		/// <summary>
		/// Creates new values.
		/// </summary>
		/// <value>
		/// The new values.
		/// </value>
		string? NewValues { get; init; }

		/// <summary>
		/// Gets a value indicating whether [service initiated].
		/// </summary>
		/// <value>
		///   <c>true</c> if [service initiated]; otherwise, <c>false</c>.
		/// </value>
		bool ServiceInitiated { get; init; }
	}
}
