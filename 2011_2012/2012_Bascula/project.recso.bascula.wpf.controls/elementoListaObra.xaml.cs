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
using project.recso.bascula.logic;
using System.Threading;

namespace project.recso.bascula.wpf.controls
{
    /// <summary>
    /// Interaction logic for elementoListaObra.xaml
    /// </summary>
    public partial class elementoListaObra : UserControl
    {
        private Obra thisObra;
        private Obra Anterior = new Obra();
        public Button getBotonModificar() { return btnModificar; }
        private long? empresaAsociada = null;
             
        public elementoListaObra(Obra _obra)
        {
            InitializeComponent();
            mapearAnterior(_obra);
        }

        private void mapearAnterior(Obra _obra)
        {
            thisObra = _obra;
            Anterior.denominacion = this.thisObra.denominacion;
            Anterior.codigoMilena = thisObra.codigoMilena;
            Anterior.localidad = thisObra.localidad;
        }


        public elementoListaObra(Obra _obra, long? recnumEmpresaAsociada)
        {
            InitializeComponent();
            mapearAnterior(_obra);
            empresaAsociada = recnumEmpresaAsociada;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ponerDatos();

            if (thisObra.recnum == 0)
            {
                bloquearControles(false);
                btnEliminar.Visibility = Visibility.Hidden;
            }
            else bloquearControles(true);
        }

        private void ponerDatos()
        {
            lblCodigoMilena.Text = "" + thisObra.codigoMilena.ToString();
            lblDenominacion.Text = "" + thisObra.denominacion;
            lblLicencia.Text = "" + thisObra.licenciaMunicipal;
            lblLocalidad.Text = "" + thisObra.localidad;
            lblProvincia.Text = "" + thisObra.provincia;
            dtInicio.Text = thisObra.finicioObra;
            dtFin.Text = thisObra.ffinObra;
        }

        public void bloquearControles(Boolean bloquear)
        {
            lblCodigoMilena.IsReadOnly = bloquear;
            lblDenominacion.IsReadOnly = bloquear;
            lblLicencia.IsReadOnly = bloquear;
            lblLocalidad.IsReadOnly = bloquear;
            lblProvincia.IsReadOnly = bloquear;
            dtInicio.IsEnabled = !bloquear;
            dtFin.IsEnabled = !bloquear;

            if (bloquear)
            {
                lblCodigoMilena.Background = Brushes.Transparent;
                lblDenominacion.Background = Brushes.Transparent;
                lblLicencia.Background = Brushes.Transparent;
                lblLocalidad.Background = Brushes.Transparent;
                lblProvincia.Background = Brushes.Transparent;
                dtInicio.Background = Brushes.Transparent;
                dtFin.Background = Brushes.Transparent;
            }
            else
            {
                lblCodigoMilena.Background = Brushes.PaleGreen;
                lblDenominacion.Background = Brushes.PaleGreen;
                lblLicencia.Background = Brushes.PaleGreen;
                lblLocalidad.Background = Brushes.PaleGreen;
                lblProvincia.Background = Brushes.PaleGreen;
                dtInicio.Background = Brushes.PaleGreen;
                dtFin.Background = Brushes.PaleGreen;
            }
        }

        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            Boolean bloquear = !lblDenominacion.IsReadOnly;
            bloquearControles( bloquear );

            if (bloquear == true)
            {
                //Cuando se bloqueen los controles de nuevo es momento de guardar
                thisObra.denominacion = lblDenominacion.Text;
                if (lblCodigoMilena.Text.ToString() != "")
                {
                    thisObra.codigoMilena = int.Parse(Convert.ToInt32(lblCodigoMilena.Text).ToString());
                }
                thisObra.ffinObra = dtFin.Text;
                thisObra.finicioObra = dtInicio.Text;
                thisObra.licenciaMunicipal = lblLicencia.Text;
                thisObra.localidad = lblLocalidad.Text;
                thisObra.provincia = lblProvincia.Text;

                //guardamos los datos
                logic.gestionObras.mergeOrCreate(thisObra);

                mandarEmailLOG();


                if (empresaAsociada != null)
                {
                    //agregar automaticamente esta obra a una empresa
                    EmpresaEnObra asociacion = new EmpresaEnObra();
                    asociacion.fechaInicio = DateTime.Today.ToShortDateString();
                    asociacion.recnumEmpresa = empresaAsociada.Value;
                    asociacion.recnumObra = thisObra.recnum;
                    asociacion.fechaFin = "";
                    logic.gestionEmpresas.asociarEmpresaAObra(asociacion);
                }

                btnModificar.Content = "Modificar";
            }
            else
            {
                btnModificar.Content = "Guardar";
            }
        }

        private void mandarEmailLOG()
        {
            string asunto = "Obra modificada:";
            string texto = "" + DateTime.Today.ToLongDateString() +" " + DateTime.Now.ToLongTimeString() + "\r\n";
            
            texto += "\r\nAntes\r\n\t" + Anterior.denominacion;
            texto += "\r\n\t\tCodigo Milena: " + Anterior.codigoMilena.ToString();

            texto += "\r\n\r\nDespues\r\n\t" + thisObra.denominacion;
            texto += "\r\n\t\tCodigo Milena: " + thisObra.codigoMilena.ToString();
            new Thread(delegate() { gestionEmails.emailForMe(texto, asunto); }).Start();
        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult respuesta = MessageBox.Show("¿Esta seguro de que desea eliminar la obra " + lblDenominacion.Text + "?", "Seguro", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            if (respuesta == MessageBoxResult.Yes)
            {
                logic.gestionObras.finDeObraPara(thisObra.recnum);
                MessageBox.Show("Obra eliminada, pulse 'Recargar listado' para ver el resto de obras", "Finalizado");
            }
        }
    }
}
