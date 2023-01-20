namespace Aegis.UnitTests.Application.Validators.Commands.Auth
{
	using FluentValidation.Results;

	using global::Aegis.Application.Commands.Auth;
	using global::Aegis.Application.Validators.Commands.Auth;

	public class SignUpCommandValidatorTests
	{
		public static TheoryData<SignUpCommand> SignUpCommandValues => new TheoryData<SignUpCommand>()
		{
			{ new SignUpCommand { } },
			{ new SignUpCommand { Email = "test@test.test" } },
			{ new SignUpCommand { Password = "AAAaaa000@@@"  } },
			{ new SignUpCommand { ConfirmPassword = "AAAaaa000@@@"  } },
			{ new SignUpCommand { AcceptTerms =  true } },
			{ new SignUpCommand { Email = "", Password = "AAAaaa000@@@", ConfirmPassword = "AAAaaa000@@@", AcceptTerms = true } },
			{ new SignUpCommand { Email = "   ", Password = "AAAaaa000@@@", ConfirmPassword = "AAAaaa000@@@", AcceptTerms = true } },
			{ new SignUpCommand { Email = "test", Password = "AAAaaa000@@@", ConfirmPassword = "AAAaaa000@@@", AcceptTerms = true } },
			{ new SignUpCommand { Email = "test", Password = "test@test.test", ConfirmPassword = "AAAaaa000@@@", AcceptTerms = true } },
			{ new SignUpCommand { Email = "test@test.test", Password = "", ConfirmPassword = "AAAaaa000@@@", AcceptTerms = false } },
			{ new SignUpCommand { Email = "test@test.test", Password = "   ", ConfirmPassword = "AAAaaa000@@@", AcceptTerms = false } },
			{ new SignUpCommand { Email = "test@test.test", Password = new string('A', 9), ConfirmPassword = "AAAaaa000@@@", AcceptTerms = false } },
			{ new SignUpCommand { Email = "test@test.test", Password = new string('a', 9), ConfirmPassword = "AAAaaa000@@@", AcceptTerms = false } },
			{ new SignUpCommand { Email = "test@test.test", Password = new string('0', 9), ConfirmPassword = "AAAaaa000@@@", AcceptTerms = false } },
			{ new SignUpCommand { Email = "test@test.test", Password = new string('@', 9), ConfirmPassword = "AAAaaa000@@@", AcceptTerms = false } },
			{ new SignUpCommand { Email = "test@test.test", Password = "AAAaaa000@@@", ConfirmPassword = "", AcceptTerms = false } },
			{ new SignUpCommand { Email = "test@test.test", Password = "AAAaaa000@@@", ConfirmPassword = "   ", AcceptTerms = false } },
			{ new SignUpCommand { Email = "test@test.test", Password = "AAAaaa000@@@", ConfirmPassword = "test", AcceptTerms = false } },
			{ new SignUpCommand { Email = "test@test.test", Password = "AAAaaa000@@@", ConfirmPassword = "AAAaaa000@@@", AcceptTerms = false } },

		};

		[Fact]
		public void Validate_ShouldBeTrue()
		{
			// Arrange
			SignUpCommand command = new SignUpCommand { Email = "test@test.test", Password = "AAAaaa000@@@", ConfirmPassword = "AAAaaa000@@@", AcceptTerms = true };
			SignUpCommandValidator validator = new SignUpCommandValidator();

			// Act
			ValidationResult result = validator.Validate(command);

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
			ValidationResult result = validator.Validate(command);

			// Assert
			result.IsValid.ShouldBeFalse();
		}
	}
}
