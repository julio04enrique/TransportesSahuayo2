using Npgsql;
using System.Data;

namespace TransporteSahuayoAsp.Models
{
	public class ViajesAdmin: AccesoDatos
	{
		#region Propiedades
		public int CveAdmin { get; set; }
		public string NombreDestino { get; set; }
		public DateTime Fecha { get; set; }
		public decimal Costo { get; set; }
		public int ControlVehicular { get; set; }
		public decimal Maniobra { get; set; }
		#endregion

		#region Constructor
		public ViajesAdmin() { }
		#endregion
		#region Metodos
		private List<ViajesAdmin> ToList(DataTable table)
		{
			List<ViajesAdmin> listViaj = new();

			foreach (DataRow row in table.Rows)
			{
				ViajesAdmin viajesAdmin = new();

				viajesAdmin.NombreDestino = row.IsNull("destino") ? string.Empty : (string)row["destino"];
				viajesAdmin.Fecha = row.IsNull("fecha") ? DateTime.MinValue : (DateTime)row["fecha"];
				viajesAdmin.Costo = row.IsNull("costo") ? decimal.MinValue : (decimal)row["costo"];
				viajesAdmin.Maniobra = row.IsNull("maniobra") ? decimal.MinValue : (decimal)row["maniobra"];
				viajesAdmin.ControlVehicular = row.IsNull("control_vehicular") ? 0 : (int)row["control_vehicular"];
				listViaj.Add(viajesAdmin);
			}
			return listViaj;
		}
		public List<ViajesAdmin> ViajesFromAdminByWeek(int CveAdmin)
		{
			List<NpgsqlParameter> listParameter = new List<NpgsqlParameter> {
			new NpgsqlParameter(":cveAdmin",CveAdmin)

			};
			const string sql = "WITH semana_actual AS (SELECT " +
				"DATE_TRUNC('week', CURRENT_DATE) AS inicio_semana, " +
				"DATE_TRUNC('week', CURRENT_DATE) + INTERVAL '1 week' - INTERVAL '1 day' AS fin_semana) " +
				"SELECT va.cve_admin,per.nombre,dest.nombre as destino,via.fecha,dest.costo,via.maniobra,via.control_vehicular from public.viajes_admin va " +
				"inner join administrador adm on va.cve_admin= adm.cve_admin inner join personas per on per.cve_persona= adm.cve_persona " +
				"inner join viaje via on va.cve_viaje= via.cve_viaje inner join destinos dest on dest.cve_destino= via.cve_destino " +
				"where adm.cve_admin = :cveAdmin and via.fecha >= (Select inicio_semana from semana_actual) " +
				"and via.fecha <= (Select fin_semana from semana_actual);";
			List<ViajesAdmin> listVia = new();
			DataTable tb = GetQuery(sql, listParameter);
			if (tb.Rows.Count <= 0)
				return null;
			listVia = ToList(tb);

			return listVia;
		}
		public decimal CosteTotal(List<ViajesAdmin> listCostos,int caso)
		{
			List<decimal> list = new List<decimal>();
			decimal total;
			switch (caso)
			{
				case 1:
					foreach (var item in listCostos)
					{
						list.Add(item.Costo);
					}
					total = list.Sum();
					return total;
				case 2:
					foreach (var item in listCostos)
					{
						list.Add(item.Maniobra);
					}
					total = list.Sum();
					return total;
				default:
					return 0;
			}
		}
		#endregion
	}
}
