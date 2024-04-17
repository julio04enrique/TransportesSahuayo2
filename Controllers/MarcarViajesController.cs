using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
namespace TransporteSahuayoAsp.Controllers
{
	[Authorize]
	public class MarcarViajesController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
