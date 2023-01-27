namespace Aegis.UnitTests.Application.Controllers
{
	using global::Aegis.Controllers;
	using global::Aegis.Core.Commands.Authentication;
	using global::Aegis.Core.Queries.Authentication;
	using global::Aegis.Models.Authentication;
	using global::Aegis.Models.Shared;

	using Microsoft.AspNetCore.Mvc;

	public partial class AuthenticationControllerTests
	{
		#region ForgotPassword
		[Fact]
		public void GetForgotPassword_ShouldReturnView()
		{
			// Arrange

			AuthenticationController controller = new AuthenticationController(_logger.Object, _dpp.Object, _m.Object);
			controller.ControllerContext.HttpContext = _hc.Object;

			// Act
			IActionResult result = controller.ForgotPassword();

			// Assert
			result.ShouldNotBeNull();
			result.ShouldBeOfType<ViewResult>();
			((ViewResult)result).ViewData.Model.ShouldBeAssignableTo<SendForgetPasswordCommand>();
		}

		[Fact]
		public void GetForgotPassword_ShouldReturnRedirect_OnAutheticatedUser()
		{
			// Arrange
			_cp.Setup(x => x.Identity!.IsAuthenticated)
				.Returns(true);
			_m.Setup(x => x.Send(It.IsAny<SignInQuery>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(SignInQueryResult.Succeeded("~/"));

			AuthenticationController controller = new AuthenticationController(_logger.Object, _dpp.Object, _m.Object);
			controller.ControllerContext.HttpContext = _hc.Object;

			// Act
			IActionResult result = controller.ForgotPassword();

			// Assert
			result.ShouldNotBeNull();
			result.ShouldBeOfType<RedirectResult>();
			((RedirectResult)result).Url.ShouldBe("~/");
		}

		[Fact]
		public void PostForgotPassword_ShouldReturnView()
		{
			// Arrange
			SendForgetPasswordCommand command = new SendForgetPasswordCommand { Email = _faker.Internet.Email() };
			AuthenticationController controller = new AuthenticationController(_logger.Object, _dpp.Object, _m.Object);
			controller.ControllerContext.HttpContext = _hc.Object;

			// Act
			IActionResult result = controller.ForgotPassword(command).GetAwaiter().GetResult();

			// Assert
			result.ShouldNotBeNull();
			result.ShouldBeOfType<ViewResult>();
			((ViewResult)result).ViewName.ShouldBe("ForgotPasswordConfirmation");
		}

		[Fact]
		public void PostForgotPassword_ShouldReturnRedirect_OnAutheticatedUser()
		{
			// Arrange
			_cp.Setup(x => x.Identity!.IsAuthenticated)
				.Returns(true);
			_m.Setup(x => x.Send(It.IsAny<SignInQuery>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(SignInQueryResult.Succeeded("~/"));

			SendForgetPasswordCommand command = new SendForgetPasswordCommand { Email = _faker.Internet.Email() };
			AuthenticationController controller = new AuthenticationController(_logger.Object, _dpp.Object, _m.Object);
			controller.ControllerContext.HttpContext = _hc.Object;

			// Act
			IActionResult result = controller.ForgotPassword(command).GetAwaiter().GetResult();

			// Assert
			result.ShouldNotBeNull();
			result.ShouldBeOfType<RedirectResult>();
			((RedirectResult)result).Url.ShouldBe("~/");
		}

		[Fact]
		public void PostForgotPassword_ShouldReturnRedirect_FailedValidation()
		{
			// Arrange
			SendForgetPasswordCommand command = new SendForgetPasswordCommand();
			AuthenticationController controller = new AuthenticationController(_logger.Object, _dpp.Object, _m.Object);
			controller.ControllerContext.HttpContext = _hc.Object;

			// Act
			IActionResult result = controller.ForgotPassword(command).GetAwaiter().GetResult();

			// Assert
			result.ShouldNotBeNull();
			result.ShouldBeOfType<ViewResult>();
			((ViewResult)result).ViewName.ShouldBe("ForgotPasswordConfirmation");
		}

		#endregion ForgotPassword

		#region ResetPassword
		[Fact]
		public void GetResetPassword_ShouldReturnView()
		{
			// Arrange
			string password = _faker.Internet.Password(8, false, "\\w", "!Aa0");

			ResetPasswordCommand command = new ResetPasswordCommand
			{
				UserId = _faker.Random.Guid().ToString(),
				Token = _faker.Random.String2(36),
				Password = password,
				ConfirmPassword = password
			};
			string commandAsString = JsonConvert.SerializeObject(command);

			_dp.Setup(x => x.Unprotect(It.IsAny<byte[]>())).Returns(Encoding.UTF8.GetBytes(commandAsString));

			_m.Setup(x => x.Send(It.IsAny<ActivateAccountCommand>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(HandlerResult.Succeeded());

			AuthenticationController controller = new AuthenticationController(_logger.Object, _dpp.Object, _m.Object);
			controller.ControllerContext.HttpContext = _hc.Object;

			// Act
			IActionResult result = controller.ResetPassword(_faker.Random.String2(36));

			// Assert
			result.ShouldNotBeNull();
			result.ShouldBeOfType<ViewResult>();
			((ViewResult)result).ViewData.Model.ShouldBeAssignableTo<ResetPasswordCommand>();
		}

		[Fact]
		public void GetResetPassword_ShouldReturnView_OnEmptyQueryString()
		{
			// Arrange
			AuthenticationController controller = new AuthenticationController(_logger.Object, _dpp.Object, _m.Object);
			controller.ControllerContext.HttpContext = _hc.Object;

			// Act
			IActionResult result = controller.ResetPassword(string.Empty);

			// Assert
			result.ShouldNotBeNull();
			result.ShouldBeOfType<ViewResult>();
			((ViewResult)result).ViewData.Model.ShouldBeAssignableTo<ResetPasswordCommand>();
		}

		[Fact]
		public void GetResetPassword_ShouldReturnRedirect_OnAutheticatedUser()
		{
			// Arrange
			_cp.Setup(x => x.Identity!.IsAuthenticated)
				.Returns(true);
			_m.Setup(x => x.Send(It.IsAny<SignInQuery>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(SignInQueryResult.Succeeded("~/"));

			AuthenticationController controller = new AuthenticationController(_logger.Object, _dpp.Object, _m.Object);
			controller.ControllerContext.HttpContext = _hc.Object;

			// Act
			IActionResult result = controller.ResetPassword(_faker.Random.String2(36));

			// Assert
			result.ShouldNotBeNull();
			result.ShouldBeOfType<RedirectResult>();
			((RedirectResult)result).Url.ShouldBe("~/");
		}

		[Fact]
		public void PostResetPassword_ShouldReturnRedirect()
		{
			// Arrange
			_m.Setup(x => x.Send(It.IsAny<ResetPasswordCommand>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(HandlerResult.Succeeded());

			string password = _faker.Internet.Password(8, false, "\\w", "!Aa0");

			ResetPasswordCommand command = new ResetPasswordCommand
			{
				UserId = _faker.Random.Guid().ToString(),
				Token = _faker.Random.String2(36),
				Password = password,
				ConfirmPassword = password
			};

			AuthenticationController controller = new AuthenticationController(_logger.Object, _dpp.Object, _m.Object);
			controller.ControllerContext.HttpContext = _hc.Object;

			// Act
			IActionResult result = controller.ResetPassword(command).GetAwaiter().GetResult();

			// Assert
			result.ShouldNotBeNull();
			result.ShouldBeOfType<RedirectToActionResult>();
			((RedirectToActionResult)result).ActionName.ShouldBe("SignIn");
		}

		[Fact]
		public void PostResetPassword_ShouldReturnRedirect_OnAutheticatedUser()
		{
			// Arrange
			string password = _faker.Internet.Password(8, false, "\\w", "!Aa0");

			ResetPasswordCommand command = new ResetPasswordCommand
			{
				UserId = _faker.Random.Guid().ToString(),
				Token = _faker.Random.String2(36),
				Password = password,
				ConfirmPassword = password
			};

			_cp.Setup(x => x.Identity!.IsAuthenticated)
				.Returns(true);
			_m.Setup(x => x.Send(It.IsAny<SignInQuery>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(SignInQueryResult.Succeeded("~/"));

			AuthenticationController controller = new AuthenticationController(_logger.Object, _dpp.Object, _m.Object);
			controller.ControllerContext.HttpContext = _hc.Object;

			// Act
			IActionResult result = controller.ResetPassword(command).GetAwaiter().GetResult();
			// Assert
			result.ShouldNotBeNull();
			result.ShouldBeOfType<RedirectResult>();
			((RedirectResult)result).Url.ShouldBe("~/");
		}

		[Fact]
		public void PostResetPassword_ShouldReturnView_FailedPasswordReset()
		{
			// Arrange
			string password = _faker.Internet.Password(8, false, "\\w", "!Aa0");

			ResetPasswordCommand command = new ResetPasswordCommand
			{
				UserId = _faker.Random.Guid().ToString(),
				Token = _faker.Random.String2(36),
				Password = password,
				ConfirmPassword = password
			};

			HandlerResult handlerResult = HandlerResult.Failed();
			handlerResult.Errors.Add(new KeyValuePair<string, string>(_faker.Random.String2(12), _faker.Random.String2(12)));

			_m.Setup(x => x.Send(It.IsAny<ResetPasswordCommand>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(handlerResult);

			AuthenticationController controller = new AuthenticationController(_logger.Object, _dpp.Object, _m.Object);
			controller.ControllerContext.HttpContext = _hc.Object;

			// Act
			IActionResult result = controller.ResetPassword(command).GetAwaiter().GetResult();

			// Assert
			result.ShouldNotBeNull();
			result.ShouldBeOfType<ViewResult>();
			((ViewResult)result).ViewData.Model.ShouldBeAssignableTo<ResetPasswordCommand>();
			((ViewResult)result).ViewData.ModelState.ShouldNotBeNull();
			((ViewResult)result).ViewData.ModelState.Count.ShouldBe(1);
			((ViewResult)result).ViewData.ModelState.ErrorCount.ShouldBe(1);
		}

		[Fact]
		public void PostResetPassword_ShouldReturnView_FailedValidation()
		{
			// Arrange
			ResetPasswordCommand command = new ResetPasswordCommand();
			AuthenticationController controller = new AuthenticationController(_logger.Object, _dpp.Object, _m.Object);
			controller.ControllerContext.HttpContext = _hc.Object;

			// Act
			IActionResult result = controller.ResetPassword(command).GetAwaiter().GetResult();

			// Assert
			((ViewResult)result).ViewData.Model.ShouldBeAssignableTo<ResetPasswordCommand>();
			((ViewResult)result).ViewData.ModelState.ShouldNotBeNull();
			((ViewResult)result).ViewData.ModelState.Count.ShouldBe(4);
			((ViewResult)result).ViewData.ModelState.ErrorCount.ShouldBe(8);
		}

		#endregion ResetPassword
	}
}
