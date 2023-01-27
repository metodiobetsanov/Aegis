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
	/// SignOut Query
	/// </summary>
	/// <seealso cref="Aegis.Core.Contracts.CQRS.IQuery&lt;Aegis.Models.Authentication.SignOutQueryResult&gt;" />
	/// <seealso cref="MediatR.IRequest&lt;Aegis.Models.Authentication.SignOutQueryResult&gt;" />
	/// <seealso cref="MediatR.IBaseRequest" />
	/// <seealso cref="System.IEquatable&lt;Aegis.Core.Queries.Authentication.SignOutQuery&gt;" />
	[DataContract]
	public sealed record SignOutQuery : IQuery<SignOutQueryResult>
	{
		/// <summary>
		/// Gets or sets the logout identifier.
		/// </summary>
		/// <value>
		/// The logout identifier.
		/// </value>
		[DataMember]
		public string? LogoutId { get; init; }

		/// <summary>
		/// Initializes a new instance of the <see cref="SignOutQuery"/> class.
		/// </summary>
		public SignOutQuery() { }
	}
}
