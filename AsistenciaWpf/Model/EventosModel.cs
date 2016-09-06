using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.OleDb;
using System.Linq;
using System.Windows;
using AsistenciaWpf.Dto;

namespace AsistenciaWpf.Model
{
    public class EventosModel : ObservableCollection<Eventos>
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["Base"].ConnectionString;

        /// <summary>
        /// Devuelve la lista de posibles justificantes a una falta que 
        /// haya sido justificada
        /// </summary>
        /// <returns></returns>
        public List<PairPropertyObject> GetListaEventos()
        {
            OleDbConnection oleConne = new OleDbConnection(connectionString);
            OleDbCommand cmd;
            OleDbDataReader reader;

            List<PairPropertyObject> eventos = new List<PairPropertyObject>();

            try
            {
                oleConne.Open();
                string miQry = "SELECT Id_Evento, Descripcion FROM Evento";
                cmd = new OleDbCommand(miQry, oleConne);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    eventos.Add(new PairPropertyObject(Convert.ToInt32(reader["Id_Evento"]), reader["Descripcion"].ToString()));
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

            return eventos;
        }

        /// <summary>
        /// Permite ingresar un evento que se repite a lo largo de un periodo de tiempo
        /// determinado en las propiedades de StartDate y EndDate recibido como parametro
        /// </summary>
        /// <param name="evento">Evento capturado</param>
        public void SetEventoPeriodico(Eventos evento)
        {
            while (evento.StartDate <= evento.EndDate)
            {
                this.SetEventoAislado(evento);

                evento.StartDate = evento.StartDate.AddDays(1);
            }
        }

        /// <summary>
        /// Ingresa un evento que se presenta solo una vez
        /// </summary>
        /// <param name="evento"></param>
        public bool SetEventoAislado(Eventos evento)
        {
            OleDbConnection oleConne = new OleDbConnection(connectionString);
            OleDbCommand cmd;

            cmd = oleConne.CreateCommand();
            cmd.Connection = oleConne;
            try
            {

                bool isEventDayExist = this.IsEventDayExist(evento);

                if (isEventDayExist)
                {
                    MessageBox.Show("El empleado seleccionado ya tiene un incidente registrado el día " + evento.StartDate.ToShortDateString() + " favor de verificar o eliminar el " +
                        " incidente anterior", "Atención:", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }

                oleConne.Open();

                cmd.CommandText = "INSERT INTO Registro_Inasis(Id_Evento, Id_Empleado, Id_Incidente, Observaciones,Fecha,Año,Mes,Dia) " +
                                  "VALUES(" + evento.IdEvento + "," + evento.IdEmpleado + "," + evento.IdIncidente + ",'" + evento.Observaciones +
                                  "','" + evento.StartDate.ToShortDateString() + "'," + evento.StartDate.Year + "," + evento.StartDate.Month + "," + evento.StartDate.Day + ")";

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, "Error Interno");
            }
            finally
            {
                oleConne.Close();
            }

            return true;
        }

        /// <summary>
        /// Elimina el registro del evento seleccionado
        /// </summary>
        /// <param name="evento"></param>
        public void DeleteEvento(Eventos evento)
        {
            OleDbConnection sqlConne = new OleDbConnection(connectionString);
            OleDbCommand cmd;

            try
            {
                cmd = sqlConne.CreateCommand();
                cmd.Connection = sqlConne;

                sqlConne.Open();

                String sqlCadena = "DELETE FROM Registro_Inasis WHERE Id_Registro = " + evento.IdEvento;

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

        public List<Personal> GetEventosConsulta(int tipoFiltro, int tipoEvento, object paramBusqueda)
        {
            OleDbConnection oleConne = new OleDbConnection(connectionString);
            OleDbCommand cmd = null;
            OleDbDataReader reader;

            List<Personal> empleadosEventos = new List<Personal>();

            try
            {
                oleConne.Open();
                string sqlCadena = "SELECT R.Id_Registro,E.Id_empleado, E.NombreCompleto, Ev.Descripcion, J.Justificacion, R.Fecha, R.Observaciones,E.Expediente " +
                                   "FROM ((Empleados E INNER JOIN Registro_Inasis R ON E.Id_empleado = R.Id_Empleado) " +
                                   "INNER JOIN Evento Ev ON R.Id_Evento = Ev.Id_Evento) " +
                                   "INNER JOIN Justificaciones_Inasistencia J ON R.Id_Incidente = J.Id_Incidente WHERE";

                switch (tipoFiltro)
                {
                    //Por año
                    case 100:
                        sqlCadena += " R.Año = @Year";
                        OleDbParameter year = new OleDbParameter("@Year", OleDbType.Numeric, 0);
                        year.Value = (Int32)paramBusqueda;
                        cmd = this.GetFiltroPorTipoEvento(oleConne, new List<OleDbParameter>() { year }, sqlCadena, tipoEvento);

                        break;
                        //Por mes
                    case 101: 
                        sqlCadena += " R.Año = @Year AND R.Mes = @Mes";
                        OleDbParameter years = new OleDbParameter("@Year", OleDbType.Numeric, 0);
                        years.Value = DateTime.Now.Year;
                        OleDbParameter mes = new OleDbParameter("@Mes", OleDbType.Numeric, 0);
                        mes.Value = (Int32)paramBusqueda;

                        cmd = this.GetFiltroPorTipoEvento(oleConne, new List<OleDbParameter>() { years, mes }, sqlCadena, tipoEvento);
                        break;
                        //Por día
                    case 102: 
                        sqlCadena += " R.Fecha = @Fecha";
                        OleDbParameter fecha = new OleDbParameter("@Fecha", OleDbType.Date, 0);
                        fecha.Value = ((DateTime)paramBusqueda).ToShortDateString();

                        cmd = this.GetFiltroPorTipoEvento(oleConne, new List<OleDbParameter>() { fecha }, sqlCadena, tipoEvento);
                        break;
                        //Por periodo
                    case 103:
                        DateTime[] fechas = (DateTime[])paramBusqueda;
                        DateTime inicio = fechas[0];//.AddDays(-1);
                        DateTime final = fechas[1];//.AddDays(1);

                        sqlCadena += " R.Fecha BETWEEN @FechaIni AND @FechaFin";
                        OleDbParameter fechaIni = new OleDbParameter("@FechaIni", OleDbType.Date, 0);
                        fechaIni.Value = inicio.ToShortDateString();
                        OleDbParameter fechaFin = new OleDbParameter("@FechaFin", OleDbType.Date, 0);
                        fechaFin.Value = final.ToShortDateString();

                        cmd = this.GetFiltroPorTipoEvento(oleConne, new List<OleDbParameter>() { fechaIni, fechaFin }, sqlCadena, tipoEvento);

                        break;
                        //Por servidor público
                    case 104: 
                        sqlCadena += " E.Id_Empleado = @Empleado";
                        OleDbParameter empleado = new OleDbParameter("@Empleado", OleDbType.Numeric, 0);
                        empleado.Value = (Int32)paramBusqueda;

                        cmd = this.GetFiltroPorTipoEvento(oleConne, new List<OleDbParameter>() { empleado }, sqlCadena, tipoEvento);
                        break;
                }

                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int index = empleadosEventos.FindIndex(r => r.Expediente == Convert.ToInt32(reader["Expediente"]));

                    if (index != -1)
                    {
                        Eventos evento = new Eventos();
                        evento.IdEvento = Convert.ToInt32(reader["Id_Registro"]);
                        evento.IdEmpleado = Convert.ToInt32(reader["Id_empleado"]);
                        evento.Descripcion = reader["Descripcion"].ToString();
                        evento.Justificante = reader["Justificacion"].ToString();
                        evento.StartDate = Convert.ToDateTime(reader["fecha"]);
                        evento.Observaciones = reader["Observaciones"].ToString();

                        empleadosEventos[index].SetEventos(evento);
                    }
                    else
                    {
                        Personal empleado = new Personal();

                        empleado.Expediente = Convert.ToInt32(reader["Expediente"]);
                        empleado.IdEmpleado = Convert.ToInt32(reader["Id_empleado"]);
                        empleado.NombreCompleto = reader["NombreCompleto"].ToString();
                        
                        Eventos evento = new Eventos();
                        evento.IdEvento = Convert.ToInt32(reader["Id_Registro"]);
                        evento.IdEmpleado = Convert.ToInt32(reader["Id_empleado"]);
                        evento.Descripcion = reader["Descripcion"].ToString();
                        evento.Justificante = reader["Justificacion"].ToString();
                        evento.StartDate = Convert.ToDateTime(reader["fecha"]);
                        evento.Observaciones = reader["Observaciones"].ToString();
                        empleado.SetEventos(evento);
                        
                        empleadosEventos.Add(empleado);
                    }
                }

                foreach (Personal empl in empleadosEventos)
                {
                    empl.NumEventos = empl.MyEventos.Count();
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

            return (from n in empleadosEventos orderby n.NombreCompleto select n).ToList();
        }

        private OleDbCommand GetFiltroPorTipoEvento(OleDbConnection oleConne, List<OleDbParameter> parameters, string sqlCadena, int tipoEvento)
        {
            if (tipoEvento != 2000 && tipoEvento != 3000)
            {
                if (tipoEvento == 1000)
                {
                    sqlCadena += " AND R.Id_Evento = 1 AND R.Id_Incidente = 0";
                }
                else if (tipoEvento == 11)
                {
                    sqlCadena += " AND R.Id_Evento = 1";
                }
                else
                {
                    sqlCadena += " AND R.Id_Evento = 1 AND R.Id_Incidente = @IdIncidente";
                    OleDbParameter idIncidente = new OleDbParameter("@IdIncidente", OleDbType.Numeric, 0);
                    idIncidente.Value = tipoEvento;

                    parameters.Add(idIncidente);
                }
            }
            else
            {
                sqlCadena += " AND R.Id_Evento = @IdEvento";
                OleDbParameter idEvento = new OleDbParameter("@IdEvento", OleDbType.Numeric, 0);
                idEvento.Value = (tipoEvento / 1000);

                parameters.Add(idEvento);
            }

            sqlCadena += " ORDER BY Fecha";
            OleDbCommand cmd = new OleDbCommand(sqlCadena, oleConne);

            foreach (OleDbParameter param in parameters)
            {
                cmd.Parameters.Add(param);
            }

            return cmd;
        }

        /// <summary>
        /// Verifica que el dia que se esta intentando ingresar un evento nuevo, no exista uno previo para el empleado seleccionado
        /// </summary>
        /// <param name="evento"></param>
        /// <returns></returns>
        public bool IsEventDayExist(Eventos evento)
        {
            OleDbConnection oleConne = new OleDbConnection(connectionString);
            OleDbCommand cmd = null;
            OleDbDataReader reader;

            bool eventExist = false;


            try
            {
                oleConne.Open();
                string sqlCadena = "SELECT * FROM Registro_Inasis WHERE Año = @Year AND Mes = @Mes AND Dia = @Dia AND Id_Empleado = @Id";

                cmd = new OleDbCommand(sqlCadena, oleConne);
                cmd.Parameters.AddWithValue("@Year", evento.StartDate.Year);
                cmd.Parameters.AddWithValue("@Mes", evento.StartDate.Month);
                cmd.Parameters.AddWithValue("@Dia", evento.StartDate.Day);
                cmd.Parameters.AddWithValue("@Id", evento.IdEmpleado);

                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    eventExist = true;
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

            return eventExist;
        }
    }
}