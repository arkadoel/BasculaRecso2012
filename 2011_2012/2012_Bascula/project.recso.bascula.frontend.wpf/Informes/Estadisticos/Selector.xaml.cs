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
using System.Windows.Forms;

namespace project.recso.bascula.frontend.wpf.Informes.Estadisticos
{
    /// <summary>
    /// Interaction logic for Selector.xaml
    /// </summary>
    public partial class Selector : Page
    {
        public Selector()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {

            FolderBrowserDialog dialogo = new FolderBrowserDialog();
            dialogo.Description="Seleccione el directorio donde se guardara la exportacion:";
            dialogo.ShowDialog();

            if (dialogo.SelectedPath != "")
            {
                string ruta = dialogo.SelectedPath;

                switch (cmbIntervaluo.Text.ToLower())
                {
                    case "hoy":
                        logic.gestionEstadisticos.exportarEstadisticoDeHoy(ruta);
                        break;
                    case "esta semana":
                        logic.gestionEstadisticos.exportarEstadisticoEstaSemana(ruta);
                        break;
                    case "este mes":
                        logic.gestionEstadisticos.exportarEstadisticoEsteMes(ruta);
                        break;
                    case "este año":
                        logic.gestionEstadisticos.exportarEstadisticoEsteAnyo(ruta);
                        break;
                }
            }
            
        }
    }
}
