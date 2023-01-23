namespace Aegis.Application.Queries.Auth
{
	using System.Runtime.Serialization;

	using Aegis.Application.Contracts.CQRS;
	using Aegis.Models.Auth;

	/// <summary>
	/// SignIn Query
	/// </summary>
	/// <seealso cref="Aegis.Application.Contracts.CQRS.IQuery&lt;Aegis.Models.Auth.SignInQueryResult&gt;" />
	/// <seealso cref="MediatR.IRequest&lt;Aegis.Models.Auth.SignInQueryResult&gt;" />
	/// <seealso cref="MediatR.IBaseRequest" />
	/// <seealso cref="System.IEquatable&lt;Aegis.Application.Queries.Auth.SignInQuery&gt;" />
	[DataContract]
	public sealed record SignInQuery : IQuery<SignInQueryResult>
	{
		/// <summary>
		/// Gets or sets the return URL.
		/// </summary>
		/// <value>
		/// The return URL.
		/// </value>
		[DataMember]
		public string? ReturnUrl { get; init; }

		/// <summary>
		/// Initializes a new instance of the <see cref="SignInQuery"/> class.
		/// </summary>
		public SignInQuery() { }
	}
}
