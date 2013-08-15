using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.OleDb;
using System.Linq;
using System.Windows;
using AsistenciaWpf.DataAccess;
using AsistenciaWpf.Dto;

namespace AsistenciaWpf.Model
{
    public class EventosModel : ObservableCollection<Eventos>
    {
        public EventosModel()
        {
            

        }

        /// <summary>
        /// Devuelve la lista de posibles justificantes a una falta que 
        /// haya sido justificada
        /// </summary>
        /// <returns></returns>
        public List<PairPropertyObject> GetListaEventos()
        {
            OleDbConnection oleConne = DbConnectionDac.GetConexion();
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
                    eventos.Add(new PairPropertyObject
                (Convert.ToInt32(reader["Id_Evento"]), reader["Descripcion"].ToString()));

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
        public void SetEventoAislado(Eventos evento)
        {
            OleDbConnection oleConne = DbConnectionDac.GetConexion();
            OleDbCommand cmd;

            cmd = oleConne.CreateCommand();
            cmd.Connection = oleConne;
            try
            {
                oleConne.Open();

                DateTime fecha = evento.StartDate;

                cmd.CommandText = "INSERT INTO Registro_Inasis(Id_Evento, Id_Empleado, Id_Incidente, Observaciones,Fecha,Año,Mes,Dia) " +
                                  "VALUES(" + evento.IdEvento + "," + evento.IdEmpleado + "," + evento.IdIncidente + ",'" + evento.Observaciones +
                                  "','" + fecha.ToShortDateString() + "'," + fecha.Year + "," + fecha.Month + "," + fecha.Day + ")";

                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
            }
            finally
            {
                oleConne.Close();
            }
        }



        public List<Empleados> GetEventosConsulta(int tipoFiltro,int tipoEvento,object paramBusqueda)
        {
            OleDbConnection oleConne = DbConnectionDac.GetConexion();
            OleDbCommand cmd = null;
            OleDbDataReader reader;

            List<Empleados> empleadosEventos = new List<Empleados>();

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
                    case 0: sqlCadena += " R.Año = @Year";
                        OleDbParameter year = new OleDbParameter("@Year", OleDbType.Numeric, 0);
                        year.Value = (Int32)paramBusqueda;
                        cmd = this.GetFiltroPorTipoEvento(oleConne, new List<OleDbParameter>() { year }, sqlCadena, tipoEvento);

                        break;
                        //Por mes
                    case 1: 
                        sqlCadena += " R.Año = @Year AND R.Mes = @Mes";
                        OleDbParameter years = new OleDbParameter("@Year", OleDbType.Numeric, 0);
                        years.Value = DateTime.Now.Year;
                        OleDbParameter mes = new OleDbParameter("@Mes", OleDbType.Numeric, 0);
                        mes.Value = (Int32)paramBusqueda;

                        cmd = this.GetFiltroPorTipoEvento(oleConne, new List<OleDbParameter>() { years,mes }, sqlCadena, tipoEvento);
                        break;
                        //Por día
                    case 2: 
                        sqlCadena += " R.Fecha = @Fecha";
                        OleDbParameter fecha = new OleDbParameter("@Fecha", OleDbType.Date, 0);
                        fecha.Value = ((DateTime)paramBusqueda).ToShortDateString();

                        cmd = this.GetFiltroPorTipoEvento(oleConne, new List<OleDbParameter>() { fecha }, sqlCadena, tipoEvento);
                        break;
                        //Por periodo
                    case 3:
                        DateTime[] fechas = (DateTime[])paramBusqueda;
                        DateTime inicio = fechas[0].AddDays(-1);
                        DateTime final = fechas[1].AddDays(1);

                        sqlCadena += " R.Fecha BETWEEN @FechaIni AND @FechaFin";
                        OleDbParameter fechaIni = new OleDbParameter("@FechaIni", OleDbType.Date, 0);
                        fechaIni.Value = inicio.ToShortDateString();
                        OleDbParameter fechaFin = new OleDbParameter("@FechaFin", OleDbType.Date, 0);
                        fechaFin.Value = final.ToShortDateString();

                        cmd = this.GetFiltroPorTipoEvento(oleConne, new List<OleDbParameter>() { fechaIni, fechaFin }, sqlCadena, tipoEvento);

                        break;
                        //Por servidor público
                    case 4: 
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
                        Empleados empleado = new Empleados();

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

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, "Error Interno");
            }
            finally
            {
                oleConne.Close();
            }

            return empleadosEventos;
        }

        private OleDbCommand GetFiltroPorTipoEvento(OleDbConnection oleConne,List<OleDbParameter> parameters,string sqlCadena, int tipoEvento)
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




    }
}
