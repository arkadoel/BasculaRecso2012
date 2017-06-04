/*
 * 
 * Usuario: Fer.d.minguela@gmail.com
 * Fecha: 04/15/2013
 * Hora: 18:39
 * 
 * 
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Linq;

namespace calculadora
{
	/// <summary>
	/// Interaction logic for Principal.xaml
	/// </summary>
	public partial class Principal : Page
	{
		public Principal()
		{
			InitializeComponent();
		}
                

        private void mnuSalir_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }

        private void mnuAbout_Click(object sender, RoutedEventArgs e)
        {
            apps.controls.Ventana2 v1 = new apps.controls.Ventana2();
            About ab = new About();
            v1.Height = 290;
            v1.Width = 400;
            v1.Titulo = "Acerca de...";
            v1.BtnRestaurar.Visibility = System.Windows.Visibility.Hidden;
            v1.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            var uri = new Uri("pack://application:,,,/calculadora.png");
            var bitmap = new BitmapImage(uri);
            v1.ImagenIcono.Source = bitmap;
            v1.ImagenIcono.Stretch = Stretch.Fill;
            v1.Icon = bitmap;

            ab.FontFamily = apps.fonts.fontManager.getMyAppFonts().First();
            

            v1.Navegador.Navigate(ab);
            v1.Show();

        }
		
		
	}
}