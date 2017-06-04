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

namespace project.recso.bascula.wpf.controls
{
    /// <summary>
    /// Interaction logic for Errores.xaml
    /// </summary>
    public partial class Errores : Window
    {
        private Exception excepcion = null;
        public Errores(Exception _ex)
        {
            InitializeComponent();
            excepcion = _ex;
            this.Loaded += new RoutedEventHandler(Errores_Loaded);
        }

        void Errores_Loaded(object sender, RoutedEventArgs e)
        {
            txtdetalle.Text = "Mensaje de error: ";
            txtdetalle.Text += excepcion.Message.ToString();
            txtdetalle.Text += "\r\n\r\nDetalle InnerException:\r\n";
            //txtdetalle.Text += excepcion.InnerException.Message.ToString();
        }
    }
}
