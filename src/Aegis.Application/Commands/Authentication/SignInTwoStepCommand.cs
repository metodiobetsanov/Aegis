namespace Aegis.Application.Commands.Authentication
{
	using System.Runtime.Serialization;

	using Aegis.Application.Contracts.CQRS;
	using Aegis.Models.Authentication;

	/// <summary>
	/// SignIn Two Step Command
	/// </summary>
	/// <seealso cref="Aegis.Application.Contracts.CQRS.ICommand&lt;Aegis.Models.Authentication.SignInCommandResult&gt;" />
	/// <seealso cref="MediatR.IRequest&lt;Aegis.Models.Authentication.SignInCommandResult&gt;" />
	/// <seealso cref="MediatR.IBaseRequest" />
	/// <seealso cref="System.IEquatable&lt;Aegis.Application.Commands.Authentication.SignInTwoStepCommand&gt;" />
	[DataContract]
	public sealed record SignInTwoStepCommand : ICommand<SignInCommandResult>
	{
		/// <summary>
		/// Gets the code.
		/// </summary>
		/// <value>
		/// The code.
		/// </value>
		[DataMember]
		public string? Code { get; init; }

		/// <summary>
		/// Gets a value indicating whether [remember me].
		/// </summary>
		/// <value>
		///   <c>true</c> if [remember me]; otherwise, <c>false</c>.
		/// </value>
		[DataMember]
		public bool RememberMe { get; init; }

		/// <summary>
		/// Gets a value indicating whether [remember client].
		/// </summary>
		/// <value>
		///   <c>true</c> if [remember client]; otherwise, <c>false</c>.
		/// </value>
		[DataMember]
		public bool RememberClient { get; init; }

		/// <summary>
		/// Gets the return URL.
		/// </summary>
		/// <value>
		/// The return URL.
		/// </value>
		[DataMember]
		public string? ReturnUrl { get; init; }

		/// <summary>
		/// Initializes a new instance of the <see cref="SignInCommand"/> class.
		/// </summary>
		public SignInTwoStepCommand() { }
	}
}
