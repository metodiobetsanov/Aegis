#region copyright
//----------------------------------------------------------------------
// Copyright 2023 MNB Software
// Licensed under the Apache License, Version 2.0
// You may obtain a copy at http://www.apache.org/licenses/LICENSE-2.0
//----------------------------------------------------------------------
#endregion
namespace Aegis.UnitTests.Core.Validators.Commands.Authentication
{
	using global::Aegis.Core.Commands.Authentication;
	using global::Aegis.Core.Validators.Commands.Authentication;

	public class SignUpCommandValidatorTests
	{
		private static readonly Faker _faker = new Faker("en");

		public static TheoryData<SignUpCommand> SignUpCommandValues => new TheoryData<SignUpCommand>()
		{
			{ new SignUpCommand { } },
			{ new SignUpCommand { Email = _faker.Internet.Email() } },
			{ new SignUpCommand { Password = _faker.Internet.Password(8, false, "\\w", "!Aa0") } },
			{ new SignUpCommand { ConfirmPassword = _faker.Internet.Password(8, false, "\\w", "!Aa0") } },
			{ new SignUpCommand { AcceptTerms = true } },
			{ new SignUpCommand { Email = "", Password = _faker.Internet.Password(8, false, "\\w", "!Aa0"), ConfirmPassword = _faker.Internet.Password(8, false, "\\w", "!Aa0"), AcceptTerms = true } },
			{ new SignUpCommand { Email = "  ", Password = _faker.Internet.Password(8, false, "\\w", "!Aa0"), ConfirmPassword = _faker.Internet.Password(8, false, "\\w", "!Aa0"), AcceptTerms = true } },
			{ new SignUpCommand { Email = "test", Password = _faker.Internet.Password(8, false, "\\w", "!Aa0"), ConfirmPassword = _faker.Internet.Password(8, false, "\\w", "!Aa0"), AcceptTerms = true } },
			{ new SignUpCommand { Email = "test", Password = _faker.Internet.Email(), ConfirmPassword = _faker.Internet.Password(8, false, "\\w", "!Aa0"), AcceptTerms = true } },
			{ new SignUpCommand { Email = _faker.Internet.Email(), Password = "", ConfirmPassword = _faker.Internet.Password(8, false, "\\w", "!Aa0"), AcceptTerms = false } },
			{ new SignUpCommand { Email = _faker.Internet.Email(), Password = "  ", ConfirmPassword = _faker.Internet.Password(8, false, "\\w", "!Aa0"), AcceptTerms = false } },
			{ new SignUpCommand { Email = _faker.Internet.Email(), Password = new string('A', 8), ConfirmPassword = _faker.Internet.Password(8, false, "\\w", "!Aa0"), AcceptTerms = false } },
			{ new SignUpCommand { Email = _faker.Internet.Email(), Password = new string('a', 8), ConfirmPassword = _faker.Internet.Password(8, false, "\\w", "!Aa0"), AcceptTerms = false } },
			{ new SignUpCommand { Email = _faker.Internet.Email(), Password = new string('0', 8), ConfirmPassword = _faker.Internet.Password(8, false, "\\w", "!Aa0"), AcceptTerms = false } },
			{ new SignUpCommand { Email = _faker.Internet.Email(), Password = new string('@', 8), ConfirmPassword = _faker.Internet.Password(8, false, "\\w", "!Aa0"), AcceptTerms = false } },
			{ new SignUpCommand { Email = _faker.Internet.Email(), Password = _faker.Internet.Password(8, false, "\\w", "!Aa0"), ConfirmPassword = "", AcceptTerms = false } },
			{ new SignUpCommand { Email = _faker.Internet.Email(), Password = _faker.Internet.Password(8, false, "\\w", "!Aa0"), ConfirmPassword = "  ", AcceptTerms = false } },
			{ new SignUpCommand { Email = _faker.Internet.Email(), Password = _faker.Internet.Password(8, false, "\\w", "!Aa0"), ConfirmPassword = "test", AcceptTerms = false } },
			{ new SignUpCommand { Email = _faker.Internet.Email(), Password = _faker.Internet.Password(8, false, "\\w", "!Aa0"), ConfirmPassword = _faker.Internet.Password(8, false, "\\w", "!Aa0"), AcceptTerms = false } },

		};

		[Fact]
		public void Validate_ShouldBeTrue()
		{
			// Arrange
			string password = _faker.Internet.Password(8, false, "\\w+", "!Aa0");
			SignUpCommand command = new SignUpCommand { Email = _faker.Internet.Email(), Password = password, ConfirmPassword = password, AcceptTerms = true };
			SignUpCommandValidator validator = new SignUpCommandValidator();

			// Act
			FluentValidation.Results.ValidationResult result = validator.Validate(command);

			// Assert
			result.IsValid.ShouldBeTrue();
		}

		[Theory]
		[MemberData(nameof(SignUpCommandValues))]
		public void Validate_ShouldBeFalse_OnWrongData(SignUpCommand command)
		{
			// Arrange
			SignUpCommandValidator validator = new SignUpCommandValidator();

			// Act
			FluentValidation.Results.ValidationResult result = validator.Validate(command);

			// Assert
			result.IsValid.ShouldBeFalse();
		}
	}
}
