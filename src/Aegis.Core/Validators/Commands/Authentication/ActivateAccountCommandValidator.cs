namespace Aegis.Core.Validators.Commands.Authentication
{
	using Aegis.Core.Commands.Authentication;

	using FluentValidation;

	/// <summary>
	/// Activate Account Command Validator
	/// </summary>
	/// <seealso cref="FluentValidation.AbstractValidator&lt;Aegis.Core.Commands.Authentication.ActivateAccountCommand&gt;" />
	public sealed class ActivateAccountCommandValidator : AbstractValidator<ActivateAccountCommand>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ActivateAccountCommandValidator"/> class.
		/// </summary>
		public ActivateAccountCommandValidator()
		{
			this.RuleFor(x => x.UserId)
				.NotNull().WithMessage("This field is required!")
				.NotEmpty().WithMessage("This field is required!");

			this.RuleFor(x => x.Token)
				.NotNull().WithMessage("This field is required!")
				.NotEmpty().WithMessage("This field is required!");
		}
	}
}
