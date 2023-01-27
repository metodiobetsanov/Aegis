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

	public class SignInCommandValidatorTests
	{
		private static readonly Faker _faker = new Faker("en");

		public static TheoryData<SignInCommand> SignInCommandValues => new TheoryData<SignInCommand>()
		{
			{ new SignInCommand { } },
			{ new SignInCommand { Email = _faker.Internet.Email() } },
			{ new SignInCommand { Email = _faker.Internet.Email(), Password = "" } },
			{ new SignInCommand { Email = _faker.Internet.Email(), Password = "  " } },
			{ new SignInCommand { Password = _faker.Internet.Password(8, false, "\\w", "!Aa0") }},
			{ new SignInCommand { Email = "", Password = _faker.Internet.Password(8, false, "\\w", "!Aa0") } },
			{ new SignInCommand { Email = "  ", Password = _faker.Internet.Password(8, false, "\\w", "!Aa0") } }
		};

		[Fact]
		public void Validate_ShouldBeTrue()
		{
			// Arrange
			SignInCommand command = new SignInCommand { Email = _faker.Internet.Email(), Password = _faker.Internet.Password(8, false, "\\w", "!Aa0") };
			SignInCommandValidator validator = new SignInCommandValidator();

			// Act
			FluentValidation.Results.ValidationResult result = validator.Validate(command);

			// Assert
			result.IsValid.ShouldBeTrue();
		}

		[Theory]
		[MemberData(nameof(SignInCommandValues))]
		public void Validate_ShouldBeFalse_OnWrongData(SignInCommand command)
		{
			// Arrange
			SignInCommandValidator validator = new SignInCommandValidator();

			// Act
			FluentValidation.Results.ValidationResult result = validator.Validate(command);

			// Assert
			result.IsValid.ShouldBeFalse();
		}
	}
}
