using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace project.recso.bascula.data
{
    public static class SQL
    {
        /// <summary>
        /// @recnumEmpresa
        /// </summary>
        public static String obrasDeUnaEmpresa = "select * from Obras where Obras.recnum in(select recnumObra from dbo.EmpresaEnObras where EmpresaEnObras.recnumEmpresa =@recnumEmpresa);";
        public static string CobrosPorEmpresa = "SELECT fechaModificacion, anteriorSaldo, importeAlbaran, saldo, tipoModificacion FROM [recso2011DB].[dbo].[logFormasPago] where recnumEmpresa=@recnumEmpresa order by fechaModificacion;";


        public static class SQLFiles
        {
            public const string ESTADISTICAS_CUADROrESUMEN = "generarEstadisticos.sql";
            public const string EMPRESAS_X_TONELADAS = "empresasXtoneladas.sql";
            public const string EMPRESAS_X_IMPORTE = "EMPRESAS_X_IMPORTE.sql";
            public const string OBRAS_X_IMPORTE = "OBRAS_IMPORTE.sql";
        }

        /// <summary>
        /// Retorna una consulta guardada en un archivo sql
        /// </summary>
        /// <param name="_ruta"></param>
        /// <returns></returns>
        public static String loadQueryFromFile(String _ruta)
        {
            string fichero = System.IO.Directory.GetCurrentDirectory().ToString() + @".\consultas\" + _ruta;
            string texto = "";

            if (System.IO.File.Exists(fichero) == true)
            {
                StreamReader lector = new StreamReader(fichero);

                texto = lector.ReadToEnd();

                lector.Close();
            }
            return texto;
        }
    }
}
