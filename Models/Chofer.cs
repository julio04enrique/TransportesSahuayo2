using Npgsql;
using System.Data;

namespace TransporteSahuayoAsp.Models
{
    public class Chofer: Persona
    {
        #region Propiedades
        public int CveChofer { get; set; }
        public int CveUsuario { get; set; }
        public string NameUsuario { get; set; }
        public string Contrasena { get; set; }
		#endregion

		#region Constructores
		public Chofer() { }
		#endregion

		#region Metodos
		private List<Chofer> ToList(DataTable table)
		{
			List<Chofer> listChofer = new();

			foreach (DataRow row in table.Rows)
			{
				Chofer chofer = new Chofer();
                chofer.CveChofer = row.IsNull("cve_chofer") ? 0 : (int)row["cve_chofer"];
                chofer.CvePersona = row.IsNull("cve_persona") ? 0 : (int)row["cve_persona"];
                chofer.CveUsuario = row.IsNull("cve_usuario") ? 0 : (int)row["cve_usuario"];
				chofer.Nombre = row.IsNull("nombre") ? string.Empty : (string)row["nombre"];
				chofer.ApellidoPat = row.IsNull("apellido_pat") ? string.Empty : (string)row["apellido_pat"];
				chofer.ApellidoMat = row.IsNull("apellido_mat") ? string.Empty : (string)row["apellido_mat"];
				chofer.NameUsuario = row.IsNull("usuario") ? string.Empty : (string)row["usuario"];
				chofer.Contrasena = row.IsNull("contrasena") ? string.Empty : (string)row["contrasena"];
                listChofer.Add(chofer);
			}
			return listChofer;
		}
		private List<NpgsqlParameter> SetParametros(Chofer chofer)
        {
            return new List<NpgsqlParameter>
            {
                new NpgsqlParameter(":cveChofer",chofer.CveChofer),
                new NpgsqlParameter(":cveUsuario", chofer.CveUsuario),
                new NpgsqlParameter(":cvePersona",  chofer.CvePersona == 0 ? DBNull.Value: chofer.CvePersona),
                new NpgsqlParameter(":nombre",chofer.Nombre == null ? DBNull.Value: chofer.Nombre),
                new NpgsqlParameter(":apellidoPat",chofer.ApellidoPat == null ? DBNull.Value : chofer.ApellidoPat),
                new NpgsqlParameter(":apellidoMat",chofer.ApellidoMat == null ? DBNull.Value : chofer.ApellidoMat),
                new NpgsqlParameter(":usuario",chofer.NameUsuario),
                new NpgsqlParameter(":contrasena",chofer.Contrasena)
            };
        }
        private string GetValores(Chofer chofer)
        {
            return string.Format("cve_chofer = {0}, cve_usuario = {1}, cve_persona = {2}, nombre={3}, apellido_pat={4}" +
                "apellido_mat={5},usuario={6}, contrasena={7}",chofer.CveChofer,
                chofer.CveUsuario,chofer.CvePersona,chofer.Nombre,chofer.ApellidoPat,chofer,ApellidoMat,
                chofer.NameUsuario,chofer.Contrasena);
        }
        public bool AgregarChofer(Chofer chofer)
        {
            const string sql = "INSERT INTO public.personas( nombre, apellido_pat, apellido_mat) VALUES (:nombre, :apellidoPat, :apellidoMat);" +
                "INSERT INTO public.usuarios(usuario, contrasena) VALUES ( :usuario, :contrasena);" +
                "INSERT INTO public.chofer(cve_persona,cve_usuario) values((SELECT (cve_persona) FROM PERSONAS ORDER BY fecha_creacion DESC LIMIT 1)," +
                "(SELECT (cve_usuario) FROM USUARIOS ORDER BY fecha_creacion DESC LIMIT 1));";
            try
            {
                int afectados = ExecuteQuery(sql, SetParametros(chofer));
                if (afectados > 0)
                    Log.EscribirLog("Datos extraescolares actualizados correctamente en personas: ", this, sql, GetValores(chofer));
                return afectados > 0;
            }
            catch (NpgsqlException exception)
            {
                ErrorLog.EscribirLog("Error al actualizar datos extraescolares en personas: " + exception.Message, this, sql, GetValores(chofer));
                return false;
            }
        }
        public bool UpdateDatosChofer(Chofer chofer)
        {
          Usuarios usua= new Usuarios{
              CveUsuario=chofer.CveUsuario,
              Usuario=chofer.NameUsuario,
              Contrasena=chofer.Contrasena
          };
          var responsePer= new Persona().UpdateDatosPersona(chofer);
          var resUsua = new Usuarios().UpdateDatosUsuario(usua);
            if(responsePer && resUsua)
                return true;
            return false;

		}
        public bool DeleteChofer (Chofer chofer)
        {
            const string sql = "DELETE FROM public.chofer WHERE cve_persona= :cvePersona and cve_usuario= :cveUsuario; " +
				"DELETE FROM public.personas WHERE cve_persona= :cvePersona; " +
				"DELETE FROM public.usuarios WHERE cve_usuario= :cveUsuario; ";
			try
			{
				int afectados = ExecuteQuery(sql, SetParametros(chofer));
				if (afectados > 0)
					Log.EscribirLog("Datos  actualizados correctamente en : ", this, sql, GetValores(chofer));
				return afectados > 0;
			}
			catch (NpgsqlException exception)
			{
				ErrorLog.EscribirLog("Error al actualizar datos  en : " + exception.Message, this, sql, GetValores(chofer));
				return false;
			}
		}
        public List<Chofer> GetAllChoferes()
        {
            const string sql = "select ch.cve_chofer,pe.cve_persona,usu.cve_usuario,pe.nombre,pe.apellido_pat,pe.apellido_mat,usu.usuario,usu.contrasena "+
				"from chofer ch inner join personas pe on ch.cve_persona=pe.cve_persona "+
				"inner join usuarios usu on ch.cve_usuario= usu.cve_usuario";
            List<Chofer> listChoferes= new ();
            DataTable tb= GetQuery(sql);
            listChoferes= ToList(tb);
            return listChoferes;
        }
        public Chofer GetChoferForClave (int cveChofer)
        {
			List<NpgsqlParameter> lstParameters = new List<NpgsqlParameter> {
				new NpgsqlParameter(":cveChofer", cveChofer)
			};
			const string sql = "select ch.cve_chofer,pe.cve_persona,usu.cve_usuario,pe.nombre,pe.apellido_pat,pe.apellido_mat,usu.usuario,usu.contrasena " +
				"from chofer ch inner join personas pe on ch.cve_persona=pe.cve_persona " +
				"inner join usuarios usu on ch.cve_usuario= usu.cve_usuario where cve_chofer= :cveChofer";
			DataTable dtChofer = GetQuery(sql, lstParameters);
			Chofer chofer = new Chofer();
			if (dtChofer.Rows.Count <= 0)
				return chofer; 
			DataRow row = dtChofer.Rows[0];
			chofer.CveChofer = row.IsNull("cve_chofer") ? 0 : (int)row["cve_chofer"];
			chofer.CvePersona = row.IsNull("cve_persona") ? 0 : (int)row["cve_persona"];
			chofer.CveUsuario = row.IsNull("cve_usuario") ? 0 : (int)row["cve_usuario"];
			chofer.Nombre = row.IsNull("nombre") ? string.Empty : (string)row["nombre"];
			chofer.ApellidoPat = row.IsNull("apellido_pat") ? string.Empty : (string)row["apellido_pat"];
			chofer.ApellidoMat = row.IsNull("apellido_mat") ? string.Empty : (string)row["apellido_mat"];
			chofer.NameUsuario = row.IsNull("usuario") ? string.Empty : (string)row["usuario"];
			chofer.Contrasena = row.IsNull("contrasena") ? string.Empty : (string)row["contrasena"];
            return chofer;
		}
        public Chofer LoginChofer (string usuario,string contrasena)
        {
            const string sql = "Select ch.cve_chofer,ch.cve_usuario,ch.cve_persona, per.nombre, per.apellido_pat, per.apellido_mat from public.chofer ch inner join usuarios usua " +
				"on ch.cve_usuario= usua.cve_usuario inner join personas per on ch.cve_persona=per.cve_persona where ch.cve_usuario=(Select cve_usuario from usuarios where usuario=:usuario and contrasena= :contrasena)";

			List<NpgsqlParameter> lstParameters = new List<NpgsqlParameter> {
				new NpgsqlParameter(":usuario",usuario),
                new NpgsqlParameter(":contrasena",contrasena)
			};
            Chofer chofer= new Chofer();
			DataTable dtChofer = GetQuery(sql, lstParameters);
			if (dtChofer.Rows.Count <= 0)
				return null;
			DataRow row = dtChofer.Rows[0];
			chofer.CveChofer = row.IsNull("cve_chofer") ? 0 : (int)row["cve_chofer"];
			chofer.CvePersona = row.IsNull("cve_persona") ? 0 : (int)row["cve_persona"];
			chofer.CveUsuario = row.IsNull("cve_usuario") ? 0 : (int)row["cve_usuario"];
			chofer.Nombre = row.IsNull("nombre") ? string.Empty : (string)row["nombre"];
			chofer.ApellidoPat = row.IsNull("apellido_pat") ? string.Empty : (string)row["apellido_pat"];
			chofer.ApellidoMat = row.IsNull("apellido_mat") ? string.Empty : (string)row["apellido_mat"];
            return chofer;
		}
		#endregion
	}
}
