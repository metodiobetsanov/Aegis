namespace Aegis.UnitTests.Core.Validators.Settings
{
	using global::Aegis.Core.Validators.Settings;
	using global::Aegis.Models.Settings;

	public class SendGridSettingsValidatorTests
	{
		private static readonly Faker _faker = new Faker("en");

		public static TheoryData<SendGridSettings> SendGridSettingsValues => new TheoryData<SendGridSettings>()
		{
			{ new SendGridSettings { } },
			{ new SendGridSettings { ApiKey = _faker.Random.String2(36) } },
			{ new SendGridSettings { EmailConfirmationTemplate = _faker.Random.String2(36) } },
			{ new SendGridSettings { VerificationCodeTemplate = _faker.Random.String2(36) } },
			{ new SendGridSettings { ApiKey = "",  ResetPasswordTemplate = _faker.Random.String2(36), EmailConfirmationTemplate = _faker.Random.String2(36), VerificationCodeTemplate = _faker.Random.String2(36)} },
			{ new SendGridSettings { ApiKey = "   ", ResetPasswordTemplate = _faker.Random.String2(36), EmailConfirmationTemplate = _faker.Random.String2(36), VerificationCodeTemplate = _faker.Random.String2(36)} },
			{ new SendGridSettings { ApiKey = _faker.Random.String2(36), ResetPasswordTemplate = "", EmailConfirmationTemplate = _faker.Random.String2(36), VerificationCodeTemplate = _faker.Random.String2(36)} },
			{ new SendGridSettings { ApiKey = _faker.Random.String2(36), ResetPasswordTemplate = "   ", EmailConfirmationTemplate = _faker.Random.String2(36), VerificationCodeTemplate = _faker.Random.String2(36)} },
			{ new SendGridSettings { ApiKey = _faker.Random.String2(36), ResetPasswordTemplate = _faker.Random.String2(36), EmailConfirmationTemplate = "", VerificationCodeTemplate = _faker.Random.String2(36)} },
			{ new SendGridSettings { ApiKey = _faker.Random.String2(36), ResetPasswordTemplate = _faker.Random.String2(36), EmailConfirmationTemplate = "   ", VerificationCodeTemplate = _faker.Random.String2(36)} },
			{ new SendGridSettings { ApiKey = _faker.Random.String2(36), ResetPasswordTemplate = _faker.Random.String2(36), EmailConfirmationTemplate = _faker.Random.String2(36), VerificationCodeTemplate = "" } },
			{ new SendGridSettings { ApiKey = _faker.Random.String2(36), ResetPasswordTemplate = _faker.Random.String2(36), EmailConfirmationTemplate = _faker.Random.String2(36), VerificationCodeTemplate = "   " } },
		};

		[Fact]
		public void Validate_ShouldBeTrue()
		{
			// Arrange
			SendGridSettings settings = new SendGridSettings
			{
				ApiKey = _faker.Random.String2(36),
				ResetPasswordTemplate = _faker.Random.String2(36),
				EmailConfirmationTemplate = _faker.Random.String2(36),
				VerificationCodeTemplate = _faker.Random.String2(36),
			};
			SendGridSettingsValidator validator = new SendGridSettingsValidator();

			// Act
			FluentValidation.Results.ValidationResult result = validator.Validate(settings);

			// Assert
			SendGridSettings.Section.ShouldBe(nameof(SendGridSettings));
			result.IsValid.ShouldBeTrue();
		}

		[Theory]
		[MemberData(nameof(SendGridSettingsValues))]
		public void Validate_ShouldBeFalse_OnWrongData(SendGridSettings settings)
		{
			// Arrange
			SendGridSettingsValidator validator = new SendGridSettingsValidator();

			// Act
			FluentValidation.Results.ValidationResult result = validator.Validate(settings);

			// Assert
			result.IsValid.ShouldBeFalse();
		}
	}
}
