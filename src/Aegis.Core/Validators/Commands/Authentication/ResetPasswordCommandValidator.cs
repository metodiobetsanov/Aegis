namespace Aegis.Core.Validators.Commands.Authentication
{
	using Aegis.Core.Commands.Authentication;

	using FluentValidation;

	/// <summary>
	/// 
	/// </summary>
	/// <seealso cref="FluentValidation.AbstractValidator&lt;Aegis.Core.Commands.Authentication.ResetPasswordCommand&gt;" />
	public sealed class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
	{
		public ResetPasswordCommandValidator()
		{
			this.RuleFor(x => x.UserId)
				.NotNull().WithMessage("Missing or Invalid data!")
				.NotEmpty().WithMessage("Missing or Invalid data!");

			this.RuleFor(x => x.Token)
				.NotNull().WithMessage("Missing or Invalid data!")
				.NotEmpty().WithMessage("Missing or Invalid data!");

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
		}
	}
}
