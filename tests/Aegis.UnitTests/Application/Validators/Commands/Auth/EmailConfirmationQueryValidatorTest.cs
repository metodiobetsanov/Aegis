namespace Aegis.UnitTests.Application.Validators.Commands.Auth
{
	using FluentValidation.Results;

	using global::Aegis.Application.Queries.Auth;
	using global::Aegis.Application.Validators.Commands.Auth;

	public class EmailConfirmationQueryValidatorTests
	{
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
			EmailConfirmationQuery query = new EmailConfirmationQuery { UserId = "test" };
			EmailConfirmationQueryValidator validator = new EmailConfirmationQueryValidator();

			// Act
			ValidationResult result = validator.Validate(query);

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
			ValidationResult result = validator.Validate(query);

			// Assert
			result.IsValid.ShouldBeFalse();
		}
	}
}
