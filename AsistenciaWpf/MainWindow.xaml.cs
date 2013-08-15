using System;
using System.Linq;
using System.Windows;

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

        private void ConsultaEventos_Click(object sender, RoutedEventArgs e)
        {
            Consulta consulta = new Consulta();
            consulta.Show();
        }

        private void EditarServidor_Click(object sender, RoutedEventArgs e)
        {
            EditEmployee edit = new EditEmployee();
            edit.Show();
        }
    }
}
