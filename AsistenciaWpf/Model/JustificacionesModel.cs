using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.OleDb;
using System.Linq;
using System.Windows;
using AsistenciaWpf.Dto;

namespace AsistenciaWpf.Model
{
    public class JustificacionesModel
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["Base"].ConnectionString;

        public List<PairPropertyObject> GetListaJustificaciones()
        {
            OleDbConnection oleConne = new OleDbConnection(connectionString);
            OleDbCommand cmd;
            OleDbDataReader reader;

            List<PairPropertyObject> justificaciones = new List<PairPropertyObject>();

            try
            {
                oleConne.Open();
                string miQry = "SELECT Id_Incidente, Justificacion FROM Justificaciones_Inasistencia";
                cmd = new OleDbCommand(miQry, oleConne);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    justificaciones.Add(new PairPropertyObject
                (Convert.ToInt32(reader["Id_Incidente"].ToString()), reader["Justificacion"].ToString()));

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, "Error Interno");
            }
            finally
            {
                oleConne.Close();
            }

            return justificaciones;

        }
    }
}
