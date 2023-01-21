namespace Aegis.UnitTests.Application.Validators.Commands.Auth
{
	using global::Aegis.Application.Queries.Auth;
	using global::Aegis.Application.Validators.Commands.Auth;

	public class ConfirmEmailQueryValidatorTests
	{
		private static readonly Faker _faker = new Faker("en");

		public static TheoryData<ConfirmEmailQuery> ConfirmEmailQueryValues => new TheoryData<ConfirmEmailQuery>()
		{
			{ new ConfirmEmailQuery() },
			{ new ConfirmEmailQuery { UserId = _faker.Random.Guid().ToString() } },
			{ new ConfirmEmailQuery { Token = _faker.Random.String(36) } },
			{ new ConfirmEmailQuery { UserId = "", Token =_faker.Random.String(36) }},
			{ new ConfirmEmailQuery { UserId = "   ", Token =_faker.Random.String(36) }},
			{ new ConfirmEmailQuery { UserId =_faker.Random.Guid().ToString(), Token = "" }},
			{ new ConfirmEmailQuery { UserId =_faker.Random.Guid().ToString(), Token = "   " }},
		};

		[Fact]
		public void Validate_ShouldBeTrue()
		{
			// Arrange
			ConfirmEmailQuery query = new ConfirmEmailQuery
			{
				UserId = _faker.Random.Guid().ToString(),
				Token = _faker.Random.String(36)
			};
			ConfirmEmailQueryValidator validator = new ConfirmEmailQueryValidator();

			// Act
			FluentValidation.Results.ValidationResult result = validator.Validate(query);

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
			FluentValidation.Results.ValidationResult result = validator.Validate(query);

			// Assert
			result.IsValid.ShouldBeFalse();
		}
	}
}
