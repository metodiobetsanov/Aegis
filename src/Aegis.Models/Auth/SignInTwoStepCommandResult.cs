namespace Aegis.Models.Auth
{
	using Aegis.Models.Shared;

	/// <summary>
	/// Sign In Two Step Command Result
	/// </summary>
	/// <seealso cref="Aegis.Models.Shared.BaseResult" />
	/// <seealso cref="System.IEquatable&lt;Aegis.Models.Shared.BaseResult&gt;" />
	/// <seealso cref="System.IEquatable&lt;Aegis.Models.Auth.SignInTwoStepCommandResult&gt;" />
	public sealed record SignInTwoStepCommandResult : BaseResult
	{
		/// <summary>
		/// Creates the success result.
		/// </summary>
		public static SignInTwoStepCommandResult Succeeded(string? returnUrl)
			=> new SignInTwoStepCommandResult { Success = true, ReturnUrl = returnUrl };

		/// <summary>
		/// Creates the failed result.
		/// </summary>
		public static SignInTwoStepCommandResult Failed()
			=> new SignInTwoStepCommandResult();

		/// <summary>
		/// Gets or sets the return URL.
		/// </summary>
		/// <value>
		/// The return URL.
		/// </value>
		public string? ReturnUrl { get; init; }

		/// <summary>
		/// Initializes a new instance of the <see cref="SignInQueryResult"/> class.
		/// </summary>
		/// <param name="success">if set to <c>true</c> [succeeded].</param>
		public SignInTwoStepCommandResult() : base() { }
	}
}
