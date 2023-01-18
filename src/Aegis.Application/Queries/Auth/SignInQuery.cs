namespace Aegis.Application.Queries.Auth
{
	using System.Runtime.Serialization;

	using Aegis.Application.Contracts.CQRS;
	using Aegis.Models.Shared;

	/// <summary>
	/// SignIn Query
	/// </summary>
	/// <seealso cref="Chimera.Application.Contracts.CQRS.IQuery&lt;Chimera.Models.Authentication.AuthenticationResult&gt;" />
	[DataContract]
	public sealed record SignInQuery : IQuery<AuthenticationResult>
	{
		/// <summary>
		/// Gets or sets the return URL.
		/// </summary>
		/// <value>
		/// The return URL.
		/// </value>
		public string? ReturnUrl { get; init; }

		/// <summary>
		/// Initializes a new instance of the <see cref="SignInQuery"/> class.
		/// </summary>
		public SignInQuery() { }
	}
}
