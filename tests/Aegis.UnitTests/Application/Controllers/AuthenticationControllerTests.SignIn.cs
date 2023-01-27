#region copyright
//----------------------------------------------------------------------
// Copyright 2023 MNB Software
// Licensed under the Apache License, Version 2.0
// You may obtain a copy at http://www.apache.org/licenses/LICENSE-2.0
//----------------------------------------------------------------------
#endregion
namespace Aegis.UnitTests.Application.Controllers
{
	using Aegis.Core.Commands.Authentication;
	using Aegis.Core.Queries.Authentication;
	using Aegis.Exceptions;

	using global::Aegis.Controllers;
	using global::Aegis.Models.Authentication;

	using Microsoft.AspNetCore.Mvc;

	public partial class AuthenticationControllerTests
	{
		public static TheoryData<SignInCommandResult> SignInCommandResultValues => new TheoryData<SignInCommandResult>()
		{
			SignInCommandResult.TwoStepRequired(_faker.Random.Guid().ToString()),
			SignInCommandResult.LockedAccount(_faker.Random.Guid().ToString())
		};

		#region SignIn
		[Fact]
		public void GetSignIn_ShouldReturnView_OnNotAuthenticatedUser()
		{
			// Arrange
			AuthenticationController controller = new AuthenticationController(_logger.Object, _dpp.Object, _m.Object);
			controller.ControllerContext.HttpContext = _hc.Object;

			// Act
			IActionResult result = controller.SignIn("/");

			// Assert
			result.ShouldNotBeNull();
			result.ShouldBeOfType<ViewResult>();
			((ViewResult)result).ViewData.Model.ShouldBeAssignableTo<SignInCommand>();
		}

