namespace Aegis.Application.Commands.Authentication
{
	using System.Runtime.Serialization;

	using Aegis.Application.Contracts.CQRS;
	using Aegis.Models.Shared;

	/// <summary>
	/// Send Confirmation Email Command
	/// </summary>
	/// <seealso cref="Aegis.Application.Contracts.CQRS.ICommand&lt;Aegis.Models.Shared.HandlerResult&gt;" />
	/// <seealso cref="MediatR.IRequest&lt;Aegis.Models.Shared.HandlerResult&gt;" />
	/// <seealso cref="MediatR.IBaseRequest" />
	/// <seealso cref="System.IEquatable&lt;Aegis.Application.Commands.Authentication.SendAccountActivationCommand&gt;" />
	[DataContract]
	public sealed record SendAccountActivationCommand : ICommand<HandlerResult>
	{
		/// <summary>
		/// Gets the user identifier.
		/// </summary>
		/// <value>
		/// The user identifier.
		/// </value>
		[DataMember]
		public string? UserId { get; init; }

		/// <summary>
		/// Initializes a new instance of the <see cref="EmailConfirmationQuery"/> class.
		/// </summary>]
		public SendAccountActivationCommand() { }
	}
}
