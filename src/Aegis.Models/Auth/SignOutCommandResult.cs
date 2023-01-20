namespace Aegis.Models.Auth
{
	/// <summary>
	/// SignOut Command Result
	/// </summary>
	/// <seealso cref="System.IEquatable&lt;Aegis.Models.Auth.SignOutCommandResult&gt;" />
	public sealed record SignOutCommandResult
	{
		/// <summary>
		/// Creates the success result.
		/// </summary>
		public static SignOutCommandResult Succeeded(string? returnUrl)
			=> new SignOutCommandResult { Success = true, ReturnUrl = returnUrl };

		/// <summary>
		/// Creates the failed result.
		/// </summary>
		public static SignOutCommandResult Failed()
			=> new SignOutCommandResult();

		/// <summary>
		/// Gets a value indicating whether this <see cref="SignOutCommandResult"/> is success.
		/// </summary>
		/// <value>
		///   <c>true</c> if success; otherwise, <c>false</c>.
		/// </value>
		public bool Success { get; init; }

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
		/// Initializes a new instance of the <see cref="SignOutCommandResult"/> class.
		/// </summary>
		/// <param name="success">if set to <c>true</c> [succeeded].</param>
		public SignOutCommandResult()
			=> this.Errors = new List<KeyValuePair<string, string>>();
	}
}
