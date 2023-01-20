﻿namespace Aegis.UnitTests.Application.Validators.Commands.Auth
{
	using FluentValidation.Results;

	using global::Aegis.Application.Commands.Auth;
	using global::Aegis.Application.Validators.Commands.Auth;

	public class SignInCommandValidatorTests
	{
		public static TheoryData<SignInCommand> SignInCommandValues => new TheoryData<SignInCommand>()
		{
			{ new SignInCommand { } },
			{ new SignInCommand { Email = "test@test.test" } },
			{ new SignInCommand { Email = "test@test.test", Password = "" } },
			{ new SignInCommand { Email = "test@test.test", Password = "   " } },
			{ new SignInCommand { Password = "AAAaaa000@@@" + new string('a', 20) }},
			{ new SignInCommand { Email = "", Password = "AAAaaa000@@@" + new string('a', 20) } },
			{ new SignInCommand { Email = "   ", Password = "AAAaaa000@@@" + new string('a', 20) } }
		};

		[Fact]
		public void Validate_ShouldBeTrue()
		{
			// Arrange
			SignInCommand command = new SignInCommand { Email = "test@test.test", Password = "AAAaaa000@@@" + new string('a', 20) };
			SignInCommandValidator validator = new SignInCommandValidator();

			// Act
			ValidationResult result = validator.Validate(command);

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
			ValidationResult result = validator.Validate(command);

			// Assert
			result.IsValid.ShouldBeFalse();
		}
	}
}