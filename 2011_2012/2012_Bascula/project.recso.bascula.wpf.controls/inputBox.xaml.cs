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
    /// Interaction logic for inputBox.xaml
    /// </summary>
    public partial class inputBox : Window
    {
        public String resultado = "";

        public inputBox(String titulo, String texto)
        {
            InitializeComponent();
            lblTexto.Content = texto;
            this.Title = titulo;
            this.Closing += new System.ComponentModel.CancelEventHandler(inputBox_Closing);
        }

        void inputBox_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            resultado = txtInput.Text;
            
            this.Close();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
