namespace Aegis.UnitTests.Application.Commands.Auth.Handlers
{
	using System.Security.Claims;

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

	using IdentityModel;

	using Microsoft.AspNetCore.Identity;

	using Moq;

	using Shouldly;

	public class SignOutCommandHandlerTests
	{
		private readonly Mock<ILogger<SignOutCommandHandler>> _logger = new Mock<ILogger<SignOutCommandHandler>>();
		private readonly Mock<IIdentityServerInteractionService> _isis = new Mock<IIdentityServerInteractionService>();
		private readonly Mock<IEventService> _es = new Mock<IEventService>();
		private readonly Mock<HttpContext> _hc = new Mock<HttpContext>();
		private readonly Mock<ClaimsPrincipal> _hccp = new Mock<ClaimsPrincipal>();
		private readonly Mock<IHttpContextAccessor> _hca = new Mock<IHttpContextAccessor>();
		private readonly Mock<SignInManager<AegisUser>> _signInManager = Helper.GetSignInManagerMock();

		public SignOutCommandHandlerTests()
		{
			_hccp.Setup(x => x.Identity)
				.Returns(new ClaimsIdentity(
					 new List<Claim>
						{
							new Claim(JwtClaimTypes.Subject, "test")
						}));
			_hc.Setup(x => x.User).Returns(_hccp.Object);
			_hca.Setup(x => x.HttpContext).Returns(_hc.Object);
		}

		[Theory]
		[InlineData(null)]
		[InlineData("")]
		[InlineData("test")]
		[InlineData("something")]
		public void Handle_ShouldReturnTrue(string logoutId)
		{
			// Arrange
			_isis.Setup(x => x.CreateLogoutContextAsync())
				.ReturnsAsync(logoutId);
			_isis.Setup(x => x.GetLogoutContextAsync(It.Is<string>(s => s == "test")))
				.ReturnsAsync(new LogoutRequest("test", new LogoutMessage { PostLogoutRedirectUri = "test" }));

			SignOutCommand command = new SignOutCommand { LogoutId = logoutId };
			SignOutCommandHandler handler = new SignOutCommandHandler(_logger.Object, _hca.Object, _isis.Object, _es.Object, _signInManager.Object);

			// Act 
			SignOutCommandResult result = handler.Handle(command, new CancellationToken()).GetAwaiter().GetResult();

			// Assert
			result.ShouldNotBeNull();
			result.Success.ShouldBeTrue();
		}

		[Fact]
		public void Handle_Exceptions()
		{
			// Arrange
			_isis.Setup(x => x.CreateLogoutContextAsync())
				.Throws(new Exception(nameof(Exception)));
			SignOutCommand command = new SignOutCommand();
			SignOutCommandHandler handler = new SignOutCommandHandler(_logger.Object, _hca.Object, _isis.Object, _es.Object, _signInManager.Object);

			// Act 
			Exception exception = Record.Exception(() => handler.Handle(command, new CancellationToken()).GetAwaiter().GetResult());

			// Assert
			exception.ShouldNotBeNull();
			exception.ShouldBeOfType<IdentityProviderException>();
			((IdentityProviderException)exception).Message.ShouldBe(IdentityProviderConstants.SomethingWentWrongWithSignOut);
			((IdentityProviderException)exception).InnerException.ShouldNotBeNull();
			((IdentityProviderException)exception).InnerException!.Message.ShouldBe(nameof(Exception));
		}
	}
}
