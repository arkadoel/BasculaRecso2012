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

namespace project.recso.bascula.frontend.wpf.web.Transitos
{
    /// <summary>
    /// Interaction logic for buscadorEmpresa.xaml
    /// </summary>
    public partial class buscadorEmpresa : UserControl
    {
        private List<Empresa> empresas;
        private TextBox cajaTexto;

        #region "Subclases de apoyo"
        public static class tipoBusqueda
        {
            public const String POSEEDOR = "POSEEDOR";
            public const String PRODUCTOR = "PRODUCTOR";
            public const String PLANTA_DE_TRANSFERENCIA = "PLANTA TRANSFERENCIA";
            public const String PLANTA_VALORIZACION = "PLANTA VALORIZACION";
            public const String TRANSPORTISTA = "TRANSPORTISTA";
        }

        private class elListaBusqueda
        {
            private int indice;

            public int Indice
            {
                get { return indice; }
                set { indice = value; }
            }
            public  long recnum { get; set; }

            public elListaBusqueda(int _indice, long _recnum)
            {
                indice = _indice;
                recnum = _recnum;
            }    
        
        }
        #endregion
        private List<elListaBusqueda> elementosLista;
        private String tipoBusquedaActual;

        public buscadorEmpresa(TextBox _texto, string _tipoBusqueda)
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(buscadorEmpresa_Loaded);
            this.lstEmpresas.SelectionChanged += new SelectionChangedEventHandler(lstEmpresas_SelectionChanged);
            this.cajaTexto = _texto;
            tipoBusquedaActual = _tipoBusqueda;
            this.btnTerminar.Click += new RoutedEventHandler(btnTerminar_Click);
            this.lstEmpresas.MouseDoubleClick += new MouseButtonEventHandler(btnTerminar_MouseDoubleClick);
            this.txtFiltro.KeyDown += new KeyEventHandler(txtFiltro_KeyDown);
        }

        void txtFiltro_KeyDown(object sender, KeyEventArgs e)
        {
            buscar();
        }

        void btnTerminar_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            terminar();
        }

        void btnTerminar_Click(object sender, RoutedEventArgs e)
        {
            terminar();
        }

        private void terminar()
        {
            int indiceLista = lstEmpresas.SelectedIndex;
            if (indiceLista != -1 && elementosLista.Count > 0)
            {
                long idEmpresaSeleccionada = ((elementosLista[indiceLista]) as elListaBusqueda).recnum;

                Empresa empresaSeleccionada = (from emp in empresas
                                               where emp.recnum == idEmpresaSeleccionada
                                               select emp).First<Empresa>();

                cajaTexto.Text = empresaSeleccionada.nombre.ToString();

                if (tipoBusquedaActual == tipoBusqueda.POSEEDOR)
                {
                    //el que paga
                    if (empresaSeleccionada.esmoroso == true)
                    {
                        Transitos.MOROSO moroso =new Transitos.MOROSO(empresaSeleccionada.nombre);
                        bascula.wpf.controls.ChildWin control = new bascula.wpf.controls.ChildWin(claseIntercambio.adminTransitos);

                        control.Height = moroso.Height;
                        control.Width = moroso.Width;
                        control.GetControles().Children.Clear();
                        control.GetControles().Children.Add(moroso);
                        claseIntercambio.adminTransitos.controles.Children.Add(control);

                    }
                    claseIntercambio.transitoActual.empPoseedor = empresaSeleccionada;
                }
                else if (tipoBusquedaActual == tipoBusqueda.PRODUCTOR)
                {
                    claseIntercambio.transitoActual.empProductor = empresaSeleccionada;
                }
                else if (tipoBusquedaActual == tipoBusqueda.PLANTA_DE_TRANSFERENCIA)
                {
                    claseIntercambio.transitoActual.empPlantaTransferencia = empresaSeleccionada;
                }
                else if (tipoBusquedaActual == tipoBusqueda.PLANTA_VALORIZACION)
                {
                    claseIntercambio.transitoActual.empPlantaValorizacion = empresaSeleccionada;
                }
                else if (tipoBusquedaActual == tipoBusqueda.TRANSPORTISTA)
                {
                    claseIntercambio.transitoActual.EmpTransportista = empresaSeleccionada;
                   
                    Vehiculo veh = new Vehiculo();
                    veh.fechaAlta = DateTime.Today.ToShortDateString();
                    veh.matriculaVehiculo = claseIntercambio.transitoActual.matricula;
                    veh.recnumEmpresa = empresaSeleccionada.recnum;
                    veh.fechaBaja = "";
                    logic.gestionVehiculos.mergeOrCreate(veh);

                    MessageBox.Show("Asociacion realizada con exito", "Exito");
                }
                this.Close();
            }
        }

        void lstEmpresas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        void buscadorEmpresa_Loaded(object sender, RoutedEventArgs e)
        {
            empresas = logic.gestionEmpresas.listarEmpresas();
            txtFiltro.Focus();
        }

        private void cerrarVentana(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        public void Close()
        {
            this.Visibility = Visibility.Hidden;
            this.UserControl_Unloaded(new object(), new RoutedEventArgs());
        }
        private void moverVentana(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void realizarBusqueda(object sender, RoutedEventArgs e)
        {
            buscar();
        }

        private void buscar()
        {
            String filtro = cmbFiltro.Text + "";
            int indice = 0;
            elementosLista = new List<elListaBusqueda>();


            if (filtro == "Nombre de empresa" || filtro == "")
            {
                var l = (from emp in empresas
                         where emp.nombre.ToLower().Contains(txtFiltro.Text.ToLower().ToString())
                         select new
                         {
                             nombre = emp.nombre,
                             recnum = emp.recnum
                         });

                lstEmpresas.Items.Clear();

                foreach (var objeto in l)
                {
                    lstEmpresas.Items.Add(objeto.nombre.ToString());
                    elementosLista.Add(new elListaBusqueda(indice, objeto.recnum));
                    indice++;
                }

            }
            else if (filtro == "CIF")
            {
                var l = (from emp in empresas
                         where emp.cif.ToLower().Contains(txtFiltro.Text.ToLower().ToString())
                         select new
                         {
                             nombre = emp.cif,
                             recnum = emp.recnum
                         });


                lstEmpresas.Items.Clear();

                foreach (var objeto in l)
                {
                    lstEmpresas.Items.Add(objeto.nombre.ToString());
                    elementosLista.Add(new elListaBusqueda(indice, objeto.recnum));
                    indice++;
                }
            }
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
