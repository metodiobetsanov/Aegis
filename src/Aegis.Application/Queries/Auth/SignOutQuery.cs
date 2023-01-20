namespace Aegis.Application.Queries.Auth
{
	using System.Runtime.Serialization;

	using Aegis.Application.Contracts.CQRS;
	using Aegis.Models.Auth;

	/// <summary>
	/// SignOut Query
	/// </summary>
	/// <seealso cref="Aegis.Application.Contracts.CQRS.IQuery&lt;Aegis.Models.Auth.SignOutQueryResult&gt;" />
	/// <seealso cref="MediatR.IRequest&lt;Aegis.Models.Auth.SignOutQueryResult&gt;" />
	/// <seealso cref="MediatR.IBaseRequest" />
	/// <seealso cref="System.IEquatable&lt;Aegis.Application.Queries.Auth.SignOutQuery&gt;" />
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
