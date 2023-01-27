namespace Aegis.Application.Commands.Authentication
{
	using System.Runtime.Serialization;

	using Aegis.Application.Contracts.CQRS;
	using Aegis.Models.Authentication;
	using Aegis.Models.Shared;

	/// <summary>
	/// SignOut Command
	/// </summary>
	/// <seealso cref="Aegis.Application.Contracts.CQRS.ICommand&lt;Aegis.Models.Authentication.SignOutCommandResult&gt;" />
	/// <seealso cref="MediatR.IRequest&lt;Aegis.Models.Authentication.SignOutCommandResult&gt;" />
	/// <seealso cref="MediatR.IBaseRequest" />
	/// <seealso cref="System.IEquatable&lt;Aegis.Application.Commands.Authentication.SignOutCommand&gt;" />
	[DataContract]
	public sealed record SignOutCommand : ICommand<SignOutCommandResult>
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
		/// Gets or sets a value indicating whether [forget client].
		/// </summary>
		/// <value>
		///   <c>true</c> if [forget client]; otherwise, <c>false</c>.
		/// </value>
		[DataMember]
		public bool ForgetClient { get; init; }

		/// <summary>
		/// Gets or sets a value indicating whether [sign out all sessions].
		/// </summary>
		/// <value>
		///   <c>true</c> if [sign out all sessions]; otherwise, <c>false</c>.
		/// </value>
		[DataMember]
		public bool SignOutAllSessions { get; init; }

		/// <summary>
		/// Initializes a new instance of the <see cref="SignOutCommand"/> class.
		/// </summary>
		public SignOutCommand() { }
	}
}
