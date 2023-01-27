#region copyright
//----------------------------------------------------------------------
// Copyright 2023 MNB Software
// Licensed under the Apache License, Version 2.0
// You may obtain a copy at http://www.apache.org/licenses/LICENSE-2.0
//----------------------------------------------------------------------
#endregion
namespace Aegis.UnitTests.Core.Commands.Authentication.Handlers
{
	using System.Security.Claims;

	using Duende.IdentityServer.Models;
	using Duende.IdentityServer.Services;

	using global::Aegis.Core.Commands.Authentication;
	using global::Aegis.Core.Commands.Authentication.Handlers;
	using global::Aegis.Core.Constants;
	using global::Aegis.Core.Exceptions;
	using global::Aegis.Models.Authentication;
	using global::Aegis.Persistence.Entities.IdentityProvider;

	using IdentityModel;

	using Microsoft.AspNetCore.Identity;

	public class SignOutCommandHandlerTests
	{
		private static readonly Faker _faker = new Faker("en");
		private static readonly Faker<AegisUser> _fakeUser = Helper.GetUserFaker();
		private static readonly Guid _subId = _faker.Random.Guid();

		private readonly Mock<ILogger<SignOutCommandHandler>> _logger = new Mock<ILogger<SignOutCommandHandler>>();
		private readonly Mock<IIdentityServerInteractionService> _isis = new Mock<IIdentityServerInteractionService>();
		private readonly Mock<IEventService> _es = new Mock<IEventService>();
		private readonly Mock<HttpContext> _hc = new Mock<HttpContext>();
		private readonly Mock<ClaimsPrincipal> _hccp = new Mock<ClaimsPrincipal>();
		private readonly Mock<IHttpContextAccessor> _hca = new Mock<IHttpContextAccessor>();
		private readonly Mock<UserManager<AegisUser>> _userManager = Helper.GetUserManagerMock();
		private readonly Mock<SignInManager<AegisUser>> _signInManager = Helper.GetSignInManagerMock();

		public SignOutCommandHandlerTests()
		{
			_hccp.Setup(x => x.Identity)
				.Returns(new ClaimsIdentity(
					 new List<Claim>
						{
							new Claim(JwtClaimTypes.Subject, _subId.ToString())
						}));
			_hc.Setup(x => x.User).Returns(_hccp.Object);
			_hca.Setup(x => x.HttpContext).Returns(_hc.Object);
		}

		[Theory]
		[InlineData(null)]
		[InlineData("")]
		[InlineData("valid")]
		[InlineData("invalid")]
		public void Handle_ShouldReturnTrue(string logoutId)
		{
			// Arrange
			AegisUser? user = _fakeUser.Generate();

			_isis.Setup(x => x.CreateLogoutContextAsync())
				.ReturnsAsync(logoutId);

			_userManager.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
				.ReturnsAsync(user);

			_userManager.Setup(x => x.UpdateSecurityStampAsync(It.IsAny<AegisUser>()))
				.ReturnsAsync(IdentityResult.Success);

			_isis.Setup(x => x.GetLogoutContextAsync(It.Is<string>(s => s == "valid")))
				.ReturnsAsync(new LogoutRequest(_faker.Internet.Url(), new LogoutMessage { PostLogoutRedirectUri = _faker.Internet.Url() }));

			SignOutCommand command = new SignOutCommand { LogoutId = logoutId, ForgetClient = false, SignOutAllSessions = false };
			SignOutCommandHandler handler = new SignOutCommandHandler(_logger.Object, _hca.Object, _isis.Object, _es.Object, _userManager.Object, _signInManager.Object);

			// Act 
			SignOutCommandResult result = handler.Handle(command, new CancellationToken()).GetAwaiter().GetResult();

			// Assert
			result.ShouldNotBeNull();
			result.Success.ShouldBeTrue();
		}

		[Theory]
		[InlineData(null)]
		[InlineData("")]
		[InlineData("valid")]
		[InlineData("invalid")]
		public void Handle_ShouldReturnTrue_OnForgetAll(string logoutId)
		{
			// Arrange
			AegisUser? user = _fakeUser.Generate();

			_isis.Setup(x => x.CreateLogoutContextAsync())
				.ReturnsAsync(logoutId);

			_userManager.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
				.ReturnsAsync(user);

			_userManager.Setup(x => x.UpdateSecurityStampAsync(It.IsAny<AegisUser>()))
				.ReturnsAsync(IdentityResult.Success);

			_isis.Setup(x => x.GetLogoutContextAsync(It.Is<string>(s => s == "valid")))
				.ReturnsAsync(new LogoutRequest(_faker.Internet.Url(), new LogoutMessage { PostLogoutRedirectUri = _faker.Internet.Url() }));

			SignOutCommand command = new SignOutCommand { LogoutId = logoutId, ForgetClient = true, SignOutAllSessions = true };
			SignOutCommandHandler handler = new SignOutCommandHandler(_logger.Object, _hca.Object, _isis.Object, _es.Object, _userManager.Object, _signInManager.Object);

			// Act 
			SignOutCommandResult result = handler.Handle(command, new CancellationToken()).GetAwaiter().GetResult();

			// Assert
			result.ShouldNotBeNull();
			result.Success.ShouldBeTrue();
		}

		[Fact]
		public void Handle_ShouldReturnFalse_OnUpdateSecurityStamp()
		{
			// Arrange
			AegisUser? user = _fakeUser.Generate();

			_userManager.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
				.ReturnsAsync(user);

			_userManager.Setup(x => x.UpdateSecurityStampAsync(It.IsAny<AegisUser>()))
				.ReturnsAsync(IdentityResult.Failed(new IdentityError { Code = _faker.Random.String2(12), Description = _faker.Random.String2(36) }));

			_isis.Setup(x => x.GetLogoutContextAsync(It.Is<string>(s => s == "valid")))
				.ReturnsAsync(new LogoutRequest(_faker.Internet.Url(), new LogoutMessage { PostLogoutRedirectUri = _faker.Internet.Url() }));

			SignOutCommand command = new SignOutCommand { LogoutId = _faker.Random.String2(12), ForgetClient = _faker.Random.Bool(), SignOutAllSessions = true };
			SignOutCommandHandler handler = new SignOutCommandHandler(_logger.Object, _hca.Object, _isis.Object, _es.Object, _userManager.Object, _signInManager.Object);

			// Act 
			SignOutCommandResult result = handler.Handle(command, new CancellationToken()).GetAwaiter().GetResult();

			// Assert
			result.ShouldNotBeNull();
			result.Success.ShouldBeFalse();
			result.Errors.Count.ShouldBe(1);
		}

		[Fact]
		public void Handle_Exceptions()
		{
			// Arrange
			_isis.Setup(x => x.CreateLogoutContextAsync())
				.Throws(new Exception(nameof(Exception)));
			SignOutCommand command = new SignOutCommand();
			SignOutCommandHandler handler = new SignOutCommandHandler(_logger.Object, _hca.Object, _isis.Object, _es.Object, _userManager.Object, _signInManager.Object);

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
