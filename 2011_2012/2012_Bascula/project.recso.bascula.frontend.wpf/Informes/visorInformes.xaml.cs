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
using MahApps.Metro;
using MahApps.Metro.Controls;

namespace project.recso.bascula.frontend.wpf.Informes
{
    /// <summary>
    /// Interaction logic for visorInformes.xaml
    /// </summary>
    public partial class visorInformes : MetroWindow
    {
        private infAlbaran informe;
        private HistoricoAlbarane historico = null;

        
        public visorInformes()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(visorInformes_Loaded);

        }

        public visorInformes(String numAlbaran)
        {
            InitializeComponent();
            historico = logic.gestionHistorialAlbaranes.getUnAlbaran(numAlbaran);
            this.Loaded += new RoutedEventHandler(visorInformes_Loaded);
        }

        void visorInformes_Loaded(object sender, RoutedEventArgs e)
        {
            ThemeManager.ChangeTheme(this, ThemeManager.DefaultAccents.First(a => a.Name == "Green"), Theme.Dark);

            if (historico == null)
            {
                informe = new project.recso.bascula.frontend.wpf.Informes.infAlbaran();
            }
            else
            {
                informe = new project.recso.bascula.frontend.wpf.Informes.infAlbaran(historico);
            }
            visor.Navigate(informe);
        }

        private void image1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //imprimir
            PrintDialog pg = new System.Windows.Controls.PrintDialog();
            if (pg.ShowDialog() == true)
            {
                pg.PrintVisual(informe.areaPapel, "Albaran de prueba");
            }
        }

        private void btnExportar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            System.Windows.Forms.SaveFileDialog dialogo = new System.Windows.Forms.SaveFileDialog();
            dialogo.Title = "Exportar como...";
            dialogo.Filter = "Microsoft XPS (*.xps)|*.xps";
            dialogo.ShowDialog();

            if (dialogo.FileName != null)
            {
               new logic.generarXPS().Generacion(dialogo.FileName, informe.areaPapel);
               MessageBox.Show("Operacion terminada");
            }
        }
    }
}
