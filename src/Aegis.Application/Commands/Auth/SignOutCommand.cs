namespace Aegis.Application.Commands.Auth
{
	using System.Runtime.Serialization;

	using Aegis.Application.Contracts.CQRS;
	using Aegis.Models.Auth;
	using Aegis.Models.Shared;

	/// <summary>
	/// SignOut Command
	/// </summary>
	/// <seealso cref="Aegis.Application.Contracts.CQRS.ICommand&lt;Aegis.Models.Auth.SignOutCommandResult&gt;" />
	/// <seealso cref="MediatR.IRequest&lt;Aegis.Models.Auth.SignOutCommandResult&gt;" />
	/// <seealso cref="MediatR.IBaseRequest" />
	/// <seealso cref="System.IEquatable&lt;Aegis.Application.Commands.Auth.SignOutCommand&gt;" />
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
		/// Initializes a new instance of the <see cref="SignOutCommand"/> class.
		/// </summary>
		public SignOutCommand() { }
	}
}
