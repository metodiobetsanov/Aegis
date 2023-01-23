namespace Aegis.Application.Validators.Commands.Auth
{
	using Aegis.Application.Commands.Auth;

	using FluentValidation;

	/// <summary>
	/// Sign In Two Step Command Validator
	/// </summary>
	/// <seealso cref="FluentValidation.AbstractValidator&lt;Aegis.Application.Commands.Auth.SignInTwoStepCommand&gt;" />
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
