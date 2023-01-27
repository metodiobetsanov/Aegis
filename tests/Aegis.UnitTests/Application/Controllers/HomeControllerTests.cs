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

	using Microsoft.AspNetCore.Mvc;

	public class HomeControllerTests
	{
		[Fact]
		public void Index_ShouldReturnView()
		{
			// Arrange
			HomeController controller = new HomeController();

			// Act
			IActionResult result = controller.Index();

			// Assert
			result.ShouldBeOfType<ViewResult>();
		}
	}
}
