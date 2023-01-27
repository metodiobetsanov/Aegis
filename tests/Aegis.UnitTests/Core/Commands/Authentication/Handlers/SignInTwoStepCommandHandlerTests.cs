#region copyright
//----------------------------------------------------------------------
// Copyright 2023 MNB Software
// Licensed under the Apache License, Version 2.0
// You may obtain a copy at http://www.apache.org/licenses/LICENSE-2.0
//----------------------------------------------------------------------
#endregion
namespace Aegis.UnitTests.Core.Commands.Authentication.Handlers
{
	using Duende.IdentityServer.Models;
	using Duende.IdentityServer.Services;
	using Duende.IdentityServer.Validation;

	using global::Aegis.Core.Commands.Authentication;
	using global::Aegis.Core.Commands.Authentication.Handlers;
	using global::Aegis.Core.Constants;
	using global::Aegis.Core.Exceptions;
	using global::Aegis.Models.Authentication;
	using global::Aegis.Persistence.Entities.IdentityProvider;

	public class SignInTwoStepCommandHandlerTests
	{
		private static readonly Faker _faker = new Faker("en");
		private static readonly Faker<AegisUser> _fakeUser = Helper.GetUserFaker();

		private readonly Mock<ILogger<SignInTwoStepCommandHandler>> _logger = new Mock<ILogger<SignInTwoStepCommandHandler>>();
		private readonly Mock<IIdentityServerInteractionService> _isis = new Mock<IIdentityServerInteractionService>();
		private readonly Mock<IEventService> _es = new Mock<IEventService>();
		private readonly Mock<SignInManager<AegisUser>> _signInManager = Helper.GetSignInManagerMock();

