using AsistenciaWpf.Controles;
using System;
using System.Linq;
using System.Windows;
using Telerik.Windows.Controls;

namespace AsistenciaWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //List<int> exp = ExpedientesSingleton.Compartidos;
            
        }

        private void RegistraEventos_Click(object sender, RoutedEventArgs e)
        {
            Prueba preuaba = new Prueba();
            preuaba.Show();
        }

        private void AgregaServidor_Click(object sender, RoutedEventArgs e)
        {
            AddEmployee add = new AddEmployee();
            add.Show();
        }

        private void EliminarServidor_Click(object sender, RoutedEventArgs e)
        {
            DelEmployee del = new DelEmployee();
            del.Show();
        }

        Consultas con;
        private void ConsultaEventos_Click(object sender, RoutedEventArgs e)
        {
            //Consulta consulta = new Consulta();
            //consulta.Show();

            con = new Consultas();

            CentralPanel.Children.Add(con);
            TabConsultas.Visibility = Visibility.Visible;
            BarraPrincipal.SelectedItem = TabConsultas;
        }

        private void EditarServidor_Click(object sender, RoutedEventArgs e)
        {
            EditEmployee edit = new EditEmployee();
            edit.Show();
        }

        private void RBtnRealizaConsulta_Click(object sender, RoutedEventArgs e)
        {
            if (con != null)
            {
                con.RealizaConsulta(searchUid);
            }
        }

        private int searchUid = 100;
        private void TipoConsultaSearch(object sender, RoutedEventArgs e)
        {
            RadRibbonButton button = sender as RadRibbonButton;
            searchUid = Convert.ToInt32(button.Uid);
            con.ChangeSearchType(searchUid);
        }

        private void RBtnImprimeReporte_Click(object sender, RoutedEventArgs e)
        {
            con.ImprimeReporte();
        }
    }
}
