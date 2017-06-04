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
using Microsoft.Windows.Controls;

namespace project.recso.bascula.frontend.wpf.web.Obras
{
    /// <summary>
    /// Interaction logic for obrasEmpresa.xaml
    /// </summary>
    public partial class obrasEmpresa : UserControl
    {
        private List<Obra> listaObras = new List<Obra>();
        private List<Obra> listaObrasEmpresa = new List<Obra>();
        
        private long idEmpresa;

        public obrasEmpresa(long _idEmpresa)
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(obrasEmpresa_Loaded);
            idEmpresa = _idEmpresa;
        }

        void obrasEmpresa_Loaded(object sender, RoutedEventArgs e)
        {
            listaObras = logic.gestionObras.listarObras();
            listaObrasEmpresa = logic.gestionObras.listarObrasDeEmpresa(idEmpresa);

           recargarListadoObras();
        }

        private void recargarListadoObras()
        {
            lstObras.Items.Clear();
            foreach (Obra o in listaObras)
            {
                lstObras.Items.Add(o.denominacion);
            }

            lstObraEmpresa.Items.Clear();
            foreach (Obra o in listaObrasEmpresa)
            {
                lstObraEmpresa.Items.Add(o.denominacion);
            }

        }

        public void Cerrar()
        {
            this.Visibility = Visibility.Hidden;
            this.UserControl_Unloaded(new object(), new RoutedEventArgs());
        }
        private void image1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Cerrar();
        }

        private void rectangle1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
           
        }

        private void btnBuscarEmpresa_Click(object sender, RoutedEventArgs e)
        {
            buscar();
        }

        private void buscar()
        {
            listaObras = logic.gestionObras.listarObras(txtfiltro.Text.ToString());

            recargarListadoObras();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            foreach (string item in lstObras.CheckedItems)
            {

                if (lstObraEmpresa.Items.Contains(item) == true)
                {
                    claseIntercambio.msg("La obra ya esta presente", "Ya añadida", MessageBoxButton.OK, MessageBoxImage.Information);

                }
                else
                {
                    lstObraEmpresa.Items.Add(item);
                    Obra obra = (from r in listaObras
                                where r.denominacion == item
                                select r).First<Obra>() ;

                    listaObrasEmpresa.Add(obra);

                    claseIntercambio.msg("Obra añadida a empresa con exito", "Exito", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

       

        private void btnQuitar_Click(object sender, RoutedEventArgs e)
        {
            foreach (string item in lstObraEmpresa.CheckedItems)
            {
                lstObraEmpresa.Items.Remove(item);

                Obra obra = (from r in listaObrasEmpresa
                             where r.denominacion.Replace(" ", "").Replace("C/", "").Contains(item.Replace(" ", "").Replace("C/", ""))
                             select r).First<Obra>();

                listaObrasEmpresa.Remove(obra);
            }
        }

        private void btnTerminar_Click(object sender, RoutedEventArgs e)
        {
            /*coger las empresas de listado de obras seleccionadas y guardar la
             * asociacion con la empresa actual
             */
            logic.gestionObras.guardarListadoObrasEmpresa(listaObrasEmpresa, idEmpresa);
            Cerrar();
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {

        }

        private void txtfiltro_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                buscar();
            }
        }
    }
}
