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
using project.recso.bascula.wpf.controls;
using Microsoft.Win32;

namespace project.recso.bascula.frontend.wpf.Informes
{
    /// <summary>
    /// Interaction logic for Consultas.xaml
    /// </summary>
    public partial class Consultas : Page
    {
        public Consultas()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(Consultas_Loaded);
            //btnReport.Click += new RoutedEventHandler(boton_Click);
            btnReport2.Click += new RoutedEventHandler(boton_Click);
        }

        /// <summary>
        /// Segun se pulse un menu u otro se hace una accion u otra
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void boton_Click(object sender, RoutedEventArgs e)
        {
            Button boton = sender as Button;
            switch (boton.Tag.ToString().ToLower())
            {
                case "informe para exportar":
                   /* di fdialog = new SaveFileDialog();
                    
                    fdialog.ShowDialog();
                    new Informes.ExportarMilena( fdialog.FileName, logic.gestionHistorialAlbaranes.getAlbaranesEntreFechas("","") );*/
                    break;

                case "reimprimir un albaran":
                    
                    inputBox dlg = new inputBox("Re imprimir un albaran", "¿Que numero de albaran desea reimprimir?");
                    dlg.ShowDialog();
                    while (dlg.IsActive) ;
                    String resultado = dlg.resultado;

                    new visorInformes(resultado).Show();
                    
                    break;
                case "filtrar albaranes":
                    this.navegador.Navigate(new Informes.filtrarHistorico());
                    break;
                case "estadisticos":
                    this.navegador.Navigate(new Informes.Estadisticos.Selector());
                    break;
            }
            visibilidadMenu();
        }

        void Consultas_Loaded(object sender, RoutedEventArgs e)
        {
            menuInformes.Visibility = Visibility.Hidden;
        }

        private void btnVerMenu_Click(object sender, RoutedEventArgs e)
        {
            visibilidadMenu();
        }

        private void visibilidadMenu()
        {
            switch (menuInformes.Visibility)
            {
                case Visibility.Hidden:
                    menuInformes.Visibility = Visibility.Visible;
                    break;
                case Visibility.Visible:
                    menuInformes.Visibility = Visibility.Hidden;
                    break;
            }
        }
    }
}
