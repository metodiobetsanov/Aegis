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
	/// Sign In Two Step Command Validator
	/// </summary>
	/// <seealso cref="FluentValidation.AbstractValidator&lt;Aegis.Core.Commands.Authentication.SignInTwoStepCommand&gt;" />
	public sealed class SignInTwoStepCommandValidator : AbstractValidator<SignInTwoStepCommand>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SignInTwoStepCommandValidator"/> class.
		/// </summary>
		public SignInTwoStepCommandValidator()
		{
			this.RuleFor(x => x.Code)
				.NotNull().WithMessage("This field is required!")
				.NotEmpty().WithMessage("This field is required!");
		}
	}
}
