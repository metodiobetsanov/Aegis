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
	using global::Aegis.Models.Auth;
	using global::Aegis.Models.Shared;
	using global::Aegis.Persistence.Entities.IdentityProvider;

	using Microsoft.AspNetCore.Identity;

	using Moq;

	using Shouldly;

	using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

	public class SignOutQueryHandlerTests
	{
		private readonly Mock<ILogger<SignOutQueryHandler>> _logger = new Mock<ILogger<SignOutQueryHandler>>();
		private readonly Mock<IIdentityServerInteractionService> _isis = new Mock<IIdentityServerInteractionService>();

		[Fact]
		public void Handle_ShouldReturnTrue()
		{
			// Arrange
			_isis.Setup(x => x.GetLogoutContextAsync(It.Is<string>(s => s == "test")))
				.ReturnsAsync(new LogoutRequest("test", null));

			SignOutQuery query = new SignOutQuery { LogoutId = "test" };
			SignOutQueryHandler handler = new SignOutQueryHandler(_logger.Object, _isis.Object);

			// Act 
			SignOutQueryResult result = handler.Handle(query, new CancellationToken()).GetAwaiter().GetResult();

			// Assert
			result.ShouldNotBeNull();
			result.ShowSignoutPrompt.ShouldBeTrue();
		}

		[Fact]
		public void Handle_ShouldReturnFalse()
		{
			// Arrange
			_isis.Setup(x => x.GetLogoutContextAsync(It.Is<string>(s => s == "test")))
				.ReturnsAsync(new LogoutRequest("test", new LogoutMessage { ClientId = "test" }));

			SignOutQuery query = new SignOutQuery { LogoutId = "test" };
			SignOutQueryHandler handler = new SignOutQueryHandler(_logger.Object, _isis.Object);

			// Act 
			SignOutQueryResult result = handler.Handle(query, new CancellationToken()).GetAwaiter().GetResult();

			// Assert
			result.ShouldNotBeNull();
			result.ShowSignoutPrompt.ShouldBeFalse();
		}

		[Fact]
		public void Handle_Exceptions()
		{
			// Arrange
			_isis.Setup(x => x.GetLogoutContextAsync(It.IsAny<string>()))
				.Throws(new Exception(nameof(Exception)));

			SignOutQuery query = new SignOutQuery();
			SignOutQueryHandler handler = new SignOutQueryHandler(_logger.Object, _isis.Object);

			// Act 
			Exception exception = Record.Exception(() => handler.Handle(query, new CancellationToken()).GetAwaiter().GetResult());

			// Assert
			exception.ShouldNotBeNull();
			exception.ShouldBeOfType<IdentityProviderException>();
			((IdentityProviderException)exception).Message.ShouldBe(IdentityProviderConstants.SomethingWentWrongWithSignOut);
			((IdentityProviderException)exception).InnerException.ShouldNotBeNull();
			((IdentityProviderException)exception).InnerException!.Message.ShouldBe(nameof(Exception));
		}
	}
}
