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

namespace project.recso.bascula.frontend.wpf.web.Vehiculos
{
    /// <summary>
    /// Interaction logic for vehiculosEmpresa.xaml
    /// </summary>
    public partial class vehiculosEmpresa : Window
    {
        private long recnumEmpresa;
        private List<Vehiculo> listadoVehiculos;

        public vehiculosEmpresa(long _recnum)
        {
            InitializeComponent();
            recnumEmpresa = _recnum;
            this.Loaded += new RoutedEventHandler(vehiculosEmpresa_Loaded);
            listaMatriculas.SelectionChanged += new SelectionChangedEventHandler(listaMatriculas_SelectionChanged);
        }

        void listaMatriculas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int indice = listaMatriculas.SelectedIndex;
            if (indice != -1)
            {
                String matricula = listaMatriculas.SelectedItem.ToString();
                Vehiculo veh = (from v in listadoVehiculos
                                where v.matriculaVehiculo == matricula
                                select v).First<Vehiculo>();
                txtMatricula.Text = veh.matriculaVehiculo;
                dpAlta.Text = veh.fechaAlta.ToString();
                dpBaja.Text = veh.fechaBaja.ToString();
                txtRecnum.Text = veh.recnum.ToString();
            }
        }

        private void limpiar()
        {
            txtMatricula.Text = "";
            dpAlta.Text = "";
            dpBaja.Text = "";
            txtRecnum.Text = "";
        }
        void vehiculosEmpresa_Loaded(object sender, RoutedEventArgs e)
        {
            dpBaja.Text = "";
            dpAlta.Text = "";
            actualizarLista();
        }

        private void actualizarLista()
        {
            listadoVehiculos = logic.gestionVehiculos.listarVehiculosEmpresa(recnumEmpresa);

            listaMatriculas.Items.Clear();
            foreach (Vehiculo veh in listadoVehiculos)
            {
                listaMatriculas.Items.Add(veh.matriculaVehiculo.ToString());
            }
            limpiar();
        }

        private void btnCerrar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void guardar(object sender, RoutedEventArgs e)
        {
            Vehiculo veh = new Vehiculo();
            veh.recnumEmpresa = recnumEmpresa;
            veh.fechaAlta = dpAlta.Text;
            veh.fechaBaja = dpBaja.Text;
            veh.matriculaVehiculo = txtMatricula.Text;

            if (txtRecnum.Text != "")
            {
                veh.recnum = long.Parse(Convert.ToInt64(txtRecnum.Text).ToString());
            }

            logic.gestionVehiculos.mergeOrCreate(veh);
            actualizarLista();
        }

        private void eliminar(object sender, RoutedEventArgs e)
        {
            if (txtRecnum.Text != "")
            {
                long rec = long.Parse(Convert.ToInt64(txtRecnum.Text).ToString());
                logic.gestionVehiculos.eliminarVehiculo(rec);

            }
            actualizarLista();
        }

        private void nuevo(object sender, RoutedEventArgs e)
        {
            actualizarLista();
        }

    }
}
