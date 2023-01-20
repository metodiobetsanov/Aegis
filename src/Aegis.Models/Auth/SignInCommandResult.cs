namespace Aegis.Models.Auth
{
	/// <summary>
	/// SignIn Command Result
	/// </summary>
	/// <seealso cref="System.IEquatable&lt;Aegis.Models.Auth.SignInCommandResult&gt;" />
	public sealed record SignInCommandResult
	{
		/// <summary>
		/// Creates the success result.
		/// </summary>
		public static SignInCommandResult Succeeded(string? returnUrl)
			=> new SignInCommandResult { Success = true, ReturnUrl = returnUrl };

		/// <summary>
		/// Creates the failed result.
		/// </summary>
		public static SignInCommandResult Failed()
			=> new SignInCommandResult();

		/// <summary>
		/// Creates the two step is required result.
		/// </summary>
		public static SignInCommandResult TwoStepRequired(Guid userid)
			=> new SignInCommandResult { RequiresTwoStep = true, UserId = userid };

		/// <summary>
		/// Creates the account not active result.
		/// </summary>
		public static SignInCommandResult NotActiveAccount(Guid userid)
			=> new SignInCommandResult { AccounNotActive = true, UserId = userid };

		/// <summary>
		/// Creates the account is locked result.
		/// </summary>
		public static SignInCommandResult LockedAccount(Guid userid)
			=> new SignInCommandResult { AccounLocked = true, UserId = userid };

		/// <summary>
		/// Gets a value indicating whether this <see cref="SignInQueryResult"/> is success.
		/// </summary>
		/// <value>
		///   <c>true</c> if success; otherwise, <c>false</c>.
		/// </value>
		public bool Success { get; init; }

		/// <summary>
		/// Gets a value indicating whether [requires two step].
		/// </summary>
		/// <value>
		///   <c>true</c> if [requires two step]; otherwise, <c>false</c>.
		/// </value>
		public bool RequiresTwoStep { get; init; }

		/// <summary>
		/// Gets a value indicating whether [account not active].
		/// </summary>
		/// <value>
		///   <c>true</c> if [account not active]; otherwise, <c>false</c>.
		/// </value>
		public bool AccounNotActive { get; init; }

		/// <summary>
		/// Gets a value indicating whether [account not active].
		/// </summary>
		/// <value>
		///   <c>true</c> if [account not active]; otherwise, <c>false</c>.
		/// </value>
		public bool AccounLocked { get; init; }

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
		/// Initializes a new instance of the <see cref="SignInCommandResult"/> class.
		/// </summary>
		public SignInCommandResult()
			=> this.Errors = new List<KeyValuePair<string, string>>();
	}
}
