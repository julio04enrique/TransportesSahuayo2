using Npgsql;
using System.Data;

namespace TransporteSahuayoAsp.Models
{
    public class Destinos :AccesoDatos
    {
        #region Propiedades
        public int CveDestino { get; set; }
        public string Nombre { get; set; }
        public decimal Costo { get; set; }
        #endregion

        #region Constructores
        public Destinos() { }
		#endregion

		#region Metodos
		private List<Destinos> ToList(DataTable table)
		{
			List<Destinos> listDest = new();

			foreach (DataRow row in table.Rows)
			{
				Destinos destino = new Destinos();
				destino.CveDestino = row.IsNull("cve_destino") ? 0 : (int)row["cve_destino"];
				destino.Nombre = row.IsNull("nombre") ? string.Empty : (string)row["nombre"];
				destino.Costo = row.IsNull("costo") ? 0 : (decimal)row["costo"];
				listDest.Add(destino);
			}
			return listDest;
		}
		private List<NpgsqlParameter> SetParametros(Destinos destino)
		{
			return new List<NpgsqlParameter>
			{
				new NpgsqlParameter(":cveDestino",destino.CveDestino),
				new NpgsqlParameter(":nombre",destino.Nombre ==string.Empty ? DBNull.Value: destino.Nombre),
				new NpgsqlParameter(":costo", destino.Costo == 0 ? DBNull.Value: destino.Costo)
			};
		}
		private string GetValores(Destinos destino)
		{
			return string.Format("cve_destino={0},nombre={1},costo={2}",destino.CveDestino,destino.Nombre,destino.Costo);
		}
		public List<Destinos> GetAllDestinos()
		{
			const string sql = "select * from destinos";
			List<Destinos> listDest = new();
			DataTable tb = GetQuery(sql);
			listDest = ToList(tb);
			return listDest;
		}
        
		public Destinos GetDestinoForClavew(int cveDestino)
        {
            List<NpgsqlParameter> lstParameters = new List<NpgsqlParameter> {
                new NpgsqlParameter(":cveDestino", cveDestino)
            };
            const string sql = "SELECT * FROM destinos where cve_destino= :cveDestino;";
            DataTable tbDestino = GetQuery(sql, lstParameters);
			Destinos destino = new Destinos();
            if (tbDestino.Rows.Count <= 0)
                return destino;
            DataRow row = tbDestino.Rows[0];
            destino.CveDestino = row.IsNull("cve_destino") ? 0 : (int)row["cve_destino"];
            destino.Nombre = row.IsNull("nombre") ? string.Empty : (string)row["nombre"];
            destino.Costo = row.IsNull("costo") ? 0 : (decimal)row["costo"];
            return destino;
        }
		public bool AgregarDestino(Destinos destino)
		{
			const string sql = "INSERT INTO public.destinos( nombre, costo) VALUES ( :nombre, :costo);";
			try
			{
				int afectados = ExecuteQuery(sql, SetParametros(destino));
				if (afectados > 0)
					Log.EscribirLog("Datos actualizados correctamente en : ", this, sql, GetValores(destino));
				return afectados > 0;
			}
			catch (NpgsqlException exception)
			{
				ErrorLog.EscribirLog("Error al actualizar datos en : " + exception.Message, this, sql, GetValores(destino));
				return false;
			}
		}
		public bool UpdateDestino(Destinos destino)
		{
			const string sql = "UPDATE public.destinos SET nombre=:nombre, costo=:costo WHERE cve_destino=:cveDestino;";
			try
			{
				int afectados = ExecuteQuery(sql, SetParametros(destino));
				if (afectados > 0)
					Log.EscribirLog("Datos  actualizados correctamente en : ", this, sql, GetValores(destino));
				return afectados > 0;
			}
			catch (NpgsqlException exception)
			{
				ErrorLog.EscribirLog("Error al actualizar datos  en : " + exception.Message, this, sql, GetValores(destino));
				return false;
			}
		}
		//public bool DeleteDestino(Destinos destino)
		//{
		//	const string sql = "DELETE FROM public.destinos WHERE cve_destino = :cveDestino;";
		//	try
		//	{
		//		int afectados = ExecuteQuery(sql, SetParametros(destino));
		//		if (afectados > 0)
		//			Log.EscribirLog("Datos  actualizados correctamente en : ", this, sql, GetValores(destino));
		//		return afectados > 0;
		//	}
		//	catch (NpgsqlException exception)
		//	{
		//		ErrorLog.EscribirLog("Error al actualizar datos  en : " + exception.Message, this, sql, GetValores(destino));
		//		return false;
		//	}
		//}
		#endregion
	}
}
