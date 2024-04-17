using Npgsql;

namespace TransporteSahuayoAsp.Models
{
    public class Persona: AccesoDatos
    {
        #region Propiedades
        public int CvePersona { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPat { get; set; }
        public string ApellidoMat { get; set;}
        #endregion

        #region Constructores
        public Persona() { 
        }
		#endregion

		#region Metodos
		private List<NpgsqlParameter> SetParametros(Persona persona)
		{
			return new List<NpgsqlParameter>
			{
				new NpgsqlParameter(":cvePersona",  persona.CvePersona == 0 ? DBNull.Value: persona.CvePersona),
				new NpgsqlParameter(":nombre",persona.Nombre == null ? DBNull.Value: persona.Nombre),
				new NpgsqlParameter(":apellidoPat",persona.ApellidoPat == null ? DBNull.Value : persona.ApellidoPat),
				new NpgsqlParameter(":apellidoMat",persona.ApellidoMat == null ? DBNull.Value : persona.ApellidoMat)
				
			};
		}
		private string GetValores(Persona persona)
		{
			return string.Format("cve_persona = {0}, nombre={1}, apellido_pat={2}" +
				"apellido_mat={3}", persona.CvePersona, persona.Nombre, persona.ApellidoPat, persona, ApellidoMat);
		}
		public bool UpdateDatosPersona(Persona persona)
		{
			const string sql = "update public.personas set nombre=:nombre, apellido_pat=:apellidoPat, apellido_mat=:apellidoMat " +
				"where cve_persona=:cvePersona;";
			try
			{
				int afectados = ExecuteQuery(sql, SetParametros(persona));
				if (afectados > 0)
					Log.EscribirLog("Datos extraescolares actualizados correctamente en personas: ", this, sql, GetValores(persona));
				return afectados > 0;
			}
			catch (NpgsqlException exception)
			{
				ErrorLog.EscribirLog("Error al actualizar datos extraescolares en personas: " + exception.Message, this, sql, GetValores(persona));
				return false;
			}
		}
		#endregion
	}
}
