
using System;
using System.Collections.Generic;

namespace TransporteSahuayoAsp.Models
{

    public static class Modales
    {

		/// <summary>
		/// Método que devuelve un string con código html para generar una ventana modal
		/// </summary>
		/// <param name="encabezado">identificador de tipo string del mensaje del encabezado</param>
		/// <param name="mensaje">Identificador de tipo string de mensajes</param>
		/// <param name="icono">icono a utilizar</param>
		/// <returns></returns>
		public static string CrearVentanaModal(string encabezado, string mensaje, int icono)
		{
			string etiquetaMesnaje = (mensaje.Length <= 100)
				? " <div class='content' style='text-align:center'><h3>" + mensaje + "</h3> </div> "
				: " <div class='content'><p>" + mensaje + "</p> </div> ";
			string icon = string.Empty;
			switch (icono)
			{
				case 1:
					icon = "<i class='exclamation triangle icon'></i>";
					break;
				case 2:
					icon = "<i class='check circle outline icon'></i>";
					break;
				case 3:
					icon = "<i class='x icon'></i>";
					break;

			}
			string alert = "<div class='ui basic modal'>" +
							   "<div class='ui icon header'>" +
									icon + " " +
									encabezado +
							   "</div>" +
							   etiquetaMesnaje +
							   "<div class='actions'>" +
								  "<div class='ui red basic cancel inverted button'>" +
								  "<i class='remove icon'></i>" +
										"Close" +
								  "</div>" +
							   "</div>" +
						   "</div>";

			return alert;
		}
	}
}
