namespace Aegis.Application.Validators.Commands.Authentication
{
	using Aegis.Application.Commands.Authentication;

	using FluentValidation;

	/// <summary>
	/// Send Forget Password Command Validator
	/// </summary>
	/// <seealso cref="FluentValidation.AbstractValidator&lt;Aegis.Application.Commands.Authentication.SendForgetPasswordCommand&gt;" />
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
