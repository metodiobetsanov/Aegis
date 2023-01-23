namespace Aegis.UnitTests.Application.Validators.Queries.Authentication
{
	using global::Aegis.Application.Queries.Authentication;
	using global::Aegis.Application.Validators.Commands.Authentication;

	public class EmailConfirmationQueryValidatorTests
	{
		private static readonly Faker _faker = new Faker("en");

		public static TheoryData<EmailConfirmationQuery> EmailConfirmationQueryValues => new TheoryData<EmailConfirmationQuery>()
		{
			{ new EmailConfirmationQuery() },
			{ new EmailConfirmationQuery { UserId = "" } },
			{ new EmailConfirmationQuery { UserId = "   "} }
		};

		[Fact]
		public void Validate_ShouldBeTrue()
		{
			// Arrange
			EmailConfirmationQuery query = new EmailConfirmationQuery { UserId = _faker.Random.Guid().ToString() };
			EmailConfirmationQueryValidator validator = new EmailConfirmationQueryValidator();

			// Act
			FluentValidation.Results.ValidationResult result = validator.Validate(query);

			// Assert
			result.IsValid.ShouldBeTrue();
		}

		[Theory]
		[MemberData(nameof(EmailConfirmationQueryValues))]
		public void Validate_ShouldBeFalse_OnWrongData(EmailConfirmationQuery query)
		{
			// Arrange
			EmailConfirmationQueryValidator validator = new EmailConfirmationQueryValidator();

			// Act
			FluentValidation.Results.ValidationResult result = validator.Validate(query);

			// Assert
			result.IsValid.ShouldBeFalse();
		}
	}
}
