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
    /// Interaction logic for EditEmployee.xaml
    /// </summary>
    public partial class EditEmployee
    {
        private Empleados empleado = null;

        public EditEmployee()
        {
            InitializeComponent();
        }

        private void RadWindow_Loaded(object sender, RoutedEventArgs e)
        {
            RcbArea.DataContext = AreasSingleton.Compartidos;
        }

        private void BtnBuscar_Click(object sender, RoutedEventArgs e)
        {
            
            int expediente = 0;

            Int32.TryParse(TxtExpediente.Text, out expediente);

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
            int pos = 0;
            Int32.TryParse(TxtExpediente.Text, out pos);

            if (pos == 0)
            {
                RadWindow.Alert("Ingrese un número de expediente válido");
            }
            else
            {
                empleado.Nombre = TxtNombre.Text;
                empleado.Apellidos = TxtApellidos.Text;
                empleado.Expediente = pos;
                empleado.IdArea = Convert.ToInt32(RcbArea.SelectedValue);
                new EmpleadosModel().UpdateEmployee(empleado);
            }
            
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
