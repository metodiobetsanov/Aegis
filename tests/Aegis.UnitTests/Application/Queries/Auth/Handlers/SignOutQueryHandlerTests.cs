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
			AuthenticationResult result = handler.Handle(query, new CancellationToken()).GetAwaiter().GetResult();

			// Assert
			result.ShouldNotBeNull();
			result.Succeeded.ShouldBeTrue();
		}

		[Fact]
		public void Handle_ShouldReturnFalse()
		{
			// Arrange
			_isis.Setup(x => x.GetLogoutContextAsync(It.Is<string>(s => s == "test")))
				.ReturnsAsync(new LogoutRequest("test", null));

			SignOutQuery query = new SignOutQuery();
			SignOutQueryHandler handler = new SignOutQueryHandler(_logger.Object, _isis.Object);

			// Act 
			AuthenticationResult result = handler.Handle(query, new CancellationToken()).GetAwaiter().GetResult();

			// Assert
			result.ShouldNotBeNull();
			result.Succeeded.ShouldBeFalse();
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
			exception.ShouldBeOfType<AuthenticationException>();
			((AuthenticationException)exception).Message.ShouldBe(IdentityProviderConstants.SomethingWentWrongWithSignOut);
			((AuthenticationException)exception).InnerException.ShouldNotBeNull();
			((AuthenticationException)exception).InnerException!.Message.ShouldBe(nameof(Exception));
		}
	}
}
