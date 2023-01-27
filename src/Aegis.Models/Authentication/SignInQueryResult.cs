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
	/// SignIn Query Result
	/// </summary>
	/// <seealso cref="Aegis.Models.Shared.BaseResult" />
	/// <seealso cref="System.IEquatable&lt;Aegis.Models.Shared.BaseResult&gt;" />
	/// <seealso cref="System.IEquatable&lt;Aegis.Models.Authentication.SignInQueryResult&gt;" />
	public sealed record SignInQueryResult : BaseResult
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
		public SignInQueryResult() : base() { }
	}
}
