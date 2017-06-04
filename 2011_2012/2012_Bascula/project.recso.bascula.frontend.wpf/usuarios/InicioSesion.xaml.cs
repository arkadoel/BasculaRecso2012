using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using project.recso.bascula.data;

namespace project.recso.bascula.frontend.wpf.usuarios
{
    /// <summary>
    /// Interaction logic for InicioSesion.xaml
    /// </summary>
    public partial class InicioSesion : Window
    {
        public InicioSesion()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(InicioSesion_Loaded);
        }

        void InicioSesion_Loaded(object sender, RoutedEventArgs e)
        {
            //txtname.Focus();
            List<Usuario> usuarios = logic.gestionUsuarios.getUsuariosParaLogin();
            foreach (Usuario user in usuarios)
            {
                ComboBoxItem elemento = new ComboBoxItem();
                elemento.Content=user.NombreReal;
                elemento.Tag = user.loginName;
                cmbLoginName.Items.Add(elemento);
            }
            cmbLoginName.SelectedIndex = 0;
            txtpwd.Focus();
                
        }

        private void ellipse1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void iniciarSesion_Click(object sender, RoutedEventArgs e)
        {
            procesarInicioSesion();
        }

        private void procesarInicioSesion()
        {
            String loginName = "";
            if (cmbLoginName.SelectedIndex == -1)
            {
                loginName = cmbLoginName.Text;
            }
            else loginName= ((ComboBoxItem)cmbLoginName.SelectedItem).Tag.ToString();

            Usuario user = bascula.logic.gestionUsuarios.logueoCorrecto(loginName, txtpwd.Password);

            if (user != null)
            {
                claseIntercambio.usuarioActual = user;
                claseIntercambio.maestra.lugar.Navigate(new bascula.frontend.wpf.web.MenuLugares());
                claseIntercambio.maestra.verBtnMenu(true);
                claseIntercambio.limpiarDeDatos();

                this.Close();
            }
            else
            {
                claseIntercambio.msg("Inicio sesion no valido, revise los datos introducidos.", "Fallo", MessageBoxButton.OK, MessageBoxImage.Error);
                //txtname.Password = "";
                txtpwd.Password = "";
                cmbLoginName.SelectedIndex = 0;
                cmbLoginName.Focus();
            }
        }

        private void txtname_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                txtpwd.Focus();
            }
        }

        private void txtpwd_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                procesarInicioSesion();
            }
        }
    }
}
