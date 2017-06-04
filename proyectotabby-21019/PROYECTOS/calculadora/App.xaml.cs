using System;
using System.Windows;
using System.Data;
using System.Xml;
using System.Configuration;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace calculadora
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		
		void Application_Startup(object sender, StartupEventArgs e)
		{
			apps.controls.Ventana2 v1 = new apps.controls.Ventana2();
			v1.Titulo ="Gran calculadora";

            var uri = new Uri("pack://application:,,,/calculadora.png");
            var bitmap = new BitmapImage(uri);
            v1.ImagenIcono.Source = bitmap;
            v1.ImagenIcono.Stretch = Stretch.Fill;
            v1.Icon = bitmap;

            v1.Height=440;
			v1.Width=510;
			//v1.ResizeMode = ResizeMode.NoResize;
			v1.BtnRestaurar.Visibility = Visibility.Hidden;
			v1.Navegador.Navigate(new Principal());
			v1.Show();
		}

        public void exit()
        {
            Application.Current.Shutdown();
        }
	}
}