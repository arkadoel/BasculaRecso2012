using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using project.recso.bascula.data;


using Excel = Microsoft.Office.Interop.Excel;
using System.Windows;
using System.Windows.Threading;

namespace project.recso.bascula.frontend.wpf.Informes
{
    public class ExportarMilena
    {

        public static String dirDestino { get; set; }  //ruta del directorio donde saldra el resultado
        private String excelFinal ;
        private List<HistoricoAlbarane> historico;
        private Informes.Proceso ventana;

        private class COLUMNAS
        {
            public const int FILA_INICIO=2;
            public const int NUM_ALBARAN = 1;
            public const int FECHA = 2;
            public const int APERTURA = 3;
            public const int CIERRE = 4;
            public const int ENTRADA_SALIDA = 5;
            public const int RESIDUO = 6;
            public const int POSEEDOR = 7;
            public const int BRUTO = 8;
            public const int TARA = 9;
            public const int NETO = 10;
            public const int PRECIO = 11;
            public const int IMPORTE_SIN_IVA = 12;
            public const int IMPORTE_FINAL = 13;
            public const int FORMA_PAGO = 14;
            public const int METROS_CUBICOS = 15;
            public const int PESO_METROS_CUBICOS = 16;
            public const int TIPO_VEHICULO = 17;
            public const int MATRICULA = 18;
            public const int PRODUCTOR = 19;
            public const int NOMBRE_OBRA = 20;
            public const int MILENA_CLIENTE = 21;
            public const int MILENA_OBRA = 22;
            public const int MILENA_PRODUCTO = 23;
        }

        public ExportarMilena(String _directorio, List<HistoricoAlbarane> _historico)
        {
            dirDestino = _directorio;

            String mes = "";
            if (DateTime.Now.Month < 10) mes = "0" + DateTime.Now.Month.ToString();
            else mes = DateTime.Now.Month.ToString();

            excelFinal = dirDestino + @"\" + DateTime.Today.Day.ToString() + mes + DateTime.Today.Year.ToString()+ ".xlsx";
            historico = _historico;
            ventana = new Proceso(_directorio);
            ventana.Terminado(false);
            ventana.Show();
            escribirExcel();
        }

        #region "Gestion de procesos"
        public void eliminarProcesos()
        {
            Process[] proc = Process.GetProcesses();
            List<Process> procesos = proc.ToList<Process>();

            var excels = from ou in procesos
                         where ou.ProcessName.ToLower().Contains("excel")
                         select ou;

            foreach (var pro in excels)
            {
                pro.Kill();
            }

        }
        #endregion




