namespace Aegis.UnitTests.Application.Commands.Auth.Handlers
{
	using global::Aegis.Application.Constants;
	using global::Aegis.Application.Exceptions;
	using global::Aegis.Application.Queries.Auth;
	using global::Aegis.Application.Queries.Auth.Handlers;
	using global::Aegis.Models.Auth;
	using global::Aegis.Persistence.Entities.IdentityProvider;

	using Microsoft.AspNetCore.Identity;

	using Moq;

	using Shouldly;

	public class ConfirmEmailQueryHandlerTests
	{
		private readonly Mock<ILogger<ConfirmEmailQueryHandler>> _logger = new Mock<ILogger<ConfirmEmailQueryHandler>>();
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

			ConfirmEmailQuery query = new ConfirmEmailQuery { UserId = "test", Token = "test" };
			ConfirmEmailQueryHandler handler = new ConfirmEmailQueryHandler(_logger.Object, _userManager.Object);

			// Act 
			EmailConfirmationQueryResult result = handler.Handle(query, new CancellationToken()).GetAwaiter().GetResult();

			// Assert
			result.ShouldNotBeNull();
			result.Success.ShouldBeTrue();
		}

		[Fact]
		public void Handle_ShouldReturnFalse_NotExistingUser()
		{
			// Arrange
			_userManager.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
				.ReturnsAsync((AegisUser?)null);

			ConfirmEmailQuery query = new ConfirmEmailQuery { UserId = "test", Token = "test" };
			ConfirmEmailQueryHandler handler = new ConfirmEmailQueryHandler(_logger.Object, _userManager.Object);

			// Act 
			EmailConfirmationQueryResult result = handler.Handle(query, new CancellationToken()).GetAwaiter().GetResult();

			// Assert
			result.ShouldNotBeNull();
			result.Success.ShouldBeFalse();
			result.Errors.Count.ShouldBe(1);
		}

		[Fact]
		public void Handle_ShouldReturnFalse_OnFailedToConfirmEmail()
		{
			AegisUser? user = new AegisUser();
			_userManager.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
				.ReturnsAsync(user);

			_userManager.Setup(x => x.ConfirmEmailAsync(It.IsAny<AegisUser>(), It.IsAny<string>()))
				.ReturnsAsync(IdentityResult.Failed(new IdentityError { Code = "test", Description = "test" }));

			ConfirmEmailQuery query = new ConfirmEmailQuery { UserId = "test", Token = "test" };
			ConfirmEmailQueryHandler handler = new ConfirmEmailQueryHandler(_logger.Object, _userManager.Object);

			// Act 
			EmailConfirmationQueryResult result = handler.Handle(query, new CancellationToken()).GetAwaiter().GetResult();

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

			ConfirmEmailQuery query = new ConfirmEmailQuery { UserId = "test", Token = "test" };
			ConfirmEmailQueryHandler handler = new ConfirmEmailQueryHandler(_logger.Object, _userManager.Object);

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
