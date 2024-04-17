using Npgsql;

namespace TransporteSahuayoAsp.Models
{
    public class Usuarios: AccesoDatos
    {
        #region Propiedades
        public int CveUsuario { get; set; }
        public string Usuario { get; set; }
        public string Contrasena { get; set; }
        #endregion

        #region Constructores
        public Usuarios() { }
		#endregion

		#region Metodos
		private List<NpgsqlParameter> SetParametros(Usuarios usuario)
		{
			return new List<NpgsqlParameter>
			{
				new NpgsqlParameter(":cveUsuario", usuario.CveUsuario),
				new NpgsqlParameter(":usuario",usuario.Usuario),
				new NpgsqlParameter(":contrasena",usuario.Contrasena)
			};
		}
		private string GetValores(Usuarios usuario)
		{
			return string.Format("cve_usuario = {0},usuario={1}, contrasena={2}",
				usuario.CveUsuario, usuario.Usuario, usuario.Contrasena);
				
		}
		public bool UpdateDatosUsuario(Usuarios usuario)
		{
			const string sql = "update public.usuarios set usuario=:usuario, contrasena=:contrasena " +
				"where cve_usuario=:cveUsuario;";
			try
			{
				int afectados = ExecuteQuery(sql, SetParametros(usuario));
				if (afectados > 0)
					Log.EscribirLog("Datos extraescolares actualizados correctamente en personas: ", this, sql, GetValores(usuario));
				return afectados > 0;
			}
			catch (NpgsqlException exception)
			{
				ErrorLog.EscribirLog("Error al actualizar datos extraescolares en personas: " + exception.Message, this, sql, GetValores(usuario));
				return false;
			}
		}
		#endregion
	}
}
