namespace Aegis.UnitTests.Application.Commands.Authentication.Handlers
{
	using global::Aegis.Application.Commands.Authentication;
	using global::Aegis.Application.Commands.Authentication.Handlers;
	using global::Aegis.Application.Constants;
	using global::Aegis.Application.Constants.Services;
	using global::Aegis.Application.Exceptions;
	using global::Aegis.Application.Queries.Authentication;
	using global::Aegis.Application.Queries.Authentication.Handlers;
	using global::Aegis.Models.Authentication;
	using global::Aegis.Models.Settings;
	using global::Aegis.Models.Shared;
	using global::Aegis.Persistence.Entities.IdentityProvider;

	public class SendAccountActivationCommandHandlerTests
	{
		private static readonly Faker _faker = new Faker("en");

		private readonly Mock<ILogger<SendAccountActivationCommandHandler>> _logger = new Mock<ILogger<SendAccountActivationCommandHandler>>();
		private readonly Mock<IMailSenderService> _mss = new Mock<IMailSenderService>();
		private readonly Mock<UserManager<AegisUser>> _userManager = Helper.GetUserManagerMock();

		private readonly AppSettings _ap = new AppSettings { PublicDomain = _faker.Internet.DomainName() };

		[Fact]
		public void Handle_ShouldReturnTrue()
		{
			// Arrange
			AegisUser? user = new AegisUser();
			_userManager.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
				.ReturnsAsync(user);

			_userManager.Setup(x => x.GenerateEmailConfirmationTokenAsync(It.IsAny<AegisUser>()))
				.ReturnsAsync(_faker.Random.String(36));

			SendAccountActivationCommand query = new SendAccountActivationCommand { UserId = _faker.Random.Guid().ToString() };
			SendAccountActivationCommandHandler handler = new SendAccountActivationCommandHandler(_logger.Object, _mss.Object, _ap, _userManager.Object);

			// Act 
			HandlerResult result = handler.Handle(query, new CancellationToken()).GetAwaiter().GetResult();

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

			SendAccountActivationCommand query = new SendAccountActivationCommand { UserId = _faker.Random.Guid().ToString() };
			SendAccountActivationCommandHandler handler = new SendAccountActivationCommandHandler(_logger.Object, _mss.Object, _ap, _userManager.Object);

			// Act 
			HandlerResult result = handler.Handle(query, new CancellationToken()).GetAwaiter().GetResult();

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

			SendAccountActivationCommand query = new SendAccountActivationCommand { UserId = _faker.Random.Guid().ToString() };
			SendAccountActivationCommandHandler handler = new SendAccountActivationCommandHandler(_logger.Object, _mss.Object, _ap, _userManager.Object);

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
