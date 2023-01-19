namespace Aegis.UnitTests.Application.Commands.Auth.Handlers
{

	using Duende.IdentityServer.Models;
	using Duende.IdentityServer.Services;
	using Duende.IdentityServer.Validation;

	using global::Aegis.Application.Commands.Auth;
	using global::Aegis.Application.Commands.Auth.Handlers;
	using global::Aegis.Application.Constants;
	using global::Aegis.Application.Exceptions;
	using global::Aegis.Application.Queries.Auth;
	using global::Aegis.Application.Queries.Auth.Handlers;
	using global::Aegis.Models.Shared;
	using global::Aegis.Persistence.Entities.IdentityProvider;

	using Microsoft.AspNetCore.Identity;

	using Moq;

	using Shouldly;

	using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

	public class SignInQueryHandlerTests
	{
		private readonly Mock<ILogger<SignInQueryHandler>> _logger = new Mock<ILogger<SignInQueryHandler>>();
		private readonly Mock<IIdentityServerInteractionService> _isis = new Mock<IIdentityServerInteractionService>();

		[Theory]
		[InlineData(null)]
		[InlineData("")]
		[InlineData("   ")]
		[InlineData("/")]
		[InlineData("/test")]
		public void Handle_ShouldReturnTrue_OnValidUser(string returnUrl)
		{
			// Arrange
			_isis.Setup(x => x.GetAuthorizationContextAsync(It.Is<string>(s => s == "/test")))
				.ReturnsAsync(new AuthorizationRequest(new ValidatedAuthorizeRequest { Client = new Client { ClientId = "test" } }));

			SignInQuery query = new SignInQuery { ReturnUrl = returnUrl };
			SignInQueryHandler handler = new SignInQueryHandler(_logger.Object, _isis.Object);

			// Act 
			AuthenticationResult result = handler.Handle(query, new CancellationToken()).GetAwaiter().GetResult();

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
			exception.ShouldBeOfType<AuthenticationException>();
			((AuthenticationException)exception).Message.ShouldBe(IdentityProviderConstants.SomethingWentWrongWithAuthentication);
			((AuthenticationException)exception).InnerException.ShouldNotBeNull();
			((AuthenticationException)exception).InnerException!.Message.ShouldBe(nameof(Exception));
		}
	}
}
