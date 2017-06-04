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
    /// Interaction logic for ListaResiduos.xaml
    /// </summary>
    public partial class ListaResiduos : UserControl
    {
        

        private Residuo residuo;

        public event EventHandler elementoSeleccionado;
    
        public ListaResiduos(Residuo _res)
        {
            InitializeComponent();
            residuo = _res;
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            lblnombre.Content = residuo.codigoLER + " " + residuo.nombre;
            String precio = Convert.ToDecimal( residuo.precio).ToString("C");
            String iva = Convert.ToDecimal(residuo.ivaAplicado/100).ToString("P");

            String strfinal = "Precio: " + precio + " con IVA al " + iva;

            lblTarifas.Content = strfinal;

            lblTipoMaterial.Content = "Material de: " + residuo.tipoMaterial;
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            elementoSeleccionado(residuo, new EventArgs());
        }
    }
}
