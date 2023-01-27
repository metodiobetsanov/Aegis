#region copyright
//----------------------------------------------------------------------
// Copyright 2023 MNB Software
// Licensed under the Apache License, Version 2.0
// You may obtain a copy at http://www.apache.org/licenses/LICENSE-2.0
//----------------------------------------------------------------------
#endregion

namespace Aegis.Models.Authentication
{
	using Aegis.Models.Shared;

	/// <summary>
	/// SignIn Command Result
	/// </summary>
	/// <seealso cref="Aegis.Models.Shared.BaseResult" />
	/// <seealso cref="System.IEquatable&lt;Aegis.Models.Shared.BaseResult&gt;" />
	/// <seealso cref="System.IEquatable&lt;Aegis.Models.Authentication.SignInCommandResult&gt;" />
	public sealed record SignInCommandResult : BaseResult
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
		public static SignInCommandResult TwoStepRequired(string userid)
			=> new SignInCommandResult { RequiresTwoStep = true, UserId = userid };

		/// <summary>
		/// Creates the account not active result.
		/// </summary>
		public static SignInCommandResult NotActiveAccount(string userid)
			=> new SignInCommandResult { AccounNotActive = true, UserId = userid };

		/// <summary>
		/// Creates the account is locked result.
		/// </summary>
		public static SignInCommandResult LockedAccount(string userid)
			=> new SignInCommandResult { AccounLocked = true, UserId = userid };

		/// <summary>
		/// Gets a value indicating whether [requires two step].
		/// </summary>
		/// <value>
		///  <c>true</c> if [requires two step]; otherwise, <c>false</c>.
		/// </value>
		public bool RequiresTwoStep { get; init; }

		/// <summary>
		/// Gets a value indicating whether [account not active].
		/// </summary>
		/// <value>
		///  <c>true</c> if [account not active]; otherwise, <c>false</c>.
		/// </value>
		public bool AccounNotActive { get; init; }

		/// <summary>
		/// Gets a value indicating whether [account not active].
		/// </summary>
		/// <value>
		///  <c>true</c> if [account not active]; otherwise, <c>false</c>.
		/// </value>
		public bool AccounLocked { get; init; }

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
		/// Initializes a new instance of the <see cref="SignInCommandResult"/> class.
		/// </summary>
		public SignInCommandResult() : base() { }
	}
}
