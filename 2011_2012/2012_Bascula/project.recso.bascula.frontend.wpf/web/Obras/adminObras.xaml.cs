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
using project.recso.bascula.logic;
using project.recso.bascula.data;

namespace project.recso.bascula.frontend.wpf.web.Obras
{
    /// <summary>
    /// Interaction logic for adminObras.xaml
    /// </summary>
    public partial class adminObras : Page
    {
        int hoja = 0;
        int totalHojas = 0;
        private const int OBRAS_HOJA = 10;

        List<Obra> lista = new List<Obra>();
        private long? recnumEmpresaAsociada = null;


        public adminObras()
        {
            InitializeComponent();
            claseIntercambio.adminObrasActual = this;
        }

        public adminObras(long recnumEmpresa)
        {
            InitializeComponent();
            claseIntercambio.adminObrasActual = this;
            recnumEmpresaAsociada = recnumEmpresa;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            recargarListadoObras();
        }

        public void recargarListadoObras()
        {
            lista = logic.gestionObras.listarObras(txtFiltro.Text);
            totalHojas =(int)( lista.Count / OBRAS_HOJA);
            totalHojas++;

            bascula.wpf.controls.elementoListaObra elemento = null;
            listado.Children.Clear();

            for (int i = 0; i < OBRAS_HOJA; i++ )
            {
                int posicion = i + hoja;
                if (posicion < lista.Count)
                {
                    Obra ob = lista[posicion];
                    elemento = new bascula.wpf.controls.elementoListaObra(ob);
                    listado.Children.Add(elemento);
                }                
            }
            lblHoja.Content = "Hoja " + hoja + " de " + totalHojas;
        }

        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            listado.Children.Clear();
            
            bascula.wpf.controls.elementoListaObra elemento = new bascula.wpf.controls.elementoListaObra( new Obra(),recnumEmpresaAsociada );
            elemento.getBotonModificar().Content = "Guardar";
            
            listado.Children.Add(elemento);
            
        }

        private void RecargarListado_Click(object sender, RoutedEventArgs e)
        {
            hoja = 0;
            recargarListadoObras();
        }

        private void btnAnterior_Click(object sender, RoutedEventArgs e)
        {
            if (hoja > 0)
            {
                hoja--;
                recargarListadoObras();
            }
        }

        private void btnSiguiente_Click(object sender, RoutedEventArgs e)
        {
            if (hoja < totalHojas)
            {
                hoja++;
                recargarListadoObras();
            }
        }
    }
}