		[Fact]
		public void GetSignIn_ShouldReturnRedirect_OnAuthenticatedUser()
		{
			// Arrange
			string url = _faker.Internet.UrlRootedPath();
			_cp.Setup(x => x.Identity!.IsAuthenticated)
				.Returns(true);
			_m.Setup(x => x.Send(It.IsAny<SignInQuery>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(SignInQueryResult.Succeeded(url));

			AuthenticationController controller = new AuthenticationController(_logger.Object, _dpp.Object, _m.Object);
			controller.ControllerContext.HttpContext = _hc.Object;

			// Act
			IActionResult result = controller.SignIn(url);

			// Assert
			result.ShouldNotBeNull();
			result.ShouldBeOfType<RedirectResult>();
			((RedirectResult)result).Url.ShouldBe(url);
		}

		[Fact]
		public void GetSignIn_ShouldThrowAnException_OnAuthenticatedUser_FailedSigninQuery()
		{
			// Arrange
			string error = _faker.Lorem.Sentence();
			SignInQueryResult result = SignInQueryResult.Failed();
			result.AddError(error);

			_cp.Setup(x => x.Identity!.IsAuthenticated)
				.Returns(true);
			_m.Setup(x => x.Send(It.IsAny<SignInQuery>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(result);

			AuthenticationController controller = new AuthenticationController(_logger.Object, _dpp.Object, _m.Object);
			controller.ControllerContext.HttpContext = _hc.Object;

			// Act
			Exception exception = Record.Exception(() => controller.SignIn("/"));

			// Assert
			exception.ShouldNotBeNull();
			exception.ShouldBeOfType<ApplicationFlowException>();
		}

		[Fact]
		public void PostSignIn_ShouldReturnRedirect_OnSuccessfulSignIn()
		{
			// Arrange 
			_m.Setup(x => x.Send(It.IsAny<SignInCommand>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(SignInCommandResult.Succeeded("~/"));

			SignInCommand command = new SignInCommand { Email = _faker.Internet.Email(), Password = _faker.Internet.Password(8, false, "\\w", "!Aa0") };
			AuthenticationController controller = new AuthenticationController(_logger.Object, _dpp.Object, _m.Object);
			controller.ControllerContext.HttpContext = _hc.Object;

			// Act
			IActionResult result = controller.SignIn(command).GetAwaiter().GetResult();

			// Assert
			result.ShouldNotBeNull();
			result.ShouldBeOfType<RedirectResult>();
			((RedirectResult)result).Url.ShouldBe("~/");
		}

		[Fact]
		public void PostSignIn_ShouldReturnRedirect_OnAuthenticatedUser()
		{
			// Arrange
			string url = _faker.Internet.UrlRootedPath();
			_cp.Setup(x => x.Identity!.IsAuthenticated)
				.Returns(true);
			_m.Setup(x => x.Send(It.IsAny<SignInQuery>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(SignInQueryResult.Succeeded(url));

			SignInCommand command = new SignInCommand { Email = _faker.Internet.Email(), Password = _faker.Internet.Password(8, false, "\\w", "!Aa0") };
			AuthenticationController controller = new AuthenticationController(_logger.Object, _dpp.Object, _m.Object);
			controller.ControllerContext.HttpContext = _hc.Object;

			// Act
			IActionResult result = controller.SignIn(command).GetAwaiter().GetResult();

			// Assert
			result.ShouldNotBeNull();
			result.ShouldBeOfType<RedirectResult>();
			((RedirectResult)result).Url.ShouldBe(url);
		}

		[Fact]
		public void PostSignIn_ShouldReturnView_OnValidationFailed()
		{
			// Arrange
			SignInCommand command = new SignInCommand();
			AuthenticationController controller = new AuthenticationController(_logger.Object, _dpp.Object, _m.Object);
			controller.ControllerContext.HttpContext = _hc.Object;

			// Act
			IActionResult result = controller.SignIn(command).GetAwaiter().GetResult();

			// Assert
			result.ShouldNotBeNull();
			result.ShouldBeOfType<ViewResult>();
			((ViewResult)result).ViewData.Model.ShouldBeAssignableTo<SignInCommand>();
			((ViewResult)result).ViewData.ModelState.ShouldNotBeNull();
			((ViewResult)result).ViewData.ModelState.Count.ShouldBe(2);
			((ViewResult)result).ViewData.ModelState.ErrorCount.ShouldBe(4);
		}

		[Fact]
		public void PostSignIn_ShouldReturnView_OnFailedSignIn()
		{
			// Arrange
			SignInCommandResult signInCommand = SignInCommandResult.Failed();
			signInCommand.Errors.Add(new KeyValuePair<string, string>(_faker.Random.String2(12), _faker.Random.String2(12)));
			_m.Setup(x => x.Send(It.IsAny<SignInCommand>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(signInCommand);

			SignInCommand command = new SignInCommand { Email = _faker.Internet.Email(), Password = _faker.Internet.Password(8, false, "\\w", "!Aa0") };
			AuthenticationController controller = new AuthenticationController(_logger.Object, _dpp.Object, _m.Object);
			controller.ControllerContext.HttpContext = _hc.Object;

			// Act
			IActionResult result = controller.SignIn(command).GetAwaiter().GetResult();

			// Assert
			result.ShouldNotBeNull();
			result.ShouldBeOfType<ViewResult>();
			((ViewResult)result).ViewData.Model.ShouldBeAssignableTo<SignInCommand>();
			((ViewResult)result).ViewData.ModelState.ShouldNotBeNull();
			((ViewResult)result).ViewData.ModelState.Count.ShouldBe(1);
			((ViewResult)result).ViewData.ModelState.ErrorCount.ShouldBe(1);
		}

		[Fact]
		public void PostSignIn_ShouldReturnView_OnNotActiveAccount()
		{
			// Arrange
			_m.Setup(x => x.Send(It.IsAny<SignInCommand>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(SignInCommandResult.NotActiveAccount(_faker.Random.Guid().ToString()));

			SignInCommand command = new SignInCommand { Email = _faker.Internet.Email(), Password = _faker.Internet.Password(8, false, "\\w", "!Aa0") };
			AuthenticationController controller = new AuthenticationController(_logger.Object, _dpp.Object, _m.Object);
			controller.ControllerContext.HttpContext = _hc.Object;

			// Act
			IActionResult result = controller.SignIn(command).GetAwaiter().GetResult();

			// Assert
			result.ShouldNotBeNull();
			result.ShouldBeOfType<ViewResult>();
			((ViewResult)result).ViewName.ShouldBe("ActivateAccountMail");
		}

		[Theory]
		[MemberData(nameof(SignInCommandResultValues))]
		public void PostSignIn_ShouldReturnRedirectToAction(SignInCommandResult signInCommandResult)
		{
			// Arrange
			_m.Setup(x => x.Send(It.IsAny<SignInCommand>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(signInCommandResult);

			SignInCommand command = new SignInCommand { Email = _faker.Internet.Email(), Password = _faker.Internet.Password(8, false, "\\w", "!Aa0") };
			AuthenticationController controller = new AuthenticationController(_logger.Object, _dpp.Object, _m.Object);
			controller.ControllerContext.HttpContext = _hc.Object;

			// Act
			IActionResult result = controller.SignIn(command).GetAwaiter().GetResult();

			// Assert
			result.ShouldNotBeNull();
			result.ShouldBeOfType<RedirectToActionResult>();

			if (signInCommandResult.RequiresTwoStep)
			{
				((RedirectToActionResult)result).ActionName.ShouldBe("SignInTwoStep");
			}
			else if (signInCommandResult.AccounLocked)
			{
				((RedirectToActionResult)result).ActionName.ShouldBe("Locked");
			}
		}
		#endregion SignIn
	}
}
