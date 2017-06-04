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
using project.recso.bascula.wpf.controls;
using System.Windows.Forms;

namespace project.recso.bascula.frontend.wpf.Informes
{
    /// <summary>
    /// Interaction logic for filtrarHistorico.xaml
    /// </summary>
    public partial class filtrarHistorico : Page
    {
        public List<HistoricoAlbarane> listado = new List<HistoricoAlbarane>();

        public filtrarHistorico()
        {
            InitializeComponent();
        }

        private void btnFiltrar_Click(object sender, RoutedEventArgs e)
        {
            List<HistoricoAlbarane> t = new List<HistoricoAlbarane>();

            if (rbFechas.IsChecked == true)
            {
                t = logic.gestionHistorialAlbaranes.getAlbaranesEntreFechas(dpInicio.Text, dpFin.Text);

            }
            if (rbPorNombre.IsChecked == true)
            {

                listado = (from u in t
                           where u.empPoseedor.Replace(" ", "").ToLower().Contains(txtNombre.Text.Replace(" ", "").ToLower())
                           select u).ToList<HistoricoAlbarane>();
            }
            else listado = t;

            if (cmbTipo.Text != "TODOS")
            {
                listado = (from y in t
                          where y.tipoResiduo.Contains(cmbTipo.Text) == true
                          select y).ToList<HistoricoAlbarane>();
            }

            ponerVisor();
        }

        private void ponerVisor()
        {
            visorElementos.Children.Clear();
            bascula.wpf.controls.ElementoAlbaran elemento=null;
            List<HistoricoAlbarane> listadoOrdenado = new List<HistoricoAlbarane>();

            //para ordenarlos se cogen todos los numeros de albaran y se ordenan
            var listaNumeros = from ord in listado
                               select new
                               {
                                    albaranNumero = ord.numAlbaran
                               };

            
            List<long> numeros = new List<long>();
            foreach (var albaran in listaNumeros) numeros.Add(long.Parse(albaran.albaranNumero.ToString()));

            numeros.Sort();

            foreach (long numeroAlbaran in numeros)
            {
                HistoricoAlbarane alba = (from albaran in listado
                                          where albaran.numAlbaran.Trim() == numeroAlbaran.ToString()
                                          select albaran).First();

                listadoOrdenado.Add(alba);
                elemento = new bascula.wpf.controls.ElementoAlbaran(alba);
                visorElementos.Children.Add(elemento);
                elemento.verAlbaran += new EventHandler(elemento_verAlbaran);
            }


            listado = listadoOrdenado;
            /*foreach (HistoricoAlbarane alba in lista)
            {
                
            }*/
        }

        void elemento_verAlbaran(object sender, EventArgs e)
        {
            HistoricoAlbarane albaran = sender as HistoricoAlbarane;
            new visorInformes(albaran.numAlbaran).Show();
        }

        private void btnExportarExcel_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dialogo = new FolderBrowserDialog();
            dialogo.ShowDialog();

            if (dialogo.SelectedPath != "")
            {
                new ExportarMilena(dialogo.SelectedPath, listado);
            }
        }
    }
}
