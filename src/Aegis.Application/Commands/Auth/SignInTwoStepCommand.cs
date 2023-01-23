namespace Aegis.Application.Commands.Auth
{
	using System.Runtime.Serialization;

	using Aegis.Application.Contracts.CQRS;
	using Aegis.Models.Auth;

	/// <summary>
	/// SignIn Two Step Command
	/// </summary>
	/// <seealso cref="Aegis.Application.Contracts.CQRS.ICommand&lt;Aegis.Models.Auth.SignInTwoStepCommandResult&gt;" />
	/// <seealso cref="MediatR.IRequest&lt;Aegis.Models.Auth.SignInTwoStepCommandResult&gt;" />
	/// <seealso cref="MediatR.IBaseRequest" />
	/// <seealso cref="System.IEquatable&lt;Aegis.Application.Commands.Auth.SignInTwoStepCommand&gt;" />
	/// <seealso cref="Aegis.Application.Contracts.CQRS.ICommand&lt;SignInTwoStepCommandResult&gt;" />
	[DataContract]
	public sealed record SignInTwoStepCommand : ICommand<SignInTwoStepCommandResult>
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
