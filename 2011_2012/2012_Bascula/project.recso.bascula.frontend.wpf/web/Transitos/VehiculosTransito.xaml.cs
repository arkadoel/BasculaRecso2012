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
using project.recso.bascula.logic;
using project.recso.bascula.data;
using MahApps.Metro.Controls;
using MahApps.Metro;

namespace project.recso.bascula.frontend.wpf.web.Transitos
{
    /// <summary>
    /// Interaction logic for VehiculosTransito.xaml
    /// </summary>
    public partial class VehiculosTransito : MetroWindow
    {
        private List<TransitosActuale> transitos = new List<TransitosActuale>();

        public VehiculosTransito()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(VehiculosTransito_Loaded);
            this.lstTransitos.SelectionChanged += new SelectionChangedEventHandler(lstTransitos_SelectionChanged);
        }

        void lstTransitos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        void VehiculosTransito_Loaded(object sender, RoutedEventArgs e)
        {
            ThemeManager.ChangeTheme(this, ThemeManager.DefaultAccents.First(a => a.Name == "Green"), Theme.Dark);
            transitos = logic.gestionTransitosActuales.listarTransitos();
            ponerTransitosEnLista();
            claseIntercambio.limpiarDeDatos();
        }

        private void ponerTransitosEnLista()
        {
            lstTransitos.Items.Clear();
            String mostrar = "";
            foreach (TransitosActuale myTran in transitos)
            {
                mostrar = "Albaran  " + myTran.numAlbaran + ": "+ myTran.matricula + "   " +myTran.fechaEntrada;
                lstTransitos.Items.Add(mostrar);
            }

        }

        private void btnTerminar_Click(object sender, RoutedEventArgs e)
        {
            cargarAlbaranSeleccionado();
        }

        private void cargarAlbaranSeleccionado()
        {
            //cargar los datos en la clase de intercambio y recargar la pantalla de transitos
            int indiceLista = lstTransitos.SelectedIndex;
            if (indiceLista != -1 && transitos.Count > 0)
            {
                TransitosActuale actual = (transitos[indiceLista]);

                claseIntercambio.transitoActual.bruto = actual.bruto;
                claseIntercambio.transitoActual.tara = actual.tara;
                claseIntercambio.transitoActual.neto = actual.neto;
                claseIntercambio.transitoActual.importeFinal = actual.importeFinal;
                claseIntercambio.transitoActual.importeIVA = actual.importeIVA;
                claseIntercambio.transitoActual.importeSinIVA = actual.importeSinIva;
                claseIntercambio.transitoActual.empPlantaTransferencia = logic.gestionEmpresas.getEmpresaByRecnum(Convert.ToInt64(actual.plantaTransferencia));
                claseIntercambio.transitoActual.empPlantaValorizacion = logic.gestionEmpresas.getEmpresaByRecnum(Convert.ToInt64(actual.plantaValorizacion));
                claseIntercambio.transitoActual.empPoseedor = logic.gestionEmpresas.getEmpresaByRecnum(Convert.ToInt64(actual.poseedor));
                claseIntercambio.transitoActual.empProductor = logic.gestionEmpresas.getEmpresaByRecnum(Convert.ToInt64(actual.productor));
                claseIntercambio.transitoActual.EmpTransportista = logic.gestionEmpresas.getEmpresaByRecnum(Convert.ToInt64(actual.transportista));
                claseIntercambio.transitoActual.fechaEntrada = actual.fechaEntrada;
                claseIntercambio.transitoActual.fechaSalida = actual.fechaSalida;
                claseIntercambio.transitoActual.residuoSeleccionado = logic.gestionResiduos.getResiduoByRecnum(Convert.ToInt32(actual.residuo));
                claseIntercambio.transitoActual.matricula = actual.matricula;
                claseIntercambio.transitoActual.numAlbaran = actual.numAlbaran;
                claseIntercambio.transitoActual.tipoVehiculo = actual.tipoVehiculo;
                claseIntercambio.transitoActual.obra = logic.gestionObras.getObraByRecnum(Convert.ToInt64(actual.obra));

                claseIntercambio.maestra.lugar.Navigate(new wpf.web.Transitos.transito());
                this.Close();
            }
        }

        private void lstTransitos_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            cargarAlbaranSeleccionado();
        }

    }
}
