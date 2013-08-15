using System;
using System.Linq;
using System.Windows;
using AsistenciaWpf.Dto;
using AsistenciaWpf.Model;
using AsistenciaWpf.Singletons;
using Telerik.Windows.Controls;

namespace AsistenciaWpf
{
    /// <summary>
    /// Interaction logic for DelEmployee.xaml
    /// </summary>
    public partial class DelEmployee
    {
        private Empleados empleado = null;

        public DelEmployee()
        {
            InitializeComponent();
        }

        private void RadWindow_Loaded(object sender, RoutedEventArgs e)
        {
            RcbArea.DataContext = AreasSingleton.Compartidos;
        }

        private void RadButton_Click(object sender, RoutedEventArgs e)
        {
            
            int expediente = 0;

                Int32.TryParse(TxtExpediente.Text,out expediente);

                if (expediente != 0)
                {
                    empleado = new EmpleadosModel().GetEmpleadoPorExpediente(expediente);

                    if (empleado != null)
                    {
                        TxtNombre.Text = empleado.Nombre;
                        TxtApellidos.Text = empleado.Apellidos;
                        RcbArea.SelectedValue = empleado.IdArea;
                    }
                    else
                    {
                        RadWindow.Alert("El número de expediente que ingreso no existe. Favor de verificar");
                    }
                }
                else
                {
                    RadWindow.Alert("Ingrese el número de expediente del servidor público que desea eliminar");
                }
        }

        private void BtnAceptar_Click(object sender, RoutedEventArgs e)
        {
            RadWindow.Confirm(new DialogParameters() { Theme = new Windows7Theme(), Content = "Estas seguro de querer dar de baja este usuario?", Closed = ConfirmOnClosed });
        }

        private void ConfirmOnClosed(object sender, WindowClosedEventArgs e)
        {
            bool? confirm = e.DialogResult;

            if (confirm == true)
                new EmpleadosModel().DisableEmployee(empleado.Expediente);


            this.Close();
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        } 
    }
}
