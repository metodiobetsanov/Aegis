namespace Aegis.Persistence.Entities.Application
{
	using Aegis.Persistence.Attributes;

	/// <summary>
	/// Audit Log Entity
	/// </summary>
	public class AuditLog
	{
		/// <summary>
		/// Gets or sets the identifier.
		/// </summary>
		/// <value>
		/// The identifier.
		/// </value>
		public Guid Id { get; set; }

		/// <summary>
		/// Gets or sets the name of the event.
		/// </summary>
		/// <value>
		/// The name of the event.
		/// </value>
		public string EventName { get; set; } = default!;

		/// <summary>
		/// Gets or sets the timestamp.
		/// </summary>
		/// <value>
		/// The timestamp.
		/// </value>
		public DateTime Timestamp { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="AuditLog"/> is succeeded.
		/// </summary>
		/// <value>
		///   <c>true</c> if succeeded; otherwise, <c>false</c>.
		/// </value>
		public bool Succeeded { get; set; }

		/// <summary>
		/// Gets or sets the module.
		/// </summary>
		/// <value>
		/// The module.
		/// </value>
		public int Module { get; set; }

		/// <summary>
		/// Gets or sets the action.
		/// </summary>
		/// <value>
		/// The action.
		/// </value>
		public int Action { get; set; }

		/// <summary>
		/// Gets or sets the type of the subject.
		/// </summary>
		/// <value>
		/// The type of the subject.
		/// </value>
		public int Subject { get; set; }

		/// <summary>
		/// Gets or sets the subject identifier.
		/// </summary>
		/// <value>
		/// The subject identifier.
		/// </value>
		[SecureColumn]
		public Guid SubjectId { get; set; }

		/// <summary>
		/// Gets or sets the user identifier.
		/// </summary>
		/// <value>
		/// The user identifier.
		/// </value>
		[SecureColumn]
		public Guid UserId { get; set; }

		/// <summary>
		/// Gets or sets the name of the user.
		/// </summary>
		/// <value>
		/// The name of the user.
		/// </value>
		[SecureColumn]
		public string? UserName { get; set; }

		/// <summary>
		/// Gets or sets the user ip.
		/// </summary>
		/// <value>
		/// The user ip.
		/// </value>
		[SecureColumn]
		public string? UserIp { get; set; }

		/// <summary>
		/// Gets or sets the user agent.
		/// </summary>
		/// <value>
		/// The user agent.
		/// </value>
		public string? UserAgent { get; set; }

		/// <summary>
		/// Gets or sets the summary.
		/// </summary>
		/// <value>
		/// The summary.
		/// </value>
		public required string Summary { get; set; }

		/// <summary>
		/// Gets or sets the old values.
		/// </summary>
		/// <value>
		/// The old values.
		/// </value>
		[SecureColumn]
		public string? OldValues { get; set; }

		/// <summary>
		/// Creates new values.
		/// </summary>
		/// <value>
		/// The new values.
		/// </value>
		[SecureColumn]
		public string? NewValues { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="AuditLog"/> class.
		/// </summary>
		public AuditLog()
		{
			this.Id = Guid.NewGuid();
			this.Timestamp = DateTime.UtcNow;
		}
	}
}
