#region copyright
//----------------------------------------------------------------------
// Copyright 2023 MNB Software
// Licensed under the Apache License, Version 2.0
// You may obtain a copy at http://www.apache.org/licenses/LICENSE-2.0
//----------------------------------------------------------------------
#endregion
namespace Aegis.UnitTests.Core.Validators.Settings
{
	using global::Aegis.Core.Validators.Settings;
	using global::Aegis.Models.Settings;

	public class AppSettingsValidatorTests
	{
		private static readonly Faker _faker = new Faker("en");

		public static TheoryData<AppSettings> AppSettingsValues => new TheoryData<AppSettings>()
		{
			{ new AppSettings { } },
			{ new AppSettings { PublicDomain = _faker.Internet.DomainName() } },
			{ new AppSettings { PublicDomain = _faker.Internet.DomainName(), DataProtectionCertificateLocation = _faker.System.FilePath() } },
			{ new AppSettings { PublicDomain = _faker.Internet.DomainName(), DataProtectionCertificatePassword = _faker.Internet.Password(8, false, "\\w", "!Aa0") } },
			{ new AppSettings { PublicDomain = "", DataProtectionCertificateLocation = _faker.System.FilePath(), DataProtectionCertificatePassword = _faker.Internet.Password(8, false, "\\w", "!Aa0")} },
			{ new AppSettings { PublicDomain = "  ", DataProtectionCertificateLocation = _faker.System.FilePath(), DataProtectionCertificatePassword = _faker.Internet.Password(8, false, "\\w", "!Aa0")} },
			{ new AppSettings { PublicDomain = _faker.Internet.DomainName(), DataProtectionCertificateLocation = "", DataProtectionCertificatePassword = _faker.Internet.Password(8, false, "\\w", "!Aa0")} },
			{ new AppSettings { PublicDomain = _faker.Internet.DomainName(), DataProtectionCertificateLocation = "  ", DataProtectionCertificatePassword = _faker.Internet.Password(8, false, "\\w", "!Aa0") } },
			{ new AppSettings { PublicDomain = _faker.Internet.DomainName(), DataProtectionCertificateLocation = _faker.System.FilePath(), DataProtectionCertificatePassword = "" } },
			{ new AppSettings { PublicDomain = _faker.Internet.DomainName(), DataProtectionCertificateLocation = _faker.System.FilePath(), DataProtectionCertificatePassword = "  " } },
			{ new AppSettings { PublicDomain = _faker.Internet.DomainName(), DataProtectionCertificateLocation = _faker.System.FilePath(), DataProtectionCertificatePassword = "Aa0@" } },
			{ new AppSettings { PublicDomain = _faker.Internet.DomainName(), DataProtectionCertificateLocation = _faker.System.FilePath(), DataProtectionCertificatePassword = new string('a', 32) } },
			{ new AppSettings { PublicDomain = _faker.Internet.DomainName(), DataProtectionCertificateLocation = _faker.System.FilePath(), DataProtectionCertificatePassword = new string('A', 32) } },
			{ new AppSettings { PublicDomain = _faker.Internet.DomainName(), DataProtectionCertificateLocation = _faker.System.FilePath(), DataProtectionCertificatePassword = new string('0', 32) } },
			{ new AppSettings { PublicDomain = _faker.Internet.DomainName(), DataProtectionCertificateLocation = _faker.System.FilePath(), DataProtectionCertificatePassword = new string('@', 32) } },
		};

		[Fact]
		public void Validate_ShouldBeTrue()
		{
			// Arrange
			AppSettings settings = new AppSettings
			{
				PublicDomain = _faker.Internet.DomainName(),
				DataProtectionCertificateLocation = _faker.System.FilePath(),
				DataProtectionCertificatePassword = _faker.Internet.Password(32, false, "\\w", "!Aa0")
			};
			AppSettingsValidator validator = new AppSettingsValidator();

			// Act
			FluentValidation.Results.ValidationResult result = validator.Validate(settings);

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
			FluentValidation.Results.ValidationResult result = validator.Validate(settings);

			// Assert
			result.IsValid.ShouldBeFalse();
		}
	}
}
