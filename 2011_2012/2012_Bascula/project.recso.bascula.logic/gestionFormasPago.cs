using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using project.recso.bascula.data;

namespace project.recso.bascula.logic
{
    public class gestionFormasPago
    {
        public class Formas{
            public const String PREPAGO = "PREPAGO";
            public const String PENDIENTE_DE_PAGO = "PENDIENTE DE PAGO";
            public const String EFECTIVO = "EFECTIVO";
        }

        public class resultadoCobro{
            public const String COBRO_CORRECTO = "CORRECTO";
            public const String FALLO_COBRO = "FALLO";
            public String mensaje;
            public String resultado;

            public resultadoCobro()
            {
                resultado = resultadoCobro.COBRO_CORRECTO;
            }
        }

        public static resultadoCobro cobrar(double _dinero, long _recnumEmpresa, string _numAlbaran)
        {
            resultadoCobro resultado = new resultadoCobro();

            recso2011DBEntities gestor = claseIntercambio.getGestor();
            //se recupera la forma de pago de la empresa
            String tipoPago = (from er in gestor.Empresas
                              where er.recnum == _recnumEmpresa
                              select er).First<Empresa>().tipoDePago;

            if (tipoPago.Contains(Formas.EFECTIVO) || tipoPago.Contains(Formas.PENDIENTE_DE_PAGO))
            {
                    //no hacer nada, todo correcto
                resultado.resultado = resultadoCobro.COBRO_CORRECTO;
                return resultado;
            }
            else {
                
                    var registros = (from f in gestor.FormasPagoes
                                              where f.recnumEmpresa == _recnumEmpresa
                                              select f);

                    if (registros.Count() > 0)
                    {
                        FormasPago pago = registros.First<FormasPago>();
                       
                        //registrar en el log 
                            logFormasPago tolog = new logFormasPago();
                            tolog.id = claseIntercambio.getIdByFecha();
                            tolog.fechaModificacion = DateTime.Now;
                            tolog.recnumEmpresa = _recnumEmpresa;
                            tolog.anteriorSaldo = pago.saldoActual;

                        if (tipoPago.Contains(Formas.PREPAGO))
                        {
                            /*
                             * Si la cantidad de saldo en el sistema es mayor de lo que se 
                             * quiere cobrar, se realiza la transaccion, sino se avisa de que no queda
                             * suficiente saldo.
                             * 
                             ** Mecanismo tarjeta prepago telefonica **
                             */

                            pago.saldoActual =pago.saldoActual - _dinero;
                            gestor.SaveChanges(System.Data.Objects.SaveOptions.AcceptAllChangesAfterSave);

                            tolog.importeAlbaran = _dinero;
                            tolog.tipoModificacion = "ALBARAN " + _numAlbaran ;
                            tolog.saldo = pago.saldoActual;
                            gestionLogFormaPago.registrarCambio(tolog);  

                            if (pago.saldoActual > _dinero)
                            {
                               // pago.saldoActual -= _dinero;
                                //gestor.SaveChanges(System.Data.Objects.SaveOptions.AcceptAllChangesAfterSave);
                            }
                            else
                            {
                                resultado.resultado = resultadoCobro.FALLO_COBRO;
                                resultado.mensaje = "La empresa pagadora no tiene saldo suficiente";
                    
                            }
                        }
                        else if (tipoPago == Formas.PENDIENTE_DE_PAGO)
                        {
                            /* Si el limiente de saldo es superior a la suma de la cantidad gastada
                             * hasta el momento (saldo Actual) mas el dinero que se pretende cobrar,
                             * la transaccion sera llevada a cabo.
                             * 
                             * En caso contrario se notifica que no se cobra porque se ha superado el 
                             * techo de gasto para dicha empresa.
                             * 
                             ** Mecanismo deuda estatal **
                             */
                          
                            /*if (pago.limiteSaldo > (pago.saldoActual + _dinero))
                            {
                                pago.saldoActual += _dinero;
                                gestor.SaveChanges(System.Data.Objects.SaveOptions.AcceptAllChangesAfterSave);
                            }
                            else
                            {
                                resultado.resultado = resultadoCobro.FALLO_COBRO;
                                resultado.mensaje = "La empresa pagadora superaria su techo de gasto permitido, por tanto no se cobrara.";
                            }*/

                        }
                    }
                    else
                    {
                        resultado.resultado = resultadoCobro.FALLO_COBRO;
                        resultado.mensaje = "No existen registros monetarios para la empresa pagadora.";
                    }
            }


            return resultado;
        }

        public static void guardarFormaPago(FormasPago _forma)
        {
            logFormasPago tolog = new logFormasPago();
            recso2011DBEntities gestor = claseIntercambio.getGestor();
             var registros = (from f in gestor.FormasPagoes
                              where f.recnumEmpresa == _forma.recnumEmpresa
                              select f);

             if (registros.Count() > 0)
             {
                 FormasPago forma = registros.First<FormasPago>();
                 //SE REGISTRA EL ANTERIOR SALDO
                 tolog.anteriorSaldo = forma.saldoActual;

                 forma.limiteSaldo = _forma.limiteSaldo;
                 forma.saldoActual = _forma.saldoActual; //NUEVO SALDO
                 forma.recnumEmpresa = _forma.recnumEmpresa;
                 forma.ultimaModificacion = _forma.ultimaModificacion;

                 //registrar en el log 

                 tolog.id = claseIntercambio.getIdByFecha();
                 tolog.fechaModificacion = DateTime.Now;
                 tolog.recnumEmpresa = _forma.recnumEmpresa;
                 tolog.saldo = _forma.saldoActual;
                 tolog.importeAlbaran = 0;
                 tolog.tipoModificacion = "MANUAL";

                 gestionLogFormaPago.registrarCambio(tolog); 
             }
             else
             {
                 gestor.AddToFormasPagoes(_forma);
             }

             gestor.SaveChanges(System.Data.Objects.SaveOptions.AcceptAllChangesAfterSave);
             
             }

        public static FormasPago getFormaPagoEmpresa(long _recnum)
        {
            recso2011DBEntities gestor = claseIntercambio.getGestor();
            var registros = (from f in gestor.FormasPagoes
                              where f.recnumEmpresa == _recnum
                              select f);

            if (registros.Count() > 0)
            {
                return registros.First<FormasPago>();
            }
            else return null;
        }
    }
}
