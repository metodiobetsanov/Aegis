namespace Aegis.UnitTests.Core.Commands.Authentication.Handlers
{
	using global::Aegis.Core.Commands.Authentication;
	using global::Aegis.Core.Commands.Authentication.Handlers;
	using global::Aegis.Core.Constants;
	using global::Aegis.Core.Constants.Services;
	using global::Aegis.Core.Exceptions;
	using global::Aegis.Models.Settings;
	using global::Aegis.Models.Shared;
	using global::Aegis.Persistence.Entities.IdentityProvider;

	public class SendForgetPasswordCommandHandlerTests
	{
		private static readonly Faker _faker = new Faker("en");

		private readonly Mock<ILogger<SendForgetPasswordCommandHandler>> _logger = new Mock<ILogger<SendForgetPasswordCommandHandler>>();
		private readonly Mock<IMediator> _m = new Mock<IMediator>();
		private readonly Mock<IDataProtector> _dp = new Mock<IDataProtector>();
		private readonly Mock<IDataProtectionProvider> _dpp = new Mock<IDataProtectionProvider>();
		private readonly Mock<IMailSenderService> _mss = new Mock<IMailSenderService>();
		private readonly Mock<UserManager<AegisUser>> _userManager = Helper.GetUserManagerMock();

		private readonly AppSettings _ap = new AppSettings { PublicDomain = _faker.Internet.DomainName() };

		public SendForgetPasswordCommandHandlerTests()
		{
			_dp.Setup(sut => sut.Protect(It.IsAny<byte[]>())).Returns(Encoding.UTF8.GetBytes(_faker.Random.String2(36)));
			_dp.Setup(sut => sut.Unprotect(It.IsAny<byte[]>())).Returns(Encoding.UTF8.GetBytes(_faker.Random.String2(36)));
			_dpp.Setup(x => x.CreateProtector(It.IsAny<string>())).Returns(_dp.Object);
		}

		[Fact]
		public void Handle_ShouldReturnTrue()
		{
			// Arrange
			AegisUser? user = new AegisUser();
			_userManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
				.ReturnsAsync(user);

			_userManager.Setup(x => x.GenerateEmailConfirmationTokenAsync(It.IsAny<AegisUser>()))
				.ReturnsAsync(_faker.Random.String2(36));

			SendForgetPasswordCommand command = new SendForgetPasswordCommand { Email = _faker.Internet.Email() };
			SendForgetPasswordCommandHandler handler = new SendForgetPasswordCommandHandler(_logger.Object, _dpp.Object, _m.Object, _mss.Object, _ap, _userManager.Object);

			// Act 
			HandlerResult result = handler.Handle(command, new CancellationToken()).GetAwaiter().GetResult();

			// Assert
			result.ShouldNotBeNull();
			result.Success.ShouldBeTrue();
		}

		[Fact]
		public void Handle_ShouldReturnTrue_NotExistingUser()
		{
			// Arrange
			_userManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
				.ReturnsAsync((AegisUser?)null);

			SendForgetPasswordCommand command = new SendForgetPasswordCommand { Email = _faker.Internet.Email() };
			SendForgetPasswordCommandHandler handler = new SendForgetPasswordCommandHandler(_logger.Object, _dpp.Object, _m.Object, _mss.Object, _ap, _userManager.Object);

			// Act 
			HandlerResult result = handler.Handle(command, new CancellationToken()).GetAwaiter().GetResult();

			// Assert
			result.ShouldNotBeNull();
			result.Success.ShouldBeTrue();
		}

		[Fact]
		public void Handle_Exceptions()
		{
			// Arrange
			_userManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
				.Throws(new Exception(nameof(Exception)));

			SendForgetPasswordCommand command = new SendForgetPasswordCommand { Email = _faker.Internet.Email() };
			SendForgetPasswordCommandHandler handler = new SendForgetPasswordCommandHandler(_logger.Object, _dpp.Object, _m.Object, _mss.Object, _ap, _userManager.Object);

			// Act 
			Exception exception = Record.Exception(() => handler.Handle(command, new CancellationToken()).GetAwaiter().GetResult());

			// Assert
			exception.ShouldNotBeNull();
			exception.ShouldBeOfType<IdentityProviderException>();
			((IdentityProviderException)exception).Message.ShouldBe(IdentityProviderConstants.SomethingWentWrong);
			((IdentityProviderException)exception).InnerException.ShouldNotBeNull();
			((IdentityProviderException)exception).InnerException!.Message.ShouldBe(nameof(Exception));
		}
	}
}
