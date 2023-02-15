namespace Aegis.Core.Queries.Authentication
{
	using System.Runtime.Serialization;

	using Aegis.Core.Contracts.CQRS;
	using Aegis.Models.Shared;

	using Microsoft.AspNetCore.Authentication;

	/// <summary>
	/// Send Code Query
	/// </summary>
	/// <seealso cref="Aegis.Core.Contracts.CQRS.IQuery&lt;Aegis.Models.Shared.HandlerResult&gt;" />
	/// <seealso cref="MediatR.IRequest&lt;Aegis.Models.Shared.HandlerResult&gt;" />
	/// <seealso cref="MediatR.IBaseRequest" />
	/// <seealso cref="System.IEquatable&lt;Aegis.Core.Queries.Authentication.SendCodeQuery&gt;" />
	[DataContract]
	public sealed record SendCodeQuery : IQuery<HandlerResult>
	{
		/// <summary>
		/// Gets the user.
		/// </summary>
		/// <value>
		/// The user.
		/// </value>
		[DataMember]
		public string? UserId { get; init; }

		/// <summary>
		/// Initializes a new instance of the <see cref="SendCodeQuery"/> class.
		/// </summary>
		public SendCodeQuery() { }
	}
}
