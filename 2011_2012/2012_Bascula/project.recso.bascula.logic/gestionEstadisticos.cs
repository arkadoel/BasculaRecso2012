using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using project.recso.bascula.data;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;


using Excel = Microsoft.Office.Interop.Excel;

namespace project.recso.bascula.logic
{
    public class gestionEstadisticos
    {

        private class COLUMNAS
        {
            public const int FILA_INICIO = 2;
            public const int FILA_INICIO_porTONELADAS = 21;
            public const int FILA_INICIO_porIMPORTE = 29;
            public const int FILA_INICIO_OBRASporTONELADAS = 36;

            public const int CODIGO_LER = 1;
            public const int TIPOS_RESIDUOS = 2;
            public const int TONELADAS = 5;
            public const int PRECIO = 6;
            public const int IMPORTE = 7;            
        }

        private class COLUMNAS_OTRAS_TABLAS
        {
            public const int NOMBRE = 2;
            public const int CANTIDAD = 3;
        }

        #region "Manejo Fechas"

        /// <summary>
        /// Obtiene la fecha del lunes
        /// </summary>
        /// <returns></returns>
        public static DateTime getFechaLunes()
        {
            string diaSemana = DateTime.Today.DayOfWeek.ToString();
            int resta = 0;

            if (diaSemana == "Monday") resta = 0;
            else if (diaSemana == "Tuesday") resta = -1;
            else if (diaSemana == "Wednesday") resta = -2;
            else if (diaSemana == "Thursday") resta = -3;
            else if (diaSemana == "Friday") resta = -4;
            else if (diaSemana == "Saturday") resta = -5;
            else if (diaSemana == "Sunday") resta = -6;

            return DateTime.Today.AddDays(resta);
        }
        public static void exportarEstadisticoDeHoy(String _fichero)
        {
            exportarTopTenResiduosEntreFechas(_fichero, DateTime.Today, DateTime.Today);
        }

        public static void exportarEstadisticoEstaSemana(String _fichero)
        {            
            exportarTopTenResiduosEntreFechas(_fichero, getFechaLunes(), getFechaLunes().AddDays(+6));
        }

        public static void exportarEstadisticoEsteMes(String _fichero)
        {
            DateTime inicio = DateTime.Parse( "1/"+ DateTime.Today.Month+ "/" + DateTime.Today.Year);
            DateTime fin = DateTime.Parse( DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month).ToString() + "/" + DateTime.Today.Month+ "/" + DateTime.Today.Year);
            exportarTopTenResiduosEntreFechas(_fichero,inicio, fin);
        }

        public static void exportarEstadisticoEsteAnyo(String _fichero)
        {
            DateTime inicio = DateTime.Parse("1/01/" + DateTime.Today.Year);
            DateTime fin = DateTime.Parse(DateTime.DaysInMonth(DateTime.Today.Year, 12).ToString() + "/12/" + DateTime.Today.Year);
            exportarTopTenResiduosEntreFechas(_fichero, inicio, fin);
        }

        #endregion
        #region "Gestion de procesos"
        public static void eliminarProcesos()
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

