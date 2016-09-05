using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using AsistenciaWpf.Dto;
using AsistenciaWpf.Model;
using AsistenciaWpf.Reporting;
using AsistenciaWpf.Singletons;
using Telerik.Windows.Controls;

namespace AsistenciaWpf
{
    /// <summary>
    /// Interaction logic for Consulta.xaml
    /// </summary>
    public partial class Consulta
    {
        private int idEvento = 0;
        private readonly EventosModel modelEventos = null;
        private List<Personal> empleados = null;

        public Consulta()
        {
            InitializeComponent();
            this.modelEventos = new EventosModel();
            this.empleados = new List<Personal>();
        }

        private void RadWindow_Loaded(object sender, RoutedEventArgs e)
        {
            RcbYear.DataContext = this.GetYears();
            RcbYear.SelectedValue = DateTime.Now.Year;
            RcbMes.DataContext = MesesSingleton.Compartidos;
            RcbEmpleado.DataContext = new EmpleadosModel().GetListaEmpleados();
            RcbJustificantes.DataContext = JustificacionesSingleton.Compartidos;
            RcbTipoConsulta.SelectedIndex = 0;

            this.Width = System.Windows.SystemParameters.PrimaryScreenWidth - (System.Windows.SystemParameters.PrimaryScreenWidth * .4);
        }

        private void RcbTipoConsulta_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            RcbYear.Visibility = (RcbTipoConsulta.SelectedIndex == 0) ? Visibility.Visible : Visibility.Collapsed;
            RcbMes.Visibility = (RcbTipoConsulta.SelectedIndex == 1) ? Visibility.Visible : Visibility.Collapsed;
            RdpDia.Visibility = (RcbTipoConsulta.SelectedIndex == 2) ? Visibility.Visible : Visibility.Collapsed;
            StackPeriodo.Visibility = (RcbTipoConsulta.SelectedIndex == 3) ? Visibility.Visible : Visibility.Collapsed;
            RcbEmpleado.Visibility = (RcbTipoConsulta.SelectedIndex == 4) ? Visibility.Visible : Visibility.Collapsed;
        }
        
        private List<int> GetYears()
        {
            List<int> years = new List<int>();

            for (int year = 2010; year <= DateTime.Now.Year + 1; year++)
            {
                years.Add(year);
            }

            return years;
        }

        private void ButtonChecked(object sender, RoutedEventArgs e)
        {
            idEvento = Convert.ToInt16(((RadRadioButton)sender).Tag);
        }

        private void RadButtonsTipoEvento(object sender, RoutedEventArgs e)
        {
            RadRadioButton btn = (RadRadioButton)sender;

            Visibility visible = (btn.Name.Equals("BtnInasistencia")) ? Visibility.Visible : Visibility.Hidden;

            RadJustifica.Visibility = visible;
            RadNoJustifica.Visibility = visible;

            if (btn != BtnInasistencia)
            {
                RcbJustificantes.Visibility = Visibility.Collapsed;
                RadJustifica.IsChecked = false;
            }

            idEvento = Convert.ToInt32(btn.Tag);
        }

        private void RadJustifica_Checked(object sender, RoutedEventArgs e)
        {
            RcbJustificantes.Visibility = Visibility.Visible;
            idEvento = 1;
        }

        private void RadJustifica_Unchecked(object sender, RoutedEventArgs e)
        {
            RcbJustificantes.Visibility = Visibility.Hidden;
        }

        private void BtnConsulta_Click(object sender, RoutedEventArgs e)
        {
            if (BtnInasistencia.IsChecked == true && RadJustifica.IsChecked == true)
            {
                if (RcbJustificantes.SelectedIndex != -1)
                    idEvento = (Int32)RcbJustificantes.SelectedValue;
                else
                {
                    MessageBox.Show("Seleccione un tipo de incidente", "Atención:", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }

            switch (RcbTipoConsulta.SelectedIndex)
            {
                //Por año
                case 0:
                    empleados = modelEventos.GetEventosConsulta(RcbTipoConsulta.SelectedIndex, idEvento, Convert.ToInt32(RcbYear.Text));
                    break;
                    //Por mes
                case 1:
                    empleados = modelEventos.GetEventosConsulta(RcbTipoConsulta.SelectedIndex, idEvento, Convert.ToInt32(RcbMes.SelectedValue));
                    break;
                    //Por día
                case 2:
                    empleados = modelEventos.GetEventosConsulta(RcbTipoConsulta.SelectedIndex, idEvento, RdpDia.SelectedDate);
                    break;
                    //Por periodo
                case 3:
                    empleados = modelEventos.GetEventosConsulta(RcbTipoConsulta.SelectedIndex, idEvento, new DateTime[] { RdpDesde.SelectedDate.Value, RdpHasta.SelectedDate.Value });
                    break;
                    //Por servidor público
                case 4:
                    empleados = modelEventos.GetEventosConsulta(RcbTipoConsulta.SelectedIndex, idEvento, Convert.ToInt32(RcbEmpleado.SelectedValue));
                    break;
            }

            RgvEventos.ItemsSource = empleados;
        }

        private void RadNoJustifica_Checked(object sender, RoutedEventArgs e)
        {
            idEvento = 1000;
        }

        private void RadNoJustifica_Unchecked(object sender, RoutedEventArgs e)
        {
            idEvento = 1;
        }

        private void BtnReporte_Click(object sender, RoutedEventArgs e)
        {
            ReporteIncidentes reporte = new ReporteIncidentes(empleados);
            reporte.GetReporteDeIncidentes();
        }

        private void BtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            foreach (Personal empleado in empleados)
            {
                List<Eventos> eventosDel = new List<Eventos>();
                foreach (Eventos evento in empleado.MyEventos)
                {
                    if (evento.IsSelected)
                        eventosDel.Add(evento);
                }

                EventosModel model = new EventosModel();

                int count = 0;
                while (count < eventosDel.Count)
                {
                    empleado.DeleteSelectedEvent(eventosDel[count]);
                    model.DeleteEvento(eventosDel[count]);
                    count++;
                }

                model = null;
            }
        }
    }
}