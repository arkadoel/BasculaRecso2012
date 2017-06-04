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

namespace project.recso.bascula.frontend.wpf.web.Transitos
{
    /// <summary>
    /// Interaction logic for MOROSO.xaml
    /// </summary>
    public partial class MOROSO : UserControl
    {
        public MOROSO(String nombreEmpresa)
        {
            InitializeComponent();
            this.Loaded+=(e,s)=>{
                texto.Text = "Se ha detectado que la empresa " + nombreEmpresa.ToUpper() + " no esta al dia de sus pagos, por favor consulte con contabilidad.";
            };

        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public void Close()
        {
            this.Visibility = Visibility.Hidden;
            this.UserControl_Unloaded(new object(), new RoutedEventArgs());
        }
        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
