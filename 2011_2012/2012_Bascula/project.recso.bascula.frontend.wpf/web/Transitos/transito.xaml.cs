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
using System.Windows.Navigation;
using System.Windows.Shapes;
using project.recso.bascula.data;
using project.recso.bascula.logic;

namespace project.recso.bascula.frontend.wpf.web.Transitos
{
    /// <summary>
    /// Interaction logic for transito.xaml
    /// </summary>
    public partial class transito : Page
    {
        #region "subclases de apoyo"
            public class forComboResiduos
            {
                public int id { get; set; }
                public int recnum { get; set; }

                /// <summary>
                /// 
                /// </summary>
                /// <param name="_idLista">indice del residuo en la lista</param>
                /// <param name="_recnum">id del residuo en la DB</param>
                public forComboResiduos(int _idLista, int _recnum)
                {
                    id = _idLista;
                    recnum = _recnum;
                }
            }
            private List<forComboResiduos> listadoResiduos;

            public class forComboTipoVehiculo
            {
                public int id { get; set; }
                public int recnum { get; set; }

                /// <summary>
                /// 
                /// </summary>
                /// <param name="_idLista">indice del TipoVehiculo en la lista</param>
                /// <param name="_recnum">id del TipoVehiculo en la DB</param>
                public forComboTipoVehiculo(int _idLista, int _recnum)
                {
                    id = _idLista;
                    recnum = _recnum;
                }
            }
            private List<forComboTipoVehiculo> listadoTiposDeVehiculo;
        #endregion
            

        public transito()
        {
            InitializeComponent();
            //cargar eventos
            Loaded += new RoutedEventHandler(transito_Loaded);
            //agregar resto de eventos
            cmbTipoResiduo.SelectionChanged += new SelectionChangedEventHandler(cmbTipoResiduo_SelectionChanged);
            //btnNuevoAlbaran.Click += new RoutedEventHandler(btnNuevoAlbaran_Click);
            cmbResiduo.SelectionChanged += new SelectionChangedEventHandler(cmbResiduo_SelectionChanged);
            cmbTipoVehiculo.SelectionChanged += new SelectionChangedEventHandler(cmbTipoVehiculo_SelectionChanged);

            txtPoseedor.TextChanged += new TextChangedEventHandler(txtPoseedor_TextChanged);
            txtBruto.KeyDown += new KeyEventHandler(txtBruto_KeyDown);
            txtTara.KeyDown += new KeyEventHandler(txtTara_KeyDown);

            claseIntercambio.adminTransitos = this;
        }

