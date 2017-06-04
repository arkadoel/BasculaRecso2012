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
using System.Diagnostics;
using System.Windows.Threading;
using System.Threading;

namespace tabby.tabbyTerminal
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        #region "Gestion de ventana"
        private void fondoVentana_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void btnCerrar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void boton_MouseLeave(object sender, MouseEventArgs e)
        {
            Label boton = e.Source as Label;
            boton.Opacity = 0.3;
        }

        private void boton_MouseEnter(object sender, MouseEventArgs e)
        {
            Label boton = e.Source as Label;
            boton.Opacity = 1;
        }

        private void restaurarVentana(object sender, MouseButtonEventArgs e)
        {
            if (this.WindowState == System.Windows.WindowState.Maximized)
            {
                this.WindowState = System.Windows.WindowState.Normal;
            }
            else this.WindowState = System.Windows.WindowState.Maximized;
        }

        private void minimizarVentana(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = System.Windows.WindowState.Minimized;

        }

        #endregion

        private void txtComando_KeyDown(object sender, KeyEventArgs e)
        {
            
            
        }

        private void txtComando_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string linea = txtComando.GetLineText(0);
                cmd.Text += linea + "\r\n";
                cmd.ScrollToEnd();
                txtComando.Text = "";
                ejecutarComando(linea);
            }
            else if (e.Key == Key.Up)
            {
                if (txtComando.CanUndo == true) txtComando.Undo();
            }
            else if (e.Key == Key.Down)
            {
                if (txtComando.CanRedo == true) txtComando.Redo();
            }
           
        }

        private void ejecutarComando(string _comando)
        {
            string comando = "";
            string parametros = "";
            if (_comando.IndexOf(' ') > -1)
            {
                comando = _comando.Substring(0, _comando.IndexOf(' ') + 1);
                parametros = _comando.Remove(0, _comando.IndexOf(' ') + 1);
            }
            else comando = _comando;
             
            
            Process pInst = new Process();
            pInst.StartInfo.FileName = comando;
            pInst.StartInfo.Arguments = parametros;
            pInst.StartInfo.UseShellExecute = false;
            pInst.StartInfo.RedirectStandardOutput = true;
            pInst.StartInfo.RedirectStandardError = true;
            pInst.StartInfo.CreateNoWindow = true;
            pInst.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

            pInst.OutputDataReceived+=new DataReceivedEventHandler(pInst_OutputDataReceived);
            try
            {
                
                pInst.Start();
                pInst.BeginOutputReadLine();

                while (!pInst.HasExited)
                {
                    this.Dispatcher.Invoke(DispatcherPriority.Render, EmptyDelegate);
                    MainWindow.DoEvents();
                        
                }

                cmd.Text += "\r\n=>\r\n";
            }
            catch (Exception ex)
            {
                cmd.Text += ex.Message + "\r\n";
            }
            
             
            //MessageBox.Show(parametros);
        }

        void pInst_OutputDataReceived(object sender, DataReceivedEventArgs outLine)
        {
            if (!String.IsNullOrEmpty(outLine.Data))
            {
                if (!cmd.Dispatcher.CheckAccess())
                {
                    // Called from a none ui thread, so use dispatcher
                    ShowLoggingDelegate showLoggingDelegate = new ShowLoggingDelegate(ShowLogging);
                    cmd.Dispatcher.Invoke(DispatcherPriority.Normal, showLoggingDelegate, outLine.Data);
                }
                else
                {
                    // Called from UI trhead so just update the textbox
                    ShowLogging(outLine.Data);
                };
            }
        }

        private delegate void ShowLoggingDelegate(string text);
        private static Action EmptyDelegate = delegate() { };

        /// <summary>
        /// Show the logging on screen
        /// </summary>
        /// <param name="text"></param>
        private void ShowLogging(string text)
        {
            this.cmd.Text +=text + "\r\n";
            this.cmd.ScrollToEnd();
        }

        public static void DoEvents()
        {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Background,
                                                  new Action(delegate { }));
        }
    }
}
