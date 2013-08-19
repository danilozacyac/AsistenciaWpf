using System;
using System.Linq;
using System.Windows;
using AsistenciaWpf.Dto;
using AsistenciaWpf.Model;
using AsistenciaWpf.Singletons;
using Telerik.Windows.Controls;
using AsistenciaWpf.Utils;

namespace AsistenciaWpf
{
    /// <summary>
    /// Interaction logic for Prueba.xaml
    /// </summary>
    public partial class Prueba
    {
        private int idEvento = 0;

        public Prueba()
        {
            InitializeComponent();
        }

       

        private void RadWindow_Loaded(object sender, RoutedEventArgs e)
        {
            
            RcbEmpleados.DataContext = new EmpleadosModel().GetListaEmpleados();
            RcbJustificantes.DataContext = JustificacionesSingleton.Compartidos;

            RdpHasta.SelectableDateStart = RdpDesde.SelectedDate;

            if (ConstAsistencia.IdUsuario == 1)
            {
                BtnInasistencia.IsChecked = true;
                BtnRetardo.IsEnabled = false;
                BtnMediodia.IsEnabled = false;
                RadJustifica.IsChecked = true;
                RadNoJustifica.IsEnabled = false;
                this.RadButtonsTipoEvento((object)BtnInasistencia, null);
            }
        }

        private void RcbEmpleados_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            try
            {
                Empleados empleado = (Empleados)RcbEmpleados.SelectedItem;

                TxtArea.Text = ((from n in AreasSingleton.Compartidos
                                    where n.IdArea == empleado.IdArea
                                    orderby n.Descripcion
                                    select n.Descripcion).ToArray())[0];
            }
            catch (NullReferenceException) { }
        }

        private void RadButtonsTipoEvento(object sender, RoutedEventArgs e)
        {
            RadRadioButton btn = (RadRadioButton)sender;

            Visibility visible = (btn.Name.Equals("BtnInasistencia")) ? Visibility.Visible : Visibility.Collapsed;

            RadJustifica.Visibility = visible;
            RadNoJustifica.Visibility = visible;
            RcbJustificantes.Visibility = visible;
            LblObservaciones.Visibility = visible;
            TxtObservaciones.Visibility = visible;
            LblAl.Visibility = visible;
            RdpHasta.Visibility = visible;

        }

        private void BtnAceptar_Click(object sender, RoutedEventArgs e)
        {
            if (idEvento == 0)
            {
                RadWindow.Alert("Seleccione el tipo de evento presentado");
                return;
            }

            Eventos evento = new Eventos();
            evento.IdEmpleado = Convert.ToInt32(RcbEmpleados.SelectedValue);
            evento.IdEvento = idEvento;
            evento.StartDate = RdpDesde.SelectedDate.Value;
            evento.EndDate = (idEvento == 1) ? RdpHasta.SelectedDate.Value : evento.StartDate;
            evento.IdIncidente = (idEvento == 1) ? Convert.ToInt32(RcbJustificantes.SelectedValue) : 0;
            evento.Observaciones = (idEvento == 1) ? TxtObservaciones.Text : " ";

            bool isEventRegisterOk = false;

            if (evento.StartDate == evento.EndDate)
                isEventRegisterOk = new EventosModel().SetEventoAislado(evento);
            else
            {
                new EventosModel().SetEventoPeriodico(evento);
                isEventRegisterOk = true;
            }

            if(isEventRegisterOk)
                this.Close();

        }

        private void ButtonChecked(object sender, RoutedEventArgs e)
        {
            idEvento = Convert.ToInt16(((RadRadioButton)sender).Tag);
        }

        private void RdpDesde_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (RdpDesde.SelectedDate != null && RdpHasta != null)
            {
                RdpHasta.SelectedDate = RdpDesde.SelectedDate;
                RdpHasta.SelectableDateStart = RdpDesde.SelectedDate;
            }
        }
    }


}
