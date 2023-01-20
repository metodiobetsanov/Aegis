﻿namespace Aegis.UnitTests.Application.Commands.Auth.Handlers
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

	using MediatR;

	using Microsoft.AspNetCore.Identity;

	using Moq;

	using Shouldly;

	public class SignUpCommandHandlerTests
	{
		private readonly Mock<ILogger<SignUpCommandHandler>> _logger = new Mock<ILogger<SignUpCommandHandler>>();
		private readonly Mock<IMediator> _m = new Mock<IMediator>();
		private readonly Mock<IIdentityServerInteractionService> _isis = new Mock<IIdentityServerInteractionService>();
		private readonly Mock<UserManager<AegisUser>> _userManager = Helper.GetUserManagerMock();
		private readonly Mock<RoleManager<AegisRole>> _roleManager = Helper.GetRoleManagerMock();

		[Theory]
		[InlineData(null)]
		[InlineData("/")]
		[InlineData("/test")]
		public void Handle_ShouldReturnTrue_OnValidUser(string? returnUrl)
		{
			// Arrange
			List<AegisRole> roles = new List<AegisRole> { new AegisRole("test", "test", true, true, true) };

			_isis.Setup(x => x.GetAuthorizationContextAsync(It.Is<string>(s => s == "/test")))
				.ReturnsAsync(new AuthorizationRequest(new ValidatedAuthorizeRequest { Client = new Client { ClientId = "test" } }));

			_userManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
				.ReturnsAsync((AegisUser?)null);

			_userManager.Setup(x => x.CreateAsync(It.IsAny<AegisUser>(), It.IsAny<string>()))
				.ReturnsAsync(IdentityResult.Success);

			_userManager.Setup(x => x.AddToRolesAsync(It.IsAny<AegisUser>(), It.IsAny<IEnumerable<string>>()))
				.ReturnsAsync(IdentityResult.Success);

			_roleManager.Setup(x => x.Roles)
				.Returns(roles.AsQueryable());

			SignUpCommand command = new SignUpCommand { Email = "test@test.test", Password = "AAAaaa000@@@", ConfirmPassword = "AAAaaa000@@@", AcceptTerms = true, ReturnUrl = returnUrl };
			SignUpCommandHandler handler = new SignUpCommandHandler(_logger.Object, _m.Object, _isis.Object, _userManager.Object, _roleManager.Object);

			// Act 
			SignUpCommandResult result = handler.Handle(command, new CancellationToken()).GetAwaiter().GetResult();

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
		public void Handle_ShouldReturnTrue_OnValidUserNoRoles()
		{
			// Arrange
			List<AegisRole> roles = new List<AegisRole>();

			_userManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
				.ReturnsAsync((AegisUser?)null);

			_userManager.Setup(x => x.CreateAsync(It.IsAny<AegisUser>(), It.IsAny<string>()))
				.ReturnsAsync(IdentityResult.Success);

			_userManager.Setup(x => x.AddToRolesAsync(It.IsAny<AegisUser>(), It.IsAny<IEnumerable<string>>()))
				.ReturnsAsync(IdentityResult.Success);

			_roleManager.Setup(x => x.Roles)
				.Returns(roles.AsQueryable());

			SignUpCommand command = new SignUpCommand { Email = "test@test.test", Password = "AAAaaa000@@@", ConfirmPassword = "AAAaaa000@@@", AcceptTerms = true };
			SignUpCommandHandler handler = new SignUpCommandHandler(_logger.Object, _m.Object, _isis.Object, _userManager.Object, _roleManager.Object);

			// Act 
			SignUpCommandResult result = handler.Handle(command, new CancellationToken()).GetAwaiter().GetResult();

			// Assert
			result.ShouldNotBeNull();
			result.Success.ShouldBeTrue();
			result.ReturnUrl.ShouldBe("~/");
		}

		[Fact]
		public void Handle_ShouldReturnFalse_OnExistingUser()
		{
			// Arrange
			AegisUser user = new AegisUser();

			_isis.Setup(x => x.GetAuthorizationContextAsync(It.Is<string>(s => s == "/test")))
				.ReturnsAsync(new AuthorizationRequest(new ValidatedAuthorizeRequest { Client = new Client { ClientId = "test" } }));

			_userManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
				.ReturnsAsync(user);

			SignUpCommand command = new SignUpCommand { Email = "test@test.test", Password = "AAAaaa000@@@", ConfirmPassword = "AAAaaa000@@@", AcceptTerms = true };
			SignUpCommandHandler handler = new SignUpCommandHandler(_logger.Object, _m.Object, _isis.Object, _userManager.Object, _roleManager.Object);

			// Act 
			SignUpCommandResult result = handler.Handle(command, new CancellationToken()).GetAwaiter().GetResult();

			// Assert
			result.ShouldNotBeNull();
			result.Success.ShouldBeFalse();
			result.Errors.Count.ShouldBe(1);
		}

		[Fact]
		public void Handle_ShouldReturnFalse_OnFailedToCreateUser()
		{
			List<AegisRole> roles = new List<AegisRole> { new AegisRole("test", "test", true, true, true) };

			_isis.Setup(x => x.GetAuthorizationContextAsync(It.Is<string>(s => s == "/test")))
				.ReturnsAsync(new AuthorizationRequest(new ValidatedAuthorizeRequest { Client = new Client { ClientId = "test" } }));

			_userManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
				.ReturnsAsync((AegisUser?)null);

			_userManager.Setup(x => x.CreateAsync(It.IsAny<AegisUser>(), It.IsAny<string>()))
				.ReturnsAsync(IdentityResult.Failed(new IdentityError { Code = "test", Description = "test" }));

			_roleManager.Setup(x => x.Roles)
				.Returns(roles.AsQueryable());

			SignUpCommand command = new SignUpCommand { Email = "test@test.test", Password = "AAAaaa000@@@", ConfirmPassword = "AAAaaa000@@@", AcceptTerms = true };
			SignUpCommandHandler handler = new SignUpCommandHandler(_logger.Object, _m.Object, _isis.Object, _userManager.Object, _roleManager.Object);

			// Act 
			SignUpCommandResult result = handler.Handle(command, new CancellationToken()).GetAwaiter().GetResult();

			// Assert
			result.ShouldNotBeNull();
			result.Success.ShouldBeFalse();
			result.Errors.Count.ShouldBe(1);
		}

		[Fact]
		public void Handle_ShouldReturnFalse_OnFailedToAssignRoles()
		{
			List<AegisRole> roles = new List<AegisRole> { new AegisRole("test", "test", true, true, true) };

			_isis.Setup(x => x.GetAuthorizationContextAsync(It.Is<string>(s => s == "/test")))
				.ReturnsAsync(new AuthorizationRequest(new ValidatedAuthorizeRequest { Client = new Client { ClientId = "test" } }));

			_userManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
				.ReturnsAsync((AegisUser?)null);

			_userManager.Setup(x => x.CreateAsync(It.IsAny<AegisUser>(), It.IsAny<string>()))
				.ReturnsAsync(IdentityResult.Success);

			_userManager.Setup(x => x.AddToRolesAsync(It.IsAny<AegisUser>(), It.IsAny<IEnumerable<string>>()))
				.ReturnsAsync(IdentityResult.Failed(new IdentityError { Code = "test", Description = "test" }));

			_roleManager.Setup(x => x.Roles)
				.Returns(roles.AsQueryable());

			SignUpCommand command = new SignUpCommand { Email = "test@test.test", Password = "AAAaaa000@@@", ConfirmPassword = "AAAaaa000@@@", AcceptTerms = true };
			SignUpCommandHandler handler = new SignUpCommandHandler(_logger.Object, _m.Object, _isis.Object, _userManager.Object, _roleManager.Object);

			// Act 
			SignUpCommandResult result = handler.Handle(command, new CancellationToken()).GetAwaiter().GetResult();

			// Assert
			result.ShouldNotBeNull();
			result.Success.ShouldBeFalse();
			result.Errors.Count.ShouldBe(1);
		}

		[Fact]
		public void Handle_ShouldThrowExceptions_ReturnUrl()
		{
			// Arrange
			SignUpCommand command = new SignUpCommand { ReturnUrl = "https://test.test" };
			SignUpCommandHandler handler = new SignUpCommandHandler(_logger.Object, _m.Object, _isis.Object, _userManager.Object, _roleManager.Object);

			// Act 
			Exception exception = Record.Exception(() => handler.Handle(command, new CancellationToken()).GetAwaiter().GetResult());

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

			SignUpCommand command = new SignUpCommand { Email = "test@test.test", Password = "AAAaaa000@@@", ConfirmPassword = "AAAaaa000@@@", AcceptTerms = true };
			SignUpCommandHandler handler = new SignUpCommandHandler(_logger.Object, _m.Object, _isis.Object, _userManager.Object, _roleManager.Object);

			// Act 
			Exception exception = Record.Exception(() => handler.Handle(command, new CancellationToken()).GetAwaiter().GetResult());

			// Assert
			exception.ShouldNotBeNull();
			exception.ShouldBeOfType<IdentityProviderException>();
			((IdentityProviderException)exception).Message.ShouldBe(IdentityProviderConstants.SomethingWentWrongWithSignUp);
			((IdentityProviderException)exception).InnerException.ShouldNotBeNull();
			((IdentityProviderException)exception).InnerException!.Message.ShouldBe(nameof(Exception));
		}
	}
}