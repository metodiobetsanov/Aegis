﻿#region copyright
//----------------------------------------------------------------------
// Copyright 2023 MNB Software
// Licensed under the Apache License, Version 2.0
// You may obtain a copy at http://www.apache.org/licenses/LICENSE-2.0
//----------------------------------------------------------------------
#endregion
namespace Aegis.UnitTests.Core.Commands.Authentication.Handlers
{
	using global::Aegis.Core.Commands.Authentication;
	using global::Aegis.Core.Commands.Authentication.Handlers;
	using global::Aegis.Core.Constants;
	using global::Aegis.Core.Exceptions;
	using global::Aegis.Models.Shared;
	using global::Aegis.Persistence.Entities.IdentityProvider;

	public class ActivateAccountCommandHandlerTests
	{
		private static readonly Faker _faker = new Faker("en");

		private readonly Mock<ILogger<ActivateAccountCommandHandler>> _logger = new Mock<ILogger<ActivateAccountCommandHandler>>();
		private readonly Mock<IMediator> _m = new Mock<IMediator>();
		private readonly Mock<UserManager<AegisUser>> _userManager = Helper.GetUserManagerMock();

		[Fact]
		public void Handle_ShouldReturnTrue()
		{
			// Arrange
			AegisUser? user = new AegisUser();
			_userManager.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
				.ReturnsAsync(user);

			_userManager.Setup(x => x.ConfirmEmailAsync(It.IsAny<AegisUser>(), It.IsAny<string>()))
				.ReturnsAsync(IdentityResult.Success);

			ActivateAccountCommand query = new ActivateAccountCommand { UserId = _faker.Random.Guid().ToString(), Token = _faker.Random.String2(36) };
			ActivateAccountCommandHandler handler = new ActivateAccountCommandHandler(_logger.Object, _m.Object, _userManager.Object);

			// Act 
			HandlerResult result = handler.Handle(query, new CancellationToken()).GetAwaiter().GetResult();

			// Assert
			result.ShouldNotBeNull();
			result.Success.ShouldBeTrue();
		}

		[Fact]
		public void Handle_ShouldReturnTrue_NotExistingUser()
		{
			// Arrange
			_userManager.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
				.ReturnsAsync((AegisUser?)null);

			ActivateAccountCommand query = new ActivateAccountCommand { UserId = _faker.Random.Guid().ToString(), Token = _faker.Random.String2(36) };
			ActivateAccountCommandHandler handler = new ActivateAccountCommandHandler(_logger.Object, _m.Object, _userManager.Object);

			// Act 
			HandlerResult result = handler.Handle(query, new CancellationToken()).GetAwaiter().GetResult();

			// Assert
			result.ShouldNotBeNull();
			result.Success.ShouldBeTrue();
		}

		[Fact]
		public void Handle_ShouldReturnFalse_OnFailedToConfirmEmail()
		{
			AegisUser? user = new AegisUser();
			_userManager.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
				.ReturnsAsync(user);

			_userManager.Setup(x => x.ConfirmEmailAsync(It.IsAny<AegisUser>(), It.IsAny<string>()))
				.ReturnsAsync(IdentityResult.Failed(new IdentityError { Code = _faker.Random.String2(12), Description = _faker.Random.String2(36) }));

			ActivateAccountCommand query = new ActivateAccountCommand { UserId = _faker.Random.Guid().ToString(), Token = _faker.Random.String2(36) };
			ActivateAccountCommandHandler handler = new ActivateAccountCommandHandler(_logger.Object, _m.Object, _userManager.Object);

			// Act 
			HandlerResult result = handler.Handle(query, new CancellationToken()).GetAwaiter().GetResult();

			// Assert
			result.ShouldNotBeNull();
			result.Success.ShouldBeFalse();
			result.Errors.Count.ShouldBe(1);
		}

		[Fact]
		public void Handle_Exceptions()
		{
			// Arrange
			_userManager.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
				.Throws(new Exception(nameof(Exception)));

			ActivateAccountCommand query = new ActivateAccountCommand { UserId = _faker.Random.Guid().ToString(), Token = _faker.Random.String2(36) };
			ActivateAccountCommandHandler handler = new ActivateAccountCommandHandler(_logger.Object, _m.Object, _userManager.Object);

			// Act 
			Exception exception = Record.Exception(() => handler.Handle(query, new CancellationToken()).GetAwaiter().GetResult());

			// Assert
			exception.ShouldNotBeNull();
			exception.ShouldBeOfType<IdentityProviderException>();
			((IdentityProviderException)exception).Message.ShouldBe(IdentityProviderConstants.SomethingWentWrong);
			((IdentityProviderException)exception).InnerException.ShouldNotBeNull();
			((IdentityProviderException)exception).InnerException!.Message.ShouldBe(nameof(Exception));
		}
	}
}
