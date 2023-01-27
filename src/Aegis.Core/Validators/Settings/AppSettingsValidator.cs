namespace Aegis.Core.Validators.Settings
{
	using Aegis.Models.Settings;

	using FluentValidation;

	/// <summary>
	/// App Settings Validator
	/// </summary>
	/// <seealso cref="AbstractValidator&lt;AppSettings&gt;" />
	public sealed class AppSettingsValidator : AbstractValidator<AppSettings>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="AppSettingsValidator"/> class.
		/// </summary>
		public AppSettingsValidator()
		{
			this.RuleFor(x => x.PublicDomain)
				.NotNull().NotEmpty().WithMessage("'PublicDomain' must not be empty!");

			this.RuleFor(x => x.DataProtectionCertificateLocation)
				.NotNull().NotEmpty().WithMessage("'PublicDomain' must not be empty!");

			this.RuleFor(x => x.DataProtectionCertificatePassword)
				.NotNull().NotEmpty().WithMessage("'DataProtectionCertificatePassword' must not be empty!")
				.MinimumLength(32).WithMessage("'DataProtectionCertificatePassword' must be 32 chars or more!")
				.Matches("[A-Z]").WithMessage("'DataProtectionCertificatePassword' must contain at least one Upper case letter!")
				.Matches("[a-z]").WithMessage("'DataProtectionCertificatePassword' must contain at least one Lower case letter!")
				.Matches("[0-9]").WithMessage("'DataProtectionCertificatePassword' must contain at least one Digit!")
				.Matches("[!@#$%^&*()-_=+<,>.]").WithMessage("'DataProtectionCertificatePassword' must contain at least one special character!");
		}
	}
}
