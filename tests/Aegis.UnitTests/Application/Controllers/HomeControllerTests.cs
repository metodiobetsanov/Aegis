namespace Aegis.UnitTests.Core.Controllers
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
