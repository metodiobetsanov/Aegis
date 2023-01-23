namespace Aegis.UnitTests.Application.Validators.Settings
{
	using global::Aegis.Application.Validators.Settings;
	using global::Aegis.Models.Settings;

	public class IdentityProviderSettingsValidatorTests
	{
		private static readonly Faker _faker = new Faker("en");

		public static TheoryData<IdentityProviderSettings> IdentityProviderSettingsValues => new TheoryData<IdentityProviderSettings>()
		{
			{ new IdentityProviderSettings { } },
			{ new IdentityProviderSettings { LookupProtectorSigningDerivationPassword = _faker.Internet.Password(8, false, "\\w", "!Aa0") } },
			{ new IdentityProviderSettings { LookupProtectorEncryptionDerivationPassword = _faker.Internet.Password(8, false, "\\w", "!Aa0") } },
			{ new IdentityProviderSettings { LookupProtectorEncryptionDerivationPassword = "" ,LookupProtectorSigningDerivationPassword = _faker.Internet.Password(8, false, "\\w", "!Aa0") } },
			{ new IdentityProviderSettings { LookupProtectorEncryptionDerivationPassword = "  " ,LookupProtectorSigningDerivationPassword = _faker.Internet.Password(8, false, "\\w", "!Aa0") } },
			{ new IdentityProviderSettings { LookupProtectorEncryptionDerivationPassword = _faker.Internet.Password(8, false, "\\w", "!Aa0") ,LookupProtectorSigningDerivationPassword = "" } },
			{ new IdentityProviderSettings { LookupProtectorEncryptionDerivationPassword = _faker.Internet.Password(8, false, "\\w", "!Aa0") ,LookupProtectorSigningDerivationPassword = "   " } },
			{ new IdentityProviderSettings { LookupProtectorEncryptionDerivationPassword = "Aa0@" ,LookupProtectorSigningDerivationPassword = _faker.Internet.Password(8, false, "\\w", "!Aa0") } },
			{ new IdentityProviderSettings { LookupProtectorEncryptionDerivationPassword = new string('a', 32) ,LookupProtectorSigningDerivationPassword = _faker.Internet.Password(8, false, "\\w", "!Aa0") } },
			{ new IdentityProviderSettings { LookupProtectorEncryptionDerivationPassword = new string('A', 32) ,LookupProtectorSigningDerivationPassword = _faker.Internet.Password(8, false, "\\w", "!Aa0") } },
			{ new IdentityProviderSettings { LookupProtectorEncryptionDerivationPassword = new string('0', 32) ,LookupProtectorSigningDerivationPassword = _faker.Internet.Password(8, false, "\\w", "!Aa0") } },
			{ new IdentityProviderSettings { LookupProtectorEncryptionDerivationPassword = new string('@', 32) ,LookupProtectorSigningDerivationPassword = _faker.Internet.Password(8, false, "\\w", "!Aa0") } },
			{ new IdentityProviderSettings { LookupProtectorEncryptionDerivationPassword = _faker.Internet.Password(8, false, "\\w", "!Aa0") ,LookupProtectorSigningDerivationPassword = "Aa0@" } },
			{ new IdentityProviderSettings { LookupProtectorEncryptionDerivationPassword = _faker.Internet.Password(8, false, "\\w", "!Aa0") ,LookupProtectorSigningDerivationPassword = new string('a', 32) } },
			{ new IdentityProviderSettings { LookupProtectorEncryptionDerivationPassword = _faker.Internet.Password(8, false, "\\w", "!Aa0") ,LookupProtectorSigningDerivationPassword = new string('A', 32) } },
			{ new IdentityProviderSettings { LookupProtectorEncryptionDerivationPassword = _faker.Internet.Password(8, false, "\\w", "!Aa0") ,LookupProtectorSigningDerivationPassword = new string('0', 32) } },
			{ new IdentityProviderSettings { LookupProtectorEncryptionDerivationPassword = _faker.Internet.Password(8, false, "\\w", "!Aa0") ,LookupProtectorSigningDerivationPassword = new string('@', 32) } },
		};

		[Fact]
		public void Validate_ShouldBeTrue()
		{
			// Arrange
			IdentityProviderSettings settings = new IdentityProviderSettings
			{
				LookupProtectorEncryptionDerivationPassword = _faker.Internet.Password(32, false, "\\w", "!Aa0"),
				LookupProtectorSigningDerivationPassword = _faker.Internet.Password(32, false, "\\w", "!Aa0")
			};
			IdentityProviderSettingsValidator validator = new IdentityProviderSettingsValidator();

			// Act
			FluentValidation.Results.ValidationResult result = validator.Validate(settings);

			// Assert
			IdentityProviderSettings.Section.ShouldBe(nameof(IdentityProviderSettings));
			result.IsValid.ShouldBeTrue();
		}

		[Theory]
		[MemberData(nameof(IdentityProviderSettingsValues))]
		public void Validate_ShouldBeFalse_OnWrongData(IdentityProviderSettings settings)
		{
			// Arrange
			IdentityProviderSettingsValidator validator = new IdentityProviderSettingsValidator();

			// Act
			FluentValidation.Results.ValidationResult result = validator.Validate(settings);

			// Assert
			result.IsValid.ShouldBeFalse();
		}
	}
}
