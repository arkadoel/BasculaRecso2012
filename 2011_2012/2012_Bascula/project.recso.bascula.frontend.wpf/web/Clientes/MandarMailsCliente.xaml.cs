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
using System.Threading;

namespace project.recso.bascula.frontend.wpf.web.Clientes
{
    /// <summary>
    /// Interaction logic for MandarMailsCliente.xaml
    /// </summary>
    public partial class MandarMailsCliente : Window
    {
        public MandarMailsCliente()
        {
            InitializeComponent();
        }

        public MandarMailsCliente(String _asunto, String _mensaje)
        {
            InitializeComponent();
            this.txtAsunto.Text = _asunto;
            this.txtCuerpoMensaje.Text = _mensaje;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var listaMails = from t in logic.gestionUsuarios.getUsuarios()
                             select new
                             {
                                 email = t.email,
                                 nombrePersona = t.NombreReal
                             };
            foreach (var persona in listaMails)
            {
                ComboBoxItem elemento = new ComboBoxItem();
                elemento.Content = persona.nombrePersona;
                elemento.Tag = persona.email;
                cmbEmail.Items.Add(elemento);
            }
        }

        private void btnEnviarEmail_Click(object sender, RoutedEventArgs e)
        {
            if (cmbEmail.SelectedIndex != -1)
            {
                String emailDestino = ((ComboBoxItem)cmbEmail.SelectedItem).Tag.ToString();
                string cuerpo = txtCuerpoMensaje.Text;
                string asunto = txtAsunto.Text;
                new Thread(delegate() { logic.gestionEmails.mandarEmail(cuerpo, asunto, emailDestino); }).Start();
            }
            else MessageBox.Show("Seleccione un destinatario", "Falta destinatario");
        }
    }
}
