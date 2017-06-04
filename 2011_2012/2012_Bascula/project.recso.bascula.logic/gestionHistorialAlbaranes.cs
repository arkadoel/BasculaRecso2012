using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using project.recso.bascula.data;

namespace project.recso.bascula.logic
{
    public class gestionHistorialAlbaranes
    {
        /// <summary>
        /// Guarda los datos de un albaran al historial de albaranes
        /// </summary>
        /// <param name="_albaran"></param>
        /// <returns></returns>
        public static Boolean guardarAHistorial(HistoricoAlbarane _albaran)
        {
            recso2011DBEntities gestor = claseIntercambio.getGestor();
            int cont = (from e in gestor.HistoricoAlbaranes
                        where e.numAlbaran == _albaran.numAlbaran
                        select e).Count();

            if (cont > 0)
            {
                HistoricoAlbarane albaran = (from e in gestor.HistoricoAlbaranes
                               where e.numAlbaran == _albaran.numAlbaran
                               select e).First<HistoricoAlbarane>();

                /*
                 * si se quiere actualizar poner el codigo aqui
                 */
                

            }
            else
            {
                _albaran.id = DateTime.Today.Year.ToString() + DateTime.Today.Month.ToString() + DateTime.Today.Day.ToString() + "_" + DateTime.Now.ToShortTimeString().Replace(":", "") + "_" + _albaran.numAlbaran;

                gestor.AddToHistoricoAlbaranes(_albaran);
            }
            gestor.SaveChanges(System.Data.Objects.SaveOptions.AcceptAllChangesAfterSave);
            return true;
        }

        /// <summary>
        /// Devuelve un albaran guardado en el historico de albaranes
        /// </summary>
        /// <param name="_numero"></param>
        /// <returns></returns>
        public static HistoricoAlbarane getUnAlbaran(String _numero)
        {
            recso2011DBEntities gestor = claseIntercambio.getGestor();
            var elem = (from e in gestor.HistoricoAlbaranes
                        where e.numAlbaran == _numero
                        select e);

            if (elem.Count() > 0)
            {
                HistoricoAlbarane albaran = (elem).First<HistoricoAlbarane>();

                /* Se devuelve los datos del albaran archivado*/
                return albaran;

            }
            return null;
        }


        public static List<HistoricoAlbarane> getAlbaranesEntreFechas(String _inicio, String _fin)
        {
            recso2011DBEntities gestor = claseIntercambio.getGestor();

            var elem = (from e in gestor.HistoricoAlbaranes
                        
                        select e);

            if (elem.Count() > 0)
            {
                List<HistoricoAlbarane> resultado = new List<HistoricoAlbarane>();
                if (_inicio != "" && _fin != "")
                {
                    DateTime inicio = DateTime.Parse(_inicio);
                    DateTime fin = DateTime.Parse(_fin.Replace("0:00:00", "23:59:59").ToString());

                   // DateTime dbIni;
                    DateTime dbFin;

                    foreach (HistoricoAlbarane albaran in elem)
                    {
                        //dbIni = DateTime.Parse(albaran.fechaEntrada);
                        dbFin = DateTime.Parse(albaran.fechaSalida);

                        if (dbFin >= inicio && dbFin <= fin)
                        {
                            //esta entre el rango
                            resultado.Add(albaran);
                        }
                    }
                }
                /* Se devuelve los datos del albaran archivado*/
                return resultado;

            }
            return null;
        }
    }
}