        public static void exportarTopTenResiduosEntreFechas(String dirDestino, DateTime _finicio, DateTime _ffin)
        {            
            string Manyana= _finicio.AddDays(1).ToString("yyyy-dd-MM");
            string ayer = _ffin.AddDays(-1).ToString("yyyy-dd-MM");

            string consulta = data.SQL.loadQueryFromFile( SQL.SQLFiles.ESTADISTICAS_CUADROrESUMEN);
            string selectPorToneladas = data.SQL.loadQueryFromFile(SQL.SQLFiles.EMPRESAS_X_TONELADAS);
            string selectPorImporte = data.SQL.loadQueryFromFile(SQL.SQLFiles.EMPRESAS_X_IMPORTE);
            string selectObrasPorImporte = data.SQL.loadQueryFromFile(SQL.SQLFiles.OBRAS_X_IMPORTE);

            //se sustituyen @finicio y @ffin por las fechas correspondientes
            consulta = consulta.Replace("@finicio", Manyana);
            consulta = consulta.Replace("@ffin", ayer);

            selectPorToneladas = selectPorToneladas.Replace("@finicio", Manyana);
            selectPorToneladas = selectPorToneladas.Replace("@ffin", ayer);

            selectPorImporte = selectPorImporte.Replace("@finicio", Manyana);
            selectPorImporte = selectPorImporte.Replace("@ffin", ayer);

            selectObrasPorImporte = selectObrasPorImporte.Replace("@finicio", Manyana);
            selectObrasPorImporte = selectObrasPorImporte.Replace("@ffin", ayer);


            using(var con = new SqlConnection(@"data source=.\SQLEXPRESS;initial catalog=recso2011DB;integrated security=True;")){
                //carga de datos
                DataSet ds= new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(consulta, con);
                da.Fill(ds, "Resumen");

                var listado = from t in ds.Tables["Resumen"].AsEnumerable()
                              select new
                              {
                                  Residuo = t.Field<String>("Residuo"),
                                  toneladas = t.Field<Int32>("toneladas"),
                                  precioResiduo= t.Field<Double>("precio"),
                                  sumaImporte = t.Field<Double>("sumaImporte")
                              };

                da = new SqlDataAdapter(selectPorToneladas, con);
                da.Fill(ds, "empresasToneladas");

                var listaEmpToneladas = from t in ds.Tables["empresasToneladas"].AsEnumerable()
                                        select new
                                        {
                                            empresa = t.Field<String>("empPoseedor"),
                                            toneladas = t.Field<Int32>("sumaNeto")
                                        };

                da = new SqlDataAdapter(selectPorImporte, con);
                da.Fill(ds, "empresasImporte");

                var listaEmpImporte = from t in ds.Tables["empresasImporte"].AsEnumerable()
                                        select new
                                        {
                                            empresa = t.Field<String>("empPoseedor"),
                                            importes = t.Field<Double>("sumaImporte")
                                        };

                da = new SqlDataAdapter(selectObrasPorImporte, con);
                da.Fill(ds, "obrasImporte");

                var listaObraImporte = from t in ds.Tables["obrasImporte"].AsEnumerable()
                                      select new
                                      {
                                          obra = t.Field<String>("obra"),
                                          importes = t.Field<Double>("sumaImporte")
                                      };

                //######### exportacion #################
                eliminarProcesos();

                String excelFinal = dirDestino + @"\Estadistico_" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + "__" + DateTime.Now.ToShortTimeString().Replace(":", "") + ".xlsx";

                System.IO.File.Copy(System.IO.Directory.GetCurrentDirectory().ToString() + @"\Informes\Plantillas\EstadisticosPlanta.xlsx", excelFinal, true);
                
                object m = Type.Missing;

                Excel.Application xlapp = new Excel.Application();
                Excel.Workbook xlwbook = xlapp.Workbooks.Open(excelFinal);
                Excel._Worksheet xlwsheet = xlwbook.Sheets[1];  //cada hoja del documento
                Excel.Range xlrango = xlwsheet.UsedRange;

                //PONER LA PRIMERA TABLA
                int filaActual = COLUMNAS.FILA_INICIO + 1;

                foreach (var registro in listado)
                {
                    
                    int numero = 0;
                    string s = registro.Residuo.Substring(0,1);
                    bool result = int.TryParse(s, out numero); //numero now = 0
                    if (result == true)
                    {
                        //tiene codigo ler
                        String codigoLER = registro.Residuo.Substring(0, 8).ToString();
                        int largo = registro.Residuo.Length;
                        string nombreResiduo = registro.Residuo.ToString().Remove(0, 9).ToString();

                        xlrango.Cells[COLUMNAS.TIPOS_RESIDUOS][filaActual].Value2 = nombreResiduo;
                        xlrango.Cells[COLUMNAS.CODIGO_LER][filaActual].Value2 = codigoLER;
                    }
                    else xlrango.Cells[COLUMNAS.TIPOS_RESIDUOS][filaActual].Value2 = registro.Residuo;
                    xlrango.Cells[COLUMNAS.PRECIO][filaActual].Value = registro.precioResiduo;
                    xlrango.Cells[COLUMNAS.TONELADAS][filaActual].Value = registro.toneladas;
                    xlrango.Cells[COLUMNAS.IMPORTE][filaActual].Value2 = registro.sumaImporte;
                    filaActual++;
                }

                xlwbook.Save();

                //PONER LA SEGUNDA TABLA
                filaActual = COLUMNAS.FILA_INICIO_porTONELADAS;
                foreach (var registro in listaEmpToneladas)
                {
                    xlrango.Cells[COLUMNAS_OTRAS_TABLAS.NOMBRE][filaActual].Value = registro.empresa;
                    xlrango.Cells[COLUMNAS_OTRAS_TABLAS.CANTIDAD][filaActual].Value = registro.toneladas;
                    filaActual++;
                }
                xlwbook.Save();

                //PONER LA TERCERA TABLA
                filaActual = COLUMNAS.FILA_INICIO_porIMPORTE;
                foreach (var registro in listaEmpImporte)
                {
                    xlrango.Cells[COLUMNAS_OTRAS_TABLAS.NOMBRE][filaActual].Value = registro.empresa;
                    xlrango.Cells[COLUMNAS_OTRAS_TABLAS.CANTIDAD][filaActual].Value = registro.importes;
                    filaActual++;
                }
                xlwbook.Save();

                //PONER CUARTA TABLA

                filaActual = COLUMNAS.FILA_INICIO_OBRASporTONELADAS;
                foreach (var registro in listaObraImporte)
                {
                    xlrango.Cells[COLUMNAS_OTRAS_TABLAS.NOMBRE][filaActual].Value = registro.obra;
                    xlrango.Cells[COLUMNAS_OTRAS_TABLAS.CANTIDAD][filaActual].Value = registro.importes;
                    filaActual++;
                }
                xlwbook.Save();

                //CERRAR TODO
                xlwbook.Close(true, m, m);
                xlapp.Quit();

                //eliminar el fichero de copia de seguridad si se genera
                string elbackup = excelFinal.Replace(dirDestino + @"\", dirDestino + @"\" + "Copia de seguridad de ");
                elbackup = elbackup.Replace(".xlsx", ".xlk");
                if (System.IO.File.Exists(elbackup)) System.IO.File.Delete(elbackup);

                
                
            }
        }
    }
}
