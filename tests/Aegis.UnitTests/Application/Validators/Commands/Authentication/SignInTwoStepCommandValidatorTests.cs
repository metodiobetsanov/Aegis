namespace Aegis.UnitTests.Application.Validators.Commands.Authentication
{
	using FluentValidation;

	using global::Aegis.Application.Commands.Authentication;
	using global::Aegis.Application.Queries.Authentication;
	using global::Aegis.Application.Validators.Commands.Authentication;

	using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

	public class SignInTwoStepCommandValidatorTests
	{
		private static readonly Faker _faker = new Faker("en");

		public static TheoryData<SignInTwoStepCommand> SignInTwoStepCommandValidatorValues => new TheoryData<SignInTwoStepCommand>()
		{
			{ new SignInTwoStepCommand() },
			{ new SignInTwoStepCommand { Code = "" } },
			{ new SignInTwoStepCommand { Code = "   " } }
		};

		[Fact]
		public void Validate_ShouldBeTrue()
		{
			// Arrange
			SignInTwoStepCommand command = new SignInTwoStepCommand
			{
				Code = _faker.Random.String(6)
			};
			SignInTwoStepCommandValidator validator = new SignInTwoStepCommandValidator();

			// Act
			FluentValidation.Results.ValidationResult result = validator.Validate(command);

			// Assert
			result.IsValid.ShouldBeTrue();
		}

		[Theory]
		[MemberData(nameof(SignInTwoStepCommandValidatorValues))]
		public void Validate_ShouldBeFalse_OnWrongData(SignInTwoStepCommand command)
		{
			// Arrange
			SignInTwoStepCommandValidator validator = new SignInTwoStepCommandValidator();

			// Act
			FluentValidation.Results.ValidationResult result = validator.Validate(command);

			// Assert
			result.IsValid.ShouldBeFalse();
		}
	}
}
