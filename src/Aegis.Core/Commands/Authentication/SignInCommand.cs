namespace Aegis.Core.Commands.Authentication
{
	using System.Runtime.Serialization;

	using Aegis.Core.Contracts.CQRS;
	using Aegis.Models.Authentication;

	using Newtonsoft.Json;

	/// <summary>
	/// SignIn Command
	/// </summary>
	/// <seealso cref="Aegis.Core.Contracts.CQRS.IQuery&lt;Aegis.Models.Authentication.SignInCommandResult&gt;" />
	/// <seealso cref="MediatR.IRequest&lt;Aegis.Models.Authentication.SignInCommandResult&gt;" />
	/// <seealso cref="MediatR.IBaseRequest" />
	/// <seealso cref="System.IEquatable&lt;Aegis.Core.Commands.Authentication.SignInCommand&gt;" />
	[DataContract]
	public sealed record SignInCommand : ICommand<SignInCommandResult>
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
		/// Initializes a new instance of the <see cref="SignInCommand"/> class.
		/// </summary>
		[JsonConstructor]
		public SignInCommand() { }
	}
}
