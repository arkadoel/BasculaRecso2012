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
using MahApps.Metro;
using MahApps.Metro.Controls;

namespace project.recso.bascula.frontend.wpf.Licencias
{
    /// <summary>
    /// Interaction logic for MsPL.xaml
    /// </summary>
    public partial class MsPL : MetroWindow
    {
        public MsPL()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ThemeManager.ChangeTheme(this, ThemeManager.DefaultAccents.First(a => a.Name == "Blue"), Theme.Dark);
        }
    }
}
