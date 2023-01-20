namespace Aegis.UnitTests.Application.Commands.Auth.Handlers
{
	using global::Aegis.Application.Constants;
	using global::Aegis.Application.Constants.Services;
	using global::Aegis.Application.Exceptions;
	using global::Aegis.Application.Queries.Auth;
	using global::Aegis.Application.Queries.Auth.Handlers;
	using global::Aegis.Models.Auth;
	using global::Aegis.Models.Settings;
	using global::Aegis.Persistence.Entities.IdentityProvider;

	using Microsoft.AspNetCore.Identity;

	using Moq;

	using Shouldly;

	public class EmailConfirmationQueryHandlerTests
	{
		private readonly Mock<ILogger<EmailConfirmationQueryHandler>> _logger = new Mock<ILogger<EmailConfirmationQueryHandler>>();
		private readonly Mock<IMailSenderService> _mss = new Mock<IMailSenderService>();
		private readonly Mock<UserManager<AegisUser>> _userManager = Helper.GetUserManagerMock();

		private readonly AppSettings _ap = new AppSettings { PublicDomain = "test" };

		[Fact]
		public void Handle_ShouldReturnTrue()
		{
			// Arrange
			AegisUser? user = new AegisUser();
			_userManager.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
				.ReturnsAsync(user);

			_userManager.Setup(x => x.GenerateEmailConfirmationTokenAsync(It.IsAny<AegisUser>()))
				.ReturnsAsync("test");

			EmailConfirmationQuery query = new EmailConfirmationQuery { UserId = "test" };
			EmailConfirmationQueryHandler handler = new EmailConfirmationQueryHandler(_logger.Object, _mss.Object, _ap, _userManager.Object);

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

			EmailConfirmationQuery query = new EmailConfirmationQuery { UserId = "test" };
			EmailConfirmationQueryHandler handler = new EmailConfirmationQueryHandler(_logger.Object, _mss.Object, _ap, _userManager.Object);

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

			EmailConfirmationQuery query = new EmailConfirmationQuery { UserId = "test" };
			EmailConfirmationQueryHandler handler = new EmailConfirmationQueryHandler(_logger.Object, _mss.Object, _ap, _userManager.Object);

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
