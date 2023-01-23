namespace Aegis.Application.Queries.Auth
{
	using System.Runtime.Serialization;

	using Aegis.Application.Contracts.CQRS;
	using Aegis.Models.Auth;

	/// <summary>
	/// Sign In Two Step Query
	/// </summary>
	/// <seealso cref="Aegis.Application.Contracts.CQRS.IQuery&lt;Aegis.Models.Auth.SignInTwoStepQueryResult&gt;" />
	/// <seealso cref="MediatR.IRequest&lt;Aegis.Models.Auth.SignInTwoStepQueryResult&gt;" />
	/// <seealso cref="MediatR.IBaseRequest" />
	/// <seealso cref="System.IEquatable&lt;Aegis.Application.Queries.Auth.SignInTwoStepQuery&gt;" />
	[DataContract]
	public sealed record SignInTwoStepQuery : IQuery<SignInTwoStepQueryResult>
	{
		/// <summary>
		/// Gets or sets a value indicating whether [remember me].
		/// </summary>
		/// <value>
		///   <c>true</c> if [remember me]; otherwise, <c>false</c>.
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
