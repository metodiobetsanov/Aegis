namespace Aegis.Application.Commands.Authentication
{
	using System.Runtime.Serialization;

	using Aegis.Application.Contracts.CQRS;
	using Aegis.Models.Shared;

	/// <summary>
	/// Send Forget Password Command
	/// </summary>
	/// <seealso cref="Aegis.Application.Contracts.CQRS.ICommand&lt;Aegis.Models.Shared.HandlerResult&gt;" />
	/// <seealso cref="MediatR.IRequest&lt;Aegis.Models.Shared.HandlerResult&gt;" />
	/// <seealso cref="MediatR.IBaseRequest" />
	/// <seealso cref="System.IEquatable&lt;Aegis.Application.Commands.Authentication.SendForgetPasswordCommand&gt;" />
	[DataContract]
	public sealed record SendForgetPasswordCommand : ICommand<HandlerResult>
	{
		/// <summary>
		/// Gets the email.
		/// </summary>
		/// <value>
		/// The email.
		/// </value>
		[DataMember]
		public string? Email { get; init; }

		/// <summary>
		/// Initializes a new instance of the <see cref="SendForgetPasswordCommand"/> class.
		/// </summary>
		public SendForgetPasswordCommand() { }
	}
}
