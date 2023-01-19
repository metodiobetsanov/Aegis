namespace Aegis.Areas.User.Controllers
{
	using Aegis.Application.Constants;

	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc;

	[Authorize]
	[Area(ApplicationConstants.UserArea)]
	public class ProfileController : Controller
	{
		[HttpGet]
		public IActionResult Index()
			=> this.View();
	}
}
