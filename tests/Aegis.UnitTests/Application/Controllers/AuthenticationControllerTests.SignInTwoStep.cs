namespace Aegis.UnitTests.Application.Controllers
{
	using Aegis.Core.Commands.Authentication;
	using Aegis.Core.Queries.Authentication;

	using global::Aegis.Controllers;
	using global::Aegis.Models.Authentication;

	using Microsoft.AspNetCore.Mvc;

	public partial class AuthenticationControllerTests
	{
		public static TheoryData<SignInCommandResult> SignInTwoStepCommandResultValues => new TheoryData<SignInCommandResult>()
		{
			SignInCommandResult.NotActiveAccount(_faker.Random.Guid().ToString()),
			SignInCommandResult.LockedAccount(_faker.Random.Guid().ToString())
		};

		#region SignInTwoStep
		[Fact]
		public void GetSignInTwoStep_ShouldReturnView()
		{
			// Arrange
			_m.Setup(x => x.Send(It.IsAny<SignInTwoStepQuery>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(SignInQueryResult.Succeeded("~/"));

			SignInTwoStepQuery query = new SignInTwoStepQuery();
			AuthenticationController controller = new AuthenticationController(_logger.Object, _dpp.Object, _m.Object);
			controller.ControllerContext.HttpContext = _hc.Object;

			// Act
			IActionResult result = controller.SignInTwoStep(query).GetAwaiter().GetResult();

			// Assert
			result.ShouldNotBeNull();
			result.ShouldBeOfType<ViewResult>();
			((ViewResult)result).ViewData.Model.ShouldBeAssignableTo<SignInTwoStepCommand>();
		}

		[Fact]
		public void GetSignInTwoStep_ShouldReturnRedirect_OnAuthenticatedUser()
		{
			// Arrange
			string url = _faker.Internet.UrlRootedPath();
			_cp.Setup(x => x.Identity!.IsAuthenticated)
				.Returns(true);
			_m.Setup(x => x.Send(It.IsAny<SignInQuery>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(SignInQueryResult.Succeeded(url));

			SignInTwoStepQuery query = new SignInTwoStepQuery();
			AuthenticationController controller = new AuthenticationController(_logger.Object, _dpp.Object, _m.Object);
			controller.ControllerContext.HttpContext = _hc.Object;

			// Act
			IActionResult result = controller.SignInTwoStep(query).GetAwaiter().GetResult();

			// Assert
			result.ShouldNotBeNull();
			result.ShouldBeOfType<RedirectResult>();
			((RedirectResult)result).Url.ShouldBe(url);
		}

		[Fact]
		public void GetSignInTwoStep_ShouldReturnView_OnFailedQuery()
		{
			// Arrange
			string error = _faker.Lorem.Sentence();
			SignInQueryResult signInQueryResult = SignInQueryResult.Failed();
			signInQueryResult.AddError(error);

			_m.Setup(x => x.Send(It.IsAny<SignInTwoStepQuery>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(signInQueryResult);

			SignInTwoStepQuery query = new SignInTwoStepQuery();
			AuthenticationController controller = new AuthenticationController(_logger.Object, _dpp.Object, _m.Object);
			controller.ControllerContext.HttpContext = _hc.Object;

			// Act
			IActionResult result = controller.SignInTwoStep(query).GetAwaiter().GetResult();

			// Assert
			result.ShouldNotBeNull();
			result.ShouldBeOfType<ViewResult>();
			((ViewResult)result).ViewData.Model.ShouldBeAssignableTo<SignInTwoStepCommand>();
			((ViewResult)result).ViewData.ModelState.ShouldNotBeNull();
			((ViewResult)result).ViewData.ModelState.Count.ShouldBe(1);
			((ViewResult)result).ViewData.ModelState.ErrorCount.ShouldBe(1);
		}

		[Fact]
		public void PostSignInTwoStep_ShouldReturnRedirect_OnSuccessfulSignIn()
		{
			// Arrange 
			_m.Setup(x => x.Send(It.IsAny<SignInTwoStepCommand>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(SignInCommandResult.Succeeded("~/"));

			SignInTwoStepCommand command = new SignInTwoStepCommand { Code = _faker.Random.String2(6) };
			AuthenticationController controller = new AuthenticationController(_logger.Object, _dpp.Object, _m.Object);
			controller.ControllerContext.HttpContext = _hc.Object;

			// Act
			IActionResult result = controller.SignInTwoStep(command).GetAwaiter().GetResult();

			// Assert
			result.ShouldNotBeNull();
			result.ShouldBeOfType<RedirectResult>();
			((RedirectResult)result).Url.ShouldBe("~/");
		}

		[Fact]
		public void PostSignInTwoStep_ShouldReturnRedirect_OnAuthenticatedUser()
		{
			// Arrange
			string url = _faker.Internet.UrlRootedPath();
			_cp.Setup(x => x.Identity!.IsAuthenticated)
				.Returns(true);
			_m.Setup(x => x.Send(It.IsAny<SignInQuery>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(SignInQueryResult.Succeeded(url));

			SignInTwoStepCommand command = new SignInTwoStepCommand { Code = _faker.Random.String2(6) };
			AuthenticationController controller = new AuthenticationController(_logger.Object, _dpp.Object, _m.Object);
			controller.ControllerContext.HttpContext = _hc.Object;

			// Act
			IActionResult result = controller.SignInTwoStep(command).GetAwaiter().GetResult();

			// Assert
			result.ShouldNotBeNull();
			result.ShouldBeOfType<RedirectResult>();
			((RedirectResult)result).Url.ShouldBe(url);
		}

		[Fact]
		public void PostSignInTwoStep_ShouldReturnView_OnValidationFailed()
		{
			// Arrange 
			SignInTwoStepCommand command = new SignInTwoStepCommand();
			AuthenticationController controller = new AuthenticationController(_logger.Object, _dpp.Object, _m.Object);
			controller.ControllerContext.HttpContext = _hc.Object;

			// Act
			IActionResult result = controller.SignInTwoStep(command).GetAwaiter().GetResult();

			// Assert
			result.ShouldNotBeNull();
			result.ShouldBeOfType<ViewResult>();
			((ViewResult)result).ViewData.Model.ShouldBeAssignableTo<SignInTwoStepCommand>();
			((ViewResult)result).ViewData.ModelState.ShouldNotBeNull();
			((ViewResult)result).ViewData.ModelState.Count.ShouldBe(1);
			((ViewResult)result).ViewData.ModelState.ErrorCount.ShouldBe(2);
		}

		[Fact]
		public void PostSignInTwoStep_ShouldReturnView_OnFailedSignIn()
		{
			// Arrange
			SignInCommandResult signInCommandResult = SignInCommandResult.Failed();
			signInCommandResult.Errors.Add(new KeyValuePair<string, string>(_faker.Random.String2(12), _faker.Random.String2(12)));
			_m.Setup(x => x.Send(It.IsAny<SignInTwoStepCommand>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(signInCommandResult);

			SignInTwoStepCommand command = new SignInTwoStepCommand { Code = _faker.Random.String2(6) };
			AuthenticationController controller = new AuthenticationController(_logger.Object, _dpp.Object, _m.Object);
			controller.ControllerContext.HttpContext = _hc.Object;

			// Act
			IActionResult result = controller.SignInTwoStep(command).GetAwaiter().GetResult();

			// Assert
			result.ShouldNotBeNull();
			result.ShouldBeOfType<ViewResult>();
			((ViewResult)result).ViewData.Model.ShouldBeAssignableTo<SignInTwoStepCommand>();
			((ViewResult)result).ViewData.ModelState.ShouldNotBeNull();
			((ViewResult)result).ViewData.ModelState.Count.ShouldBe(1);
			((ViewResult)result).ViewData.ModelState.ErrorCount.ShouldBe(1);
		}

		[Theory]
		[MemberData(nameof(SignInTwoStepCommandResultValues))]
		public void PostSignInTwoStep_ShouldReturnRedirectToAction(SignInCommandResult signInCommandResult)
		{
			// Arrange
			_m.Setup(x => x.Send(It.IsAny<SignInTwoStepCommand>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(signInCommandResult);

			SignInTwoStepCommand command = new SignInTwoStepCommand { Code = _faker.Random.String2(6) };
			AuthenticationController controller = new AuthenticationController(_logger.Object, _dpp.Object, _m.Object);
			controller.ControllerContext.HttpContext = _hc.Object;

			// Act
			IActionResult result = controller.SignInTwoStep(command).GetAwaiter().GetResult();

			// Assert
			result.ShouldNotBeNull();
			result.ShouldBeOfType<RedirectToActionResult>();

			if (signInCommandResult.AccounNotActive)
			{
				((RedirectToActionResult)result).ActionName.ShouldBe("ActivateAccount");
			}
			else if (signInCommandResult.AccounLocked)
			{
				((RedirectToActionResult)result).ActionName.ShouldBe("Locked");
			}
		}
		#endregion SignInTwoStep
	}
}
