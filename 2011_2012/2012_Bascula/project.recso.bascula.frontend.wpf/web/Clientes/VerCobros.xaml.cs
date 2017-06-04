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

namespace project.recso.bascula.frontend.wpf.web.Clientes
{
    /// <summary>
    /// Interaction logic for VerCobros.xaml
    /// </summary>
    public partial class VerCobros : UserControl
    {
        long recnumEmpresa;
        public VerCobros(long _recnumEmpresa)
        {
            InitializeComponent();
            recnumEmpresa = _recnumEmpresa;
            this.Loaded += new RoutedEventHandler(VerCobros_Loaded);
        }

        class filaTabla{
            public String fechaModificacion { get; set; }
            public double anteriorSaldo { get; set; }
            public double importeAlbaran { get; set; }
            public double nuevoSaldo { get; set; }
            public string tipoModificacion { get; set; }
        }

        void VerCobros_Loaded(object sender, RoutedEventArgs e)
        {
            List<logFormasPago> listado = logic.gestionLogFormaPago.getMovimientosByEmpresa(recnumEmpresa);
            List<filaTabla> filas = new List<filaTabla>();

            foreach (logFormasPago mov in listado)
            {
                filaTabla fila = new filaTabla();
                DateTime fe = mov.fechaModificacion.Value;
                fila.fechaModificacion = (fe.Day+ "/" + fe.Month + "/" +fe.Year + " "  + fe.Hour + ":" + fe.Minute + ":" + fe.Second).ToString();
                fila.anteriorSaldo = mov.anteriorSaldo.Value;
                fila.importeAlbaran = mov.importeAlbaran.Value;
                fila.nuevoSaldo = mov.saldo.Value;
                fila.tipoModificacion = mov.tipoModificacion;
                filas.Add(fila);
            }

            tabla.ItemsSource = filas;

            if (filas.Count > 0)
            {

                tabla.Columns[0].Header = "Fecha de modificación";
                tabla.Columns[1].Header = "Saldo anterior";
                tabla.Columns[2].Header = "Importe del albarán";
                tabla.Columns[3].Header = "Nuevo saldo";
                tabla.Columns[4].Header = "Tipo de modificación";
                tabla.IsReadOnly = true;
            }
        }
    }
}
