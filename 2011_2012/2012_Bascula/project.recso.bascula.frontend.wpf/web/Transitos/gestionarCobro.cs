using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using project.recso.bascula.logic;

namespace project.recso.bascula.frontend.wpf.web.Transitos
{
    public class gestionarCobro
    {
        public static  Boolean cobradoCorrectamente()
        {
            try
            {
                double dinero = Convert.ToDouble(claseIntercambio.transitoActual.importeFinal);
                long recnumPAgador = claseIntercambio.transitoActual.empPoseedor.recnum;

                logic.gestionFormasPago.resultadoCobro resultado = logic.gestionFormasPago.cobrar(dinero, recnumPAgador, claseIntercambio.transitoActual.numAlbaran);

                if (resultado.resultado == logic.gestionFormasPago.resultadoCobro.FALLO_COBRO)
                {
                    claseIntercambio.msg(resultado.mensaje, "Mensaje.", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                    return true;
                }
            }
            catch (Exception ex)
            {

            }

            return true;
        }
    }
}
