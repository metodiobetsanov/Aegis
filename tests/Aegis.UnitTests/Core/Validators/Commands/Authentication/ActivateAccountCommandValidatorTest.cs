namespace Aegis.UnitTests.Core.Validators.Commands.Authentication
{
	using global::Aegis.Core.Commands.Authentication;
	using global::Aegis.Core.Queries.Authentication;
	using global::Aegis.Core.Validators.Commands.Authentication;

	public class ConfirmEmailQueryValidatorTests
	{
		private static readonly Faker _faker = new Faker("en");

		public static TheoryData<ActivateAccountCommand> ActivateAccountCommandValues => new TheoryData<ActivateAccountCommand>()
		{
			{ new ActivateAccountCommand() },
			{ new ActivateAccountCommand { UserId = _faker.Random.Guid().ToString() } },
			{ new ActivateAccountCommand { Token = _faker.Random.String2(36) } },
			{ new ActivateAccountCommand { UserId = "", Token =_faker.Random.String2(36) }},
			{ new ActivateAccountCommand { UserId = "   ", Token =_faker.Random.String2(36) }},
			{ new ActivateAccountCommand { UserId =_faker.Random.Guid().ToString(), Token = "" }},
			{ new ActivateAccountCommand { UserId =_faker.Random.Guid().ToString(), Token = "   " }},
		};

		[Fact]
		public void Validate_ShouldBeTrue()
		{
			// Arrange
			ActivateAccountCommand command = new ActivateAccountCommand
			{
				UserId = _faker.Random.Guid().ToString(),
				Token = _faker.Random.String2(36)
			};
			ActivateAccountCommandValidator validator = new ActivateAccountCommandValidator();

			// Act
			FluentValidation.Results.ValidationResult result = validator.Validate(command);

			// Assert
			result.IsValid.ShouldBeTrue();
		}

		[Theory]
		[MemberData(nameof(ActivateAccountCommandValues))]
		public void Validate_ShouldBeFalse_OnWrongData(ActivateAccountCommand command)
		{
			// Arrange
			ActivateAccountCommandValidator validator = new ActivateAccountCommandValidator();

			// Act
			FluentValidation.Results.ValidationResult result = validator.Validate(command);

			// Assert
			result.IsValid.ShouldBeFalse();
		}
	}
}
