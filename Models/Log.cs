using System;
using System.IO;

namespace TransporteSahuayoAsp.Models
{
    public static class Log
    {
        /// <summary>
        /// Propiedad que permite obtener la fecha.
        /// </summary>
        static readonly string FechaHoy = string.Format("{0:yyyy-MM-dd}", DateTime.Today);
        /// <summary>
        /// Propiedad que permite establecer la ruta en la que serán almacenados los archivos Log.
        /// </summary>
        static readonly string Ruta = Path.Combine("wwwroot/Log/Log/") + "Log-" + FechaHoy + ".log";

        /// <summary>
        /// Permite escribir en un archivo Log.
        /// </summary>
        /// <param name="mensaje">Mensaje que se escribirá en el Log.</param>
        public static void EscribirLog(string mensaje)
        {
            string log = string.Format("Fecha: {0}\nClase: {1}\nLog: {2}\nConsulta: {3}\n\n", DateTime.Now, "No especificada", mensaje, "No especificada");
            File.AppendAllText(Ruta, log);
        }

        /// <summary>
        /// Permite escribir en un archivo Log.
        /// </summary>
        /// <param name="mensaje">Mensaje que se escribirá en el Log.</param>
        /// <param name="clase">Nombre de la clase.</param>
        public static void EscribirLog(string mensaje, object clase)
        {
            string log = string.Format("Fecha: {0}\nClase: {1}\nLog: {2}\nConsulta: {3}\n\n", DateTime.Now, clase.GetType(), mensaje, "No especificada");
            File.AppendAllText(Ruta, log);
        }

        /// <summary>
        /// Permite escribir en un archivo Log.
        /// </summary>
        /// <param name="mensaje">Mensaje que se escribirá en el Log.</param>
        /// <param name="clase">Nombre de la clase.</param>
        /// <param name="consulta">Instrucción SQL.</param>
        public static void EscribirLog(string mensaje, object clase, string consulta)
        {
            string log = string.Format("Fecha: {0}\nClase: {1}\nLog: {2}\nConsulta: {3}\n\n", DateTime.Now, clase.GetType(), mensaje, consulta);
            File.AppendAllText(Ruta, log);
        }

        /// <summary>
        /// Permite escribir en un archivo Log.
        /// </summary>
        /// <param name="mensaje">Mensaje que se escribirá en el Log.</param>
        /// <param name="clase">Nombre de la clase.</param>
        /// <param name="consulta">Instrucción SQL.</param>
        /// <param name="valores">Valores utilizados en la instrucción SQL.</param>
        public static void EscribirLog(string mensaje, object clase, string consulta, string valores)
        {
            string log = string.Format("Fecha: {0}\nClase: {1}\nLog: {2}\nConsulta: {3}\nValores: {4}\n\n", DateTime.Now, clase.GetType(), mensaje, consulta, valores);
            //File.AppendAllText(Ruta, log);
        }

        /// <summary>
        /// Permite limpiar un archivo Log.
        /// </summary>
        /// <param name="nombreArchivo">Nombre del archivo Log.</param>
        public static void LimpiarLog(string nombreArchivo)
        {
            File.WriteAllText(Ruta, string.Empty);
        }

        /// <summary>
        /// Permite limpiar distintos archivos Log.
        /// </summary>
        /// <param name="mes">Mes en que fue creado el archivo Log.</param>
        /// <param name="anio">Año en que fue creado el archivo Log.</param>
        /// <param name="dia">Día en que fue creado el archivo Log.</param>
        public static void LimpiarLog(string mes, string anio, string dia)
        {
            string rutaArch = Path.Combine("wwwroot/Log/Log/") + "Log-" + anio + "-" + mes + "-" + dia + ".log";
            File.WriteAllText(rutaArch, string.Empty);
        }

        /// <summary>
        /// Permite eliminar un archivo Log.
        /// </summary>
        /// <param name="nombreArchivo">Nombre del archivo Log.</param>
        public static void EliminarLog(string nombreArchivo)
        {
            File.Delete(Ruta);
        }
    }
}
