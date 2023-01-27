namespace Aegis.Core.Validators.Settings
{
	using Aegis.Models.Settings;

	using FluentValidation;

	/// <summary>
	/// Identity Provider Settings Validator
	/// </summary>
	/// <seealso cref="AbstractValidator&lt;IdentityProviderSettings&gt;" />
	public sealed class IdentityProviderSettingsValidator : AbstractValidator<IdentityProviderSettings>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="IdentityProviderSettingsValidator"/> class.
		/// </summary>
		public IdentityProviderSettingsValidator()
		{
			this.RuleFor(x => x.LookupProtectorEncryptionDerivationPassword)
				.NotNull().NotEmpty().WithMessage("'LookupProtectorEncryptionDerivationPassword' must not be empty!")
				.MinimumLength(32).WithMessage("'LookupProtectorEncryptionDerivationPassword' must be 32 chars or more!")
				.Matches("[A-Z]").WithMessage("'LookupProtectorEncryptionDerivationPassword' must contain at least one Upper case letter!")
				.Matches("[a-z]").WithMessage("'LookupProtectorEncryptionDerivationPassword' must contain at least one Lower case letter!")
				.Matches("[0-9]").WithMessage("'LookupProtectorEncryptionDerivationPassword' must contain at least one Digit!")
				.Matches("[!@#$%^&*()-_=+<,>.]").WithMessage("'LookupProtectorEncryptionDerivationPassword' must contain at least one special character!");

			this.RuleFor(x => x.LookupProtectorSigningDerivationPassword)
				.NotNull().NotEmpty().WithMessage("'LookupProtectorSigningDerivationPassword' must not be empty!")
				.MinimumLength(32).WithMessage("'LookupProtectorSigningDerivationPassword' must be 32 chars or more!")
				.Matches("[A-Z]").WithMessage("'LookupProtectorSigningDerivationPassword' must contain at least one Upper case letter!")
				.Matches("[a-z]").WithMessage("'LookupProtectorSigningDerivationPassword' must contain at least one Lower case letter!")
				.Matches("[0-9]").WithMessage("'LookupProtectorSigningDerivationPassword' must contain at least one Digit!")
				.Matches("[!@#$%^&*()-_=+<,>.]").WithMessage("'LookupProtectorSigningDerivationPassword' must contain at least one special character!");
		}
	}
}
