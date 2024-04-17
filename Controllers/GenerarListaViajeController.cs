using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TransporteSahuayoAsp.Models;

namespace TransporteSahuayoAsp.Controllers
{
	[Authorize]
	public class GenerarListaViajeController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
		[HttpPost]
		public ActionResult ConsultaListado(string accion,int cveChofer, DateTime inicio, DateTime fin)
		{
			switch (accion)
			{
				case "GenListForWeek":
					if(cveChofer==0)
					{
						TempData["Exception"] = Modales.CrearVentanaModal("Advertiencia", "Favor Selecciona un Chofer", 1);
						return View("Index");
					}
					else
					{
						var respForWeek = new ViajesChofer().ViajesFromChoferByWeek(cveChofer);
						if (respForWeek != null)
						{
							ViewData["CosteTotalManiobras"] = new ViajesChofer().CosteTotal(respForWeek.ToList(), 2);
							ViewData["CosteTotal"] = new ViajesChofer().CosteTotal(respForWeek.ToList(), 1);
							ViewData["ListViajes"] = respForWeek;
							return View("Index");
						}
						else
						{
							TempData["Exception"] = Modales.CrearVentanaModal("Advertiencia", "Este Chofer NO cuenta con Viajes", 1);
							return View("Index");
						}
					}
				case "GenList":
					var resp = new ViajesChofer().ViajesFromChoferByDate(cveChofer, inicio, fin);
					if (resp != null)
					{
						ViewData["CosteTotalManiobras"] = new ViajesChofer().CosteTotal(resp.ToList(), 2);
						ViewData["CosteTotal"] = new ViajesChofer().CosteTotal(resp.ToList(), 1);
						ViewData["ListViajes"] = resp;
						return View("Index");
					}
					else
					{
						TempData["Exception"] = Modales.CrearVentanaModal("Advertiencia", "Este Chofer NO cuenta con Viajes", 1);
						return View("Index");
					}
				default:
					return View("Index");
			}
			
		}
		public JsonResult GetChoferes()
		{
			var res = new Chofer().GetAllChoferes();
			return Json(res);
		}
	}
}
