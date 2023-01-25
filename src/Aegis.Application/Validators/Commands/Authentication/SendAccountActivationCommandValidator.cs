namespace Aegis.Application.Validators.Commands.Authentication
{
	using Aegis.Application.Commands.Authentication;

	using FluentValidation;

	/// <summary>
	/// Send Account Activation Command Validator
	/// </summary>
	/// <seealso cref="FluentValidation.AbstractValidator&lt;Aegis.Application.Commands.Authentication.SendAccountActivationCommand&gt;" />
	public sealed class SendAccountActivationCommandValidator : AbstractValidator<SendAccountActivationCommand>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SendAccountActivationCommandValidator"/> class.
		/// </summary>
		public SendAccountActivationCommandValidator()
		{
			this.RuleFor(x => x.UserId)
				.NotNull().WithMessage("This field is required!")
				.NotEmpty().WithMessage("This field is required!");
		}
	}
}
