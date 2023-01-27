#region copyright
//----------------------------------------------------------------------
// Copyright 2023 MNB Software
// Licensed under the Apache License, Version 2.0
// You may obtain a copy at http://www.apache.org/licenses/LICENSE-2.0
//----------------------------------------------------------------------
#endregion
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
		#region SendAccountActivation
		[Fact]
		public void GetSendAccountActivation_ShouldReturnView()
		{
			// Arrange
			_m.Setup(x => x.Send(It.IsAny<SendAccountActivationCommand>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(HandlerResult.Succeeded());

			AuthenticationController controller = new AuthenticationController(_logger.Object, _dpp.Object, _m.Object);
			controller.ControllerContext.HttpContext = _hc.Object;

			// Act
			IActionResult result = controller.ActivateAccount();

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

			AuthenticationController controller = new AuthenticationController(_logger.Object, _dpp.Object, _m.Object);
			controller.ControllerContext.HttpContext = _hc.Object;

			// Act
			IActionResult result = controller.ActivateAccount();
			result.ShouldNotBeNull();
			result.ShouldBeOfType<RedirectResult>();
			((RedirectResult)result).Url.ShouldBe("~/");
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
			((ViewResult)result).ViewName.ShouldBe("ActivateAccountConfirmation");
		}

		[Fact]
		public void GetActivateAccount_ShouldReturnView_OnEpmtyQueryString()
		{
			// Arrange
			AuthenticationController controller = new AuthenticationController(_logger.Object, _dpp.Object, _m.Object);
			controller.ControllerContext.HttpContext = _hc.Object;

			// Act
			IActionResult result = controller.ActivateAccount(string.Empty).GetAwaiter().GetResult();

			// Assert
			result.ShouldNotBeNull();
			result.ShouldBeOfType<ViewResult>();
			((ViewResult)result).ViewName.ShouldBe("ActivateAccountConfirmation");
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
			((ViewResult)result).ViewName.ShouldBe("ActivateAccountConfirmation");
		}
		#endregion ActivateAccount
	}
}
