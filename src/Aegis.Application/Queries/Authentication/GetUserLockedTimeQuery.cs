namespace Aegis.Application.Queries.Authentication
{
	using System.Runtime.Serialization;

	using Aegis.Application.Contracts.CQRS;
	using Aegis.Models.Authentication;

	/// <summary>
	/// Get User Locked Time Query
	/// </summary>
	/// <seealso cref="Aegis.Application.Contracts.CQRS.IQuery&lt;Aegis.Models.Authentication.GetUserLockedTimeQueryResult&gt;" />
	/// <seealso cref="MediatR.IRequest&lt;Aegis.Models.Authentication.GetUserLockedTimeQueryResult&gt;" />
	/// <seealso cref="MediatR.IBaseRequest" />
	/// <seealso cref="System.IEquatable&lt;Aegis.Application.Queries.Authentication.Handlers.UserLockedQuery&gt;" />
	[DataContract]
	public sealed record GetUserLockedTimeQuery : IQuery<GetUserLockedTimeQueryResult>
	{
		/// <summary>
		/// Gets or sets the user identifier.
		/// </summary>
		/// <value>
		/// The user identifier.
		/// </value>
		[DataMember]
		public string? UserId { get; set; }
	}
}
