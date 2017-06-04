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
using project.recso.bascula.data;
using Microsoft.VisualBasic;

namespace project.recso.bascula.frontend.wpf.web.Clientes
{
    /// <summary>
    /// Interaction logic for editCliente.xaml
    /// </summary>
    public partial class editCliente : UserControl
    {
        private String recnum;

        public editCliente()
        {
            InitializeComponent();
            recnum = "";
        }

        public editCliente(long id)
        {
            InitializeComponent();
            recnum = id.ToString();
        }

        public void Cerrar()
        {
            this.Visibility = Visibility.Hidden;
            this.UserControl_Unloaded(new object(), new RoutedEventArgs());
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            claseIntercambio.adminClientesActual.generarListado();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (recnum.Trim() != "")
            {
                Empresa o = logic.gestionEmpresas.getEmpresaByRecnum(long.Parse(Convert.ToDecimal(recnum).ToString()));

                if (o != null)
                {
                    txtCif.Text = o.cif;
                    txtCodigoMilena.Text = o.codigoMilena.ToString();
                    txtCuentaBancaria.Text = o.cuentaBancaria;
                    txtDireccion.Text = o.direccion;
                    txtEmail.Text = o.email;
                    txtLocalidad.Text = o.localidad;
                    txtProvincia.Text = o.provincia;
                    txtNombre.Text = o.nombre;
                    txtRazon.Text = o.razonSocial;
                    txtTelefono.Text = o.telefono;

                    asignarTipoDeEmpresa(o);
                    asignarTipoDePago(o);

                    chkmoroso.IsChecked = o.esmoroso;
                    
                }
            }
        }

        private void asignarTipoDeEmpresa(Empresa o)
        {
            bool salir = false;

            for (short i = 0; i < cmbTipoCliente.Items.Count && !salir; i++)
            {
                String textoItem = ((ComboBoxItem)cmbTipoCliente.Items[i]).Content.ToString();

                if (o.tipoDeEmpresa != null)
                {
                    if (textoItem.Trim() == o.tipoDeEmpresa.Trim())
                    {
                        cmbTipoCliente.SelectedIndex = i;
                        salir = true;
                    }
                }
            }
        }

        private void asignarTipoDePago(Empresa o)
        {
            bool salir = false;

            for (short i = 0; i < cmbTipoPago.Items.Count && !salir; i++)
            {
                String textoItem = ((ComboBoxItem)cmbTipoPago.Items[i]).Content.ToString();

                if (textoItem.Trim() == o.tipoDePago.Trim().Replace("EN ",""))
                {
                    cmbTipoPago.SelectedIndex = i;                    
                    salir = true;
                }
            }

            if (o.tipoDePago.Contains("EFECTIVO") == false && o.tipoDePago.Contains("EN EFECTIVO") == false)
            {
                FormasPago forma = logic.gestionFormasPago.getFormaPagoEmpresa(o.recnum);
                if (forma != null)
                {
                    txtSaldoActual.Text = forma.saldoActual.ToString();
                    lblUltimaModificacion.Content = "Ultima modificacion: " + forma.ultimaModificacion.ToString();
                    if (o.tipoDePago.Contains("PENDIENTE"))
                    {
                        txtLimiteSaldo.Text = forma.limiteSaldo.ToString();
                    }
                }
            }
        }

        private void ellipse1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Cerrar();
        }

