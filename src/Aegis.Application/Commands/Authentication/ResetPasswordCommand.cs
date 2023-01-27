namespace Aegis.Application.Commands.Authentication
{
	using System.Runtime.Serialization;

	using Aegis.Application.Contracts.CQRS;
	using Aegis.Models.Shared;

	/// <summary>
	/// Reset Password Command
	/// </summary>
	/// <seealso cref="Aegis.Application.Contracts.CQRS.ICommand&lt;Aegis.Models.Shared.HandlerResult&gt;" />
	/// <seealso cref="MediatR.IRequest&lt;Aegis.Models.Shared.HandlerResult&gt;" />
	/// <seealso cref="MediatR.IBaseRequest" />
	/// <seealso cref="System.IEquatable&lt;Aegis.Application.Commands.Authentication.ResetPasswordCommand&gt;" />
	[DataContract]
	public sealed record ResetPasswordCommand : ICommand<HandlerResult>
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
		/// Gets the token.
		/// </summary>
		/// <value>
		/// The token.
		/// </value>
		[DataMember]
		public string? Token { get; init; }

		/// <summary>
		/// Gets the password.
		/// </summary>
		/// <value>
		/// The password.
		/// </value>
		[DataMember]
		public string? Password { get; init; }

		/// <summary>
		/// Gets the confirm password.
		/// </summary>
		/// <value>
		/// The confirm password.
		/// </value>
		[DataMember]
		public string? ConfirmPassword { get; init; }
	}
}
