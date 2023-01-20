namespace Aegis.Application.Queries.Auth
{
	using System;

	using Aegis.Application.Contracts.CQRS;
	using Aegis.Models.Auth;

	/// <summary>
	/// Send Confirmation Email Query
	/// </summary>
	/// <seealso cref="Aegis.Application.Contracts.CQRS.IQuery&lt;Aegis.Models.Auth.EmailConfirmationQueryResult&gt;" />
	/// <seealso cref="MediatR.IRequest&lt;Aegis.Models.Auth.EmailConfirmationQueryResult&gt;" />
	/// <seealso cref="MediatR.IBaseRequest" />
	/// <seealso cref="System.IEquatable&lt;Aegis.Application.Queries.Auth.EmailConfirmationQuery&gt;" />
	public sealed record EmailConfirmationQuery : IQuery<EmailConfirmationQueryResult>
	{
		/// <summary>
		/// Gets the user identifier.
		/// </summary>
		/// <value>
		/// The user identifier.
		/// </value>
		public string? UserId { get; init; }

		/// <summary>
		/// Initializes a new instance of the <see cref="EmailConfirmationQuery"/> class.
		/// </summary>
		public EmailConfirmationQuery() { }
	}
}
