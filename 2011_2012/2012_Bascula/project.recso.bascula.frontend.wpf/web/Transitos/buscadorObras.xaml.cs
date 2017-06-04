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
using System.Windows.Threading;

namespace project.recso.bascula.frontend.wpf.web.Transitos
{
    /// <summary>
    /// Interaction logic for buscadorObras.xaml
    /// </summary>
    public partial class buscadorObras : UserControl
    {
        private List<gestionObras.obraReducida> listaobras = new List<gestionObras.obraReducida>();
        private long idEmpresa;
        private TextBox cajaTexto;

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
       
        private List<elListaBusqueda> elementosLista;

        public buscadorObras(TextBox _texto, long _idEmpresa)
        {
            InitializeComponent();
            idEmpresa = _idEmpresa;
            this.cajaTexto = _texto;
            this.Unloaded += new RoutedEventHandler(buscadorObras_Unloaded);
            this.lstObras.MouseDoubleClick += new MouseButtonEventHandler(btnTerminar_Click);
            btnVer.KeyDown += new KeyEventHandler(btnVer_KeyDown);
            this.Loaded += new RoutedEventHandler(buscadorObras_Loaded);
           
        }

        public static void DoEvents()
        {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Background,
                                                  new Action(delegate { }));
        }

        void buscadorObras_Loaded(object sender, RoutedEventArgs e)
        {
            listaobras = logic.gestionObras.listarObrasDeEmpresaFormatoReducido(idEmpresa);
            txtFiltro.Focus();
        }

        void btnVer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) buscar();
        }

        

        private void realizarBusqueda(object sender, RoutedEventArgs e)
        {
            buscar();

        }

        private void buscar()
        {
            lstObras.Items.Clear();
            elementosLista = new List<elListaBusqueda>();
            List<gestionObras.obraReducida> listado = new List<gestionObras.obraReducida>();

            foreach (gestionObras.obraReducida obra in listaobras)
            {
                string s1 = claseIntercambio.paraFiltros(obra.denominacion);
                string s2 = claseIntercambio.paraFiltros(txtFiltro.Text);

                if (s1.Contains(s2))
                {
                    listado.Add(obra);
                }
            }

            /*var listado = from u in listaobras
                          where   u.denominacion.ToLower().Contains(txtFiltro.Text.ToLower().ToString() ) == true
                          select u;*/
            int indice = 0;
            foreach (var ele in listado)
            {
                lstObras.Items.Add(ele.denominacion.ToString());
                elementosLista.Add(new elListaBusqueda(indice, ele.recnum));
                indice++;
            }
        }



        private void btnTerminar_Click(object sender, RoutedEventArgs e)
        {
            int indiceLista = lstObras.SelectedIndex;
            if (indiceLista != -1 && elementosLista.Count > 0)
            {
                long idObraSeleccionada = ((elementosLista[indiceLista]) as elListaBusqueda).recnum;
                gestionObras.obraReducida obraSeleccionada = (from una in listaobras
                                         where una.recnum == idObraSeleccionada
                                         select una).First<gestionObras.obraReducida>();
                cajaTexto.Text = obraSeleccionada.denominacion.ToString();

                claseIntercambio.transitoActual.obra = logic.gestionObras.getObraByRecnum( obraSeleccionada.recnum);

                this.Close();
            }
        }

        private void rectangle1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
           
        }

        void buscadorObras_Unloaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void image1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        public void Close()
        {
            this.Visibility = Visibility.Hidden;
            this.buscadorObras_Unloaded(new object(), new RoutedEventArgs());
        }

       

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            Obras.obrasEmpresa editor = new Obras.obrasEmpresa(idEmpresa);
            bascula.wpf.controls.ChildWin control = new bascula.wpf.controls.ChildWin(claseIntercambio.adminTransitos);

            control.Height = editor.Height;
            control.Width = editor.Width;
            control.GetControles().Children.Clear();
            control.GetControles().Children.Add(editor);
            claseIntercambio.adminTransitos.controles.Children.Add(control);
            this.Close();
        }

        private void txtFiltro_KeyUp(object sender, KeyEventArgs e)
        {
             buscar();
        }
    }
}
