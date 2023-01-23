namespace Aegis.Models.Auth
{
	using Aegis.Models.Shared;

	/// <summary>
	/// SignUp Command Result
	/// </summary>
	/// <seealso cref="Aegis.Models.Shared.BaseResult" />
	/// <seealso cref="System.IEquatable&lt;Aegis.Models.Shared.BaseResult&gt;" />
	/// <seealso cref="System.IEquatable&lt;Aegis.Models.Auth.SignUpCommandResult&gt;" />
	public sealed record SignUpCommandResult : BaseResult
	{
		/// <summary>
		/// Creates the success result.
		/// </summary>
		public static SignUpCommandResult Succeeded(string userId, string? returnUrl)
			=> new SignUpCommandResult { Success = true, UserId = userId, ReturnUrl = returnUrl };

		/// <summary>
		/// Creates the failed result.
		/// </summary>
		public static SignUpCommandResult Failed()
			=> new SignUpCommandResult();

		/// <summary>
		/// Gets the user identifier.
		/// </summary>
		/// <value>
		/// The user identifier.
		/// </value>
		public string? UserId { get; init; }

		/// <summary>
		/// Gets or sets the return URL.
		/// </summary>
		/// <value>
		/// The return URL.
		/// </value>
		public string? ReturnUrl { get; init; }

		/// <summary>
		/// Initializes a new instance of the <see cref="SignUpCommandResult"/> class.
		/// </summary>
		/// <param name="success">if set to <c>true</c> [succeeded].</param>
		public SignUpCommandResult() : base() { }
	}
}
