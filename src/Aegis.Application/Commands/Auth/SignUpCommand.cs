namespace Aegis.Application.Commands.Auth
{
	using System.Runtime.Serialization;

	using Aegis.Application.Contracts.CQRS;
	using Aegis.Models.Auth;
	using Aegis.Models.Shared;

	/// <summary>
	/// SignUp Command
	/// </summary>
	/// <seealso cref="Aegis.Application.Contracts.CQRS.ICommand&lt;Aegis.Models.Auth.SignUpCommandResult&gt;" />
	/// <seealso cref="MediatR.IRequest&lt;Aegis.Models.Auth.SignUpCommandResult&gt;" />
	/// <seealso cref="MediatR.IBaseRequest" />
	/// <seealso cref="System.IEquatable&lt;Aegis.Application.Commands.Auth.SignUpCommand&gt;" />
	[DataContract]
	public sealed record SignUpCommand : ICommand<SignUpCommandResult>
	{
		/// <summary>
		/// Gets or sets the email.
		/// </summary>
		/// <value>
		/// The email.
		/// </value>
		[DataMember]
		public string? Email { get; init; }

		/// <summary>
		/// Gets or sets the password.
		/// </summary>
		/// <value>
		/// The password.
		/// </value>
		[DataMember]
		public string? Password { get; init; }

		/// <summary>
		/// Gets or sets the confirm password.
		/// </summary>
		/// <value>
		/// The confirm password.
		/// </value>
		[DataMember]
		public string? ConfirmPassword { get; init; }

		/// <summary>
		/// Gets or sets a value indicating whether [accept terms].
		/// </summary>
		/// <value>
		///   <c>true</c> if [accept terms]; otherwise, <c>false</c>.
		/// </value>
		[DataMember]
		public bool AcceptTerms { get; init; }

		/// <summary>
		/// Gets or sets the return URL.
		/// </summary>
		/// <value>
		/// The return URL.
		/// </value>
		[DataMember]
		public string? ReturnUrl { get; init; }

		/// <summary>
		/// Initializes a new instance of the <see cref="SignUpCommand"/> class.
		/// </summary>
		public SignUpCommand() { }
	}
}
