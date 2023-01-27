namespace Aegis.Application.Validators.Settings
{
	using Aegis.Models.Settings;

	using FluentValidation;

	/// <summary>
	/// SendGrid Settings Validator
	/// </summary>
	/// <seealso cref="AbstractValidator&lt;AppSettings&gt;" />
	public sealed class SendGridSettingsValidator : AbstractValidator<SendGridSettings>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SendGridSettingsValidator"/> class.
		/// </summary>
		public SendGridSettingsValidator()
		{
			this.RuleFor(x => x.ApiKey)
				.NotNull().NotEmpty().WithMessage("'ApiKey' must not be empty!");

			this.RuleFor(x => x.ResetPasswordTemplate)
				.NotNull().NotEmpty().WithMessage("'ResetPasswordTemplate' must not be empty!");

			this.RuleFor(x => x.EmailConfirmationTemplate)
				.NotNull().NotEmpty().WithMessage("'EmailConfirmationTemplate' must not be empty!");

			this.RuleFor(x => x.VerificationCodeTemplate)
				.NotNull().NotEmpty().WithMessage("'VerificationCodeTemplate' must not be empty!");
		}
	}
}
