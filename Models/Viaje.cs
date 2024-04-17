using Npgsql;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace TransporteSahuayoAsp.Models
{
    public class Viaje : AccesoDatos
    {
        #region Propiedades
        public int CveViaje { get; set; }
        public int CveDestino { get; set; }
        public DateTime Fecha { get; set; }
        public int ControlVehicular { get; set; }
        public int CveTransportista { get; set; }
        public decimal Maniobra { get; set; }
        #endregion

        #region Constructores
        public Viaje()
        {

        }
        #endregion

        #region Metodos
        
        private List<NpgsqlParameter> SetParametros(Viaje viaje)
        {
            return new List<NpgsqlParameter>
            {
                new NpgsqlParameter(":cveViaje",viaje.CveViaje),
                new NpgsqlParameter(":cveDiestino",viaje.CveDestino ==0 ? DBNull.Value: viaje.CveDestino),
                new NpgsqlParameter(":fecha",viaje.Fecha),
                new NpgsqlParameter(":controlVehicular",viaje.ControlVehicular==0 ? DBNull.Value: viaje.ControlVehicular),
                new NpgsqlParameter(":cveTransportista", viaje.CveTransportista==0 ? DBNull.Value: viaje.CveTransportista),
                new NpgsqlParameter(":maniobra", viaje.Maniobra== decimal.MinValue ? DBNull.Value: viaje.Maniobra)
            };
        }
        private string GetValores(Viaje viaje)
        {
            return string.Format("cve_viaje={0},cve_destino={1},fecha={2},control_vehicular={3}, maniobra={4}",viaje.CveViaje,viaje.CveDestino,
                viaje.Fecha,viaje.ControlVehicular, viaje.Maniobra);
        }
		public bool AgregarViajeChofer(Viaje viaje)
		{
			const string sql = "INSERT INTO public.viaje(cve_destino, control_vehicular,maniobra) VALUES (:cveDiestino, :controlVehicular, :maniobra); " +
				"INSERT INTO public.viajes_chofer(cve_chofer, cve_viaje) VALUES (:cveTransportista,(SELECT cve_viaje FROM viaje ORDER BY fecha DESC LIMIT 1));";
			try
			{
				int afectados = ExecuteQuery(sql, SetParametros(viaje));
				if (afectados > 0)
					Log.EscribirLog("Datos actualizados correctamente en : ", this, sql, GetValores(viaje));
				return afectados > 0;
			}
			catch (NpgsqlException exception)
			{
				ErrorLog.EscribirLog("Error al actualizar datos  en : " + exception.Message, this, sql, GetValores(viaje));
				return false;
			}
		}
		public bool AgregarViajeAdmin(Viaje viaje)
		{
			const string sql = "INSERT INTO public.viaje(cve_destino, control_vehicular, maniobra) VALUES (:cveDiestino, :controlVehicular, :maniobra); " +
				"INSERT INTO public.viajes_admin(cve_admin, cve_viaje) VALUES (:cveTransportista,(SELECT cve_viaje FROM viaje ORDER BY fecha DESC LIMIT 1));";
			try
			{
				int afectados = ExecuteQuery(sql, SetParametros(viaje));
				if (afectados > 0)
					Log.EscribirLog("Datos actualizados correctamente en : ", this, sql, GetValores(viaje));
				return afectados > 0;
			}
			catch (NpgsqlException exception)
			{
				ErrorLog.EscribirLog("Error al actualizar datos  en : " + exception.Message, this, sql, GetValores(viaje));
				return false;
			}
		}
		#endregion
	}
}
