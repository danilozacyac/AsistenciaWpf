using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.OleDb;
using System.Linq;
using System.Windows;
using AsistenciaWpf.Dto;

namespace AsistenciaWpf.Model
{
    public class MesesModel
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["Base"].ConnectionString;

        public List<PairPropertyObject> GetListaMeses()
        {
            OleDbConnection oleConne = new OleDbConnection(connectionString);
            OleDbCommand cmd;
            OleDbDataReader reader;

            List<PairPropertyObject> meses = new List<PairPropertyObject>();

            try
            {
                oleConne.Open();
                cmd = new OleDbCommand("SELECT * FROM Meses", oleConne);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    meses.Add(new PairPropertyObject
                (Convert.ToInt32(reader["IdMes"].ToString()), reader["Mes"].ToString()));

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

            return meses;
        }

    }
}
