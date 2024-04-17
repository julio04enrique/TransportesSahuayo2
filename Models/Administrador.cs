using Npgsql;
using System.Data;

namespace TransporteSahuayoAsp.Models
{
    public class Administrador :Persona
    {
        #region Propiedades
        public int CveAdmin { get; set; }
        public int CveUsuario { get; set; }
		public string NameUsuario { get; set; }
		public string Contrasena { get; set; }
		#endregion

		#region Constructores
		public Administrador()
		{

		}
		#endregion

		#region Metodos
		public Administrador LoginAdmin(string usuario, string contrasena)
		{
			const string sql = "Select adm.cve_admin,adm.cve_usuario,adm.cve_persona, per.nombre, per.apellido_pat, per.apellido_mat from public.administrador adm inner join usuarios usua " +
				"on adm.cve_usuario= usua.cve_usuario inner join personas per on adm.cve_persona=per.cve_persona where adm.cve_usuario=(Select cve_usuario from usuarios where usuario=:usuario and contrasena=:contrasena)";
			List<NpgsqlParameter> lstParameters = new List<NpgsqlParameter> {
				new NpgsqlParameter(":usuario",usuario),
				new NpgsqlParameter(":contrasena",contrasena)
			};
			Administrador administrador = new Administrador();
			DataTable dtAdmin = GetQuery(sql, lstParameters);
			if (dtAdmin.Rows.Count <= 0)
				return null;
			DataRow row = dtAdmin.Rows[0];
			administrador.CveAdmin = row.IsNull("cve_admin") ? 0 : (int)row["cve_admin"];
			administrador.CvePersona = row.IsNull("cve_persona") ? 0 : (int)row["cve_persona"];
			administrador.CveUsuario = row.IsNull("cve_usuario") ? 0 : (int)row["cve_usuario"];
			administrador.Nombre = row.IsNull("nombre") ? string.Empty : (string)row["nombre"];
			administrador.ApellidoPat = row.IsNull("apellido_pat") ? string.Empty : (string)row["apellido_pat"];
			administrador.ApellidoMat = row.IsNull("apellido_mat") ? string.Empty : (string)row["apellido_mat"];
			return administrador;
		}
		#endregion
	}
}
