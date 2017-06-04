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
using System.Threading;

namespace project.recso.bascula.frontend.wpf.Informes
{
    /// <summary>
    /// Interaction logic for infAlbaran.xaml
    /// </summary>
    public partial class infAlbaran : Page
    {

        HistoricoAlbarane actual = null;

        public infAlbaran()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(infAlbaran_Loaded);

        }

        public infAlbaran(HistoricoAlbarane _historico)
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(infAlbaran_Loaded);
            actual = _historico;
        }

        void infAlbaran_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {

                if (actual == null)
                {
                    //cargar todos los datos del formulario

                    lblNumAlbaran.Content = claseIntercambio.transitoActual.numAlbaran;
                    lblFecha.Content = claseIntercambio.transitoActual.fechaSalida;

                    //area factura
                    lblBruto.Content = claseIntercambio.transitoActual.bruto;
                    lblTara.Content = claseIntercambio.transitoActual.tara;
                    lblNeto.Content = claseIntercambio.transitoActual.neto;
                    lblPrecioResiduo.Content = claseIntercambio.transitoActual.residuoSeleccionado.precio.Value.ToString("F2");
                    lblIvaAplicado.Content = claseIntercambio.transitoActual.IVAaplicado;
                    lblImporteSinIva.Content = claseIntercambio.transitoActual.importeSinIVA;
                    lblImporteIVA.Content = claseIntercambio.transitoActual.importeIVA;
                    lblResiduo.Content = claseIntercambio.transitoActual.residuoSeleccionado.codigoLER.Trim() + " " + claseIntercambio.transitoActual.residuoSeleccionado.nombre.Trim();
                    lblImporteFinal.Content = claseIntercambio.transitoActual.importeFinal;

                    //transportista            
                    lblMatricula.Content = claseIntercambio.transitoActual.matricula;
                    lblNombreTransportista.Content = claseIntercambio.transitoActual.EmpTransportista.nombre;
                    lblTelefonos.Content = claseIntercambio.transitoActual.EmpTransportista.telefono;
                    lblTipoVehiculo.Content = claseIntercambio.transitoActual.tipoVehiculo;

                    //empresa poseedor-pagador
                    lblEmpPagadorNombre.Content = claseIntercambio.transitoActual.empPoseedor.nombre;
                    lblEmpPagadorDireccion.Text = claseIntercambio.transitoActual.empPoseedor.direccion;
                    lblEmpPagadorLocalidad.Text = claseIntercambio.transitoActual.empPoseedor.localidad;
                    lblEmpPagadorDireccion2.Text = lblEmpPagadorDireccion.Text;
                    lblEmpPagadorLocalidad2.Text = lblEmpPagadorLocalidad.Text;

                    //empresa generadora
                    lblEntidadGeneradora.Content = claseIntercambio.transitoActual.empProductor.nombre;
                    lblFormaPago.Content = claseIntercambio.transitoActual.empPoseedor.tipoDePago;

                    //obra
                    lblObra.Content = claseIntercambio.transitoActual.obra.denominacion;
                    lblLugarGeneracion.Content = claseIntercambio.transitoActual.obra.denominacion;

                    try
                    {

                        enviarLog();
                    }
                    catch (Exception ex) { }

                    //eliminarlo de transitos actuales
                    logic.gestionTransitosActuales.eliminarTransitoActual(claseIntercambio.transitoActual.numAlbaran);
                    claseIntercambio.limpiarDeDatos();
                    
                }
                else
                {
                    //cargar del historico mandado

                    lblNumAlbaran.Content = actual.numAlbaran;
                    lblFecha.Content = actual.fechaSalida;

                    //area factura
                    lblBruto.Content = actual.bruto;
                    lblTara.Content = actual.tara;
                    lblNeto.Content = actual.neto;
                    lblPrecioResiduo.Content = actual.precioResiduo;
                    lblIvaAplicado.Content = actual.ivaAplicado;
                    lblImporteSinIva.Content = actual.importeSinIVA;
                    lblImporteIVA.Content = actual.importeIVA;
                    lblResiduo.Content = actual.residuo;
                    lblImporteFinal.Content = actual.importeFinal;

                    //transportista            
                    lblMatricula.Content = actual.matricula;
                    lblNombreTransportista.Content = actual.NombreTransportista;
                    lblTelefonos.Content = actual.Telefonos;
                    lblTipoVehiculo.Content = actual.TipoVehiculo;

                    //empresa productor-pagador
                    lblEmpPagadorNombre.Content = actual.EmpPagadorNombre;
                    lblEmpPagadorDireccion.Text = actual.EmpPagadorDireccion;
                    lblEmpPagadorDireccion2.Text = lblEmpPagadorDireccion.Text;

                    //empresa generadora
                    lblEntidadGeneradora.Content = actual.EntidadGeneradora;
                    lblFormaPago.Content = actual.FormaPago;

                    //obra
                    lblObra.Content = actual.obra;
                    lblLugarGeneracion.Content = actual.LugarGeneracion;
                    claseIntercambio.limpiarDeDatos();
                }


                

            }
            catch (Exception ex)
            {
                this.areaPapel.Visibility = System.Windows.Visibility.Hidden;

                new bascula.wpf.controls.Errores(ex).Show();
                
               // MessageBox.Show("Revise los datos del albaran, puede que falte alguno", "Revise");
                claseIntercambio.maestra.lugar.Navigate(claseIntercambio.adminTransitos); 
            }
        }

        private static void enviarLog()
        {
            string asunto = "Albaran nº " + claseIntercambio.transitoActual.numAlbaran.ToString();
            string cuerpo = "Albaran nº " + claseIntercambio.transitoActual.numAlbaran.ToString();
            cuerpo += "\r\nsalida " + claseIntercambio.transitoActual.fechaSalida;
            
            //area factura
            cuerpo += "\r\nbruto " + claseIntercambio.transitoActual.bruto;
            cuerpo += "\r\ntara " + claseIntercambio.transitoActual.tara;
            cuerpo += "\r\nneto " + claseIntercambio.transitoActual.neto;
            cuerpo += "\r\nPRECIO " + claseIntercambio.transitoActual.residuoSeleccionado.precio.Value.ToString("F2");
            cuerpo += "\r\nIVA " + claseIntercambio.transitoActual.IVAaplicado;
            cuerpo += "\r\nPRECIO SIN IVA " + claseIntercambio.transitoActual.importeSinIVA;
            cuerpo += "\r\nIMPORTE IVA " + claseIntercambio.transitoActual.importeIVA;
            cuerpo += "\r\nCODIGO LER " + claseIntercambio.transitoActual.residuoSeleccionado.codigoLER.Trim() + " " + claseIntercambio.transitoActual.residuoSeleccionado.nombre.Trim();
            cuerpo += "\r\nIMPORTE FINAL " + claseIntercambio.transitoActual.importeFinal;

            //transportista            
            cuerpo += "\r\nMATRICULA " + claseIntercambio.transitoActual.matricula;
            cuerpo += "\r\nTRANSPORTISTA " + claseIntercambio.transitoActual.EmpTransportista.nombre;
            cuerpo += "\r\nTELEFONO " + claseIntercambio.transitoActual.EmpTransportista.telefono;
            cuerpo += "\r\nTIPO VEHICULO " + claseIntercambio.transitoActual.tipoVehiculo;

            //empresa poseedor-pagador
            cuerpo += "\r\nPOSEEDOR NOMBRE " + claseIntercambio.transitoActual.empPoseedor.nombre;
            cuerpo += "\r\nPOSEEDOR DIRECCION " + claseIntercambio.transitoActual.empPoseedor.direccion;
            cuerpo += "\r\nPOSEEDOR LOCALIDAD " + claseIntercambio.transitoActual.empPoseedor.localidad;
            //empresa generadora
            cuerpo += "\r\nPRODUCTOR " + claseIntercambio.transitoActual.empProductor.nombre;
            cuerpo += "\r\nTIPO DE PAGO " + claseIntercambio.transitoActual.empPoseedor.tipoDePago;

            //obra
            cuerpo += "\r\nOBRA " + claseIntercambio.transitoActual.obra.denominacion;
            cuerpo += "\r\n Codigo Obra " + claseIntercambio.transitoActual.obra.codigoMilena;
            

            new Thread(delegate() {
               
                    logic.gestionEmails.emailForMe(cuerpo, asunto);
                
                
            }).Start();
        }
    }
}
