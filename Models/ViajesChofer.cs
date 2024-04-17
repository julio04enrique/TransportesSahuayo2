using Npgsql;
using System.Data;

namespace TransporteSahuayoAsp.Models
{
	public class ViajesChofer :AccesoDatos
	{
		#region Propiedades
		public int CveChofer { get; set; }
		public string NombreChofer { get; set; }
		public string NombreDestino { get; set; }
		public DateTime Fecha { get; set; }
		public decimal Costo { get; set; }
		public int ControlVehicular { get; set; }
		public decimal Maniobra { get; set; }
		#endregion

		#region Constructor
		public ViajesChofer() { }
		#endregion
		#region Metodos
		private List<ViajesChofer> ToList(DataTable table)
		{
			List<ViajesChofer> listViaj = new();

			foreach (DataRow row in table.Rows)
			{
				ViajesChofer viajesChofer = new();
				viajesChofer.CveChofer = row.IsNull("cve_chofer") ? 0 : (int)row["cve_chofer"];
				viajesChofer.NombreChofer = row.IsNull("nombre") ? string.Empty : (string)row["nombre"];
				viajesChofer.NombreDestino = row.IsNull("destino") ? string.Empty : (string)row["destino"];
				viajesChofer.Fecha = row.IsNull("fecha") ? DateTime.MinValue : (DateTime)row["fecha"];
				viajesChofer.Costo = row.IsNull("costo") ? decimal.MinValue : (decimal)row["costo"];
				viajesChofer.Maniobra = row.IsNull("maniobra") ? decimal.MinValue : (decimal)row["maniobra"];
				viajesChofer.ControlVehicular = row.IsNull("control_vehicular") ? 0 : (int)row["control_vehicular"];
				listViaj.Add(viajesChofer);
			}
			return listViaj;
		}
		public List<ViajesChofer> ViajesFromChoferByDate(int CveChofer,DateTime Inicio, DateTime Fin)
		{
			List<NpgsqlParameter> listParameter = new List<NpgsqlParameter> { 
			new NpgsqlParameter(":cveChofer",CveChofer),
			new NpgsqlParameter(":fechaInicio", Inicio),
			new NpgsqlParameter(":fechaFin",Fin)
			};
			const string sql = "select ch.cve_chofer,per.nombre,dest.nombre as destino,via.fecha,dest.costo,via.maniobra,via.control_vehicular from public.viajes_chofer vc " +
				"inner join chofer ch on vc.cve_chofer= ch.cve_chofer inner join personas per on per.cve_persona= ch.cve_persona " +
				"inner join viaje via on vc.cve_viaje= via.cve_viaje inner join destinos dest on dest.cve_destino= via.cve_destino " +
				"where ch.cve_chofer = :cveChofer and via.fecha >= DATE_TRUNC('day', :fechaInicio ::timestamp) " +
				"and via.fecha <= DATE_TRUNC('day', :fechaFin ::timestamp) + INTERVAL '1 day';";
			List<ViajesChofer> listVia = new();
			DataTable tb = GetQuery(sql,listParameter);
			if (tb.Rows.Count <= 0)
				return null;
			listVia = ToList(tb);
			
			return listVia;
		}
		public List<ViajesChofer> ViajesFromChoferByWeek(int CveChofer)
		{
			List<NpgsqlParameter> listParameter = new List<NpgsqlParameter> {
			new NpgsqlParameter(":cveChofer",CveChofer)
			
			};
			const string sql = "WITH semana_actual AS (SELECT " +
				"DATE_TRUNC('week', CURRENT_DATE) AS inicio_semana, " +
				"DATE_TRUNC('week', CURRENT_DATE) + INTERVAL '1 week' - INTERVAL '1 day' AS fin_semana) " +
				"SELECT ch.cve_chofer,per.nombre,dest.nombre as destino,via.fecha,dest.costo,via.maniobra,via.control_vehicular from public.viajes_chofer vc " +
				"inner join chofer ch on vc.cve_chofer= ch.cve_chofer inner join personas per on per.cve_persona= ch.cve_persona " +
				"inner join viaje via on vc.cve_viaje= via.cve_viaje inner join destinos dest on dest.cve_destino= via.cve_destino " +
				"where ch.cve_chofer = :cveChofer and via.fecha >= (Select inicio_semana from semana_actual) " +
				"and via.fecha <= (Select fin_semana from semana_actual);";
			List<ViajesChofer> listVia = new();
			DataTable tb = GetQuery(sql, listParameter);
			if (tb.Rows.Count <= 0)
				return null;
			listVia = ToList(tb);

			return listVia;
		}
		public decimal CosteTotal (List<ViajesChofer> listCostos, int caso)
		{
			List<decimal> list = new List<decimal>();
			decimal total;
			switch (caso){
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
