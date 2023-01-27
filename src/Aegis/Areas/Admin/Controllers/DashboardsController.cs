namespace Aegis.Areas.Admin.Controllers
{
	using Aegis.Core.Constants;

	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc;

	[Authorize]
	[Area(ApplicationConstants.AdminArea)]
	public class DashboardsController : Controller
	{
		[HttpGet]
		public IActionResult Index()
			=> this.View();
	}
}
