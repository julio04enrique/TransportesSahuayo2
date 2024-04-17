using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TransporteSahuayoAsp.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace TransporteSahuayoAsp.Controllers
{
	public class LoginController : Controller
	{
		public IActionResult Index()
		{
			return View("Log");
		}
		[HttpPost]
		public async Task<IActionResult> Index(string Usuario, string Contrasena)
		{
			   var logAdmin = new Administrador().LoginAdmin(Usuario, Contrasena);
			if (logAdmin != null)
			{
				var claims = new List<Claim> {
				new Claim(ClaimTypes.Name, logAdmin.Nombre),
				new Claim(ClaimTypes.Role,"Admin"),
				new Claim(ClaimTypes.NameIdentifier, logAdmin.CveAdmin.ToString())
				};
				var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
				await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
				return RedirectToAction("IndexA", "Home");
			}
			else
			{
				var logChofer = new Chofer().LoginChofer(Usuario, Contrasena);
				if (logChofer != null)
				{
					var claims = new List<Claim> {
					new Claim(ClaimTypes.Name, logChofer.Nombre),
					new Claim(ClaimTypes.Role, "Chofer"),
					new Claim(ClaimTypes.NameIdentifier, logChofer.CveChofer.ToString())
					};
					var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
					await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
					return RedirectToAction("IndexC", "Home");
				}
				else
				{
					//TempData["Exception"] = Modales.CrearVentanaModal("Error", "Usuario No registrado", 1);
					return View("Log");
				}
			}


		}
		public IActionResult AccesoDenegado()
		{
			return View("Log");
		}
		public async Task<IActionResult> CerrarSesion()
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			return View("Log");
		}

		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
