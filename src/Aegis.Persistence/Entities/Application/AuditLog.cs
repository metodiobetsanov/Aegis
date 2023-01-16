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
		public string EventName { get; set; }

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
		public string Summary { get; set; }

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
		/// <param name="eventName">Name of the event.</param>
		/// <param name="succeeded">if set to <c>true</c> [succeeded].</param>
		/// <param name="module">The module.</param>
		/// <param name="action">The action.</param>
		/// <param name="subject">The subject.</param>
		/// <param name="subjectId">The subject identifier.</param>
		/// <param name="userId">The user identifier.</param>
		/// <param name="userName">Name of the user.</param>
		/// <param name="userIp">The user ip.</param>
		/// <param name="userAgent">The user agent.</param>
		/// <param name="summary">The summary.</param>
		/// <param name="oldValues">The old values.</param>
		/// <param name="newValues">The new values.</param>
		public AuditLog(
			string eventName,
			bool succeeded,
			int module,
			int action,
			int subject,
			Guid subjectId,
			Guid userId,
			string? userName,
			string? userIp,
			string? userAgent,
			string summary,
			string? oldValues = null,
			string? newValues = null)
		{
			this.Timestamp = DateTime.UtcNow;
			this.EventName = eventName;
			this.Succeeded = succeeded;
			this.Module = module;
			this.Action = action;
			this.Subject = subject;
			this.SubjectId = subjectId;
			this.UserId = userId;
			this.UserName = userName;
			this.UserIp = userIp;
			this.UserAgent = userAgent;
			this.Summary = summary;
			this.OldValues = oldValues;
			this.NewValues = newValues;
		}
	}
}
