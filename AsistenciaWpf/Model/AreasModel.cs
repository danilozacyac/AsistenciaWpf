using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.OleDb;
using System.Linq;
using System.Windows;
using AsistenciaWpf.Dto;

namespace AsistenciaWpf.Model
{
    public class AreasModel
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["Base"].ConnectionString;

        public List<Areas> GetListaAreas()
        {
            List<Areas> areas = new List<Areas>();

            OleDbConnection oleConn = new OleDbConnection(connectionString);
            OleDbCommand cmd;
            OleDbDataReader reader;

            try
            {
                oleConn.Open();
                cmd = new OleDbCommand("SELECT * FROM Areas", oleConn);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Areas area = new Areas()
                    {
                        IdArea = Convert.ToInt32(reader["Id_Area"]),
                        Descripcion = reader["Descripcion"].ToString(),
                        DescMay = reader["DescMay"].ToString(),
                        Abreviatura = reader["Abreviatura"].ToString()
                    };

                    areas.Add(area);

                }

            }
            catch (OleDbException ex)
            {
                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, "Error Interno");
            }
            finally
            {
                oleConn.Close();
            }

            return areas;
        }
    }
}
