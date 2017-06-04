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

namespace project.recso.bascula.frontend.wpf.web.Vehiculos
{
    /// <summary>
    /// Interaction logic for adminTipoDeVehiculo.xaml
    /// </summary>
    public partial class adminTipoDeVehiculo : Page
    {
        private String recnum="";

        public adminTipoDeVehiculo()
        {
            InitializeComponent();
            recnum = "";
            Loaded += new RoutedEventHandler(adminTipoDeVehiculo_Loaded);
            btnAgregar.Click += new RoutedEventHandler(btnAgregar_Click);
            btnCerrar.Click += new RoutedEventHandler(btnCerrar_Click);
            btnEliminar.Click += new RoutedEventHandler(btnEliminar_Click);
            btnGuardar.Click += new RoutedEventHandler(btnGuardar_Click);
        }

        void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            TipoVehiculo tip = new TipoVehiculo();
            tip.capacidad = float.Parse(Convert.ToDouble(txtCapacidad.Text).ToString());
            tip.nombre = txtNombre.Text;

            if (recnum != "") tip.recnum = Convert.ToInt32(recnum);


            try
            {
                logic.gestionTipoVehiculo.mergeOrCreate(tip);
                MessageBox.Show("Se realizo la operacion con exito", "Guardado", MessageBoxButton.OK, MessageBoxImage.Information);
                cargarListado();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Fallo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            gridEdit.Visibility = Visibility.Hidden;
            recnum = "";
        }

        void btnEliminar_Click(object sender, RoutedEventArgs e)
        {

            if (recnum != "")
            {
                MessageBoxResult pregunta = MessageBox.Show("¿Esta seguro de que quiere eliminar este tipo de vehiculo?", "Seguro", MessageBoxButton.YesNoCancel, MessageBoxImage.Question, MessageBoxResult.No);

                if (pregunta == MessageBoxResult.Yes)
                {
                    Boolean conseguido = logic.gestionTipoVehiculo.eliminarTipoDeVehiculo(Convert.ToInt32(recnum));
                    MessageBox.Show("Se realizo la operacion con exito", "Eliminado", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            gridEdit.Visibility = Visibility.Hidden;
            recnum = "";
        }

        void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            gridEdit.Visibility = Visibility.Hidden;
            recnum = "";
        }

        void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            gridEdit.Visibility = Visibility.Visible;
            recnum = "";
            txtNombre.Text = "";
            txtCapacidad.Text = "";
        }

        void adminTipoDeVehiculo_Loaded(object sender, RoutedEventArgs e)
        {
            cargarListado();
        }

        private void cargarListado()
        {
            gridEdit.Visibility = Visibility.Hidden;
            //cargar el listado
            List<TipoVehiculo> tipos = logic.gestionTipoVehiculo.getTiposDeVehiculos();
            listado.Children.Clear();

            foreach (TipoVehiculo tipo in tipos)
            {
                bascula.wpf.controls.elementoTipoDeVehiculo elemento = new bascula.wpf.controls.elementoTipoDeVehiculo(tipo);

                listado.Children.Add(elemento);
                elemento.elementoSeleccionado += new EventHandler(elemento_elementoSeleccionado);
            }
        }

        void elemento_elementoSeleccionado(object sender, EventArgs e)
        {
            TipoVehiculo tip = (TipoVehiculo)sender;

            if (tip != null)
            {
                txtNombre.Text = tip.nombre;
                txtCapacidad.Text = tip.capacidad.ToString();
                recnum = tip.recnum.ToString();
                gridEdit.Visibility = Visibility.Visible;
            }
        }
    }
}
