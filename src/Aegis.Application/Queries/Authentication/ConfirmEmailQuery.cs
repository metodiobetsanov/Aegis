namespace Aegis.Application.Queries.Authentication
{
	using System.Runtime.Serialization;

	using Aegis.Application.Contracts.CQRS;
	using Aegis.Models.Authentication;

	/// <summary>
	/// Confirm Email Query
	/// </summary>
	/// <seealso cref="Aegis.Application.Contracts.CQRS.IQuery&lt;Aegis.Models.Authentication.EmailConfirmationQueryResult&gt;" />
	/// <seealso cref="MediatR.IRequest&lt;Aegis.Models.Authentication.EmailConfirmationQueryResult&gt;" />
	/// <seealso cref="MediatR.IBaseRequest" />
	/// <seealso cref="System.IEquatable&lt;Aegis.Application.Queries.Authentication.ConfirmEmailQuery&gt;" />
	[DataContract]
	public sealed record ConfirmEmailQuery : IQuery<EmailConfirmationQueryResult>
	{
		/// <summary>
		/// Gets or sets the user identifier.
		/// </summary>
		/// <value>
		/// The user identifier.
		/// </value>
		[DataMember]
		public string? UserId { get; init; }

		/// <summary>
		/// Gets or sets the token.
		/// </summary>
		/// <value>
		/// The token.
		/// </value>
		[DataMember]
		public string? Token { get; init; }

		/// <summary>
		/// Initializes a new instance of the <see cref="ConfirmEmailQuery"/> class.
		/// </summary>
		public ConfirmEmailQuery() { }
	}
}
