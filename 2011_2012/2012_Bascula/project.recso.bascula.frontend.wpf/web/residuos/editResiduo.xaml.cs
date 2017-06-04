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

namespace project.recso.bascula.frontend.wpf.web.residuos
{
    /// <summary>
    /// Interaction logic for editResiduo.xaml
    /// </summary>
    public partial class editResiduo : UserControl
    {
        private String recnum;

        public editResiduo()
        {
            InitializeComponent();
            recnum = "";
        }

        public editResiduo(Int32 _recnum)
        {
            InitializeComponent();
            recnum = _recnum.ToString();
        }
        public void Cerrar()
        {
            this.Visibility = Visibility.Hidden;
            this.UserControl_Unloaded(new object(), new RoutedEventArgs());
        }

        private void ellipse1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Cerrar();
        }

        private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
           
        }

        private void aceptar(object sender, RoutedEventArgs e)
        {
            //mapear formulario a residuo
            bascula.data.Residuo residuo = new bascula.data.Residuo();

                if (recnum != "")
                {
                    residuo.recnum = Convert.ToInt32( recnum);
                }

                residuo.codigoLER = this.txtCodigoLER.Text;
                residuo.descripcion = this.txtDescrip.Text;
                residuo.ivaAplicado = float.Parse( Convert.ToDouble( this.txtIva.Text).ToString());
                residuo.nombre = this.txtNombre.Text;
                residuo.precio = float.Parse( Convert.ToDouble( txtPrecio.Text).ToString() );
                residuo.tipoMaterial = cmbMaterial.Text;
                residuo.milena = txtMilena.Text;

                try
                {
                    Boolean correcto = logic.gestionResiduos.mergeOrCreate(residuo);
                    MessageBox.Show("Se realizo la operacion con exito", "Guardado", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Fallo", MessageBoxButton.OK, MessageBoxImage.Error);
                }


                Cerrar();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (recnum != "")
            {
                Residuo residuo = logic.gestionResiduos.getResiduoByRecnum(Convert.ToInt32(recnum));

                if (residuo != null)
                {
                    txtCodigoLER.Text = residuo.codigoLER;
                    txtDescrip.Text = residuo.descripcion;
                    txtIva.Text = residuo.ivaAplicado.ToString();
                    txtPrecio.Text = residuo.precio.ToString();
                    txtMilena.Text = residuo.milena;
                    
                    bool salir=false;

                    for(short i=0; i< cmbMaterial.Items.Count && !salir ; i++)
                    {
                        String textoItem = ((ComboBoxItem)cmbMaterial.Items[i]).Content.ToString();
                        
                        if (textoItem.Trim() == residuo.tipoMaterial.Trim() )
                        {
                            cmbMaterial.SelectedIndex = i;
                            salir = true;
                        }
                    }

                    txtNombre.Text = residuo.nombre;
                }
            }
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            Cerrar();
        }

        private void eliminarResiduo(object sender, RoutedEventArgs e)
        {
            if (recnum != "")
            {
                MessageBoxResult pregunta = MessageBox.Show("¿Esta seguro de que quiere eliminar este residuo?", "Seguro", MessageBoxButton.YesNoCancel, MessageBoxImage.Question, MessageBoxResult.No);

                if (pregunta == MessageBoxResult.Yes)
                {
                    Boolean conseguido = logic.gestionResiduos.eliminarResiduo(Convert.ToInt32(recnum));
                    MessageBox.Show("Se realizo la operacion con exito", "Eliminado", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            Cerrar();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            claseIntercambio.adminResiduosActual.crearListado();
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
