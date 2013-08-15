using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using AsistenciaWpf.Dto;
using AsistenciaWpf.Model;
using AsistenciaWpf.Singletons;

namespace AsistenciaWpf
{
    /// <summary>
    /// Interaction logic for AddEmployee.xaml
    /// </summary>
    public partial class AddEmployee
    {
        Empleados empleado = null;
        private int errors = 0;

        public AddEmployee()
        {
            InitializeComponent();
        }

        public AddEmployee(Empleados empleado)
        {
            this.empleado = empleado;
        }

        private void RadWindow_Loaded(object sender, RoutedEventArgs e)
        {
            RcbArea.DataContext = AreasSingleton.Compartidos;
            datosEmpleado.DataContext = new Empleados();
        }

        private void BtnAceptar_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = errors == 0;
            e.Handled = true;
        }

        private void BtnAceptar_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
            //e.Handled = true;
        }

        private void BtnAceptar_Click(object sender, RoutedEventArgs e)
        {
                empleado = new Empleados();
                empleado.Expediente = Convert.ToInt32(TxtExpediente.Text);
                empleado.Nombre = TxtNombre.Text;
                empleado.Apellidos = TxtApellidos.Text;
                empleado.NombreCompleto = TxtNombre.Text.ToUpper() + " " + TxtApellidos.Text.ToUpper();
                empleado.IdArea =   Convert.ToInt32(RcbArea.SelectedValue);

                new EmpleadosModel().SetNewEmployee(empleado);
            
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Validation_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
                errors++;
            else
                errors--;
        }

        private void TxtExpediente_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private static bool IsTextAllowed(string text)
        {
            // Regex NumEx = new Regex(@"^\d+(?:.\d{0,2})?$"); 
            Regex regex = new Regex("[^0-9.-]+"); //regex that matches disallowed text 
            return !regex.IsMatch(text);
        }

        
        
    }
}
