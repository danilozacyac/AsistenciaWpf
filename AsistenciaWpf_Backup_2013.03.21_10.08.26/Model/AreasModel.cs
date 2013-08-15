using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using AsistenciaWpf.Dto;
using AsistenciaWpf.DataAccess;
using System.Windows;

namespace AsistenciaWpf.Model
{
    public class AreasModel
    {


        public List<Areas> GetListaAreas()
        {
            List<Areas> areas = new List<Areas>();

            OleDbConnection oleConn = DbConnectionDac.GetConexion();
            OleDbCommand cmd;
            OleDbDataReader reader;

            cmd = oleConn.CreateCommand();
            cmd.Connection = oleConn;
            
            try
            {
                oleConn.Open();
                string miQry = "SELECT * FROM Areas";
                cmd = new OleDbCommand(miQry, oleConn);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Areas area = new Areas();
                    area.IdArea = Convert.ToInt32(reader["Id_Area"]);
                    area.Descripcion = reader["Descripcion"].ToString();
                    area.DescMay = reader["DescMay"].ToString();
                    area.Abreviatura = reader["Abreviatura"].ToString();

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