		[Theory]
		[InlineData(null)]
		[InlineData("")]
		[InlineData("  ")]
		[InlineData("/")]
		[InlineData("/valid")]
		public void Handle_ShouldReturnTrue_OnValidUser(string returnUrl)
		{
			// Arrange
			AegisUser? user = _fakeUser.Generate();
			_isis.Setup(x => x.GetAuthorizationContextAsync(It.Is<string>(s => s == "/valid")))
				.ReturnsAsync(new AuthorizationRequest(new ValidatedAuthorizeRequest { Client = new Client { ClientId = _faker.Random.String2(12) } }));

			_signInManager.Setup(x => x.GetTwoFactorAuthenticationUserAsync())
				.ReturnsAsync(user);

			_signInManager.Setup(x => x.TwoFactorSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
				.ReturnsAsync(SignInResult.Success);

			SignInTwoStepCommand command = new SignInTwoStepCommand { Code = _faker.Random.String2(6), RememberClient = true, RememberMe = true, ReturnUrl = returnUrl };
			SignInTwoStepCommandHandler handler = new SignInTwoStepCommandHandler(_logger.Object, _isis.Object, _es.Object, _signInManager.Object);

			// Act 
			SignInCommandResult result = handler.Handle(command, new CancellationToken()).GetAwaiter().GetResult();

			// Assert
			result.ShouldNotBeNull();
			result.Success.ShouldBeTrue();

			if (string.IsNullOrWhiteSpace(returnUrl))
			{
				result.ReturnUrl.ShouldBe("~/");
			}
			else
			{
				result.ReturnUrl.ShouldBe(returnUrl);
			}
		}

		[Theory]
		[InlineData("/")]
		[InlineData("/valid")]
		public void Handle_ShouldReturnFalse_OnIsLockedOut(string returnUrl)
		{
			// Arrange
			AegisUser? user = _fakeUser.Generate();
			_isis.Setup(x => x.GetAuthorizationContextAsync(It.Is<string>(s => s == "/valid")))
				.ReturnsAsync(new AuthorizationRequest(new ValidatedAuthorizeRequest { Client = new Client { ClientId = _faker.Random.String2(12) } }));

			_signInManager.Setup(x => x.GetTwoFactorAuthenticationUserAsync())
				.ReturnsAsync(user);

			_signInManager.Setup(x => x.TwoFactorSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
				.ReturnsAsync(SignInResult.LockedOut);

			SignInTwoStepCommand command = new SignInTwoStepCommand { Code = _faker.Random.String2(6), RememberClient = true, RememberMe = true, ReturnUrl = returnUrl };
			SignInTwoStepCommandHandler handler = new SignInTwoStepCommandHandler(_logger.Object, _isis.Object, _es.Object, _signInManager.Object);

			// Act 
			SignInCommandResult result = handler.Handle(command, new CancellationToken()).GetAwaiter().GetResult();

			// Assert
			result.ShouldNotBeNull();
			result.Success.ShouldBeFalse();
			result.AccounLocked.ShouldBeTrue();
			result.ReturnUrl.ShouldBeNull();
		}

		[Theory]
		[InlineData("/")]
		[InlineData("/valid")]
		public void Handle_ShouldReturnFalse_OnIsNotAllowed(string returnUrl)
		{
			// Arrange
			AegisUser? user = _fakeUser.Generate();
			_isis.Setup(x => x.GetAuthorizationContextAsync(It.Is<string>(s => s == "/valid")))
				.ReturnsAsync(new AuthorizationRequest(new ValidatedAuthorizeRequest { Client = new Client { ClientId = _faker.Random.String2(12) } }));

			_signInManager.Setup(x => x.GetTwoFactorAuthenticationUserAsync())
				.ReturnsAsync(user);

			_signInManager.Setup(x => x.TwoFactorSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
				.ReturnsAsync(SignInResult.NotAllowed);

			SignInTwoStepCommand command = new SignInTwoStepCommand { Code = _faker.Random.String2(6), RememberClient = true, RememberMe = true, ReturnUrl = returnUrl };
			SignInTwoStepCommandHandler handler = new SignInTwoStepCommandHandler(_logger.Object, _isis.Object, _es.Object, _signInManager.Object);

			// Act 
			SignInCommandResult result = handler.Handle(command, new CancellationToken()).GetAwaiter().GetResult();

			// Assert
			result.ShouldNotBeNull();
			result.Success.ShouldBeFalse();
			result.AccounNotActive.ShouldBeTrue();
			result.ReturnUrl.ShouldBeNull();
		}

		[Theory]
		[InlineData("/")]
		[InlineData("/valid")]
		public void Handle_ShouldReturnFalse_OnFailure(string returnUrl)
		{
			// Arrange
			AegisUser? user = _fakeUser.Generate();
			_isis.Setup(x => x.GetAuthorizationContextAsync(It.Is<string>(s => s == "/valid")))
				.ReturnsAsync(new AuthorizationRequest(new ValidatedAuthorizeRequest { Client = new Client { ClientId = _faker.Random.String2(12) } }));

			_signInManager.Setup(x => x.GetTwoFactorAuthenticationUserAsync())
				.ReturnsAsync(user);

			_signInManager.Setup(x => x.TwoFactorSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
				.ReturnsAsync(SignInResult.Failed);

			SignInTwoStepCommand command = new SignInTwoStepCommand { Code = _faker.Random.String2(6), RememberClient = true, RememberMe = true, ReturnUrl = returnUrl };
			SignInTwoStepCommandHandler handler = new SignInTwoStepCommandHandler(_logger.Object, _isis.Object, _es.Object, _signInManager.Object);

			// Act 
			SignInCommandResult result = handler.Handle(command, new CancellationToken()).GetAwaiter().GetResult();

			// Assert
			result.ShouldNotBeNull();
			result.Success.ShouldBeFalse();
			result.Errors.Count.ShouldBe(1);
		}

		[Theory]
		[InlineData("/")]
		[InlineData("/valid")]
		public void Handle_ShouldReturnFalse_OnNotExistingUser(string returnUrl)
		{
			// Arrange
			_isis.Setup(x => x.GetAuthorizationContextAsync(It.Is<string>(s => s == "/valid")))
				.ReturnsAsync(new AuthorizationRequest(new ValidatedAuthorizeRequest { Client = new Client { ClientId = _faker.Random.String2(12) } }));

			SignInTwoStepCommand command = new SignInTwoStepCommand { Code = _faker.Random.String2(6), RememberClient = true, RememberMe = true, ReturnUrl = returnUrl };
			SignInTwoStepCommandHandler handler = new SignInTwoStepCommandHandler(_logger.Object, _isis.Object, _es.Object, _signInManager.Object);

			// Act 
			SignInCommandResult result = handler.Handle(command, new CancellationToken()).GetAwaiter().GetResult();

			// Assert
			result.ShouldNotBeNull();
			result.Success.ShouldBeFalse();
		}

		[Fact]
		public void Handle_ShouldThrowExceptions_ReturnUrl()
		{
			// Arrange
			SignInTwoStepCommand command = new SignInTwoStepCommand { Code = _faker.Random.String2(6), RememberClient = true, RememberMe = true, ReturnUrl = _faker.Internet.Url() };
			SignInTwoStepCommandHandler handler = new SignInTwoStepCommandHandler(_logger.Object, _isis.Object, _es.Object, _signInManager.Object);

			// Act 
			Exception exception = Record.Exception(() => handler.Handle(command, new CancellationToken()).GetAwaiter().GetResult());

			// Assert
			exception.ShouldNotBeNull();
			exception.ShouldBeOfType<IdentityServerException>();
			((IdentityServerException)exception).Message.ShouldBe("Invalid return URL!");
		}

		[Fact]
		public void Handle_Exceptions()
		{
			// Arrange
			_isis.Setup(x => x.GetAuthorizationContextAsync(It.IsAny<string>()))
				.Throws(new Exception(nameof(Exception)));
			SignInTwoStepCommand command = new SignInTwoStepCommand { Code = _faker.Random.String2(6), RememberClient = true, RememberMe = true, ReturnUrl = "/" };
			SignInTwoStepCommandHandler handler = new SignInTwoStepCommandHandler(_logger.Object, _isis.Object, _es.Object, _signInManager.Object);

			// Act 
			Exception exception = Record.Exception(() => handler.Handle(command, new CancellationToken()).GetAwaiter().GetResult());

			// Assert
			exception.ShouldNotBeNull();
			exception.ShouldBeOfType<IdentityProviderException>();
			((IdentityProviderException)exception).Message.ShouldBe(IdentityProviderConstants.SomethingWentWrongWithSignIn);
			((IdentityProviderException)exception).InnerException.ShouldNotBeNull();
			((IdentityProviderException)exception).InnerException!.Message.ShouldBe(nameof(Exception));
		}
	}
}
