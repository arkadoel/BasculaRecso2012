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

namespace project.recso.bascula.wpf.controls
{
    /// <summary>
    /// Interaction logic for ElementoAlbaran.xaml
    /// </summary>
    public partial class ElementoAlbaran : UserControl
    {
        private HistoricoAlbarane albaran;
        public event EventHandler verAlbaran;

        public ElementoAlbaran(HistoricoAlbarane _albaran)
        {
            albaran = _albaran;
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(ElementoAlbaran_Loaded);
           
            
        }

        void ElementoAlbaran_Loaded(object sender, RoutedEventArgs e)
        {
            this.numAlbaran.Content = albaran.numAlbaran;
            this.lblFechaSalida.Content = albaran.fechaSalida;
        
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            }

        private void image1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            verAlbaran(albaran, new EventArgs());
        }
    }
}