        void txtTara_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                txtNeto.Focus();
            }
        }

        void txtBruto_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                txtTara.Focus();
            }
        }

        void txtPoseedor_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (claseIntercambio.transitoActual.empPoseedor != null)
            {
                lblTipoPago.Content = claseIntercambio.transitoActual.empPoseedor.tipoDePago;
                txtObra.Text = "";
                //claseIntercambio.transitoActual.obra = new Obra();
            }
        }


        void cmbTipoVehiculo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //cambiar los datos de: metros cubicos
            int indice = (cmbTipoVehiculo.SelectedIndex);
            if (indice > -1)
            {
                int recnum = (listadoTiposDeVehiculo[indice] as forComboTipoVehiculo).recnum;

                TipoVehiculo tipo = logic.gestionTipoVehiculo.getTipoVehiculoByRecnum(recnum);

                lblm3.Content = tipo.capacidad.ToString();
                claseIntercambio.transitoActual.tipoVehiculo = tipo.nombre;
                claseIntercambio.transitoActual.metrosCubicos = tipo.capacidad.ToString();

                calcularDensidad();
            }
        }

        /// <summary>
        /// Calcula la densidad dependiendo del peso neto y los metros
        /// cubicos del vehiculo        
        /// </summary>   
        private void calcularDensidad()
        {
            if (txtNeto.Text != "" && lblm3.Content.ToString() != "")
            {
                long neto = long.Parse(Convert.ToInt64(txtNeto.Text).ToString());
                float m3 = float.Parse(Convert.ToDouble(lblm3.Content).ToString());

                float densidad = ((neto / m3) / 1000);

                lblDensidad.Content = densidad.ToString("F2") + " Tm/m3";
                claseIntercambio.transitoActual.pesoMetrosCubicos = lblDensidad.Content.ToString();
            }
        }

        void cmbResiduo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            recalcularResiduo();
            recalcularImporte();
        }

        private void recalcularResiduo()
        {
            int indice = (cmbResiduo.SelectedIndex);
            if (indice > -1)
            {
                int recnum = (listadoResiduos[indice] as forComboResiduos).recnum;

                Residuo residuo = logic.gestionResiduos.getResiduoByRecnum(recnum);
                claseIntercambio.transitoActual.residuoSeleccionado = residuo;
            }
        }

        private void recalcularImporte()
        {
            //cambia el valor del importe
            int indice = (cmbResiduo.SelectedIndex);
            if (indice > -1 && txtNeto.Text !="")
            {
                int recnum = (listadoResiduos[indice] as forComboResiduos).recnum;

                Residuo residuo = logic.gestionResiduos.getResiduoByRecnum(recnum);
                claseIntercambio.transitoActual.residuoSeleccionado = residuo;

                float neto = float.Parse(Convert.ToInt64(txtNeto.Text).ToString());
                float precioTm = float.Parse(residuo.precio.ToString());
                float ivaAplicado = float.Parse(residuo.ivaAplicado.ToString());

                float importe = (neto / 1000) * precioTm;

                if (residuo.tipoMaterial.Contains( gestionResiduos.__TIPO_RESIDUO__.TARIFA_PLANA))
                {
                    importe = precioTm;
                }

                float eurosIVA = (importe * ivaAplicado) / 100;

                float importeFinal = importe + eurosIVA;               

                txtImporteFinal.Text = importeFinal.ToString("F2");

                claseIntercambio.transitoActual.residuoSeleccionado = residuo;
                claseIntercambio.transitoActual.importeFinal = txtImporteFinal.Text;
                claseIntercambio.transitoActual.importeIVA = eurosIVA.ToString("F2");
                claseIntercambio.transitoActual.importeSinIVA = importe.ToString("F2");
                claseIntercambio.transitoActual.IVAaplicado = "IVA " + ivaAplicado.ToString("F2").ToString() + "%";

            }
        }

        void btnNuevoAlbaran_Click(object sender, RoutedEventArgs e)
        {
            guardarAlbaran(false);
            //limpia la pantalla para un nuevo albaran
            bloquearControles(false);

            claseIntercambio.limpiarDeDatos(); 
            
            //asignar numero de albaran y fecha de entrada
            txtFechaEntrada.Text = DateTime.Today.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
            logic.gestionConfiguracionApp.sumarUnAlbaran();
            txtNumAlbaran.Text = logic.gestionConfiguracionApp.getUltimoAlbaran().ToString();
            cargarCombos();
            cmbTipoResiduo.Text="TODOS";
            

            claseIntercambio.transitoActual.numAlbaran = txtNumAlbaran.Text;
            claseIntercambio.transitoActual.fechaEntrada = txtFechaEntrada.Text;

            //para evitar errores de tipo null
            claseIntercambio.transitoActual.obra = new Obra();
            claseIntercambio.transitoActual.empProductor = new Empresa();
            claseIntercambio.transitoActual.empPlantaTransferencia = new Empresa();
            claseIntercambio.transitoActual.empPlantaValorizacion = new Empresa();
            claseIntercambio.transitoActual.empPoseedor = new Empresa();
            claseIntercambio.transitoActual.EmpTransportista = new Empresa();
            guardarAlbaran(true);
            txtMatricula.Focus();
        }

        void cmbTipoResiduo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cargarComboResiduos( ((ComboBoxItem)cmbTipoResiduo.SelectedValue).Content.ToString() );
        }

        void transito_Loaded(object sender, RoutedEventArgs e)
        {
            limpiarDeDatos();
            bloquearControles(true);

            if (claseIntercambio.transitoActual.numAlbaran != null )
            {
                rellenarConDatosAnteriores();
            }
        }

        private void rellenarConDatosAnteriores()
        {
            bloquearControles(false);
            txtBruto.Text = claseIntercambio.transitoActual.bruto;
            txtFechaEntrada.Text = claseIntercambio.transitoActual.fechaEntrada;
            txtFechaSalida.Text = "";
            txtImporteFinal.Text = claseIntercambio.transitoActual.importeFinal;
            txtMatricula.Text = claseIntercambio.transitoActual.matricula;
            txtNeto.Text = claseIntercambio.transitoActual.neto;
            txtNumAlbaran.Text = claseIntercambio.transitoActual.numAlbaran;
            txtObra.Text = claseIntercambio.transitoActual.obra.denominacion;

            if (claseIntercambio.transitoActual.empPlantaTransferencia != null)
            {
                txtPlantaTransferencia.Text = claseIntercambio.transitoActual.empPlantaTransferencia.nombre;
            }
            if (claseIntercambio.transitoActual.empPlantaValorizacion != null)
            {
                txtPlantaValorizacion.Text = claseIntercambio.transitoActual.empPlantaValorizacion.nombre;
            }
            if (claseIntercambio.transitoActual.empPoseedor != null)
            {
                txtPoseedor.Text = claseIntercambio.transitoActual.empPoseedor.nombre;
                lblTipoPago.Content = claseIntercambio.transitoActual.empPoseedor.tipoDePago;
            }
            if (claseIntercambio.transitoActual.empProductor != null)
            {
                txtProductor.Text = claseIntercambio.transitoActual.empProductor.nombre;
            }
            if (claseIntercambio.transitoActual.obra != null)
            {
                txtObra.Text = claseIntercambio.transitoActual.obra.denominacion;
            }
            txtTara.Text = claseIntercambio.transitoActual.tara;

            ponerTextoCombos(cmbResiduo, claseIntercambio.transitoActual.residuoSeleccionado.codigoLER + "" + claseIntercambio.transitoActual.residuoSeleccionado.nombre);
            ponerTextoCombos(cmbTipoVehiculo, claseIntercambio.transitoActual.tipoVehiculo);
            
        }

        private void ponerTextoCombos(ComboBox _combo, String _buscado)
        {
            if (_buscado != null)
            {
                Boolean salir = false;
                _buscado = _buscado.ToLower().Replace(" ", "").Trim();
                for (int i = 0; i < _combo.Items.Count && !salir; i++)
                {
                    String textoItem = _combo.Items[i].ToString().ToLower().Replace(" ", "").Trim();

                    if (textoItem == _buscado.Trim())
                    {
                        _combo.SelectedIndex = i;
                        salir = true;
                    }
                }
            }
        }

        /// <summary>Vacia todas las cajas de texto y quita todos los datos de la interfaz 
        /// </summary>
        private void limpiarDeDatos()
        {
            txtBruto.Text = "";
            txtFechaEntrada.Text = "";
            txtFechaSalida.Text = "";
            txtImporteFinal.Text = "";
            txtMatricula.Text = "";
            txtNeto.Text = "";
            txtNumAlbaran.Text = "";
            txtObra.Text = "";
            txtPlantaTransferencia.Text = "";
            txtPlantaValorizacion.Text = "";
            txtPoseedor.Text = "";
            txtProductor.Text = "";
            txtTara.Text = "";

            cargarCombos();
        }

        /// <summary>Bloquea los controles de la interfaz para evitar su uso incorrecto
        /// </summary>
        /// <param name="bloquear"></param>
        private void bloquearControles(Boolean bloquear)
        {
            //bloquear es lo distinto de Enabled
            txtBruto.IsEnabled = !bloquear;
            txtFechaEntrada.IsEnabled = !bloquear;
            txtFechaSalida.IsEnabled = !bloquear;
            txtImporteFinal.IsEnabled = !bloquear;
            txtMatricula.IsEnabled = !bloquear;
            txtNeto.IsEnabled = !bloquear;
            txtNumAlbaran.IsEnabled = !bloquear;
            txtObra.IsEnabled = !bloquear;
            txtPlantaTransferencia.IsEnabled = !bloquear;
            txtPlantaValorizacion.IsEnabled = !bloquear;
            txtPoseedor.IsEnabled = !bloquear;
            txtProductor.IsEnabled = !bloquear;
            txtTara.IsEnabled = !bloquear;
            cmbResiduo.IsEnabled = !bloquear;
            cmbTipoResiduo.IsEnabled = !bloquear;
            cmbTipoVehiculo.IsEnabled = !bloquear;


            if (bloquear == false)
            {
                limpiarDeDatos();
                
                //poner controles en amarillo
                txtBruto.Background = Brushes.LemonChiffon;
                txtFechaEntrada.Background = Brushes.LemonChiffon;
                txtFechaSalida.Background = Brushes.LemonChiffon;
                txtImporteFinal.Background = Brushes.LemonChiffon;
                txtMatricula.Background = Brushes.LemonChiffon;
                txtNeto.Background = Brushes.LemonChiffon;
                txtNumAlbaran.Background = Brushes.LemonChiffon;
                txtObra.Background = Brushes.LemonChiffon;
                txtPlantaTransferencia.Background = Brushes.LemonChiffon;
                txtPlantaValorizacion.Background = Brushes.LemonChiffon;
                txtPoseedor.Background = Brushes.LemonChiffon;
                txtProductor.Background = Brushes.LemonChiffon;
                txtTara.Background = Brushes.LemonChiffon;
                cmbResiduo.Background = Brushes.LemonChiffon;
                cmbTipoResiduo.Background = Brushes.LemonChiffon;
                cmbTipoVehiculo.Background = Brushes.LemonChiffon;
            }
            else
            {
                //poner todo en gris
                txtBruto.Background = Brushes.Gray;
                txtFechaEntrada.Background = Brushes.Gray;
                txtFechaSalida.Background = Brushes.Gray;
                txtImporteFinal.Background = Brushes.Gray;
                txtMatricula.Background = Brushes.Gray;
                txtNeto.Background = Brushes.Gray;
                txtNumAlbaran.Background = Brushes.Gray;
                txtObra.Background = Brushes.Gray;
                txtPlantaTransferencia.Background = Brushes.Gray;
                txtPlantaValorizacion.Background = Brushes.Gray;
                txtPoseedor.Background = Brushes.Gray;
                txtProductor.Background = Brushes.Gray;
                txtTara.Background = Brushes.Gray;
                cmbResiduo.Background = Brushes.Gray;
                cmbTipoResiduo.Background = Brushes.Gray;
                cmbTipoVehiculo.Background = Brushes.Gray;
            }
        }

        private void cargarCombos()
        {
            //combo residuo
            cargarComboResiduos(gestionResiduos.__TIPO_RESIDUO__.TODOS);

            cargarComboTipoDeVehiculo();

        }

        private void cargarComboTipoDeVehiculo()
        {
            List<TipoVehiculo> tiposDeVehiculos = logic.gestionTipoVehiculo.getTiposDeVehiculos();
            cmbTipoVehiculo.Items.Clear();
            int indice = 0;
            listadoTiposDeVehiculo = new List<forComboTipoVehiculo>();

            foreach (TipoVehiculo tipoVeh in tiposDeVehiculos)
            {
                cmbTipoVehiculo.Items.Add(tipoVeh.nombre.ToUpper().ToString());
                listadoTiposDeVehiculo.Add( new forComboTipoVehiculo(indice, tipoVeh.recnum));
                indice++;
            }
        }

        private void cargarComboResiduos(String tipo)
        {
            List<Residuo> listado = new List<Residuo>();

            listado = logic.gestionResiduos.getListaResiduos(tipo);

            cmbResiduo.Items.Clear();
            int indice = 0;
            listadoResiduos = new List<forComboResiduos>();
            foreach (Residuo residuo in listado)
            {
                cmbResiduo.Items.Add(residuo.codigoLER.ToString().Trim() + " "  + residuo.nombre.ToString());
                listadoResiduos.Add( new forComboResiduos(indice, residuo.recnum));
                indice++;
            }

            if (tipo.ToLower().Contains("salida"))
            {
                //el productor sera recso
                Empresa recso = logic.gestionEmpresas.getRECSO();

                txtProductor.Text = recso.nombre;
                claseIntercambio.transitoActual.empProductor = recso;
            }

        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //imprimir
            //new frontend.wpf.Informes.visorInformes().Show();
            txtFechaSalida.Text = DateTime.Today.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
            guardarAlbaran(false);
        }

        private void seleccionarPoseedor(object sender, MouseButtonEventArgs e)
        {
            claseIntercambio.adminTransitos = this;
            buscadorEmpresa buscador = new buscadorEmpresa(txtPoseedor, buscadorEmpresa.tipoBusqueda.POSEEDOR);
            bascula.wpf.controls.ChildWin control = new bascula.wpf.controls.ChildWin(this);

            control.Height = buscador.Height;
            control.Width = buscador.Width;
            control.GetControles().Children.Clear();
            control.GetControles().Children.Add(buscador);
            controles.Children.Add(control);
            
        }

        private void seleccionarProductor(object sender, MouseButtonEventArgs e)
        {
            buscadorEmpresa buscador = new buscadorEmpresa(txtProductor, buscadorEmpresa.tipoBusqueda.PRODUCTOR);
            bascula.wpf.controls.ChildWin control = new bascula.wpf.controls.ChildWin(this);

            control.Height = buscador.Height;
            control.Width = buscador.Width;
            control.GetControles().Children.Clear();
            control.GetControles().Children.Add(buscador);
            controles.Children.Add(control);
        }

        private void seleccionarPlantaTransferencia(object sender, MouseButtonEventArgs e)
        {
           buscadorEmpresa buscador = new buscadorEmpresa(txtPlantaTransferencia, buscadorEmpresa.tipoBusqueda.PLANTA_DE_TRANSFERENCIA);
           bascula.wpf.controls.ChildWin control = new bascula.wpf.controls.ChildWin(this);

           control.Height = buscador.Height;
           control.Width = buscador.Width;
           control.GetControles().Children.Clear();
           control.GetControles().Children.Add(buscador);
           controles.Children.Add(control);
        }

        private void seleccionarObra(object sender, MouseButtonEventArgs e)
        {

            if (claseIntercambio.transitoActual.empPoseedor == null)
            {
                MessageBox.Show("Escoja un poseedor");
            }
            else
            {
                claseIntercambio.adminTransitos = this;
                buscadorObras buscador = new buscadorObras(txtObra, claseIntercambio.transitoActual.empPoseedor.recnum);

                bascula.wpf.controls.ChildWin control = new bascula.wpf.controls.ChildWin(this);

                control.Height = buscador.Height;
                control.Width = buscador.Width;
                control.GetControles().Children.Clear();
                control.GetControles().Children.Add(buscador);
                controles.Children.Add(control);
            }
        }

        private void seleccionarPlantaValorizacion(object sender, MouseButtonEventArgs e)
        {
            buscadorEmpresa buscador = new buscadorEmpresa(txtPlantaValorizacion, buscadorEmpresa.tipoBusqueda.PLANTA_VALORIZACION);
            bascula.wpf.controls.ChildWin control = new bascula.wpf.controls.ChildWin(this);

            control.Height = buscador.Height;
            control.Width = buscador.Width;
            control.GetControles().Children.Clear();
            control.GetControles().Children.Add(buscador);
            controles.Children.Add(control);
        }

        private void brutoPierdeFoco(object sender, RoutedEventArgs e)
        {
            calcularNETO();
        }

        private void calcularNETO()
        {
            if (txtBruto.Text != "" && txtTara.Text != "")
            {
                long bruto = long.Parse(Convert.ToInt64(txtBruto.Text).ToString());
                long tara = long.Parse(Convert.ToInt64(txtTara.Text).ToString());

                long neto = bruto - tara;

              /*  if (neto < 0)
                {
                    MessageBox.Show("Revise los pesos Bruto y Tara, el peso neto no puede ser negativo", "Revise", MessageBoxButton.OK, MessageBoxImage.Warning);
                }*/

                txtNeto.Text = neto.ToString();

                calcularDensidad();
                recalcularImporte();

            }

            claseIntercambio.transitoActual.bruto = txtBruto.Text;
            claseIntercambio.transitoActual.tara = txtTara.Text;
            claseIntercambio.transitoActual.neto = txtNeto.Text;
        }

        private void taraPierdeFoco(object sender, RoutedEventArgs e)
        {
            calcularNETO();
        }

        private void txtMatricula_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                //evaluar si la matricula tiene una empresa asociada
                Empresa emptransportista = logic.gestionVehiculos.getEmpresaAsociada(txtMatricula.Text);

                claseIntercambio.transitoActual.matricula = txtMatricula.Text;

                if (emptransportista == null)
                {
                    MessageBoxResult resultado = MessageBox.Show("La matricula no tiene una empresa asociada, ¿Desea asociarla ahora?", "Matricula no asociada", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
                    if (resultado == MessageBoxResult.Yes)
                    {
                        //se desea asociar la matricula a una empresa
                        
                       buscadorEmpresa buscador= new buscadorEmpresa(new TextBox(), buscadorEmpresa.tipoBusqueda.TRANSPORTISTA);
                       bascula.wpf.controls.ChildWin control = new bascula.wpf.controls.ChildWin(this);

                       control.Height = buscador.Height;
                       control.Width = buscador.Width;
                       control.GetControles().Children.Clear();
                       control.GetControles().Children.Add(buscador);
                       controles.Children.Add(control);
                    }
                }
                else
                {
                    //empresa asociada correctamente
                    claseIntercambio.transitoActual.EmpTransportista = emptransportista;
                    txtMatricula.Background = Brushes.DarkSeaGreen;
                }
                cmbTipoVehiculo.Focus();
            }
        }

        private void guardarActual(object sender, RoutedEventArgs e)
        {
            guardarAlbaran(false);
            
             
        }

        public void guardarAlbaran(bool silencioso)
        {            

            if (txtNumAlbaran.Text.Replace(" ","").ToString()!= "")
            {
                calcularNETO();
                recalcularResiduo();

                TransitosActuale actual = claseIntercambio.toTransitoDB();

                if (txtFechaSalida.Text == null || txtFechaSalida.Text.Replace(" ", "") == "")
                {
                    //solo guardarlo en temporal
                    logic.gestionTransitosActuales.mergeOrCreate(actual);

                   // claseIntercambio.msg("Se han guardado los datos con exito.", "Guardado", MessageBoxButton.OK, MessageBoxImage.Information);
                   if(silencioso==false) limpiarDeDatos();

                }
                else
                {
                    //hay que darlo de salida ya (guardarlo como albaran finalizado)
                    String comprobacion = claseIntercambio.comprobarSiDatosRellenados();
                    if (comprobacion == "")
                    {
                        if (gestionarCobro.cobradoCorrectamente())
                        {
                            claseIntercambio.transitoActual.fechaSalida = DateTime.Today.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
                            //guardar al historico de albaranes
                            HistoricoAlbarane albaran = claseIntercambio.toAlbaranHistorico();
                            logic.gestionHistorialAlbaranes.guardarAHistorial(albaran);

                            //imprimir
                            frontend.wpf.Informes.visorInformes visor =new frontend.wpf.Informes.visorInformes();                           
                            visor.Show();

                            visor.Closed += (objeto, rtorno) => 
                            {
                                limpiarDeDatos();
                                bloquearControles(true);
                            };                         
                            
                        }
                    }
                    else claseIntercambio.msg(comprobacion, "Informacion", MessageBoxButton.OK, MessageBoxImage.Information);

                }
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            guardarAlbaran(false);
            //ver ventana transitos actuales
            new VehiculosTransito().Show();
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            txtFechaSalida.Text = DateTime.Today.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
                
        }

        private void imgPoseedor_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {

        }

        private void addNewObra(object sender, RoutedEventArgs e)
        {
            
            bascula.wpf.controls.ChildWin control = new bascula.wpf.controls.ChildWin(this);

            if (claseIntercambio.transitoActual.empPoseedor != null)
            {
                Frame frame = new Frame();
                frame.Margin = new Thickness(5);
                frame.Navigate(new Obras.adminObras(claseIntercambio.transitoActual.empPoseedor.recnum));

                control.Width = 980;
                control.Height = 600;
                control.GetControles().Children.Add(frame);
                controles.Children.Add(control);
            }
        }

        private void txtNumAlbaran_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                //el usuario desea que el contador actual no cuente, hay que restar uno al contador
                String resultado = logic.gestionConfiguracionApp.restarUnAlbaran(txtNumAlbaran.Text);
                if (resultado == "")
                {
                    //todo correcto
                    txtNumAlbaran.Background = Brushes.DarkBlue;
                    txtNumAlbaran.Foreground = Brushes.White;
                    claseIntercambio.transitoActual.numAlbaran = txtNumAlbaran.Text;
                }
                else
                {
                    
                    claseIntercambio.msg(resultado, "Fallo", MessageBoxButton.OK, MessageBoxImage.Information);
                    txtNumAlbaran.Text = claseIntercambio.transitoActual.numAlbaran;
                }
                
            }
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            guardarAlbaran(false);
        }
    }
}
