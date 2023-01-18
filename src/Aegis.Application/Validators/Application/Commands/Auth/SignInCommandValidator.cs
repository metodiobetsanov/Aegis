namespace Aegis.Application.Validators.Application.Commands.Auth
{
	using Aegis.Application.Commands.Auth;

	using FluentValidation;

	/// <summary>
	/// SignIn Command Validator
	/// </summary>
	/// <seealso cref="FluentValidation.AbstractValidator&lt;Aegis.Application.Commands.Auth.SignInCommand&gt;" />
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
