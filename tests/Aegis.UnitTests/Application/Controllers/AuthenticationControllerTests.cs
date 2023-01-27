namespace Aegis.UnitTests.Core.Controllers
{
	using global::Aegis.Controllers;
	using global::Aegis.Core.Commands.Authentication;
	using global::Aegis.Core.Queries.Authentication;
	using global::Aegis.Exceptions;
	using global::Aegis.Models.Authentication;
	using global::Aegis.Models.Shared;

	using Microsoft.AspNetCore.Mvc;

	using Moq;

	public class AuthenticationControllerTests
	{
		#region Setup
		private static readonly Faker _faker = new Faker("en");

		private readonly Mock<ILogger<AuthenticationController>> _logger = new Mock<ILogger<AuthenticationController>>();
		private readonly Mock<IDataProtector> _dp = new Mock<IDataProtector>();
		private readonly Mock<IDataProtectionProvider> _dpp = new Mock<IDataProtectionProvider>();
		private readonly Mock<IMediator> _m = new Mock<IMediator>();
		private readonly Mock<ClaimsPrincipal> _cp = new Mock<ClaimsPrincipal>();
		private readonly Mock<HttpContext> _hc = new Mock<HttpContext>();

		public static TheoryData<SignInCommandResult> SignInCommandResultValues => new TheoryData<SignInCommandResult>()
		{
			SignInCommandResult.TwoStepRequired(_faker.Random.Guid().ToString()),
			SignInCommandResult.NotActiveAccount(_faker.Random.Guid().ToString()),
			SignInCommandResult.LockedAccount(_faker.Random.Guid().ToString())
		};

		public static TheoryData<SignInCommandResult> SignInTwoStepCommandResultValues => new TheoryData<SignInCommandResult>()
		{
			SignInCommandResult.NotActiveAccount(_faker.Random.Guid().ToString()),
			SignInCommandResult.LockedAccount(_faker.Random.Guid().ToString())
		};

		public AuthenticationControllerTests()
		{
			_hc.Setup(x => x.User).Returns(_cp.Object);
			_dpp.Setup(x => x.CreateProtector(It.IsAny<string>())).Returns(_dp.Object);
		}
		#endregion Setup

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
			else if (signInCommandResult.AccounNotActive)
			{
				((RedirectToActionResult)result).ActionName.ShouldBe("SendAccountActivation");
			}
			else if (signInCommandResult.AccounLocked)
			{
				((RedirectToActionResult)result).ActionName.ShouldBe("Locked");
			}
		}
		#endregion SignIn

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
				((RedirectToActionResult)result).ActionName.ShouldBe("SendAccountActivation");
			}
			else if (signInCommandResult.AccounLocked)
			{
				((RedirectToActionResult)result).ActionName.ShouldBe("Locked");
			}
		}
		#endregion SignInTwoStep

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
			((RedirectToActionResult)result).ActionName.ShouldBe("SendAccountActivation");
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

		#region SignOut
		[Fact]
		public void GetSignOut_ShouldReturnView()
		{
			// Arrange
			_m.Setup(x => x.Send(It.IsAny<SignOutQuery>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(SignOutQueryResult.Show(true));

			SignOutQuery query = new SignOutQuery();
			AuthenticationController controller = new AuthenticationController(_logger.Object, _dpp.Object, _m.Object);
			controller.ControllerContext.HttpContext = _hc.Object;

			// Act
			IActionResult result = controller.SignOut(query).GetAwaiter().GetResult();

			// Assert
			result.ShouldNotBeNull();
			result.ShouldBeOfType<ViewResult>();
			((ViewResult)result).ViewData.Model.ShouldBeAssignableTo<SignOutCommand>();
		}

		[Fact]
		public void GetSignOut_ShouldReturnRedirect()
		{
			// Arrange
			_m.Setup(x => x.Send(It.IsAny<SignOutQuery>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(SignOutQueryResult.Show(false));

			_m.Setup(x => x.Send(It.IsAny<SignOutCommand>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(SignOutCommandResult.Succeeded("~/"));

			SignOutQuery query = new SignOutQuery();
			AuthenticationController controller = new AuthenticationController(_logger.Object, _dpp.Object, _m.Object);
			controller.ControllerContext.HttpContext = _hc.Object;

			// Act
			IActionResult result = controller.SignOut(query).GetAwaiter().GetResult();

			// Assert
			result.ShouldNotBeNull();
			result.ShouldBeOfType<RedirectResult>();
			((RedirectResult)result).Url.ShouldBe("~/");
		}

		[Fact]
		public void PostSignOut_ShouldReturnRdirect()
		{
			// Arrange
			_m.Setup(x => x.Send(It.IsAny<SignOutCommand>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(SignOutCommandResult.Succeeded("~/"));

			SignOutCommand command = new SignOutCommand();
			AuthenticationController controller = new AuthenticationController(_logger.Object, _dpp.Object, _m.Object);
			controller.ControllerContext.HttpContext = _hc.Object;

			// Act
			IActionResult result = controller.SignOut(command).GetAwaiter().GetResult();

			// Assert
			result.ShouldNotBeNull();
			result.ShouldBeOfType<RedirectResult>();
			((RedirectResult)result).Url.ShouldBe("~/");
		}

		[Fact]
		public void GetSignOut_ShouldReturnView_OnFailedSignOut()
		{
			// Arrange
			SignOutCommandResult signOutCommandResult = SignOutCommandResult.Failed();
			signOutCommandResult.Errors.Add(new KeyValuePair<string, string>(_faker.Random.String2(12), _faker.Random.String2(12)));
			_m.Setup(x => x.Send(It.IsAny<SignOutCommand>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(signOutCommandResult);

			SignOutCommand command = new SignOutCommand();
			AuthenticationController controller = new AuthenticationController(_logger.Object, _dpp.Object, _m.Object);
			controller.ControllerContext.HttpContext = _hc.Object;

			// Act
			IActionResult result = controller.SignOut(command).GetAwaiter().GetResult();

			// Assert
			result.ShouldNotBeNull();
			result.ShouldBeOfType<ViewResult>();
			((ViewResult)result).ViewData.Model.ShouldBeAssignableTo<SignOutCommand>();
			((ViewResult)result).ViewData.ModelState.ShouldNotBeNull();
			((ViewResult)result).ViewData.ModelState.Count.ShouldBe(1);
			((ViewResult)result).ViewData.ModelState.ErrorCount.ShouldBe(1);
		}
		#endregion SignOut

		#region Locked
		[Fact]
		public void GetLocked_ShouldReturnView()
		{
			// Arrange
			_m.Setup(x => x.Send(It.IsAny<GetUserLockedTimeQuery>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(GetUserLockedTimeQueryResult.Succeeded(DateTime.UtcNow));

			GetUserLockedTimeQuery query = new GetUserLockedTimeQuery { UserId = _faker.Random.Guid().ToString() };
			AuthenticationController controller = new AuthenticationController(_logger.Object, _dpp.Object, _m.Object);
			controller.ControllerContext.HttpContext = _hc.Object;

			// Act
			IActionResult result = controller.Locked(query).GetAwaiter().GetResult();

			// Assert
			result.ShouldNotBeNull();
			result.ShouldBeOfType<ViewResult>();
		}

		[Fact]
		public void GetLocked_ShouldReturnRedirect_OnAutheticatedUser()
		{
			// Arrange
			_cp.Setup(x => x.Identity!.IsAuthenticated)
				.Returns(true);
			_m.Setup(x => x.Send(It.IsAny<SignInQuery>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(SignInQueryResult.Succeeded("~/"));

			GetUserLockedTimeQuery query = new GetUserLockedTimeQuery { UserId = _faker.Random.Guid().ToString() };
			AuthenticationController controller = new AuthenticationController(_logger.Object, _dpp.Object, _m.Object);
			controller.ControllerContext.HttpContext = _hc.Object;

			// Act
			IActionResult result = controller.Locked(query).GetAwaiter().GetResult();

			// Assert
			result.ShouldNotBeNull();
			result.ShouldBeOfType<RedirectResult>();
			((RedirectResult)result).Url.ShouldBe("~/");
		}

		[Fact]
		public void GetLocked_ShouldReturnView_OnFailedQuery()
		{
			// Arrange
			string error = _faker.Lorem.Sentence();
			GetUserLockedTimeQueryResult handlerResult = GetUserLockedTimeQueryResult.Failed();
			handlerResult.AddError(error);
			_m.Setup(x => x.Send(It.IsAny<GetUserLockedTimeQuery>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(handlerResult);

			GetUserLockedTimeQuery query = new GetUserLockedTimeQuery { UserId = _faker.Random.Guid().ToString() };
			AuthenticationController controller = new AuthenticationController(_logger.Object, _dpp.Object, _m.Object);
			controller.ControllerContext.HttpContext = _hc.Object;

			// Act
			IActionResult result = controller.Locked(query).GetAwaiter().GetResult();

			// Assert
			result.ShouldNotBeNull();
			result.ShouldBeOfType<ViewResult>();
			((ViewResult)result).ViewData.ModelState.ShouldNotBeNull();
			((ViewResult)result).ViewData.ModelState.Count.ShouldBe(1);
			((ViewResult)result).ViewData.ModelState.ErrorCount.ShouldBe(1);
		}
		#endregion Locked

		#region SendAccountActivation
		[Fact]
		public void GetSendAccountActivation_ShouldReturnView()
		{
			// Arrange
			_m.Setup(x => x.Send(It.IsAny<SendAccountActivationCommand>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(HandlerResult.Succeeded());

			SendAccountActivationCommand command = new SendAccountActivationCommand { UserId = _faker.Random.Guid().ToString() };
			AuthenticationController controller = new AuthenticationController(_logger.Object, _dpp.Object, _m.Object);
			controller.ControllerContext.HttpContext = _hc.Object;

			// Act
			IActionResult result = controller.SendAccountActivation(command).GetAwaiter().GetResult();

			// Assert
			result.ShouldNotBeNull();
			result.ShouldBeOfType<ViewResult>();
		}

		[Fact]
		public void GetSendAccountActivation_ShouldReturnRedirect_OnAutheticatedUser()
		{
			// Arrange
			_cp.Setup(x => x.Identity!.IsAuthenticated)
				.Returns(true);
			_m.Setup(x => x.Send(It.IsAny<SignInQuery>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(SignInQueryResult.Succeeded("~/"));

			SendAccountActivationCommand command = new SendAccountActivationCommand { UserId = _faker.Random.Guid().ToString() };
			AuthenticationController controller = new AuthenticationController(_logger.Object, _dpp.Object, _m.Object);
			controller.ControllerContext.HttpContext = _hc.Object;

			// Act
			IActionResult result = controller.SendAccountActivation(command).GetAwaiter().GetResult();

			// Assert
			result.ShouldNotBeNull();
			result.ShouldBeOfType<RedirectResult>();
			((RedirectResult)result).Url.ShouldBe("~/");
		}

		[Fact]
		public void GetSendAccountActivation_ShouldReturnView_OnFailedEmailConfirmation()
		{
			// Arrange
			string error = _faker.Lorem.Sentence();
			HandlerResult handlerResult = HandlerResult.Failed();
			handlerResult.AddError(error);
			_m.Setup(x => x.Send(It.IsAny<SendAccountActivationCommand>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(handlerResult);

			SendAccountActivationCommand command = new SendAccountActivationCommand { UserId = _faker.Random.Guid().ToString() };
			AuthenticationController controller = new AuthenticationController(_logger.Object, _dpp.Object, _m.Object);
			controller.ControllerContext.HttpContext = _hc.Object;

			// Act
			IActionResult result = controller.SendAccountActivation(command).GetAwaiter().GetResult();

			// Assert
			result.ShouldNotBeNull();
			result.ShouldBeOfType<ViewResult>();
			((ViewResult)result).ViewData.ModelState.ShouldNotBeNull();
			((ViewResult)result).ViewData.ModelState.Count.ShouldBe(1);
			((ViewResult)result).ViewData.ModelState.ErrorCount.ShouldBe(1);
		}

		[Fact]
		public void GetSendAccountActivation_ShouldReturnView_OnFailedValidation()
		{
			// Arrange
			SendAccountActivationCommand command = new SendAccountActivationCommand();
			AuthenticationController controller = new AuthenticationController(_logger.Object, _dpp.Object, _m.Object);
			controller.ControllerContext.HttpContext = _hc.Object;

			// Act
			IActionResult result = controller.SendAccountActivation(command).GetAwaiter().GetResult();

			// Assert
			result.ShouldNotBeNull();
			result.ShouldBeOfType<ViewResult>();
			((ViewResult)result).ViewData.ModelState.ShouldNotBeNull();
			((ViewResult)result).ViewData.ModelState.Count.ShouldBe(1);
			((ViewResult)result).ViewData.ModelState.ErrorCount.ShouldBe(2);
		}
		#endregion SendAccountActivation

		#region ActivateAccount
		[Fact]
		public void GetActivateAccount_ShouldReturnView()
		{
			// Arrange
			ActivateAccountCommand command = new ActivateAccountCommand { UserId = _faker.Random.Guid().ToString(), Token = _faker.Random.String2(36) };
			string commandAsString = JsonConvert.SerializeObject(command);

			_dp.Setup(x => x.Unprotect(It.IsAny<byte[]>())).Returns(Encoding.UTF8.GetBytes(commandAsString));

			_m.Setup(x => x.Send(It.IsAny<ActivateAccountCommand>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(HandlerResult.Succeeded());

			AuthenticationController controller = new AuthenticationController(_logger.Object, _dpp.Object, _m.Object);
			controller.ControllerContext.HttpContext = _hc.Object;

			// Act
			IActionResult result = controller.ActivateAccount(_faker.Random.String2(36)).GetAwaiter().GetResult();

			// Assert
			result.ShouldNotBeNull();
			result.ShouldBeOfType<ViewResult>();
		}

		[Fact]
		public void GetActivateAccountCommand_ShouldReturnRedirect_OnAutheticatedUser()
		{
			// Arrange
			_cp.Setup(x => x.Identity!.IsAuthenticated)
				.Returns(true);

			_m.Setup(x => x.Send(It.IsAny<SignInQuery>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(SignInQueryResult.Succeeded("~/"));

			AuthenticationController controller = new AuthenticationController(_logger.Object, _dpp.Object, _m.Object);
			controller.ControllerContext.HttpContext = _hc.Object;

			// Act
			IActionResult result = controller.ActivateAccount(_faker.Random.String2(36)).GetAwaiter().GetResult();

			// Assert
			result.ShouldNotBeNull();
			result.ShouldBeOfType<RedirectResult>();
			((RedirectResult)result).Url.ShouldBe("~/");
		}

		[Fact]
		public void GetActivateAccount_ShouldReturnView_OnFailedValidation()
		{
			// Arrange
			ActivateAccountCommand command = new ActivateAccountCommand();
			string commandAsString = JsonConvert.SerializeObject(command);

			_dp.Setup(x => x.Unprotect(It.IsAny<byte[]>())).Returns(Encoding.UTF8.GetBytes(commandAsString));

			AuthenticationController controller = new AuthenticationController(_logger.Object, _dpp.Object, _m.Object);
			controller.ControllerContext.HttpContext = _hc.Object;

			// Act
			IActionResult result = controller.ActivateAccount(_faker.Random.String2(36).ToBase64()).GetAwaiter().GetResult();

			// Assert
			result.ShouldNotBeNull();
			result.ShouldBeOfType<ViewResult>();
			((ViewResult)result).ViewData.ModelState.ShouldNotBeNull();
			((ViewResult)result).ViewData.ModelState.Count.ShouldBe(2);
			((ViewResult)result).ViewData.ModelState.ErrorCount.ShouldBe(4);
		}
		#endregion ActivateAccount
	}
}
