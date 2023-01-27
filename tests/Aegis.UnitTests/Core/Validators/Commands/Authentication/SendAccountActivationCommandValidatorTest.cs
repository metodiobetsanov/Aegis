namespace Aegis.UnitTests.Core.Validators.Commands.Authentication
{
	using global::Aegis.Core.Commands.Authentication;
	using global::Aegis.Core.Queries.Authentication;
	using global::Aegis.Core.Validators.Commands.Authentication;

	public class SendAccountActivationCommandValidatorTest
	{
		private static readonly Faker _faker = new Faker("en");

		public static TheoryData<SendAccountActivationCommand> SendAccountActivationCommandValues => new TheoryData<SendAccountActivationCommand>()
		{
			{ new SendAccountActivationCommand() },
			{ new SendAccountActivationCommand { UserId = "" } },
			{ new SendAccountActivationCommand { UserId = "   "} }
		};

		[Fact]
		public void Validate_ShouldBeTrue()
		{
			// Arrange
			SendAccountActivationCommand command = new SendAccountActivationCommand { UserId = _faker.Random.Guid().ToString() };
			SendAccountActivationCommandValidator validator = new SendAccountActivationCommandValidator();

			// Act
			FluentValidation.Results.ValidationResult result = validator.Validate(command);

			// Assert
			result.IsValid.ShouldBeTrue();
		}

		[Theory]
		[MemberData(nameof(SendAccountActivationCommandValues))]
		public void Validate_ShouldBeFalse_OnWrongData(SendAccountActivationCommand command)
		{
			// Arrange
			SendAccountActivationCommandValidator validator = new SendAccountActivationCommandValidator();

			// Act
			FluentValidation.Results.ValidationResult result = validator.Validate(command);

			// Assert
			result.IsValid.ShouldBeFalse();
		}
	}
}
