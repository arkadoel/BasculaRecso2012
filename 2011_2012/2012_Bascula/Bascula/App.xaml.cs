using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace Bascula
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static void cerrar()
        {
            Application.Current.Shutdown();

        }
        private void Application_Exit(object sender, ExitEventArgs e)
        {

        }
    }
}
