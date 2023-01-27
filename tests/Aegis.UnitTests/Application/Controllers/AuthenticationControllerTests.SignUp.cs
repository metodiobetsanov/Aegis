namespace Aegis.UnitTests.Application.Controllers
{
	using global::Aegis.Controllers;
	using global::Aegis.Core.Commands.Authentication;
	using global::Aegis.Core.Queries.Authentication;
	using global::Aegis.Models.Authentication;

	using Microsoft.AspNetCore.Mvc;

	public partial class AuthenticationControllerTests
	{
		#region SignUp
		[Fact]
		public void GetSignUp_ShouldReturnView()
		{
			AuthenticationController controller = new AuthenticationController(_logger.Object, _dpp.Object, _m.Object);
			controller.ControllerContext.HttpContext = _hc.Object;

			// Act
			IActionResult result = controller.SignUp("/");

			// Assert
			result.ShouldNotBeNull();
			result.ShouldBeOfType<ViewResult>();
			((ViewResult)result).ViewData.Model.ShouldBeAssignableTo<SignUpCommand>();
		}

		[Fact]
		public void GetSignUp_ShouldReturnRedirect_OnAutheticatedUser()
		{
			// Arrange
			_cp.Setup(x => x.Identity!.IsAuthenticated)
				.Returns(true);
			_m.Setup(x => x.Send(It.IsAny<SignInQuery>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(SignInQueryResult.Succeeded("~/"));

			AuthenticationController controller = new AuthenticationController(_logger.Object, _dpp.Object, _m.Object);
			controller.ControllerContext.HttpContext = _hc.Object;

			// Act
			IActionResult result = controller.SignUp("/");

			// Assert
			result.ShouldNotBeNull();
			result.ShouldBeOfType<RedirectResult>();
			((RedirectResult)result).Url.ShouldBe("~/");
		}

		[Fact]
		public void PostSignUp_ShouldReturnRedirect_OnSuccessfulSignUp()
		{
			// Arrange 
			_m.Setup(x => x.Send(It.IsAny<SignUpCommand>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(SignUpCommandResult.Succeeded(_faker.Random.Guid().ToString(), "~/"));

			string password = _faker.Internet.Password(8, false, "\\w", "!Aa0");
			SignUpCommand command = new SignUpCommand { Email = _faker.Internet.Email(), Password = password, ConfirmPassword = password, AcceptTerms = true };
			AuthenticationController controller = new AuthenticationController(_logger.Object, _dpp.Object, _m.Object);
			controller.ControllerContext.HttpContext = _hc.Object;

			// Act
			IActionResult result = controller.SignUp(command).GetAwaiter().GetResult();

			// Assert
			result.ShouldNotBeNull();
			result.ShouldBeOfType<RedirectToActionResult>();
			((RedirectToActionResult)result).ActionName.ShouldBe("ActivateAccount");
		}

		[Fact]
		public void PostSignUp_ShouldReturnRedirect_OnAutheticatedUser()
		{
			// Arrange
			_cp.Setup(x => x.Identity!.IsAuthenticated)
				.Returns(true);
			_m.Setup(x => x.Send(It.IsAny<SignInQuery>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(SignInQueryResult.Succeeded("~/"));

			string password = _faker.Internet.Password(8, false, "\\w", "!Aa0");
			SignUpCommand command = new SignUpCommand { Email = _faker.Internet.Email(), Password = password, ConfirmPassword = password, AcceptTerms = true };
			AuthenticationController controller = new AuthenticationController(_logger.Object, _dpp.Object, _m.Object);
			controller.ControllerContext.HttpContext = _hc.Object;

			// Act
			IActionResult result = controller.SignUp(command).GetAwaiter().GetResult();

			// Assert
			result.ShouldNotBeNull();
			result.ShouldBeOfType<RedirectResult>();
			((RedirectResult)result).Url.ShouldBe("~/");
		}

		[Fact]
		public void PostSignUp_ShouldReturnView_OnValidationFailed()
		{
			// Arrange 
			SignUpCommand command = new SignUpCommand();
			AuthenticationController controller = new AuthenticationController(_logger.Object, _dpp.Object, _m.Object);
			controller.ControllerContext.HttpContext = _hc.Object;

			// Act
			IActionResult result = controller.SignUp(command).GetAwaiter().GetResult();

			// Assert
			result.ShouldNotBeNull();
			result.ShouldBeOfType<ViewResult>();
			((ViewResult)result).ViewData.Model.ShouldBeAssignableTo<SignUpCommand>();
			((ViewResult)result).ViewData.ModelState.ShouldNotBeNull();
			((ViewResult)result).ViewData.ModelState.Count.ShouldBe(4);
			((ViewResult)result).ViewData.ModelState.ErrorCount.ShouldBe(8);
		}

		[Fact]
		public void PostSignUp_ShouldReturnView_OnFailedSignUp()
		{
			// Arrange
			SignUpCommandResult signUpCommandResult = SignUpCommandResult.Failed();
			signUpCommandResult.Errors.Add(new KeyValuePair<string, string>(_faker.Random.String2(12), _faker.Random.String2(12)));
			_m.Setup(x => x.Send(It.IsAny<SignUpCommand>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(signUpCommandResult);

			string password = _faker.Internet.Password(8, false, "\\w", "!Aa0");
			SignUpCommand command = new SignUpCommand { Email = _faker.Internet.Email(), Password = password, ConfirmPassword = password, AcceptTerms = true };
			AuthenticationController controller = new AuthenticationController(_logger.Object, _dpp.Object, _m.Object);
			controller.ControllerContext.HttpContext = _hc.Object;

			// Act
			IActionResult result = controller.SignUp(command).GetAwaiter().GetResult();

			// Assert
			result.ShouldNotBeNull();
			result.ShouldBeOfType<ViewResult>();
			((ViewResult)result).ViewData.Model.ShouldBeAssignableTo<SignUpCommand>();
			((ViewResult)result).ViewData.ModelState.ShouldNotBeNull();
			((ViewResult)result).ViewData.ModelState.Count.ShouldBe(1);
			((ViewResult)result).ViewData.ModelState.ErrorCount.ShouldBe(1);
		}
		#endregion SignUp
	}
}
