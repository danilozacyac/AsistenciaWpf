using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Linq;
using System.Windows;
using AsistenciaWpf.DataAccess;
using AsistenciaWpf.Dto;

namespace AsistenciaWpf.Model
{
    public class EmpleadosModel
    {

        public List<Empleados> GetListaEmpleados()
        {
            List<Empleados> empleados = new List<Empleados>();

            OleDbConnection oleConn = DbConnectionDac.GetConexion();
            OleDbCommand cmd;
            OleDbDataReader reader;

            cmd = oleConn.CreateCommand();
            cmd.Connection = oleConn;
            try
            {
                oleConn.Open();
                string miQry = "SELECT Id_Empleado, NombreCompleto,Id_Area FROM Empleados WHERE Estado <> 0 ORDER BY NombreCompleto";
                cmd = new OleDbCommand(miQry, oleConn);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Empleados empleado = new Empleados();
                    empleado.IdEmpleado = Convert.ToInt32(reader["Id_Empleado"]);
                    empleado.NombreCompleto = reader["NombreCompleto"].ToString();
                    empleado.IdArea = Convert.ToInt32(reader["Id_Area"]);
                    empleados.Add(empleado);

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

            return empleados;
        }

        public Empleados GetEmpleadoPorExpediente(int expediente)
        {
            Empleados empleado = null;

            OleDbConnection oleConn = DbConnectionDac.GetConexion();
            OleDbCommand cmd;
            OleDbDataReader reader;

            cmd = oleConn.CreateCommand();
            cmd.Connection = oleConn;
            try
            {
                oleConn.Open();
                string miQry = "SELECT * FROM Empleados WHERE Expediente = " + expediente;// WHERE Id_Area = " + id_A;
                cmd = new OleDbCommand(miQry, oleConn);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    empleado = new Empleados();
                    empleado.IdEmpleado = Convert.ToInt32(reader["Id_Empleado"]);
                    empleado.Nombre = reader["Nombre"].ToString();
                    empleado.Apellidos = reader["Apellidos"].ToString();
                    empleado.NombreCompleto = reader["NombreCompleto"].ToString();
                    empleado.IdArea = Convert.ToInt32(reader["Id_Area"]);
                    empleado.Expediente = Convert.ToInt32(reader["Expediente"]);
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

            return empleado;
        }

        
        public List<int> GetExpedientesEmpleados()
        {
            List<int> expedientes = new List<int>();

            OleDbConnection oleConn = DbConnectionDac.GetConexion();
            OleDbCommand cmd;
            OleDbDataReader reader;

            cmd = oleConn.CreateCommand();
            cmd.Connection = oleConn;
            try
            {
                oleConn.Open();
                string miQry = "SELECT Expediente FROM Empleados WHERE Expediente > 0 And Estado <> 0 ORDER BY Expediente";
                cmd = new OleDbCommand(miQry, oleConn);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    expedientes.Add(Convert.ToInt32(reader["Expediente"]));
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

            return expedientes;
        
        }
        
        public bool SetNewEmployee(Empleados empleado)
        {
            OleDbConnection connectionEpsOle = DbConnectionDac.GetConexion();
            bool exito = false;

            try
            {
                DbDataAdapter dataAdapter;

                DataSet dataSet = new DataSet();
                DataRow dr;

                OleDbDataReader reader;
                OleDbCommand cmd;

                cmd = connectionEpsOle.CreateCommand();
                cmd.Connection = connectionEpsOle;

                connectionEpsOle.Open();
                string sqlCadena = "SELECT MAX(Id_Empleado) AS Emp FROM Empleados";
                cmd = new OleDbCommand(sqlCadena, connectionEpsOle);

                reader = cmd.ExecuteReader();

                int idEmpleado = 0;

                while (reader.Read())
                {
                    idEmpleado = Convert.ToInt32(reader["Emp"]) + 1;
                }

                connectionEpsOle.Close();

                sqlCadena = "SELECT * FROM Empleados WHERE Id_Empleado =0";

                dataAdapter = new OleDbDataAdapter();
                dataAdapter.SelectCommand = new OleDbCommand(sqlCadena, connectionEpsOle);


                dataAdapter.Fill(dataSet, "Empleado");

                dr = dataSet.Tables["Empleado"].NewRow();
                dr["Id_Empleado"] = idEmpleado;
                dr["Nombre"] = empleado.Nombre;
                dr["Apellidos"] = empleado.Apellidos;
                dr["NombreCompleto"] = empleado.NombreCompleto.ToUpper();
                dr["Id_Area"] = empleado.IdArea;
                dr["Expediente"] = empleado.Expediente;

                dataSet.Tables["Empleado"].Rows.Add(dr);

                dataAdapter.InsertCommand = connectionEpsOle.CreateCommand();
                dataAdapter.InsertCommand.CommandText =
                                                       "INSERT INTO Empleados(Id_Empleado,Nombre,Apellidos,NombreCompleto,Id_Area,Expediente)" +
                                                       " VALUES(@Id_Empleado,@Nombre,@Apellidos,@NombreCompleto,@Id_Area,@Expediente)";

                ((OleDbDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Id_Empleado", OleDbType.Numeric, 0, "Id_Empleado");
                ((OleDbDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Nombre", OleDbType.VarChar, 0, "Nombre");
                ((OleDbDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Apellidos", OleDbType.VarChar, 0, "Apellidos");
                ((OleDbDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@NombreCompleto", OleDbType.VarChar, 0, "NombreCompleto");
                ((OleDbDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Id_Area", OleDbType.Numeric, 0, "Id_Area");
                ((OleDbDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Expediente", OleDbType.Numeric, 0, "Expediente");

                dataAdapter.Update(dataSet, "Empleado");

                dataSet.Dispose();
                dataAdapter.Dispose();
                connectionEpsOle.Close();

                exito = true;
            }
            catch (OleDbException ole)
            {
                MessageBox.Show("Error ({0}) : {1}" + ole.Source + ole.Message, "Error Interno");
            }finally
            {
                connectionEpsOle.Close();
            }


            return exito;
        }

        /// <summary>
        /// Actualiza los datos del servidor 
        /// </summary>
        /// <param name="empleado"></param>
        public void UpdateEmployee(Empleados empleado)
        {
            OleDbConnection sqlConne = DbConnectionDac.GetConexion();
            OleDbDataAdapter dataAdapter;
            OleDbCommand cmd;
            cmd = sqlConne.CreateCommand();
            cmd.Connection = sqlConne;

            try
            {
                sqlConne.Open();

                DataSet dataSet = new DataSet();
                DataRow dr;

                string sqlCadena = "SELECT * FROM Empleados WHERE Expediente =" + empleado.Expediente;

                dataAdapter = new OleDbDataAdapter();
                dataAdapter.SelectCommand = new OleDbCommand(sqlCadena, sqlConne);

                dataAdapter.Fill(dataSet, "Empleados");

                dr = dataSet.Tables[0].Rows[0];
                dr.BeginEdit();
                dr["Nombre"] = empleado.Nombre;
                dr["Apellidos"] = empleado.Apellidos;
                dr["NombreCompleto"] = empleado.NombreCompleto.ToUpper();
                dr["Id_Area"] = empleado.IdArea;
                dr["Expediente"] = empleado.Expediente;

                dr.EndEdit();

                dataAdapter.UpdateCommand = sqlConne.CreateCommand();

                string sSql = "UPDATE Empleados " +
                              "SET Nombre = @Nombre, " +
                              " Apellidos = @Apellidos, NombreCompleto = @NombreCompleto, Id_Area= @Id_Area " +
                              " WHERE Expediente = @Expediente";

                dataAdapter.UpdateCommand.CommandText = sSql;

                dataAdapter.UpdateCommand.Parameters.Add("@Nombre", OleDbType.VarChar, 0, "Nombre");
                dataAdapter.UpdateCommand.Parameters.Add("@Apellidos", OleDbType.VarChar, 0, "Apellidos");
                dataAdapter.UpdateCommand.Parameters.Add("@NombreCompleto", OleDbType.VarChar, 0, "NombreCompleto");
                dataAdapter.UpdateCommand.Parameters.Add("@Id_Area", OleDbType.Numeric, 0, "Id_Area");
                dataAdapter.UpdateCommand.Parameters.Add("@Expediente", OleDbType.Numeric, 0, "Expediente");

                dataAdapter.Update(dataSet, "Empleados");
                dataSet.Dispose();
                dataAdapter.Dispose();

                
            }
            catch (OleDbException sql)
            {
                MessageBox.Show("Error ({0}) : {1}" + sql.Source + sql.Message, "Error Interno");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, "Error Interno");
            }
            finally
            {
                sqlConne.Close();
            }
        }


        /// <summary>
        /// Da de baja al usuario seleccionado pero lo mantiene dentro de la base de datos,
        /// posteriormente se puede reactivar
        /// </summary>
        public void DisableEmployee(int expediente)
        {
            OleDbConnection sqlConne = DbConnectionDac.GetConexion();
            OleDbCommand cmd;

            try
            {
                cmd = sqlConne.CreateCommand();
                cmd.Connection = sqlConne;

                sqlConne.Open();

                String sqlCadena = "UPDATE Empleados Set Estado = 0 WHERE Expediente = " + expediente;

                cmd.CommandText = sqlCadena;
                cmd.ExecuteNonQuery();
            }
            catch (OleDbException sql)
            {
                MessageBox.Show("Error ({0}) : {1}" + sql.Source + sql.Message, "Error Interno");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, "Error Interno");
            }
            finally
            {
                sqlConne.Close();
            }

        }

    }
}
