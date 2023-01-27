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
	using global::Aegis.Core.Exceptions;
	using global::Aegis.Core.Queries.Authentication;
	using global::Aegis.Core.Queries.Authentication.Handlers;
	using global::Aegis.Models.Authentication;

	public class SignInQueryHandlerTests
	{
		private static readonly Faker _faker = new Faker("en");

		private readonly Mock<ILogger<SignInQueryHandler>> _logger = new Mock<ILogger<SignInQueryHandler>>();
		private readonly Mock<IIdentityServerInteractionService> _isis = new Mock<IIdentityServerInteractionService>();

		[Theory]
		[InlineData(null)]
		[InlineData("")]
		[InlineData("  ")]
		[InlineData("/")]
		[InlineData("/test")]
		public void Handle_ShouldReturnTrue_OnValidUser(string returnUrl)
		{
			// Arrange
			_isis.Setup(x => x.GetAuthorizationContextAsync(It.Is<string>(s => s == "/test")))
				.ReturnsAsync(new AuthorizationRequest(new ValidatedAuthorizeRequest { Client = new Client { ClientId = _faker.Random.String2(12) } }));

			SignInQuery query = new SignInQuery { ReturnUrl = returnUrl };
			SignInQueryHandler handler = new SignInQueryHandler(_logger.Object, _isis.Object);

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
		public void Handle_ShouldThrowExceptions_ReturnUrl()
		{
			// Arrange
			SignInQuery query = new SignInQuery { ReturnUrl = _faker.Internet.Url() };
			SignInQueryHandler handler = new SignInQueryHandler(_logger.Object, _isis.Object);

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

			SignInQuery query = new SignInQuery();
			SignInQueryHandler handler = new SignInQueryHandler(_logger.Object, _isis.Object);

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
