namespace Aegis.Application.Queries.Auth
{
	using System.Runtime.Serialization;

	using Aegis.Application.Contracts.CQRS;
	using Aegis.Models.Shared;

	/// <summary>
	/// SignOut Query
	/// </summary>
	[DataContract]
	public sealed record SignOutQuery : IQuery<AuthenticationResult>
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
