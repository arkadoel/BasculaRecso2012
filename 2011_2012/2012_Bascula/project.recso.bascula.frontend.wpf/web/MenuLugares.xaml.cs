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

namespace project.recso.bascula.frontend.wpf.web
{
    /// <summary>
    /// Interaction logic for MenuLugares.xaml
    /// </summary>
    public partial class MenuLugares : Page
    {
        public MenuLugares()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(MenuLugares_Loaded);
        }

        void MenuLugares_Loaded(object sender, RoutedEventArgs e)
        {
            restringirVision();
        }

        /// <summary>
        /// Segun los permisos que posea el usuario, podra ver unos menus u otros
        /// </summary>
        private void restringirVision()
        {
            //Por defecto todos los menus visibles
            String permiso = claseIntercambio.usuarioActual.permiso.ToLower();
            switch (permiso)
            {
                case "admin":       //ver todos los menus
                    break;
                case "gerencia":    //ver todos los menus
                    break;
                case "operador":    //restriccion de menus
                   // quitarBoton(btnClientes);
                    quitarBoton(btnAdmin);
                    break;
            }
        }

        /// <summary>
        /// Quita el boton que se le pasa como referencia
        /// </summary>
        /// <param name="_boton"></param>
        private void quitarBoton(UIElement _boton)
        {
            zonaBotones.Children.Remove(_boton);
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void IrResiduos(object sender, MouseButtonEventArgs e)
        {
            claseIntercambio.maestra.lugar.Navigate(new wpf.web.residuos.adminResiduos());
            claseIntercambio.maestra.menu = null;
            //this.Close();

        }

        private void IrClientes(object sender, MouseButtonEventArgs e)
        {
            claseIntercambio.maestra.lugar.Navigate(new wpf.web.Clientes.adminClientes());
            claseIntercambio.maestra.menu = null;
            //this.Close();
        }

        private void IrTransitos(object sender, MouseButtonEventArgs e)
        {
            if (claseIntercambio.adminTransitos != null)
            {
                claseIntercambio.maestra.lugar.Navigate(claseIntercambio.adminTransitos); 
            }
            else
            {
                claseIntercambio.maestra.lugar.Navigate(new wpf.web.Transitos.transito());                
            }
            claseIntercambio.maestra.menu = null;
            //this.Close();
        }

        private void IrTiposVehiculos(object sender, MouseButtonEventArgs e)
        {
            claseIntercambio.maestra.lugar.Navigate(new wpf.web.Vehiculos.adminTipoDeVehiculo() );
            claseIntercambio.maestra.menu = null;
            //this.Close();
        }

        private void IrObras(object sender, MouseButtonEventArgs e)
        {
            claseIntercambio.maestra.lugar.Navigate(new Obras.adminObras()); 
        }

        private void IrInformes(object sender, MouseButtonEventArgs e)
        {
            
           Informes.Consultas win = new Informes.Consultas();

           claseIntercambio.maestra.lugar.Navigate(win);
           claseIntercambio.maestra.menu = null;
            
        }

        private void IrAdministracion(object sender, MouseButtonEventArgs e)
        {
            new Admin.Admin().Show();

        }
    }
}