        private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void guardar(object sender, RoutedEventArgs e)
        {
            Empresa o = new Empresa();
            
                o.cif= txtCif.Text;
                if (txtCodigoMilena.Text != "")
                {
                    o.codigoMilena = long.Parse(txtCodigoMilena.Text.ToString());
                }
                o.cuentaBancaria = txtCuentaBancaria.Text;
                o.direccion = txtDireccion.Text;
                o.email = txtEmail.Text;
                o.localidad = txtLocalidad.Text;
                o.provincia = txtProvincia.Text;
                o.nombre = txtNombre.Text;
                o.razonSocial = txtRazon.Text;
                o.telefono = txtTelefono.Text;
                o.tipoDeEmpresa = cmbTipoCliente.Text;
                o.tipoDePago = cmbTipoPago.Text;
                o.esmoroso = chkmoroso.IsChecked;

                if (recnum.Trim() != "") o.recnum = long.Parse(Convert.ToDecimal(recnum).ToString());

                if (o.tipoDePago.Contains("EFECTIVO")== false && txtSaldoActual.Text != "")
                {
                    FormasPago forma = new FormasPago();
                    forma.recnumEmpresa = o.recnum;

                    forma.saldoActual = Convert.ToDouble(txtSaldoActual.Text);
                    forma.ultimaModificacion = DateTime.Today.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();

                    if (o.tipoDePago.Contains("PENDIENTE"))
                    {
                        forma.limiteSaldo = Convert.ToDouble(txtLimiteSaldo.Text);
                    }
                    logic.gestionFormasPago.guardarFormaPago(forma);
                }

                try
                {
                    logic.gestionEmpresas.mergeOrCreate(o);
                    MessageBox.Show("Se realizo la operacion con exito", "Guardado", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Fallo", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            claseIntercambio.adminClientesActual.generarListado();
            Cerrar();
        }

        private void cerrar(object sender, RoutedEventArgs e)
        {
            Cerrar();
        }

        private void eliminar(object sender, RoutedEventArgs e)
        {
            if (recnum != "")
            {
                MessageBoxResult pregunta = MessageBox.Show("¿Esta seguro de que quiere eliminar esta empresa?", "Seguro", MessageBoxButton.YesNoCancel, MessageBoxImage.Question, MessageBoxResult.No);

                if (pregunta == MessageBoxResult.Yes)
                {
                    Boolean conseguido = logic.gestionEmpresas.eliminarEmpresa(long.Parse( Convert.ToInt32(recnum).ToString()));
                    MessageBox.Show("Se realizo la operacion con exito", "Eliminado", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            Cerrar();
        }

        private void cmbTipoPago_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            if((sender as ComboBox).SelectedValue.ToString().Contains("EFECTIVO"))
            {            
                    zonaPago.Visibility = Visibility.Hidden;
            }
            if ((sender as ComboBox).SelectedValue.ToString().Contains("PENDIENTE DE PAGO"))
            {
                zonaPago.Visibility = Visibility.Visible;
                txtLimiteSaldo.Visibility = Visibility.Visible;
                lblLimiteSaldo.Visibility = Visibility.Visible;
                ponerZonaPagos("pendiente");
            }
            if((sender as ComboBox).SelectedValue.ToString().Contains("PREPAGO"))
            {                
                    zonaPago.Visibility = Visibility.Visible;
                    txtLimiteSaldo.Visibility = Visibility.Hidden;
                    lblLimiteSaldo.Visibility = Visibility.Hidden;
                    ponerZonaPagos("prepago");
            }
        }

        public void ponerZonaPagos(String _tipoPago)
        {
            long numero =0;

            if (recnum.Trim() != "")
            {
                numero = long.Parse(Convert.ToDecimal(recnum).ToString());
                FormasPago forma = logic.gestionFormasPago.getFormaPagoEmpresa(numero);

                if (forma != null)
                {
                    txtSaldoActual.Text = forma.saldoActual.ToString();

                    switch (_tipoPago)
                    {
                        case "pendiente":
                            txtLimiteSaldo.Text = forma.limiteSaldo.ToString();
                            break;
                        case "prepago":
                            txtLimiteSaldo.Text = "";
                            break;
                    }
                }
            }
            else MessageBox.Show("Cree primero la empresa y despues añada las formas de pago.");            
            
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {

        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            string money = Microsoft.VisualBasic.Interaction.InputBox("Escriba la cantidad a añadir", "Agregar dinero");

            if (money != "")
            {
                    double dinero = Convert.ToDouble(money);
                    if (txtSaldoActual.Text == "") txtSaldoActual.Text = "0";
                    double actual = Convert.ToDouble(txtSaldoActual.Text);
                    txtSaldoActual.Text = (actual + dinero).ToString();
            }
        }

        private void mandarEmailConDatos(object sender, MouseButtonEventArgs e)
        {
            String asunto = "Recso Bascula (Datos de empresa: " + txtNombre.Text + ")";
            string texto = "Nombre: " + txtNombre.Text;
            texto += "\r\n\tRazon social: " + txtRazon.Text;
            texto += "\r\n\tDireccion:  " + txtDireccion.Text;
            texto += "\r\n\tLocalidad: " + txtLocalidad.Text;
            texto += "\r\n\tProvincia: " + txtProvincia.Text;
            texto += "\r\n\tCIF: " + txtCif.Text;
            texto += "\r\n\tEmail: " + txtEmail.Text;
            texto += "\r\n\tTelefono " + txtTelefono.Text;
            texto += "\r\n\tForma de pago: " + cmbTipoPago.Text;
            texto += "\r\n\tCuenta bancaria: " + txtCuentaBancaria.Text;


            new Clientes.MandarMailsCliente(asunto,texto).Show();
        }
    }
}
