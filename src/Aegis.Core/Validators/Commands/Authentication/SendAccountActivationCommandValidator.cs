namespace Aegis.Core.Validators.Commands.Authentication
{
	using Aegis.Core.Commands.Authentication;

	using FluentValidation;

	/// <summary>
	/// Send Account Activation Command Validator
	/// </summary>
	/// <seealso cref="FluentValidation.AbstractValidator&lt;Aegis.Core.Commands.Authentication.SendAccountActivationCommand&gt;" />
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
