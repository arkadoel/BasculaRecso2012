using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using project.recso.bascula.data;

namespace project.recso.bascula.logic
{
    public class gestionVehiculos
    {
        /// <summary>
        /// Lista los vehiculos que posee una empresa
        /// </summary>
        /// <param name="_recnum">Id de la empresa</param>
        /// <returns>List<Vehiculo></returns>
        public static List<Vehiculo> listarVehiculosEmpresa(long _recnum)
        {
            recso2011DBEntities gestor = claseIntercambio.getGestor();
            List<Vehiculo> listado = new List<Vehiculo>();

            listado = (from v in gestor.Vehiculos
                      where v.recnumEmpresa == _recnum && v.fechaBaja == ""
                      select v).ToList<Vehiculo>();

            return listado;
        }

        /// <summary>
        /// Crea o actualiza un registro de tipo vehiculo en la DB
        /// </summary>
        /// <param name="_veh"></param>
        /// <returns></returns>
        public static Boolean mergeOrCreate(Vehiculo _veh)
        {
            if (_veh.matriculaVehiculo != null )
            {
                if (_veh.fechaAlta != "IGNORAR")
                {
                recso2011DBEntities gestor = claseIntercambio.getGestor();
                int cont = (from e in gestor.Vehiculos
                            where e.matriculaVehiculo == _veh.matriculaVehiculo
                            select e).Count();

                if (cont > 0)
                {
                    //existe, toca modificar
                    Vehiculo veh = (from e in gestor.Vehiculos
                                    where e.matriculaVehiculo == _veh.matriculaVehiculo
                                    select e).First<Vehiculo>();

                    veh.matriculaVehiculo = _veh.matriculaVehiculo;
                    veh.recnumEmpresa = _veh.recnumEmpresa;
                    
                        veh.fechaAlta = _veh.fechaAlta;
                                   
                    veh.fechaBaja = _veh.fechaBaja;
                }
                else
                {
                    if (_veh.fechaAlta != "IGNORAR")
                    {
                        _veh.fechaAlta = DateTime.Today.ToShortDateString();
                        gestor.AddToVehiculos(_veh);
                    }
                    
                }

                gestor.SaveChanges(System.Data.Objects.SaveOptions.AcceptAllChangesAfterSave);
            }
            }
            return true;
        }

        /// <summary>
        /// Deshabilitar vehiculo en la DB
        /// </summary>
        /// <param name="_recnum"></param>
        /// <returns></returns>
        public static Boolean eliminarVehiculo(long _recnum)
        {
            recso2011DBEntities gestor = claseIntercambio.getGestor();
            //obtenemos el tipo de vehiculo
            var tipos = (from e in gestor.Vehiculos
                         where e.recnum == _recnum
                         select e);
            if (tipos.Count() > 0)
            {
                Vehiculo veh = tipos.First<Vehiculo>();
                veh.fechaBaja = DateTime.Today.ToShortDateString();

                gestionVehiculos.mergeOrCreate(veh);
            }
            else
            {   //no existe ningun vehiculo con ese numero
                return false;
            }
            return true;
        }

        /// <summary>
        /// Obtiene los datos de la empresa asociada a una matricula
        /// determinada, en caso contrario devuelve NULL
        /// </summary>
        /// <param name="_matricula">String con la matricula</param>
        /// <returns>Objeto Empresa</returns>
        public static Empresa getEmpresaAsociada(String _matricula)
        {
            recso2011DBEntities gestor = claseIntercambio.getGestor();
            Empresa emp = null;
            _matricula = _matricula.Replace(" ", "");

            var l=from e in gestor.Vehiculos
                               where e.matriculaVehiculo.Replace(" ","") == _matricula && e.fechaBaja==""
                               select e;

            if(l.Count() >0){
                long idEmpresa = l.First<Vehiculo>().recnumEmpresa.Value;
                emp = gestionEmpresas.getEmpresaByRecnum(idEmpresa);
            }   
            return emp;
        }
    }
}
