#region copyright
//----------------------------------------------------------------------
// Copyright 2023 MNB Software
// Licensed under the Apache License, Version 2.0
// You may obtain a copy at http://www.apache.org/licenses/LICENSE-2.0
//----------------------------------------------------------------------
#endregion

namespace Aegis.Core.Validators.Commands.Authentication
{
	using Aegis.Core.Commands.Authentication;

	using FluentValidation;

	/// <summary>
	/// SignIn Command Validator
	/// </summary>
	/// <seealso cref="AbstractValidator&lt;SignInCommand&gt;" />
	public sealed class SignInCommandValidator : AbstractValidator<SignInCommand>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SignInCommandValidator"/> class.
		/// </summary>
		public SignInCommandValidator()
		{
			this.RuleFor(x => x.Email)
				.NotNull().WithMessage("This field is required!")
				.NotEmpty().WithMessage("This field is required!")
				.EmailAddress().WithMessage("Provide valid email address!");

			this.RuleFor(x => x.Password)
				.NotNull().WithMessage("This field is required!")
				.NotEmpty().WithMessage("This field is required!");
		}
	}
}
