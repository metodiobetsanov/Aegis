namespace Aegis.Application.Validators.Commands.Authentication
{
	using Aegis.Application.Commands.Authentication;

	using FluentValidation;

	/// <summary>
	/// Sign In Two Step Command Validator
	/// </summary>
	/// <seealso cref="FluentValidation.AbstractValidator&lt;Aegis.Application.Commands.Authentication.SignInTwoStepCommand&gt;" />
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
