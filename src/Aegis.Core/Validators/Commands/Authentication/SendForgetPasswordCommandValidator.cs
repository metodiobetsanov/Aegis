﻿#region copyright
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
	/// Send Forget Password Command Validator
	/// </summary>
	/// <seealso cref="FluentValidation.AbstractValidator&lt;Aegis.Core.Commands.Authentication.SendForgetPasswordCommand&gt;" />
	public sealed class SendForgetPasswordCommandValidator : AbstractValidator<SendForgetPasswordCommand>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SendForgetPasswordCommandValidator"/> class.
		/// </summary>
		public SendForgetPasswordCommandValidator()
		{
			this.RuleFor(x => x.Email)
				.NotNull().WithMessage("This field is required!")
				.NotEmpty().WithMessage("This field is required!")
				.EmailAddress().WithMessage("Provide valid email address!");
		}
	}
}
