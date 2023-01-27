namespace Aegis.UnitTests.Application.Commands.Authentication.Handlers
{
	using Duende.IdentityServer.Models;
	using Duende.IdentityServer.Services;
	using Duende.IdentityServer.Validation;

	using global::Aegis.Application.Commands.Authentication;
	using global::Aegis.Application.Commands.Authentication.Handlers;
	using global::Aegis.Application.Constants;
	using global::Aegis.Application.Exceptions;
	using global::Aegis.Models.Authentication;
	using global::Aegis.Persistence.Entities.IdentityProvider;

	public class SignInCommandHandlerTests
	{
		private static readonly Faker _faker = new Faker("en");
		private static readonly Faker<AegisUser> _fakeUser = Helper.GetUserFaker();

		private readonly Mock<ILogger<SignInCommandHandler>> _logger = new Mock<ILogger<SignInCommandHandler>>();
		private readonly Mock<IIdentityServerInteractionService> _isis = new Mock<IIdentityServerInteractionService>();
		private readonly Mock<IEventService> _es = new Mock<IEventService>();
		private readonly Mock<UserManager<AegisUser>> _userManager = Helper.GetUserManagerMock();
		private readonly Mock<SignInManager<AegisUser>> _signInManager = Helper.GetSignInManagerMock();

		[Theory]
		[InlineData(null)]
		[InlineData("")]
		[InlineData("   ")]
		[InlineData("/")]
		[InlineData("/valid")]
		public void Handle_ShouldReturnTrue_OnValidUser(string returnUrl)
		{
			// Arrange
			AegisUser? user = _fakeUser.Generate();
			_isis.Setup(x => x.GetAuthorizationContextAsync(It.Is<string>(s => s == "/valid")))
				.ReturnsAsync(new AuthorizationRequest(new ValidatedAuthorizeRequest { Client = new Client { ClientId = _faker.Random.String2(12) } }));

			_userManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
				.ReturnsAsync(user);

			_signInManager.Setup(x => x.PasswordSignInAsync(It.IsAny<AegisUser>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
				.ReturnsAsync(SignInResult.Success);

			SignInCommand command = new SignInCommand { Email = user.Email, Password = _faker.Internet.Password(8, false, "\\w", "!Aa0"), RememberMe = true, ReturnUrl = returnUrl };
			SignInCommandHandler handler = new SignInCommandHandler(_logger.Object, _isis.Object, _es.Object, _userManager.Object, _signInManager.Object);

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
		public void Handle_ShouldReturnFalse_OnRequiresTwoFactor(string returnUrl)
		{
			// Arrange
			AegisUser? user = _fakeUser.Generate();
			_isis.Setup(x => x.GetAuthorizationContextAsync(It.Is<string>(s => s == "/valid")))
				.ReturnsAsync(new AuthorizationRequest(new ValidatedAuthorizeRequest { Client = new Client { ClientId = _faker.Random.String2(12) } }));

			_userManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
				.ReturnsAsync(user);

			_signInManager.Setup(x => x.PasswordSignInAsync(It.IsAny<AegisUser>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
				.ReturnsAsync(SignInResult.TwoFactorRequired);

			SignInCommand command = new SignInCommand { Email = user.Email, Password = _faker.Internet.Password(8, false, "\\w", "!Aa0"), ReturnUrl = returnUrl };
			SignInCommandHandler handler = new SignInCommandHandler(_logger.Object, _isis.Object, _es.Object, _userManager.Object, _signInManager.Object);

			// Act 
			SignInCommandResult result = handler.Handle(command, new CancellationToken()).GetAwaiter().GetResult();

			// Assert
			result.ShouldNotBeNull();
			result.Success.ShouldBeFalse();
			result.RequiresTwoStep.ShouldBeTrue();
			result.ReturnUrl.ShouldBeNull();
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

			_userManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
				.ReturnsAsync(user);

			_signInManager.Setup(x => x.PasswordSignInAsync(It.IsAny<AegisUser>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
				.ReturnsAsync(SignInResult.LockedOut);

			SignInCommand command = new SignInCommand { Email = _faker.Internet.Email(), Password = _faker.Internet.Password(8, false, "\\w", "!Aa0"), ReturnUrl = returnUrl };
			SignInCommandHandler handler = new SignInCommandHandler(_logger.Object, _isis.Object, _es.Object, _userManager.Object, _signInManager.Object);

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

			_userManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
				.ReturnsAsync(user);

			_signInManager.Setup(x => x.PasswordSignInAsync(It.IsAny<AegisUser>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
				.ReturnsAsync(SignInResult.NotAllowed);
			SignInCommand command = new SignInCommand { Email = _faker.Internet.Email(), Password = _faker.Internet.Password(8, false, "\\w", "!Aa0"), ReturnUrl = returnUrl };
			SignInCommandHandler handler = new SignInCommandHandler(_logger.Object, _isis.Object, _es.Object, _userManager.Object, _signInManager.Object);

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

			_userManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
				.ReturnsAsync(user);

			_signInManager.Setup(x => x.PasswordSignInAsync(It.IsAny<AegisUser>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
				.ReturnsAsync(SignInResult.Failed);

			SignInCommand command = new SignInCommand { Email = _faker.Internet.Email(), Password = _faker.Internet.Password(8, false, "\\w", "!Aa0"), ReturnUrl = returnUrl };
			SignInCommandHandler handler = new SignInCommandHandler(_logger.Object, _isis.Object, _es.Object, _userManager.Object, _signInManager.Object);

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

			SignInCommand command = new SignInCommand { Email = _faker.Internet.Email(), Password = _faker.Internet.Password(8, false, "\\w", "!Aa0"), ReturnUrl = returnUrl };
			SignInCommandHandler handler = new SignInCommandHandler(_logger.Object, _isis.Object, _es.Object, _userManager.Object, _signInManager.Object);

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
			SignInCommand command = new SignInCommand { ReturnUrl = _faker.Internet.Url() };
			SignInCommandHandler handler = new SignInCommandHandler(_logger.Object, _isis.Object, _es.Object, _userManager.Object, _signInManager.Object);

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
			SignInCommand command = new SignInCommand { Email = _faker.Internet.Email(), Password = _faker.Internet.Password(8, false, "\\w", "!Aa0") };
			SignInCommandHandler handler = new SignInCommandHandler(_logger.Object, _isis.Object, _es.Object, _userManager.Object, _signInManager.Object);

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
