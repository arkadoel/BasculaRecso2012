using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using project.recso.bascula.data;


namespace project.recso.bascula.logic
{
    public class gestionConfiguracionApp
    {
        /// <summary>
        /// 
        /// Devuelve el numero correspondiente al ultimo albaran
        /// </summary>
        /// <returns></returns>
        public static long getUltimoAlbaran()
        {
            //el maximo es de 18 digitos 999.999.999.999.999.998
            recso2011DBEntities gestor = claseIntercambio.getGestor();
            return gestor.ConfiguracionApps.First<ConfiguracionApp>().ultimoAlbaran;
        }

        public static void setUltimoAlbaran(long _ultimoAlbaran)
        {
            recso2011DBEntities gestor = claseIntercambio.getGestor();
            var n = gestor.ConfiguracionApps.First<ConfiguracionApp>();
            long numero = n.ultimoAlbaran;
            numero = _ultimoAlbaran;
            n.ultimoAlbaran = numero;

            gestor.SaveChanges(System.Data.Objects.SaveOptions.AcceptAllChangesAfterSave);
        }

        public static void sumarUnAlbaran()
        {
            recso2011DBEntities gestor = claseIntercambio.getGestor();
            var n = gestor.ConfiguracionApps.First<ConfiguracionApp>();
            long numero = n.ultimoAlbaran;
            numero += 1;
            n.ultimoAlbaran = numero;

            gestor.SaveChanges(System.Data.Objects.SaveOptions.AcceptAllChangesAfterSave);
       
        }

        public static string restarUnAlbaran(String nuevo)
        {
            recso2011DBEntities gestor = claseIntercambio.getGestor();
            var n = gestor.ConfiguracionApps.First<ConfiguracionApp>();            
            long numero = n.ultimoAlbaran;
            numero -= 1;


            int cuantos = (from t in gestor.HistoricoAlbaranes
                               where t.numAlbaran == nuevo
                               select t).Count();
            if (cuantos > 0)
            {
                return "El numero de albaran ya esta en uso, elija otro.";
            }
            else
            {
                n.ultimoAlbaran = numero;

                gestor.SaveChanges(System.Data.Objects.SaveOptions.AcceptAllChangesAfterSave);
                return "";
            }

        }
    }
}
