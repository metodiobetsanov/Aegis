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

	using global::Aegis.Core.Constants;
	using global::Aegis.Core.Constants.Services;
	using global::Aegis.Core.Exceptions;
	using global::Aegis.Core.Queries.Authentication;
	using global::Aegis.Core.Queries.Authentication.Handlers;
	using global::Aegis.Models.Authentication;
	using global::Aegis.Persistence.Entities.IdentityProvider;

	public class SignInTwoStepQueryHandlerTests
	{
		private static readonly Faker _faker = new Faker("en");
		private static readonly Faker<AegisUser> _fakeUser = Helper.GetUserFaker();

		private readonly Mock<ILogger<SignInTwoStepQueryHandler>> _logger = new Mock<ILogger<SignInTwoStepQueryHandler>>();
		private readonly Mock<IIdentityServerInteractionService> _isis = new Mock<IIdentityServerInteractionService>();
		private readonly Mock<IEventService> _es = new Mock<IEventService>();
		private readonly Mock<IMailSenderService> _mss = new Mock<IMailSenderService>();
		private readonly Mock<UserManager<AegisUser>> _userManager = Helper.GetUserManagerMock();
		private readonly Mock<SignInManager<AegisUser>> _signInManager = Helper.GetSignInManagerMock();

		[Theory]
		[InlineData(null)]
		[InlineData("")]
		[InlineData("  ")]
		[InlineData("/")]
		[InlineData("/test")]
		public void Handle_ShouldReturnTrue_OnValidUser(string returnUrl)
		{
			// Arrange
			AegisUser? user = _fakeUser.Generate();
			_isis.Setup(x => x.GetAuthorizationContextAsync(It.Is<string>(s => s == "/valid")))
				.ReturnsAsync(new AuthorizationRequest(new ValidatedAuthorizeRequest { Client = new Client { ClientId = _faker.Random.String2(12) } }));

			_signInManager.Setup(x => x.GetTwoFactorAuthenticationUserAsync())
				.ReturnsAsync(user);

			SignInTwoStepQuery query = new SignInTwoStepQuery { RememberMe = true, ReturnUrl = returnUrl };
			SignInTwoStepQueryHandler handler = new SignInTwoStepQueryHandler(_logger.Object, _isis.Object, _es.Object, _mss.Object, _userManager.Object, _signInManager.Object);

			// Act 
			SignInQueryResult result = handler.Handle(query, new CancellationToken()).GetAwaiter().GetResult();

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

		[Fact]
		public void Handle_ShouldReturnFalse_OnNotExistingUser()
		{
			// Arrange
			SignInTwoStepQuery query = new SignInTwoStepQuery();
			SignInTwoStepQueryHandler handler = new SignInTwoStepQueryHandler(_logger.Object, _isis.Object, _es.Object, _mss.Object, _userManager.Object, _signInManager.Object);

			// Act 
			SignInQueryResult result = handler.Handle(query, new CancellationToken()).GetAwaiter().GetResult();

			// Assert
			result.ShouldNotBeNull();
			result.Success.ShouldBeFalse();
		}

		[Fact]
		public void Handle_ShouldThrowExceptions_ReturnUrl()
		{
			// Arrange
			SignInTwoStepQuery query = new SignInTwoStepQuery { RememberMe = true, ReturnUrl = _faker.Internet.Url() };
			SignInTwoStepQueryHandler handler = new SignInTwoStepQueryHandler(_logger.Object, _isis.Object, _es.Object, _mss.Object, _userManager.Object, _signInManager.Object);

			// Act 
			Exception exception = Record.Exception(() => handler.Handle(query, new CancellationToken()).GetAwaiter().GetResult());

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

			SignInTwoStepQuery query = new SignInTwoStepQuery { RememberMe = true, ReturnUrl = "/" };
			SignInTwoStepQueryHandler handler = new SignInTwoStepQueryHandler(_logger.Object, _isis.Object, _es.Object, _mss.Object, _userManager.Object, _signInManager.Object);

			// Act 
			Exception exception = Record.Exception(() => handler.Handle(query, new CancellationToken()).GetAwaiter().GetResult());

			// Assert
			exception.ShouldNotBeNull();
			exception.ShouldBeOfType<IdentityProviderException>();
			((IdentityProviderException)exception).Message.ShouldBe(IdentityProviderConstants.SomethingWentWrongWithSignIn);
			((IdentityProviderException)exception).InnerException.ShouldNotBeNull();
			((IdentityProviderException)exception).InnerException!.Message.ShouldBe(nameof(Exception));
		}
	}
}
