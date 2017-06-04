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
using project.recso.bascula.data;

namespace project.recso.bascula.wpf.controls
{

    /// <summary>
    /// Interaction logic for elementoCliente.xaml
    /// </summary>
    public partial class elementoCliente : UserControl
    {
        private Empresa actual;
        public event EventHandler elementoSeleccionado;
        public event EventHandler verObrasCliente;
        public event EventHandler verVehiculosCliente;
        public event EventHandler verCobrosCliente;

        public elementoCliente(Empresa _e)
        {
            InitializeComponent();
            actual = _e;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            txtCodigoMilena.Content = actual.codigoMilena.ToString();
            txtNombreEmpresa.Content = actual.nombre;
            if (actual.tipoDePago.ToLower().Contains("prepago"))
            {
                btnVerCobros.Visibility = Visibility.Visible;
            }
            else btnVerCobros.Visibility = Visibility.Hidden;
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            elementoSeleccionado(actual, new EventArgs());
        }

        private void btnObras_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            verObrasCliente(actual, new EventArgs());
        }

        private void btnVehiculos_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            verVehiculosCliente(actual, new EventArgs());
        }

        private void btnVerCobros_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            verCobrosCliente(actual, new EventArgs());
        }
    }
}
