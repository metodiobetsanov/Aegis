namespace Aegis.UnitTests.Aegis.Controllers
{
	using Duende.IdentityServer.Extensions;

	using global::Aegis.Application.Commands.Auth;
	using global::Aegis.Application.Queries.Auth;
	using global::Aegis.Controllers;
	using global::Aegis.Models.Auth;

	using IdentityModel;

	using MediatR;

	using Microsoft.AspNetCore.Mvc;

	using Moq;

	public class AuthControllerTests
	{
		private static readonly Faker _faker = new Faker("en");

		private readonly Mock<ILogger<AuthController>> _logger = new Mock<ILogger<AuthController>>();
		private readonly Mock<IMediator> _m = new Mock<IMediator>();
		private readonly Mock<ClaimsPrincipal> _cp = new Mock<ClaimsPrincipal>();
		private readonly Mock<HttpContext> _hc = new Mock<HttpContext>();

		public static TheoryData<SignInCommandResult> SignInCommandResultValues => new TheoryData<SignInCommandResult>()
		{
			SignInCommandResult.TwoStepRequired(_faker.Random.Guid().ToString()),
			SignInCommandResult.NotActiveAccount(_faker.Random.Guid().ToString()),
			SignInCommandResult.LockedAccount(_faker.Random.Guid().ToString())
		};

		public AuthControllerTests()
		{
			_hc.Setup(x => x.User).Returns(_cp.Object);
		}

		#region SignIn
		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void GetSignIn_ShouldReturnView(bool authenticated)
		{
			// Arrange
			if (authenticated)
			{
				_cp.Setup(x => x.Identity!.IsAuthenticated)
					.Returns(authenticated);
				_m.Setup(x => x.Send(It.IsAny<SignInQuery>(), It.IsAny<CancellationToken>()))
					.ReturnsAsync(SignInQueryResult.Failed());
			}

			SignInQuery query = new SignInQuery();
			AuthController controller = new AuthController(_logger.Object, _m.Object);
			controller.ControllerContext.HttpContext = _hc.Object;

			// Act
			IActionResult result = controller.SignIn(query).GetAwaiter().GetResult();

			// Assert
			result.ShouldNotBeNull();
			result.ShouldBeOfType<ViewResult>();
			((ViewResult)result).ViewData.Model.ShouldBeAssignableTo<SignInCommand>();
		}

		[Fact]
		public void GetSignIn_ShouldReturnRedirect_OnAutheticatedUser()
		{
			// Arrange
			_cp.Setup(x => x.Identity!.IsAuthenticated)
				.Returns(true);
			_m.Setup(x => x.Send(It.IsAny<SignInQuery>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(SignInQueryResult.Succeeded("~/"));

			SignInQuery query = new SignInQuery();
			AuthController controller = new AuthController(_logger.Object, _m.Object);
			controller.ControllerContext.HttpContext = _hc.Object;

			// Act
			IActionResult result = controller.SignIn(query).GetAwaiter().GetResult();

			// Assert
			result.ShouldNotBeNull();
			result.ShouldBeOfType<RedirectResult>();
			((RedirectResult)result).Url.ShouldBe("~/");
		}

		[Fact]
		public void PostSignIn_ShouldReturnRedirect_OnSuccessfulSignIn()
		{
			// Arrange 
			_m.Setup(x => x.Send(It.IsAny<SignInCommand>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(SignInCommandResult.Succeeded("~/"));

			SignInCommand command = new SignInCommand { Email = _faker.Internet.Email(), Password = _faker.Internet.Password(8, false, "\\w", "!Aa0") };
			AuthController controller = new AuthController(_logger.Object, _m.Object);
			controller.ControllerContext.HttpContext = _hc.Object;

			// Act
			IActionResult result = controller.SignIn(command).GetAwaiter().GetResult();

			// Assert
			result.ShouldNotBeNull();
			result.ShouldBeOfType<RedirectResult>();
			((RedirectResult)result).Url.ShouldBe("~/");
		}

		[Fact]
		public void PostSignIn_ShouldReturnView_OnValidationFailed()
		{
			// Arrange
			SignInCommand command = new SignInCommand();
			AuthController controller = new AuthController(_logger.Object, _m.Object);
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
			signInCommand.Errors.Add(new KeyValuePair<string, string>(_faker.Random.String(12), _faker.Random.String(12)));
			_m.Setup(x => x.Send(It.IsAny<SignInCommand>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(signInCommand);

			SignInCommand command = new SignInCommand { Email = _faker.Internet.Email(), Password = _faker.Internet.Password(8, false, "\\w", "!Aa0") };
			AuthController controller = new AuthController(_logger.Object, _m.Object);
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
			AuthController controller = new AuthController(_logger.Object, _m.Object);
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
				((RedirectToActionResult)result).ActionName.ShouldBe("EmailConfirmation");
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
				.ReturnsAsync(SignInTwoStepQueryResult.Succeeded("~/"));

			SignInTwoStepQuery query = new SignInTwoStepQuery();
			AuthController controller = new AuthController(_logger.Object, _m.Object);
			controller.ControllerContext.HttpContext = _hc.Object;

			// Act
			IActionResult result = controller.SignInTwoStep(query).GetAwaiter().GetResult();

			// Assert
			result.ShouldNotBeNull();
			result.ShouldBeOfType<ViewResult>();
			((ViewResult)result).ViewData.Model.ShouldBeAssignableTo<SignInTwoStepCommand>();
		}

		[Fact]
		public void GetSignInTwoStep_ShouldReturnView_OnFailedQuery()
		{
			// Arrange
			SignInTwoStepQueryResult signInTwoStepQueryResult = SignInTwoStepQueryResult.Failed();
			signInTwoStepQueryResult.Errors.Add(new KeyValuePair<string, string>(_faker.Random.String(12), _faker.Random.String(12)));

			_m.Setup(x => x.Send(It.IsAny<SignInTwoStepQuery>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(signInTwoStepQueryResult);

			SignInTwoStepQuery query = new SignInTwoStepQuery();
			AuthController controller = new AuthController(_logger.Object, _m.Object);
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
				.ReturnsAsync(SignInTwoStepCommandResult.Succeeded("~/"));

			SignInTwoStepCommand command = new SignInTwoStepCommand { Code = _faker.Random.String(6) };
			AuthController controller = new AuthController(_logger.Object, _m.Object);
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
			AuthController controller = new AuthController(_logger.Object, _m.Object);
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
			SignInTwoStepCommandResult signInTwoStepCommand = SignInTwoStepCommandResult.Failed();
			signInTwoStepCommand.Errors.Add(new KeyValuePair<string, string>(_faker.Random.String(12), _faker.Random.String(12)));
			_m.Setup(x => x.Send(It.IsAny<SignInTwoStepCommand>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(signInTwoStepCommand);

			SignInTwoStepCommand command = new SignInTwoStepCommand { Code = _faker.Random.String(6) };
			AuthController controller = new AuthController(_logger.Object, _m.Object);
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
		#endregion SignInTwoStep

		#region SignUp
		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void GetSignUp_ShouldReturnView(bool authenticated)
		{
			// Arrange
			if (authenticated)
			{
				_cp.Setup(x => x.Identity!.IsAuthenticated)
					.Returns(authenticated);
				_m.Setup(x => x.Send(It.IsAny<SignInQuery>(), It.IsAny<CancellationToken>()))
					.ReturnsAsync(SignInQueryResult.Failed());
			}

			SignInQuery query = new SignInQuery();
			AuthController controller = new AuthController(_logger.Object, _m.Object);
			controller.ControllerContext.HttpContext = _hc.Object;

			// Act
			IActionResult result = controller.SignUp(query).GetAwaiter().GetResult();

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

			SignInQuery query = new SignInQuery();
			AuthController controller = new AuthController(_logger.Object, _m.Object);
			controller.ControllerContext.HttpContext = _hc.Object;

			// Act
			IActionResult result = controller.SignUp(query).GetAwaiter().GetResult();

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
			AuthController controller = new AuthController(_logger.Object, _m.Object);
			controller.ControllerContext.HttpContext = _hc.Object;

			// Act
			IActionResult result = controller.SignUp(command).GetAwaiter().GetResult();

			// Assert
			result.ShouldNotBeNull();
			result.ShouldBeOfType<RedirectToActionResult>();
			((RedirectToActionResult)result).ActionName.ShouldBe("EmailConfirmation");
		}

		[Fact]
		public void PostSignUp_ShouldReturnView_OnValidationFailed()
		{
			// Arrange 
			SignUpCommand command = new SignUpCommand();
			AuthController controller = new AuthController(_logger.Object, _m.Object);
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
			signUpCommandResult.Errors.Add(new KeyValuePair<string, string>(_faker.Random.String(12), _faker.Random.String(12)));
			_m.Setup(x => x.Send(It.IsAny<SignUpCommand>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(signUpCommandResult);

			string password = _faker.Internet.Password(8, false, "\\w", "!Aa0");
			SignUpCommand command = new SignUpCommand { Email = _faker.Internet.Email(), Password = password, ConfirmPassword = password, AcceptTerms = true };
			AuthController controller = new AuthController(_logger.Object, _m.Object);
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
			AuthController controller = new AuthController(_logger.Object, _m.Object);
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
			AuthController controller = new AuthController(_logger.Object, _m.Object);
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
			AuthController controller = new AuthController(_logger.Object, _m.Object);
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
			signOutCommandResult.Errors.Add(new KeyValuePair<string, string>(_faker.Random.String(12), _faker.Random.String(12)));
			_m.Setup(x => x.Send(It.IsAny<SignOutCommand>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(signOutCommandResult);

			SignOutCommand command = new SignOutCommand();
			AuthController controller = new AuthController(_logger.Object, _m.Object);
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

		#region EmailConfirmation
		[Fact]
		public void GetEmailConfirmation_ShouldReturnView()
		{
			// Arrange
			_m.Setup(x => x.Send(It.IsAny<EmailConfirmationQuery>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(EmailConfirmationQueryResult.Succeeded());

			EmailConfirmationQuery query = new EmailConfirmationQuery { UserId = _faker.Random.Guid().ToString() };
			AuthController controller = new AuthController(_logger.Object, _m.Object);
			controller.ControllerContext.HttpContext = _hc.Object;

			// Act
			IActionResult result = controller.EmailConfirmation(query).GetAwaiter().GetResult();

			// Assert
			result.ShouldNotBeNull();
			result.ShouldBeOfType<ViewResult>();
		}

		[Fact]
		public void GetEmailConfirmation_ShouldReturnView_OnFailedEmailConfirmation()
		{
			// Arrange
			EmailConfirmationQueryResult emailConfirmationQueryResult = EmailConfirmationQueryResult.Failed();
			emailConfirmationQueryResult.Errors.Add(new KeyValuePair<string, string>(_faker.Random.String(12), _faker.Random.String(12)));
			_m.Setup(x => x.Send(It.IsAny<EmailConfirmationQuery>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(emailConfirmationQueryResult);

			EmailConfirmationQuery query = new EmailConfirmationQuery { UserId = _faker.Random.Guid().ToString() };
			AuthController controller = new AuthController(_logger.Object, _m.Object);
			controller.ControllerContext.HttpContext = _hc.Object;

			// Act
			IActionResult result = controller.EmailConfirmation(query).GetAwaiter().GetResult();

			// Assert
			result.ShouldNotBeNull();
			result.ShouldBeOfType<ViewResult>();
			((ViewResult)result).ViewData.ModelState.ShouldNotBeNull();
			((ViewResult)result).ViewData.ModelState.Count.ShouldBe(1);
			((ViewResult)result).ViewData.ModelState.ErrorCount.ShouldBe(1);
		}

		[Fact]
		public void GetEmailConfirmation_ShouldReturnView_OnFailedValidation()
		{
			// Arrange
			EmailConfirmationQuery query = new EmailConfirmationQuery();
			AuthController controller = new AuthController(_logger.Object, _m.Object);
			controller.ControllerContext.HttpContext = _hc.Object;

			// Act
			IActionResult result = controller.EmailConfirmation(query).GetAwaiter().GetResult();

			// Assert
			result.ShouldNotBeNull();
			result.ShouldBeOfType<ViewResult>();
			((ViewResult)result).ViewData.ModelState.ShouldNotBeNull();
			((ViewResult)result).ViewData.ModelState.Count.ShouldBe(1);
			((ViewResult)result).ViewData.ModelState.ErrorCount.ShouldBe(2);
		}
		#endregion EmailConfirmation

		#region ConfirmEmail
		[Fact]
		public void GetConfirmEmail_ShouldReturnView()
		{
			// Arrange
			_m.Setup(x => x.Send(It.IsAny<ConfirmEmailQuery>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(EmailConfirmationQueryResult.Succeeded());

			ConfirmEmailQuery query = new ConfirmEmailQuery { UserId = _faker.Random.Guid().ToString(), Token = _faker.Random.String(36) };
			AuthController controller = new AuthController(_logger.Object, _m.Object);
			controller.ControllerContext.HttpContext = _hc.Object;

			// Act
			IActionResult result = controller.ConfirmEmail(query).GetAwaiter().GetResult();

			// Assert
			result.ShouldNotBeNull();
			result.ShouldBeOfType<ViewResult>();
		}

		[Fact]
		public void GetConfirmEmail_ShouldReturnView_OnFailedEmailConfirmation()
		{
			// Arrange
			EmailConfirmationQueryResult emailConfirmationQueryResult = EmailConfirmationQueryResult.Failed();
			emailConfirmationQueryResult.Errors.Add(new KeyValuePair<string, string>(_faker.Random.String(12), _faker.Random.String(12)));
			_m.Setup(x => x.Send(It.IsAny<ConfirmEmailQuery>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(emailConfirmationQueryResult);

			ConfirmEmailQuery query = new ConfirmEmailQuery { UserId = _faker.Random.Guid().ToString(), Token = _faker.Random.String(36) };
			AuthController controller = new AuthController(_logger.Object, _m.Object);
			controller.ControllerContext.HttpContext = _hc.Object;

			// Act
			IActionResult result = controller.ConfirmEmail(query).GetAwaiter().GetResult();

			// Assert
			result.ShouldNotBeNull();
			result.ShouldBeOfType<ViewResult>();
			((ViewResult)result).ViewData.ModelState.ShouldNotBeNull();
			((ViewResult)result).ViewData.ModelState.Count.ShouldBe(1);
			((ViewResult)result).ViewData.ModelState.ErrorCount.ShouldBe(1);
		}

		[Fact]
		public void GetConfirmEmail_ShouldReturnView_OnFailedValidation()
		{
			// Arrange
			ConfirmEmailQuery query = new ConfirmEmailQuery();
			AuthController controller = new AuthController(_logger.Object, _m.Object);
			controller.ControllerContext.HttpContext = _hc.Object;

			// Act
			IActionResult result = controller.ConfirmEmail(query).GetAwaiter().GetResult();

			// Assert
			result.ShouldNotBeNull();
			result.ShouldBeOfType<ViewResult>();
			((ViewResult)result).ViewData.ModelState.ShouldNotBeNull();
			((ViewResult)result).ViewData.ModelState.Count.ShouldBe(2);
			((ViewResult)result).ViewData.ModelState.ErrorCount.ShouldBe(4);
		}
		#endregion ConfirmEmail
	}
}
