#region copyright
//----------------------------------------------------------------------
// Copyright 2023 MNB Software
// Licensed under the Apache License, Version 2.0
// You may obtain a copy at http://www.apache.org/licenses/LICENSE-2.0
//----------------------------------------------------------------------
#endregion
namespace Aegis.UnitTests.Core.Validators.Commands.Authentication
{
	using FluentValidation;

	using global::Aegis.Core.Commands.Authentication;
	using global::Aegis.Core.Queries.Authentication;
	using global::Aegis.Core.Validators.Commands.Authentication;

	using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

	public class SignInTwoStepCommandValidatorTests
	{
		private static readonly Faker _faker = new Faker("en");

		public static TheoryData<SignInTwoStepCommand> SignInTwoStepCommandValidatorValues => new TheoryData<SignInTwoStepCommand>()
		{
			{ new SignInTwoStepCommand() },
			{ new SignInTwoStepCommand { Code = "" } },
			{ new SignInTwoStepCommand { Code = "  " } }
		};

		[Fact]
		public void Validate_ShouldBeTrue()
		{
			// Arrange
			SignInTwoStepCommand command = new SignInTwoStepCommand
			{
				Code = _faker.Random.String2(6)
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
