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

namespace project.recso.bascula.frontend.wpf.web.residuos
{
    /// <summary>
    /// Interaction logic for adminResiduos.xaml
    /// </summary>
    public partial class adminResiduos : Page
    {
        public adminResiduos()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            claseIntercambio.adminResiduosActual = this;
            crearListado();

        }

        public void crearListado()
        {
            listado.Children.Clear();
            List<Residuo> lista = logic.gestionResiduos.getListaResiduos();

            foreach (Residuo residuo in lista)
            {
                project.recso.bascula.wpf.controls.ListaResiduos actual = new project.recso.bascula.wpf.controls.ListaResiduos(residuo);
                actual.elementoSeleccionado += new EventHandler(actual_elementoSeleccionado);
                listado.Children.Add(actual);
            }
        }

        void actual_elementoSeleccionado(object sender, EventArgs e)
        {
            Residuo residuo = (Residuo)sender;
            editarResiduo(residuo);
        }

        private void editarResiduo(Residuo res)
        {
            editResiduo editor = null;
            if (res == null)
            {
                editor = new editResiduo();
            }
            else
            {
                editor = new editResiduo(res.recnum);
            }
            bascula.wpf.controls.ChildWin control = new bascula.wpf.controls.ChildWin(this);

            control.Height = editor.Height;
            control.Width = editor.Width;
            control.GetControles().Children.Clear();
            control.GetControles().Children.Add(editor);
            controles.Children.Add(control);
        }

        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            //agregar nuevo residuo
            editarResiduo(null);
        }
    }
}
