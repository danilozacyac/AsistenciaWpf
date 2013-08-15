using System;
using System.Data.OleDb;
using System.Linq;

namespace AsistenciaWpf.DataAccess
{
    public class DbConnectionDac
    {
        public static OleDbConnection GetConexion()
        {
            String url = @"Provider=Microsoft.JET.OLEDB.4.0;Data Source=Asistencia.mdb;User Id=Admin;Password=";

            return new OleDbConnection(url);
        }

    }
}
