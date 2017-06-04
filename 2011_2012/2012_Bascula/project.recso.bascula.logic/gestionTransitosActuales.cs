using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using project.recso.bascula.data;

namespace project.recso.bascula.logic
{
    public class gestionTransitosActuales
    {

        public static void mergeOrCreate(TransitosActuale _transito)
        {
            recso2011DBEntities gestor = claseIntercambio.getGestor();
            var recuento = from t in gestor.TransitosActuales
                           where t.numAlbaran == _transito.numAlbaran
                           select t;
            if (recuento.Count() > 0)
            {
                TransitosActuale transito = recuento.First<TransitosActuale>();
                transito.bruto = _transito.bruto;
                transito.fechaEntrada = _transito.fechaEntrada;
                transito.fechaSalida = _transito.fechaSalida;
                transito.importeFinal = _transito.importeFinal;
                transito.importeIVA = _transito.importeIVA;
                transito.importeSinIva = _transito.importeSinIva;
                transito.IVAaplicado = _transito.IVAaplicado;
                transito.matricula = _transito.matricula;
                transito.neto = _transito.neto;
                transito.numAlbaran = _transito.numAlbaran;
                transito.obra = _transito.obra;
                transito.plantaTransferencia = _transito.plantaTransferencia;
                transito.plantaValorizacion = _transito.plantaValorizacion;
                transito.poseedor = _transito.poseedor;
                transito.productor = _transito.productor;
                transito.residuo = _transito.residuo;
                transito.tara = _transito.tara;
                transito.tipoVehiculo = _transito.tipoVehiculo;
                transito.transportista = _transito.transportista;

            }
            else
            {
                gestor.AddToTransitosActuales(_transito);
            }

            gestor.SaveChanges(System.Data.Objects.SaveOptions.AcceptAllChangesAfterSave);
       
        }

        public static List<TransitosActuale> listarTransitos()
        {
            return claseIntercambio.getGestor().TransitosActuales.ToList<TransitosActuale>();
        }

        public static void eliminarTransitoActual(string _numeroAlbaran)
        {
            recso2011DBEntities gestor = claseIntercambio.getGestor();
            var transitorios = from t in gestor.TransitosActuales
                            where t.numAlbaran == _numeroAlbaran
                           select t;

            if (transitorios.Count() > 0)
            {
                var transito = transitorios.First<TransitosActuale>();

                gestor.TransitosActuales.DeleteObject(transito);
                gestor.SaveChanges(System.Data.Objects.SaveOptions.AcceptAllChangesAfterSave);
            }
            
        }

    }
}
