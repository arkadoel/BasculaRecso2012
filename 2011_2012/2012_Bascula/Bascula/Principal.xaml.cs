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
using System.Windows.Navigation;
using MahApps.Metro.Controls;
using MahApps.Metro;

namespace Bascula
{
    /// <summary>
    /// Interaction logic for Principal.xaml
    /// </summary>
    public partial class Principal : MetroWindow
    {
        public Principal()
        {
            InitializeComponent();
        }

        private void Frame_LoadCompleted(object sender, NavigationEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            navegador.Navigate(new project.recso.bascula.frontend.wpf.inicio());
            this.Title += "  Version " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            ThemeManager.ChangeTheme(this, ThemeManager.DefaultAccents.First(a => a.Name == "Green"), Theme.Dark);
        }

        private void navegador_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            MessageBoxResult resultado = MessageBox.Show("¿Esta seguro de que desea salir de la aplicacion?", "Seguro", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);

            if (resultado == MessageBoxResult.Yes)
            {
                App.cerrar();
            }
        }
    }
}
