using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using project.recso.bascula.data;

namespace project.recso.bascula.logic
{
    public class gestionLogFormaPago
    {
        public static void registrarCambio(logFormasPago _pago)
        {
            recso2011DBEntities gestor = claseIntercambio.getGestor();
            gestor.AddTologFormasPagoes(_pago);
            gestor.SaveChanges(System.Data.Objects.SaveOptions.AcceptAllChangesAfterSave);
        }

        public static List<logFormasPago> getMovimientosByEmpresa(long _recnum)
        {
            recso2011DBEntities gestor = claseIntercambio.getGestor();
            List<logFormasPago> l = new List<logFormasPago>();

            var movimientos = from u in gestor.logFormasPagoes
                              where u.recnumEmpresa == _recnum
                              orderby u.fechaModificacion ascending
                              select u;

            if (movimientos.Count() > 0) l = movimientos.ToList<logFormasPago>();

            return l;
        }
    }
}
