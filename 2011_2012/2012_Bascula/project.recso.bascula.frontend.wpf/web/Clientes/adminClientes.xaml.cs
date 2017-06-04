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

namespace project.recso.bascula.frontend.wpf.web.Clientes
{
    /// <summary>
    /// Interaction logic for adminClientes.xaml
    /// </summary>
    public partial class adminClientes : Page
    {
        int hoja = 0;
        int totalHojas = 0;
        private const int OBRAS_HOJA = 10;

        public adminClientes()
        {
            InitializeComponent();
        }

        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            editarCliente(null);
           // new Clientes.editCliente().Show();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            claseIntercambio.adminClientesActual = this;
            generarListado();
        }

        public void generarListado()
        {
            List<Empresa> lista = logic.gestionEmpresas.listarEmpresas(txtFiltro.Text);
            totalHojas = (int)(lista.Count / OBRAS_HOJA);
            totalHojas++;
           //hoja = 0;
            Empresa emp = null;

            listado.Children.Clear();    
             for (int i = 0; i < OBRAS_HOJA; i++ )
            {
                int posicion = i + hoja;
                if (posicion < lista.Count)
                {
                    emp = lista[posicion];
                    recso.bascula.wpf.controls.elementoCliente elemento = new recso.bascula.wpf.controls.elementoCliente(emp);
                    elemento.elementoSeleccionado += new EventHandler(elemento_elementoSeleccionado);
                    elemento.verVehiculosCliente += new EventHandler(elemento_verVehiculosCliente);
                    elemento.verObrasCliente += new EventHandler(elemento_verObrasCliente);
                    elemento.verCobrosCliente += new EventHandler(elemento_verCobrosCliente);
                    listado.Children.Add(elemento);
                }
            }
             lblHoja.Content = "Hoja " + hoja + " de " + totalHojas;
        }

        void elemento_verCobrosCliente(object sender, EventArgs e)
        {
            Empresa enviado = (Empresa)sender;

            VerCobros editor = new VerCobros(enviado.recnum);
            bascula.wpf.controls.ChildWin control = new bascula.wpf.controls.ChildWin(claseIntercambio.adminTransitos);

            control.Height = editor.Height;
            control.Width = editor.Width;
            //control.GetControles().Children.Clear();
            control.GetControles().Children.Add(editor);
            controles.Children.Add(control);
        }

        void elemento_verObrasCliente(object sender, EventArgs e)
        {
            Empresa enviado = (Empresa)sender;

            Obras.obrasEmpresa editor = new Obras.obrasEmpresa(enviado.recnum);
            bascula.wpf.controls.ChildWin control = new bascula.wpf.controls.ChildWin(claseIntercambio.adminTransitos);

            control.Height = editor.Height;
            control.Width = editor.Width;
            control.GetControles().Children.Clear();
            control.GetControles().Children.Add(editor);
            controles.Children.Add(control);

            //new Obras.obrasEmpresa().Show();

        }

        void elemento_verVehiculosCliente(object sender, EventArgs e)
        {
            Empresa enviado = (Empresa)sender;
            new Vehiculos.vehiculosEmpresa(enviado.recnum).Show();
        }

        void elemento_elementoSeleccionado(object sender, EventArgs e)
        {
            Empresa enviado = (Empresa)sender;
           // new Clientes.editCliente( enviado.recnum ).Show();
            editarCliente(enviado);
        }

        private void editarCliente(Empresa enviado)
        {
            editCliente editor = null;

            if (enviado == null)
            {
                editor = new Clientes.editCliente();
            }
            else editor = new Clientes.editCliente(enviado.recnum);
            bascula.wpf.controls.ChildWin control = new bascula.wpf.controls.ChildWin(this);

            control.Height = editor.Height;
            control.Width = editor.Width;
            control.GetControles().Children.Clear();
            control.GetControles().Children.Add(editor);
            controles.Children.Add(control);
        }

        private void btnAnterior_Click(object sender, RoutedEventArgs e)
        {
            if (hoja > 0)
            {
                hoja--;
                generarListado();
            }
        }

        private void btnSiguiente_Click(object sender, RoutedEventArgs e)
        {
            if (hoja < totalHojas)
            {
                hoja++;
                generarListado();
            }
        }

        private void RecargarListado_Click(object sender, RoutedEventArgs e)
        {
            generarListado();
        }

        private void txtFiltro_KeyDown(object sender, KeyEventArgs e)
        {
            /*if (e.Key == Key.Enter)
            {*/
                generarListado();
            //}
        }

        private void Image_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {

        }

        
    }
}
