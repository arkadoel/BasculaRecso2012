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
    /// Interaction logic for elementoTipoDeVehiculo.xaml
    /// </summary>
    public partial class elementoTipoDeVehiculo : UserControl
    {
        private TipoVehiculo tipoActual;
        public event EventHandler elementoSeleccionado;

        public elementoTipoDeVehiculo(TipoVehiculo _tipoVeh)
        {
            InitializeComponent();
            tipoActual = _tipoVeh;

            Loaded += new RoutedEventHandler(elementoTipoDeVehiculo_Loaded);
            this.MouseLeftButtonDown += new MouseButtonEventHandler(elementoTipoDeVehiculo_MouseLeftButtonDown);
        }

        void elementoTipoDeVehiculo_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            elementoSeleccionado(tipoActual, new EventArgs());
        }

        void elementoTipoDeVehiculo_Loaded(object sender, RoutedEventArgs e)
        {
            lblNombre.Content = tipoActual.nombre.ToString();
            lblCapacidad.Content = tipoActual.capacidad.ToString();
        }

    }
}
