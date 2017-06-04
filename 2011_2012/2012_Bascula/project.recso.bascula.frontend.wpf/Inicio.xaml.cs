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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace project.recso.bascula.frontend.wpf
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class inicio : Page
    {
        public wpf.web.MenuLugares menu = null;
        public inicio()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (claseIntercambio.adminTransitos != null) claseIntercambio.adminTransitos.guardarAlbaran(true);

            if (btnSesionLabel.Content.ToString() == "Iniciar sesion")
            {
                new usuarios.InicioSesion().Show();
                
            }
            else
            {   //cerrar sesion
                verBtnMenu(false);
                lugar.Navigate(new wpf.web.paginaInicio());
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            claseIntercambio.maestra = this;
            verBtnMenu(false);
            VisorZoom.Visibility = Visibility.Hidden;
            
            lugar.Navigate(new wpf.web.paginaInicio());
            
        }

        public void verBtnMenu(bool ver)
        {
            if (claseIntercambio.adminTransitos != null) claseIntercambio.adminTransitos.guardarAlbaran(true);

            switch (ver)
            {
                case true:
                    btnMenu.Visibility = System.Windows.Visibility.Visible;
                    btnMenuFondo.Visibility = Visibility.Visible;
                    btnSesionLabel.Content = "Cerrar sesion";
                    break;
                case false:
                    btnMenu.Visibility = System.Windows.Visibility.Hidden;
                    btnMenuFondo.Visibility = Visibility.Hidden;
                    btnSesionLabel.Content = "Iniciar sesion";
                    

                    break;
            }
            lugar.Navigate(new web.MenuLugares());

        }

        private void label1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
           /* Point p = e.GetPosition(btnMenu);

            if (menu == null)
            {
                menu = new web.MenuLugares();
                menu.Left = p.X + 200;
                menu.Top = p.Y + 200;
                menu.Show();
            }
            else
            {
                menu.Close();
                menu = null;
            }*/
            if (claseIntercambio.adminTransitos != null) claseIntercambio.adminTransitos.guardarAlbaran(true);

            lugar.Navigate(new web.MenuLugares());
        }

        private void desarrollador_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            new Licencias.MsPL().Show();
        }

        private void ZoomIcon_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (VisorZoom.Visibility == Visibility.Hidden)
            {
                VisorZoom.Visibility = Visibility.Visible;
            }
            else VisorZoom.Visibility = Visibility.Hidden;
        }
    }
}
