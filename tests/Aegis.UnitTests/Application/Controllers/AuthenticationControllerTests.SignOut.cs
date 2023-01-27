namespace Aegis.UnitTests.Application.Controllers
{
	using global::Aegis.Controllers;
	using global::Aegis.Core.Commands.Authentication;
	using global::Aegis.Core.Queries.Authentication;
	using global::Aegis.Models.Authentication;

	using Microsoft.AspNetCore.Mvc;

	public partial class AuthenticationControllerTests
	{
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
	}
}
