namespace Aegis.Core.Validators.Commands.Authentication
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	using Aegis.Core.Commands.Authentication;

	using FluentValidation;

	/// <summary>
	/// SignUp Command Validator
	/// </summary>
	public sealed class SignUpCommandValidator : AbstractValidator<SignUpCommand>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SignUpCommandValidator"/> class.
		/// </summary>
		public SignUpCommandValidator()
		{
			this.RuleFor(x => x.Email)
				.NotNull().WithMessage("This field is required!")
				.NotEmpty().WithMessage("This field is required!")
				.EmailAddress().WithMessage("Provide valid email address!");

			this.RuleFor(x => x.Password)
				.NotNull().WithMessage("This field is required!")
				.NotEmpty().WithMessage("This field is required!")
				.MinimumLength(8).WithMessage("'Password' must be 8 chars or more!")
				.Matches("[A-Z]").WithMessage("'Password' must contain at least one Upper case letter!")
				.Matches("[a-z]").WithMessage("'Password' must contain at least one Lower case letter!")
				.Matches("[0-9]").WithMessage("'Password' must contain at least one Digit!")
				.Matches("[!@#$%^&*()-_=+<,>.]").WithMessage("'Password' must contain at least one special character!");

			this.RuleFor(x => x.ConfirmPassword)
				.NotNull().WithMessage("This field is required!")
				.NotEmpty().WithMessage("This field is required!")
				.Equal(x => x.Password);

			this.RuleFor(x => x.AcceptTerms)
				.NotNull().WithMessage("This field is required!")
				.NotEmpty().WithMessage("This field is required!")
				.Equal(true);
		}
	}
}
