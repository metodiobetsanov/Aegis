namespace Aegis.Models.Auth
{
	/// <summary>
	/// SignUp Command Result
	/// </summary>
	/// <seealso cref="System.IEquatable&lt;Aegis.Models.Auth.SignUpCommandResult&gt;" />
	public sealed record SignUpCommandResult
	{
		/// <summary>
		/// Creates the success result.
		/// </summary>
		public static SignUpCommandResult Succeeded(Guid userId, string? returnUrl)
			=> new SignUpCommandResult { Success = true, UserId = userId, ReturnUrl = returnUrl };

		/// <summary>
		/// Creates the failed result.
		/// </summary>
		public static SignUpCommandResult Failed()
			=> new SignUpCommandResult();

		/// <summary>
		/// Gets a value indicating whether this <see cref="SignUpCommandResult"/> is success.
		/// </summary>
		/// <value>
		///   <c>true</c> if success; otherwise, <c>false</c>.
		/// </value>
		public bool Success { get; init; }

		/// <summary>
		/// Gets the user identifier.
		/// </summary>
		/// <value>
		/// The user identifier.
		/// </value>
		public Guid UserId { get; init; }

		/// <summary>
		/// Gets or sets the return URL.
		/// </summary>
		/// <value>
		/// The return URL.
		/// </value>
		public string? ReturnUrl { get; init; }

		/// <summary>
		/// Gets or sets the errors.
		/// </summary>
		/// <value>
		/// The errors.
		/// </value>
		public List<KeyValuePair<string, string>> Errors { get; init; }

		/// <summary>
		/// Initializes a new instance of the <see cref="SignUpCommandResult"/> class.
		/// </summary>
		/// <param name="success">if set to <c>true</c> [succeeded].</param>
		public SignUpCommandResult()
			=> this.Errors = new List<KeyValuePair<string, string>>();
	}
}
