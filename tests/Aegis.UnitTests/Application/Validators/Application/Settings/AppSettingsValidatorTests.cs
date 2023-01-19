namespace Aegis.UnitTests.Application.Validators.Application.Settings
{
	using FluentValidation.Results;

	using global::Aegis.Application.Validators.Application.Settings;
	using global::Aegis.Models.Settings;

	public class AppSettingsValidatorTests
	{
		public static TheoryData<AppSettings> AppSettingsValues => new TheoryData<AppSettings>()
		{
			{ new AppSettings { } },
			{ new AppSettings { PublicDomain = "test.test.test" } },
			{ new AppSettings { PublicDomain = "test.test.test", DataProtectionCertificateLocation = "c:\\some\\place" } },
			{ new AppSettings { PublicDomain = "test.test.test", DataProtectionCertificateLocation = "c:\\some\\place", DataProtectionCertificatePassword = "AAAaaa000@@@" } },
			{ new AppSettings { PublicDomain = "", DataProtectionCertificateLocation = "c:\\some\\place", DataProtectionCertificatePassword = "AAAaaa000@@@" + new string('a', 20)} },
			{ new AppSettings { PublicDomain = "   ", DataProtectionCertificateLocation = "c:\\some\\place", DataProtectionCertificatePassword = "AAAaaa000@@@" + new string('a', 20)} },
			{ new AppSettings { PublicDomain = "test.test.test", DataProtectionCertificateLocation = "", DataProtectionCertificatePassword = "AAAaaa000@@@" + new string('a', 20)} },
			{ new AppSettings { PublicDomain = "test.test.test", DataProtectionCertificateLocation = "   ", DataProtectionCertificatePassword = "AAAaaa000@@@" + new string('a', 20)} },
			{ new AppSettings { PublicDomain = "test.test.test", DataProtectionCertificateLocation = "c:\\some\\place", DataProtectionCertificatePassword = "" } },
			{ new AppSettings { PublicDomain = "test.test.test", DataProtectionCertificateLocation = "c:\\some\\place", DataProtectionCertificatePassword = "   " } },
			{ new AppSettings { PublicDomain = "test.test.test", DataProtectionCertificateLocation = "c:\\some\\place", DataProtectionCertificatePassword = new string('a', 32) } },
			{ new AppSettings { PublicDomain = "test.test.test", DataProtectionCertificateLocation = "c:\\some\\place", DataProtectionCertificatePassword = new string('A', 32) } },
			{ new AppSettings { PublicDomain = "test.test.test", DataProtectionCertificateLocation = "c:\\some\\place", DataProtectionCertificatePassword = new string('0', 32) } },
			{ new AppSettings { PublicDomain = "test.test.test", DataProtectionCertificateLocation = "c:\\some\\place", DataProtectionCertificatePassword = new string('@', 32) } },
		};


		[Fact]
		public void Validate_ShouldBeTrue()
		{
			// Arrange
			AppSettings settings = new AppSettings { PublicDomain = "test.test.test", DataProtectionCertificateLocation = "c:\\some\\place", DataProtectionCertificatePassword = "AAAaaa000@@@" + new string('a', 20) };
			AppSettingsValidator validator = new AppSettingsValidator();

			// Act
			ValidationResult result = validator.Validate(settings);

			// Assert
			AppSettings.Section.ShouldBe(nameof(AppSettings));
			result.IsValid.ShouldBeTrue();
		}

		[Theory]
		[MemberData(nameof(AppSettingsValues))]
		public void Validate_ShouldBeFalse_OnWrongData(AppSettings settings)
		{
			// Arrange
			AppSettingsValidator validator = new AppSettingsValidator();

			// Act
			ValidationResult result = validator.Validate(settings);

			// Assert
			result.IsValid.ShouldBeFalse();
		}
	}
}
