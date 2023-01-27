#region copyright
//----------------------------------------------------------------------
// Copyright 2023 MNB Software
// Licensed under the Apache License, Version 2.0
// You may obtain a copy at http://www.apache.org/licenses/LICENSE-2.0
//----------------------------------------------------------------------
#endregion

namespace Aegis.Core.Queries.Authentication
{
	using System.Runtime.Serialization;

	using Aegis.Core.Contracts.CQRS;
	using Aegis.Models.Authentication;

	/// <summary>
	/// Sign In Two Step Query
	/// </summary>
	/// <seealso cref="Aegis.Core.Contracts.CQRS.IQuery&lt;Aegis.Models.Authentication.SignInQueryResult&gt;" />
	/// <seealso cref="MediatR.IRequest&lt;Aegis.Models.Authentication.SignInQueryResult&gt;" />
	/// <seealso cref="MediatR.IBaseRequest" />
	/// <seealso cref="System.IEquatable&lt;Aegis.Core.Queries.Authentication.SignInTwoStepQuery&gt;" />
	[DataContract]
	public sealed record SignInTwoStepQuery : IQuery<SignInQueryResult>
	{
		/// <summary>
		/// Gets or sets a value indicating whether [remember me].
		/// </summary>
		/// <value>
		///  <c>true</c> if [remember me]; otherwise, <c>false</c>.
		/// </value>
		[DataMember]
		public bool RememberMe { get; init; }

		/// <summary>
		/// Gets or sets the return URL.
		/// </summary>
		/// <value>
		/// The return URL.
		/// </value>
		[DataMember]
		public string? ReturnUrl { get; init; }

		/// <summary>
		/// Initializes a new instance of the <see cref="SignInTwoStepQuery"/> class.
		/// </summary>
		public SignInTwoStepQuery() { }
	}
}
