using Microsoft.AspNetCore.Mvc;
using TransporteSahuayoAsp.Models;
using Microsoft.AspNetCore.Authorization;
namespace TransporteSahuayoAsp.Controllers
{
	[Authorize]
	public class AdministrarChoferesController : Controller
	{
		public IActionResult Index()
		{
			ViewData["ChoferesList"] = new Chofer().GetAllChoferes();
			return View();
		}
		[HttpPost]
		public ActionResult Index(string accion, Chofer chofer)
		{
			switch (accion)
			{
				case "GenerarNuevoView":
					ViewData["genNew"] = true;
					return View("DatosChofer");
				case "GenerarNuevo":
					var respInsert= new Chofer().AgregarChofer(chofer);
					if (respInsert)
					{
						ViewData["ChoferesList"] = new Chofer().GetAllChoferes();
						TempData["Exception"] = Modales.CrearVentanaModal("Listo!", "Chofer creado Correctamente", 2);
						return View("Index");
					}
					else
					{
                        TempData["Exception"] = Modales.CrearVentanaModal("Error!", "No se fue Posible esta Accion", 3);
                        return View("DatosChofer");
                    }	
					
				case "Modificar":
					var respUpdate= new Chofer().UpdateDatosChofer(chofer);
					if (respUpdate)
					{
						ViewData["ChoferesList"] = new Chofer().GetAllChoferes();
						TempData["Exception"] = Modales.CrearVentanaModal("Listo!", "Datos Guardados", 2);
						return View("Index");
                    }
					else
					{
                        TempData["Exception"] = Modales.CrearVentanaModal("Error!", "No se fue Posible esta Accion", 3);
						return View("DatosChofer");
                    }
					
				case "Eliminar":
					var respDelete= new Chofer().DeleteChofer(chofer);
					if (respDelete)
					{
						ViewData["ChoferesList"] = new Chofer().GetAllChoferes();
						TempData["Exception"] = Modales.CrearVentanaModal("Listo!", "Chofer Eliminado", 2);
						return View("Index");
					}
					else
					{
						TempData["Exception"] = Modales.CrearVentanaModal("Error!", "No se fue Posible esta Accion", 3);
						return View("DatosChofer");
					}


				default: return View();
			}
		}
		
		public ActionResult SelectChofer(int CveChofer)
		{
			ViewData["DatosChofer"]= new Chofer().GetChoferForClave(CveChofer);
			ViewData["modifi"] = true;
			return View("DatosChofer");
		}
	}
}
