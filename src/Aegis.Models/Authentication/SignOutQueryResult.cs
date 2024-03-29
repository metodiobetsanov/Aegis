﻿#region copyright
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
	/// SignOut Query Result
	/// </summary>
	/// <seealso cref="Aegis.Models.Shared.BaseResult" />
	/// <seealso cref="System.IEquatable&lt;Aegis.Models.Shared.BaseResult&gt;" />
	/// <seealso cref="System.IEquatable&lt;Aegis.Models.Authentication.SignOutQueryResult&gt;" />
	public sealed record SignOutQueryResult : BaseResult
	{
		/// <summary>
		/// Creates the success result.
		/// </summary>
		public static SignOutQueryResult Show(bool show)
			=> new SignOutQueryResult { ShowSignoutPrompt = show };

		/// <summary>
		/// Gets a value indicating whether [show signout prompt].
		/// </summary>
		/// <value>
		///  <c>true</c> if [show signout prompt]; otherwise, <c>false</c>.
		/// </value>
		public bool ShowSignoutPrompt { get; init; }

		/// <summary>
		/// Initializes a new instance of the <see cref="SignOutQueryResult"/> class.
		/// </summary>
		public SignOutQueryResult() : base() { }
	}
}
