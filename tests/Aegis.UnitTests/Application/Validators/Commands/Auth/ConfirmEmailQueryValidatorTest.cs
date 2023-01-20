namespace Aegis.UnitTests.Application.Validators.Commands.Auth
{
	using FluentValidation.Results;

	using global::Aegis.Application.Queries.Auth;
	using global::Aegis.Application.Validators.Commands.Auth;

	public class ConfirmEmailQueryValidatorTests
	{
		public static TheoryData<ConfirmEmailQuery> ConfirmEmailQueryValues => new TheoryData<ConfirmEmailQuery>()
		{
			{ new ConfirmEmailQuery() },
			{ new ConfirmEmailQuery { UserId = "test" } },
			{ new ConfirmEmailQuery { Token = "test" } },
			{ new ConfirmEmailQuery { UserId = "", Token = "test" }},
			{ new ConfirmEmailQuery { UserId = "   ", Token = "test" }},
			{ new ConfirmEmailQuery { UserId = "test", Token = "" }},
			{ new ConfirmEmailQuery { UserId = "test", Token = "   " }},
		};

		[Fact]
		public void Validate_ShouldBeTrue()
		{
			// Arrange
			ConfirmEmailQuery query = new ConfirmEmailQuery { UserId = "test", Token = "test" };
			ConfirmEmailQueryValidator validator = new ConfirmEmailQueryValidator();

			// Act
			ValidationResult result = validator.Validate(query);

			// Assert
			result.IsValid.ShouldBeTrue();
		}

		[Theory]
		[MemberData(nameof(ConfirmEmailQueryValues))]
		public void Validate_ShouldBeFalse_OnWrongData(ConfirmEmailQuery query)
		{
			// Arrange
			ConfirmEmailQueryValidator validator = new ConfirmEmailQueryValidator();

			// Act
			ValidationResult result = validator.Validate(query);

			// Assert
			result.IsValid.ShouldBeFalse();
		}
	}
}