        public void escribirExcel()
        {
            ventana.msg("Eliminando procesos...");
            ventana.progresoAl(1);
            eliminarProcesos();

            System.IO.File.Copy(System.IO.Directory.GetCurrentDirectory().ToString() + @"\Informes\Plantillas\Consultamensual.xlsx", excelFinal, true);


            object m = Type.Missing;

            Excel.Application xlapp = new Excel.Application();
            Excel.Workbook xlwbook = xlapp.Workbooks.Open(excelFinal);
            Excel._Worksheet xlwsheet = xlwbook.Sheets[1];  //cada hoja del documento
            Excel.Range xlrango = xlwsheet.UsedRange;

            int filaActual = COLUMNAS.FILA_INICIO+1;
            int progreso = 1;
            int maximo = historico.Count;
            ventana.setRango(maximo + 1);

            foreach(HistoricoAlbarane albaran in historico)
            {
                ventana.msg("Procesando albaran (" + progreso.ToString() + " de " + maximo.ToString() + ") \r\n");
                ventana.progresoAl(progreso);

                try
                {

                    xlrango.Cells[COLUMNAS.NUM_ALBARAN][filaActual].Value2 = albaran.numAlbaran;
                    xlrango.Cells[COLUMNAS.BRUTO][filaActual].Value2 = albaran.bruto;
                    xlrango.Cells[COLUMNAS.TARA][filaActual].Value2 = albaran.tara;
                    xlrango.Cells[COLUMNAS.NETO][filaActual].Value2 = albaran.neto;
                    xlrango.Cells[COLUMNAS.IMPORTE_SIN_IVA][filaActual].Value = albaran.importeSinIVA.Replace(",", ".");
                    xlrango.Cells[COLUMNAS.IMPORTE_FINAL][filaActual].Value = albaran.importeFinal.Replace(",", ".");

                    if (albaran.fechaEntrada != null)
                    {
                        DateTime fentrada = DateTime.Parse(albaran.fechaEntrada);
                        xlrango.Cells[COLUMNAS.APERTURA][filaActual].Value = fentrada.Hour + ":" + fentrada.Minute;
                    }

                    DateTime fsalida = DateTime.Parse(albaran.fechaSalida);
                    xlrango.Cells[COLUMNAS.FECHA][filaActual].Value2 = fsalida.Month.ToString() + "/" + fsalida.Day.ToString() + "/" + fsalida.Year.ToString();
                    xlrango.Cells[COLUMNAS.CIERRE][filaActual].Value = fsalida.Hour + ":" + fsalida.Minute;

                    xlrango.Cells[COLUMNAS.ENTRADA_SALIDA][filaActual].Value2 = albaran.tipoResiduo;
                    xlrango.Cells[COLUMNAS.POSEEDOR][filaActual].Value2 = albaran.empPoseedor;
                    xlrango.Cells[COLUMNAS.RESIDUO][filaActual].Value2 = albaran.residuo;
                    xlrango.Cells[COLUMNAS.PRECIO][filaActual].Value = albaran.precioResiduo.Replace(",", ".");
                    xlrango.Cells[COLUMNAS.METROS_CUBICOS][filaActual].Value2 = albaran.metrosCubicos.ToString().Replace("Tm/m3", " ");
                    xlrango.Cells[COLUMNAS.PESO_METROS_CUBICOS][filaActual].Value2 = albaran.PesoMCubicos.ToString().Replace("Tm/m3", " ");
                    xlrango.Cells[COLUMNAS.TIPO_VEHICULO][filaActual].Value2 = albaran.TipoVehiculo;
                    xlrango.Cells[COLUMNAS.MATRICULA][filaActual].Value2 = albaran.matricula;
                    xlrango.Cells[COLUMNAS.PRODUCTOR][filaActual].Value2 = albaran.empProductor;
                    xlrango.Cells[COLUMNAS.NOMBRE_OBRA][filaActual].Value2 = albaran.obra;
                    xlrango.Cells[COLUMNAS.MILENA_CLIENTE][filaActual].Value2 = albaran.empClienteMilena;
                    xlrango.Cells[COLUMNAS.MILENA_OBRA][filaActual].Value2 = albaran.milenaObra;
                    xlrango.Cells[COLUMNAS.MILENA_PRODUCTO][filaActual].Value2 = albaran.milenaProducto;
                    xlrango.Cells[COLUMNAS.FORMA_PAGO][filaActual].Value2 = albaran.FormaPago;

                }
                catch (Exception ex)
                {

                }

                filaActual++;
                progreso++;
                DoEvents();

            }

            xlwbook.Save();
            xlwbook.Close(true, m, m);
            xlapp.Quit();

            ventana.msg("Finalizo la exportacion.");
            ventana.Terminado(true);

            //eliminar el fichero de copia de seguridad si se genera
            string elbackup = excelFinal.Replace(dirDestino + @"\", dirDestino + @"\" + "Copia de seguridad de ");
            elbackup = elbackup.Replace(".xlsx", ".xlk");
            if (System.IO.File.Exists(elbackup)) System.IO.File.Delete(elbackup);

        }


        public static void DoEvents()
        {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Background,
                                                  new Action(delegate { }));
        }

    }
}
