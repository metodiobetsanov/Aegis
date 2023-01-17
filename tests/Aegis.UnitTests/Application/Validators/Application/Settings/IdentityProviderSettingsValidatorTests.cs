namespace Aegis.UnitTests.Application.Validators.Application.Settings
{
	using FluentValidation.Results;

	using global::Aegis.Application.Validators.Application.Settings;
	using global::Aegis.Models.Settings;

	public class IdentityProviderSettingsValidatorTests
	{
		public static TheoryData<IdentityProviderSettings> IdentityProviderSettingsValues => new TheoryData<IdentityProviderSettings>()
		{
			{ new IdentityProviderSettings { } },
			{ new IdentityProviderSettings { LookupProtectorSigningDerivationPassword = "AAAaaa000@@@" + new string('a', 20) } },
			{ new IdentityProviderSettings { LookupProtectorEncryptionDerivationPassword = "AAAaaa000@@@" + new string('a', 20) } },
			{ new IdentityProviderSettings { LookupProtectorEncryptionDerivationPassword = new string('a', 32) ,LookupProtectorSigningDerivationPassword = "AAAaaa000@@@" + new string('a', 20) } },
			{ new IdentityProviderSettings { LookupProtectorEncryptionDerivationPassword = new string('A', 32) ,LookupProtectorSigningDerivationPassword = "AAAaaa000@@@" + new string('a', 20) } },
			{ new IdentityProviderSettings { LookupProtectorEncryptionDerivationPassword = new string('0', 32) ,LookupProtectorSigningDerivationPassword = "AAAaaa000@@@" + new string('a', 20) } },
			{ new IdentityProviderSettings { LookupProtectorEncryptionDerivationPassword = new string('@', 32) ,LookupProtectorSigningDerivationPassword = "AAAaaa000@@@" + new string('a', 20) } },
			{ new IdentityProviderSettings { LookupProtectorEncryptionDerivationPassword = "AAAaaa000@@@" + new string('a', 20) ,LookupProtectorSigningDerivationPassword = new string('a', 32) } },
			{ new IdentityProviderSettings { LookupProtectorEncryptionDerivationPassword = "AAAaaa000@@@" + new string('a', 20) ,LookupProtectorSigningDerivationPassword = new string('A', 32) } },
			{ new IdentityProviderSettings { LookupProtectorEncryptionDerivationPassword = "AAAaaa000@@@" + new string('a', 20) ,LookupProtectorSigningDerivationPassword = new string('0', 32) } },
			{ new IdentityProviderSettings { LookupProtectorEncryptionDerivationPassword = "AAAaaa000@@@" + new string('a', 20) ,LookupProtectorSigningDerivationPassword = new string('@', 32) } },
		};

		[Fact]
		public void Validate_ShouldBeTrue()
		{
			// Arrange
			IdentityProviderSettings settings = new IdentityProviderSettings { LookupProtectorEncryptionDerivationPassword = "AAAaaa000@@@" + new string('a', 20), LookupProtectorSigningDerivationPassword = "AAAaaa000@@@" + new string('a', 20) };
			IdentityProviderSettingsValidator validator = new IdentityProviderSettingsValidator();

			// Act
			ValidationResult result = validator.Validate(settings);

			// Assert
			result.IsValid.ShouldBeTrue();
		}

		[Theory]
		[MemberData(nameof(IdentityProviderSettingsValues))]
		public void Validate_ShouldBeFalse_OnWrongData(IdentityProviderSettings settings)
		{
			// Arrange
			IdentityProviderSettingsValidator validator = new IdentityProviderSettingsValidator();

			// Act
			ValidationResult result = validator.Validate(settings);

			// Assert
			result.IsValid.ShouldBeFalse();
		}
	}
}
