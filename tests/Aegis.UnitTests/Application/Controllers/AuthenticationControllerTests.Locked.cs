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
	using global::Aegis.Core.Queries.Authentication;
	using global::Aegis.Models.Authentication;

	using Microsoft.AspNetCore.Mvc;

	public partial class AuthenticationControllerTests
	{
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
	}
}
