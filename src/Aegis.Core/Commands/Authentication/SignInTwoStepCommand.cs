﻿#region copyright
//----------------------------------------------------------------------
// Copyright 2023 MNB Software
// Licensed under the Apache License, Version 2.0
// You may obtain a copy at http://www.apache.org/licenses/LICENSE-2.0
//----------------------------------------------------------------------
#endregion

namespace Aegis.Core.Commands.Authentication
{
	using System.Runtime.Serialization;

	using Aegis.Core.Contracts.CQRS;
	using Aegis.Models.Authentication;

	/// <summary>
	/// SignIn Two Step Command
	/// </summary>
	/// <seealso cref="Aegis.Core.Contracts.CQRS.ICommand&lt;Aegis.Models.Authentication.SignInCommandResult&gt;" />
	/// <seealso cref="MediatR.IRequest&lt;Aegis.Models.Authentication.SignInCommandResult&gt;" />
	/// <seealso cref="MediatR.IBaseRequest" />
	/// <seealso cref="System.IEquatable&lt;Aegis.Core.Commands.Authentication.SignInTwoStepCommand&gt;" />
	[DataContract]
	public sealed record SignInTwoStepCommand : ICommand<SignInCommandResult>
	{
		/// <summary>
		/// Gets the code.
		/// </summary>
		/// <value>
		/// The code.
		/// </value>
		[DataMember]
		public string? Code { get; init; }

		/// <summary>
		/// Gets a value indicating whether [remember me].
		/// </summary>
		/// <value>
		///  <c>true</c> if [remember me]; otherwise, <c>false</c>.
		/// </value>
		[DataMember]
		public bool RememberMe { get; init; }

		/// <summary>
		/// Gets a value indicating whether [remember client].
		/// </summary>
		/// <value>
		///  <c>true</c> if [remember client]; otherwise, <c>false</c>.
		/// </value>
		[DataMember]
		public bool RememberClient { get; init; }

		/// <summary>
		/// Gets the return URL.
		/// </summary>
		/// <value>
		/// The return URL.
		/// </value>
		[DataMember]
		public string? ReturnUrl { get; init; }

		/// <summary>
		/// Initializes a new instance of the <see cref="SignInCommand"/> class.
		/// </summary>
		public SignInTwoStepCommand() { }
	}
}
