using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using AsistenciaWpf.Dto;
using AsistenciaWpf.Model;
using AsistenciaWpf.Reporting;
using AsistenciaWpf.Singletons;
using Telerik.Windows.Controls;

namespace AsistenciaWpf.Controles
{
    /// <summary>
    /// Interaction logic for Consultas.xaml
    /// </summary>
    public partial class Consultas : UserControl
    {
        private int idEvento = 0;
        private List<Personal> empleados = null;

        public Consultas()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            RcbYear.DataContext = this.GetYears();
            RcbYear.SelectedValue = DateTime.Now.Year;
            RcbMes.DataContext = MesesSingleton.Compartidos;
            RcbEmpleado.DataContext = new EmpleadosModel().GetListaEmpleados();
            RcbJustificantes.DataContext = JustificacionesSingleton.Compartidos;
            //RcbTipoConsulta.SelectedIndex = 0;
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

        //private void RcbTipoConsulta_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    RcbYear.Visibility = (RcbTipoConsulta.SelectedIndex == 0) ? Visibility.Visible : Visibility.Collapsed;
        //    RcbMes.Visibility = (RcbTipoConsulta.SelectedIndex == 1) ? Visibility.Visible : Visibility.Collapsed;
        //    RdpDia.Visibility = (RcbTipoConsulta.SelectedIndex == 2) ? Visibility.Visible : Visibility.Collapsed;
        //    StackPeriodo.Visibility = (RcbTipoConsulta.SelectedIndex == 3) ? Visibility.Visible : Visibility.Collapsed;
        //    RcbEmpleado.Visibility = (RcbTipoConsulta.SelectedIndex == 4) ? Visibility.Visible : Visibility.Collapsed;
        //}

        public void ChangeSearchType(int uid)
        {
            GpoYear.Visibility = (uid == 100) ? Visibility.Visible : Visibility.Collapsed;
            GpoMes.Visibility = (uid == 101) ? Visibility.Visible : Visibility.Collapsed;
            GpoDia.Visibility = (uid == 102) ? Visibility.Visible : Visibility.Collapsed;
            GpoPeriodo.Visibility = (uid == 103) ? Visibility.Visible : Visibility.Collapsed;
            GpoFuncionario.Visibility = (uid == 104) ? Visibility.Visible : Visibility.Collapsed;
        }


        private void RadButtonsTipoEvento(object sender, RoutedEventArgs e)
        {
            RadRadioButton btn = (RadRadioButton)sender;

            idEvento = Convert.ToInt32(btn.Tag);

            Visibility visible = (idEvento == 1000) ? Visibility.Visible : Visibility.Collapsed;
            RadJustifica.Visibility = visible;
            RadNoJustifica.Visibility = visible;
            RcbJustificantes.Visibility = visible;
            LblEvento.Visibility = visible;
            LblMotivo.Visibility = visible;

            if (idEvento != 1000)
                RadJustifica.IsChecked = false;

        }

        private void RadJustifica_Checked(object sender, RoutedEventArgs e)
        {
            RcbJustificantes.Visibility = Visibility.Visible;
            RcbJustificantes.SelectedIndex = -1;
        }

        private void RadNoJustifica_Checked(object sender, RoutedEventArgs e)
        {
            RcbJustificantes.Visibility = Visibility.Collapsed;
        }


        public void RealizaConsulta(int uid)
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

            EventosModel modelEventos = new EventosModel();
            switch (uid)
            {
                //Por año
                case 100:
                    empleados = modelEventos.GetEventosConsulta(uid, idEvento, Convert.ToInt32(RcbYear.Text));
                    break;
                //Por mes
                case 101:
                    empleados = modelEventos.GetEventosConsulta(uid, idEvento, Convert.ToInt32(RcbMes.SelectedValue));
                    break;
                //Por día
                case 102:
                    empleados = modelEventos.GetEventosConsulta(uid, idEvento, RdpDia.SelectedDate);
                    break;
                //Por periodo
                case 103:
                    empleados = modelEventos.GetEventosConsulta(uid, idEvento, new DateTime[] { RdpDesde.SelectedDate.Value, RdpHasta.SelectedDate.Value });
                    break;
                //Por servidor público
                case 104:
                    empleados = modelEventos.GetEventosConsulta(uid, idEvento, Convert.ToInt32(RcbEmpleado.SelectedValue));
                    break;
            }

            RgvEventos.ItemsSource = empleados;
        }

        public void ImprimeReporte()
        {
            ReporteIncidentes reporte = new ReporteIncidentes(empleados);
            reporte.GetReporteDeIncidentes();
        }
    }
}
