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
using System.Diagnostics;

namespace project.recso.bascula.frontend.wpf.Informes
{
    /// <summary>
    /// Interaction logic for Proceso.xaml
    /// </summary>
    public partial class Proceso : Window
    {
        String ubicacion = "";
        public Proceso(String _ubicacion)
        {
            InitializeComponent();
            ubicacion = _ubicacion;
        }

        public void msg(String _texto)
        {
            txtProgreso.Text = _texto;
        }
        public void clear()
        {
            txtProgreso.Text = "";
        }

        public void progresoAl(double _porcentaje)
        {
            barraProgreso.Value = _porcentaje;
            
        }

        public void setRango(double maximum)
        {
            barraProgreso.Maximum = maximum;
        }

        public void Terminado(Boolean ter)
        {
            switch (ter)
            {
                case true:
                    btnCerrar.Visibility = System.Windows.Visibility.Visible;
                    barraProgreso.Visibility = System.Windows.Visibility.Hidden;
                    break;
                case false:
                    btnCerrar.Visibility = System.Windows.Visibility.Hidden;
                    barraProgreso.Visibility = System.Windows.Visibility.Visible;
                    break;
            }
        }

        private void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("Explorer", ubicacion);
            this.Close();
        }
    }
}
