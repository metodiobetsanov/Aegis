namespace Aegis.Models.Shared
{
	/// <summary>
	/// Logged User
	/// </summary>
	public sealed record LoggedUser
	{
		// <summary>
		/// Gets the user identifier.
		/// </summary>
		/// <value>
		/// The user identifier.
		/// </value>
		public Guid UserId { get; init; }

		/// <summary>
		/// Gets the name of the user.
		/// </summary>
		/// <value>
		/// The name of the user.
		/// </value>
		public string UserName { get; init; }

		/// <summary>
		/// Gets the user ip.
		/// </summary>
		/// <value>
		/// The user ip.
		/// </value>
		public string? UserIp { get; init; }

		/// <summary>
		/// Gets the user agent.
		/// </summary>
		/// <value>
		/// The user agent.
		/// </value>
		public string? UserAgent { get; init; }

		/// <summary>
		/// Initializes a new instance of the <see cref="LoggedUser"/> class.
		/// </summary>
		/// <param name="userId">The user identifier.</param>
		/// <param name="userName">Name of the user.</param>
		/// <param name="userIp">The user ip.</param>
		/// <param name="userAgent">The user agent.</param>
		public LoggedUser(Guid userId, string userName, string? userIp, string? userAgent)
		{
			this.UserId = userId;
			this.UserName = userName;
			this.UserIp = userIp;
			this.UserAgent = userAgent;
		}
	}
}
