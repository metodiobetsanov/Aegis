#region copyright
//----------------------------------------------------------------------
// Copyright 2023 MNB Software
// Licensed under the Apache License, Version 2.0
// You may obtain a copy at http://www.apache.org/licenses/LICENSE-2.0
//----------------------------------------------------------------------
#endregion
namespace Aegis.UnitTests.Core.Validators.Commands.Authentication
{
	using global::Aegis.Core.Commands.Authentication;
	using global::Aegis.Core.Validators.Commands.Authentication;

	public class SendForgetPasswordCommandValidatorTests
	{
		private static readonly Faker _faker = new Faker("en");

		public static TheoryData<SendForgetPasswordCommand> SendForgetPasswordCommandValues => new TheoryData<SendForgetPasswordCommand>()
		{
			{ new SendForgetPasswordCommand() },
			{ new SendForgetPasswordCommand { Email = _faker.Random.Word() } },
			{ new SendForgetPasswordCommand { Email = "" } },
			{ new SendForgetPasswordCommand { Email = "  " } },
		};

		[Fact]
		public void Validate_ShouldBeTrue()
		{
			// Arrange
			SendForgetPasswordCommand command = new SendForgetPasswordCommand
			{
				Email = _faker.Internet.Email()
			};
			SendForgetPasswordCommandValidator validator = new SendForgetPasswordCommandValidator();

			// Act
			FluentValidation.Results.ValidationResult result = validator.Validate(command);

			// Assert
			result.IsValid.ShouldBeTrue();
		}

		[Theory]
		[MemberData(nameof(SendForgetPasswordCommandValues))]
		public void Validate_ShouldBeFalse_OnWrongData(SendForgetPasswordCommand command)
		{
			// Arrange
			SendForgetPasswordCommandValidator validator = new SendForgetPasswordCommandValidator();

			// Act
			FluentValidation.Results.ValidationResult result = validator.Validate(command);

			// Assert
			result.IsValid.ShouldBeFalse();
		}
	}
}
