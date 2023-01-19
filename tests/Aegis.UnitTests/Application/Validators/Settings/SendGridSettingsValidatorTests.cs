namespace Aegis.UnitTests.Application.Validators.Settings
{
	using FluentValidation.Results;

	using global::Aegis.Application.Validators.Settings;
	using global::Aegis.Models.Settings;

	public class SendGridSettingsValidatorTests
	{
		public static TheoryData<SendGridSettings> SendGridSettingsValues => new TheoryData<SendGridSettings>()
		{
			{ new SendGridSettings { } },
			{ new SendGridSettings { ApiKey = "test.test.test" } },
			{ new SendGridSettings { EmailConfirmationTemplate = "test" } },
			{ new SendGridSettings { VerificationCodeTemplate = "test" } },
			{ new SendGridSettings { ApiKey = "", EmailConfirmationTemplate = "test", VerificationCodeTemplate = "test" } },
			{ new SendGridSettings { ApiKey = "   ", EmailConfirmationTemplate = "test", VerificationCodeTemplate = "test" } },
			{ new SendGridSettings { ApiKey = "test.test.test", EmailConfirmationTemplate = "", VerificationCodeTemplate = "test" } },
			{ new SendGridSettings { ApiKey = "test.test.test", EmailConfirmationTemplate = "   ", VerificationCodeTemplate = "test" } },
			{ new SendGridSettings { ApiKey = "test.test.test", EmailConfirmationTemplate = "test", VerificationCodeTemplate = "" } },
			{ new SendGridSettings { ApiKey = "test.test.test", EmailConfirmationTemplate = "test", VerificationCodeTemplate = "   " } },
		};

		[Fact]
		public void Validate_ShouldBeTrue()
		{
			// Arrange
			SendGridSettings settings = new SendGridSettings { ApiKey = "test.test.test", EmailConfirmationTemplate = "test", VerificationCodeTemplate = "test" };
			SendGridSettingsValidator validator = new SendGridSettingsValidator();

			// Act
			ValidationResult result = validator.Validate(settings);

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
			ValidationResult result = validator.Validate(settings);

			// Assert
			result.IsValid.ShouldBeFalse();
		}
	}
}
