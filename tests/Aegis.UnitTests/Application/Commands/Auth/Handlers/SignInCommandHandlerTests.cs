namespace Aegis.UnitTests.Application.Commands.Auth.Handlers
{

	using Duende.IdentityServer.Models;
	using Duende.IdentityServer.Services;
	using Duende.IdentityServer.Validation;

	using global::Aegis.Application.Commands.Auth;
	using global::Aegis.Application.Commands.Auth.Handlers;
	using global::Aegis.Application.Constants;
	using global::Aegis.Application.Exceptions;
	using global::Aegis.Models.Shared;
	using global::Aegis.Persistence.Entities.IdentityProvider;

	using Microsoft.AspNetCore.Identity;

	using Moq;

	using Shouldly;

	public class SignInCommandHandlerTests
	{
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
		[InlineData("/test")]
		public void Handle_ShouldReturnTrue_OnValidUser(string returnUrl)
		{
			// Arrange
			AegisUser? user = new AegisUser();
			_isis.Setup(x => x.GetAuthorizationContextAsync(It.Is<string>(s => s == "/test")))
				.ReturnsAsync(new AuthorizationRequest(new ValidatedAuthorizeRequest { Client = new Client { ClientId = "test" } }));

			_userManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
				.ReturnsAsync(user);

			_signInManager.Setup(x => x.PasswordSignInAsync(It.IsAny<AegisUser>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
				.ReturnsAsync(SignInResult.Success);

			SignInCommand command = new SignInCommand { Email = "test", Password = "test", RememberMe = true, ReturnUrl = returnUrl };
			SignInCommandHandler handler = new SignInCommandHandler(_logger.Object, _isis.Object, _es.Object, _userManager.Object, _signInManager.Object);

			// Act 
			AuthenticationResult result = handler.Handle(command, new CancellationToken()).GetAwaiter().GetResult();

			// Assert
			result.ShouldNotBeNull();
			result.Succeeded.ShouldBeTrue();

			if (string.IsNullOrWhiteSpace(returnUrl))
			{
				result.ReturnUrl.ShouldBe("~/");
			}
			else
			{
				result.ReturnUrl.ShouldBe(returnUrl);
			}
		}

		[Fact]
		public void Handle_ShouldReturnFalse_OnRequiresTwoFactor()
		{
			// Arrange
			AegisUser? user = new AegisUser();
			_isis.Setup(x => x.GetAuthorizationContextAsync(It.Is<string>(s => s == "/test")))
				.ReturnsAsync(new AuthorizationRequest(new ValidatedAuthorizeRequest { Client = new Client { ClientId = "test" } }));

			_userManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
				.ReturnsAsync(user);

			_signInManager.Setup(x => x.PasswordSignInAsync(It.IsAny<AegisUser>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
				.ReturnsAsync(SignInResult.TwoFactorRequired);

			SignInCommand command = new SignInCommand { Email = "test", Password = "test", RememberMe = true, ReturnUrl = "/" };
			SignInCommandHandler handler = new SignInCommandHandler(_logger.Object, _isis.Object, _es.Object, _userManager.Object, _signInManager.Object);

			// Act 
			AuthenticationResult result = handler.Handle(command, new CancellationToken()).GetAwaiter().GetResult();

			// Assert
			result.ShouldNotBeNull();
			result.Succeeded.ShouldBeFalse();
			result.RedirectToAction.ShouldBeTrue();
			result.Action.ShouldBe("SignInTwoStep");
			result.ReturnUrl.ShouldBeNull();
		}

		[Fact]
		public void Handle_ShouldReturnFalse_OnIsLockedOut()
		{
			// Arrange
			AegisUser? user = new AegisUser();
			_isis.Setup(x => x.GetAuthorizationContextAsync(It.Is<string>(s => s == "/test")))
				.ReturnsAsync(new AuthorizationRequest(new ValidatedAuthorizeRequest { Client = new Client { ClientId = "test" } }));

			_userManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
				.ReturnsAsync(user);

			_signInManager.Setup(x => x.PasswordSignInAsync(It.IsAny<AegisUser>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
				.ReturnsAsync(SignInResult.LockedOut);

			SignInCommand command = new SignInCommand { Email = "test", Password = "test", RememberMe = true, ReturnUrl = "/" };
			SignInCommandHandler handler = new SignInCommandHandler(_logger.Object, _isis.Object, _es.Object, _userManager.Object, _signInManager.Object);

			// Act 
			AuthenticationResult result = handler.Handle(command, new CancellationToken()).GetAwaiter().GetResult();

			// Assert
			result.ShouldNotBeNull();
			result.Succeeded.ShouldBeFalse();
			result.RedirectToAction.ShouldBeTrue();
			result.Action.ShouldBe("LockedOut");
			result.ReturnUrl.ShouldBeNull();
		}

		[Fact]
		public void Handle_ShouldReturnFalse_OnIsNotAllowed()
		{
			// Arrange
			AegisUser? user = new AegisUser();
			_isis.Setup(x => x.GetAuthorizationContextAsync(It.Is<string>(s => s == "/test")))
				.ReturnsAsync(new AuthorizationRequest(new ValidatedAuthorizeRequest { Client = new Client { ClientId = "test" } }));

			_userManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
				.ReturnsAsync(user);

			_signInManager.Setup(x => x.PasswordSignInAsync(It.IsAny<AegisUser>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
				.ReturnsAsync(SignInResult.NotAllowed);
			SignInCommand command = new SignInCommand { Email = "test", Password = "test", RememberMe = true, ReturnUrl = "/" };
			SignInCommandHandler handler = new SignInCommandHandler(_logger.Object, _isis.Object, _es.Object, _userManager.Object, _signInManager.Object);

			// Act 
			AuthenticationResult result = handler.Handle(command, new CancellationToken()).GetAwaiter().GetResult();

			// Assert
			result.ShouldNotBeNull();
			result.Succeeded.ShouldBeFalse();
			result.RedirectToAction.ShouldBeTrue();
			result.Action.ShouldBe("EmailNotConfimed");
			result.ReturnUrl.ShouldBeNull();
		}

		[Fact]
		public void Handle_ShouldReturnFalse_OnFailure()
		{
			// Arrange
			AegisUser? user = new AegisUser();
			_isis.Setup(x => x.GetAuthorizationContextAsync(It.Is<string>(s => s == "/test")))
				.ReturnsAsync(new AuthorizationRequest(new ValidatedAuthorizeRequest { Client = new Client { ClientId = "test" } }));

			_userManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
				.ReturnsAsync(user);

			_signInManager.Setup(x => x.PasswordSignInAsync(It.IsAny<AegisUser>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
				.ReturnsAsync(SignInResult.Failed);

			SignInCommand command = new SignInCommand { Email = "test", Password = "test", RememberMe = true, ReturnUrl = "/" };
			SignInCommandHandler handler = new SignInCommandHandler(_logger.Object, _isis.Object, _es.Object, _userManager.Object, _signInManager.Object);

			// Act 
			AuthenticationResult result = handler.Handle(command, new CancellationToken()).GetAwaiter().GetResult();

			// Assert
			result.ShouldNotBeNull();
			result.Succeeded.ShouldBeFalse();
			result.RedirectToAction.ShouldBeFalse();
			result.ReturnUrl.ShouldBeNull();
			result.Errors.Count.ShouldBe(1);
		}

		[Fact]
		public void Handle_ShouldReturnFalse_OnNotExistingUser()
		{
			// Arrange
			SignInCommand command = new SignInCommand { Email = "test", Password = "test", RememberMe = true, ReturnUrl = "/" };
			SignInCommandHandler handler = new SignInCommandHandler(_logger.Object, _isis.Object, _es.Object, _userManager.Object, _signInManager.Object);

			// Act 
			AuthenticationResult result = handler.Handle(command, new CancellationToken()).GetAwaiter().GetResult();

			// Assert
			result.ShouldNotBeNull();
			result.Succeeded.ShouldBeFalse();
			result.RedirectToAction.ShouldBeFalse();
			result.ReturnUrl.ShouldBeNull();
		}

		[Fact]
		public void Handle_Exceptions()
		{
			// Arrange
			_isis.Setup(x => x.GetAuthorizationContextAsync(It.IsAny<string>()))
				.Throws(new Exception(nameof(Exception)));
			SignInCommand command = new SignInCommand { Email = "test", Password = "test", RememberMe = true, ReturnUrl = "/" };
			SignInCommandHandler handler = new SignInCommandHandler(_logger.Object, _isis.Object, _es.Object, _userManager.Object, _signInManager.Object);

			// Act 
			Exception exception = Record.Exception(() => handler.Handle(command, new CancellationToken()).GetAwaiter().GetResult());

			// Assert
			exception.ShouldNotBeNull();
			exception.ShouldBeOfType<AuthenticationException>();
			((AuthenticationException)exception).Message.ShouldBe(IdentityProviderConstants.SomethingWentWrongWithAuthentication);
			((AuthenticationException)exception).InnerException.ShouldNotBeNull();
			((AuthenticationException)exception).InnerException!.Message.ShouldBe(nameof(Exception));
		}
	}
}
