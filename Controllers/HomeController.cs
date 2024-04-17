using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TransporteSahuayoAsp.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace TransporteSahuayoAsp.Controllers
{
	[Authorize]
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
		}
		public IActionResult IndexA()
		{
			var cve = User.FindFirst(ClaimTypes.NameIdentifier);
			var respViaj = new ViajesAdmin().ViajesFromAdminByWeek(int.Parse(cve.Value));
			if (respViaj == null)
			{
				return View("Index");
			}
			else
			{
				ViewData["CosteTotalManiobras"] = new ViajesAdmin().CosteTotal(respViaj.ToList(), 2);
				ViewData["CosteTotal"] = new ViajesAdmin().CosteTotal(respViaj.ToList(),1);
				ViewData["WeekViajes"] = respViaj;
				return View("Index");
			}
		}
		
		public IActionResult IndexC()
		{
			var cve= User.FindFirst(ClaimTypes.NameIdentifier); 
			var respViaj = new ViajesChofer().ViajesFromChoferByWeek(int.Parse(cve.Value));
			if(respViaj == null)
			{
				return View("Index");
			}
			else
			{
				ViewData["CosteTotalManiobras"] = new ViajesChofer().CosteTotal(respViaj.ToList(), 2);
				ViewData["CosteTotal"] = new ViajesChofer().CosteTotal(respViaj.ToList(), 1);
                ViewData["WeekViajes"] = respViaj;
				return View("Index");
			}
		}



	}
}