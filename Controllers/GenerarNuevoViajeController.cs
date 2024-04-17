using Microsoft.AspNetCore.Mvc;
using TransporteSahuayoAsp.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace TransporteSahuayoAsp.Controllers
{
	[Authorize]
	public class GenerarNuevoViajeController : Controller
	{
		public IActionResult Index()
		{
			return View(); 
		}
		[HttpPost]
		public ActionResult IndexA (string accion, Viaje viaje)
		{
			switch (accion)
			{
				case "NuevoViaje":
					if (viaje.CveDestino == 0)
						return View("Index");
					var cve = User.FindFirst(ClaimTypes.NameIdentifier);
					viaje.CveTransportista = int.Parse(cve.Value);
					var respAgregar = new Viaje().AgregarViajeAdmin(viaje);
					if (respAgregar)
					{
						TempData["Exception"] = Modales.CrearVentanaModal("Listo!", "Viaje generado Correctamente", 2);
						return View("Index");
					}
					else
					{
						TempData["Exception"] = Modales.CrearVentanaModal("Error!", "No se fue Posible esta Accion", 3);
						return View("Index");
					}


				default: return View("Index");

            }
		}
		[HttpPost]
		public ActionResult IndexC(string accion, Viaje viaje)
		{

			switch (accion)
			{
				case "NuevoViaje":
					var cve = User.FindFirst(ClaimTypes.NameIdentifier);
					viaje.CveTransportista = int.Parse(cve.Value);
					var respAgregar = new Viaje().AgregarViajeChofer(viaje);
					if (respAgregar)
					{
						TempData["Exception"] = Modales.CrearVentanaModal("Listo!", "Viaje generado Correctamente", 2);
						return View("Index");
					}
					else
					{
						TempData["Exception"] = Modales.CrearVentanaModal("Error!", "No se fue Posible esta Accion", 3);
						return View("Index");
					}

				default: return View("Index");

			}
		}
		public JsonResult GetDestinos()
		{
			var resp = new Destinos().GetAllDestinos();
			return Json(resp);
		}
		public JsonResult GetDatosDestino(int cveDestino)
		{
            var resp = new Destinos().GetDestinoForClavew(cveDestino);
            return Json(resp);
        }

    }
}
