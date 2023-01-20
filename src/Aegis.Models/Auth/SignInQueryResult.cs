namespace Aegis.Models.Auth
{
	/// <summary>
	/// SignIn Query Result
	/// </summary>
	/// <seealso cref="System.IEquatable&lt;Aegis.Models.Auth.SignInQueryResult&gt;" />
	public sealed record SignInQueryResult
	{
		/// <summary>
		/// Creates the success result.
		/// </summary>
		public static SignInQueryResult Succeeded(string? returnUrl)
			=> new SignInQueryResult { Success = true, ReturnUrl = returnUrl };

		/// <summary>
		/// Creates the failed result.
		/// </summary>
		public static SignInQueryResult Failed()
			=> new SignInQueryResult();

		/// <summary>
		/// Gets a value indicating whether this <see cref="SignInQueryResult"/> is success.
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
		/// Initializes a new instance of the <see cref="SignInQueryResult"/> class.
		/// </summary>
		/// <param name="success">if set to <c>true</c> [succeeded].</param>
		public SignInQueryResult()
			=> this.Errors = new List<KeyValuePair<string, string>>();
	}
}
