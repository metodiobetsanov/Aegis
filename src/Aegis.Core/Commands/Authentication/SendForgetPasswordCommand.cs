#region copyright
//----------------------------------------------------------------------
// Copyright 2023 MNB Software
// Licensed under the Apache License, Version 2.0
// You may obtain a copy at http://www.apache.org/licenses/LICENSE-2.0
//----------------------------------------------------------------------
#endregion

namespace Aegis.Core.Commands.Authentication
{
	using System.Runtime.Serialization;

	using Aegis.Core.Contracts.CQRS;
	using Aegis.Models.Shared;

	/// <summary>
	/// Send Forget Password Command
	/// </summary>
	/// <seealso cref="Aegis.Core.Contracts.CQRS.ICommand&lt;Aegis.Models.Shared.HandlerResult&gt;" />
	/// <seealso cref="MediatR.IRequest&lt;Aegis.Models.Shared.HandlerResult&gt;" />
	/// <seealso cref="MediatR.IBaseRequest" />
	/// <seealso cref="System.IEquatable&lt;Aegis.Core.Commands.Authentication.SendForgetPasswordCommand&gt;" />
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
