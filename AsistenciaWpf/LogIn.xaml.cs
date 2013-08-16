using System;
using System.Linq;
using System.Windows;
using AsistenciaWpf.Utils;

namespace AsistenciaWpf
{
    /// <summary>
    /// Lógica de interacción para LogIn.xaml
    /// </summary>
    public partial class LogIn : Window
    {
        public LogIn()
        {
            InitializeComponent();
        }

        private void RbtnAceptar_Click(object sender, RoutedEventArgs e)
        {
            if (TxtUsuario.Text.ToUpper().Equals("SQUIROZ") && TxtPass.Password.ToUpper().Equals("LICENCIAS"))
            {
                ConstAsistencia.IdUsuario = 1;
                this.Hide();
                MainWindow main = new MainWindow();
                main.Show();
                this.Close();
            }
            else if (TxtUsuario.Text.ToUpper().Equals("ADIAZ") && TxtPass.Password.ToUpper().Equals("DIAZ"))
            {
                ConstAsistencia.IdUsuario = 0;
                this.Hide();
                MainWindow main = new MainWindow();
                main.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Usuario y/o contraseña incorrectos, favor de verificar");
            }
        }
    }
}
