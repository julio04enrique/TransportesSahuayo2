using Microsoft.AspNetCore.Mvc;
using TransporteSahuayoAsp.Models;
using Microsoft.AspNetCore.Authorization;
namespace TransporteSahuayoAsp.Controllers
{
	[Authorize]
	public class AdministrarDestinosController : Controller
	{
		public IActionResult Index()
		{
			ViewData["ListDest"] = new Destinos().GetAllDestinos();
			return View();
		}
		[HttpPost]
		public ActionResult Index(string accion, Destinos destino)
		{
			switch (accion)
			{
				case "GenerarNuevoView":
					ViewData["genNew"] = true;
					return View("DatosDestino");
				case "CrearDestino":
					var respInsert = new Destinos().AgregarDestino(destino);
					if (respInsert)
					{
						TempData["Exception"] = Modales.CrearVentanaModal("Listo!", "Se Agrego un Nuevo Destino", 2);
						ViewData["ListDest"] = new Destinos().GetAllDestinos();
						return View("Index");
					}
					else
					{
						TempData["Exception"] = Modales.CrearVentanaModal("Error!", "No se fue Posible esta Accion", 3);
						return View("DatosDestino");
					}
				case "Modificar":
					var respUpdate= new Destinos().UpdateDestino(destino);
					if (respUpdate)
					{
						TempData["Exception"] = Modales.CrearVentanaModal("Listo!", "Cambios Guardados", 2);
						ViewData["ListDest"] = new Destinos().GetAllDestinos();
						return View("Index");
					}
					else
					{
						TempData["Exception"] = Modales.CrearVentanaModal("Error!", "No se fue Posible esta Accion", 3);
						return View("DatosDestino");
					}
				
				default: return View();
			}
		}
		public ActionResult SelectDestino(int CveDestino)
		{
			ViewData["DataDestino"] = new Destinos().GetDestinoForClavew(CveDestino);
            ViewData["modifi"] = true;
			return View("DatosDestino");
        }

    }
}
