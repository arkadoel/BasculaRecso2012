using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using project.recso.bascula.data;

namespace project.recso.bascula.logic
{
    public class gestionTipoVehiculo
    {
        /// <summary>
        /// Devuelve la lista con los tipos de vehiculos registrados
        /// </summary>
        /// <returns></returns>
        public static List<TipoVehiculo> getTiposDeVehiculos()
        {
            recso2011DBEntities gestor = claseIntercambio.getGestor();
            return gestor.TipoVehiculoes.ToList<TipoVehiculo>();
        }

        /// <summary>
        /// Modifica o crea el tipo de vehiculo en la base de datos
        /// </summary>
        /// <param name="_tip"></param>
        /// <returns></returns>
        public static Boolean mergeOrCreate(TipoVehiculo _tip)
        {
            recso2011DBEntities gestor = claseIntercambio.getGestor();

            int cont = (from e in gestor.TipoVehiculoes
                        where e.recnum == _tip.recnum
                        select e).Count();

            if (cont > 0)
            {
                //existe, toca modificar
                TipoVehiculo tipo = (from e in gestor.TipoVehiculoes
                                     where e.recnum == _tip.recnum
                                     select e).First<TipoVehiculo>();

                tipo.nombre = _tip.nombre;
                tipo.capacidad = _tip.capacidad;
            }
            else
            {
                gestor.AddToTipoVehiculoes(_tip);
            }
            gestor.SaveChanges(System.Data.Objects.SaveOptions.AcceptAllChangesAfterSave);
            return true;
        }

        /// <summary>
        /// Elimina un tipo de vehiculo dependidneo del numero de registro
        /// </summary>
        /// <param name="_recnum"></param>
        /// <returns></returns>
        public static Boolean eliminarTipoDeVehiculo(Int32 _recnum)
        {
            recso2011DBEntities gestor = claseIntercambio.getGestor();
            //obtenemos el tipo de vehiculo
            var tipos = (from e in gestor.TipoVehiculoes
                            where e.recnum == _recnum
                            select e);
            if (tipos.Count() > 0)
            {
                var tipo = tipos.First();
                gestor.DeleteObject(tipo);
                gestor.SaveChanges(System.Data.Objects.SaveOptions.AcceptAllChangesAfterSave);
            }
            else
            {   //no existe ningun tipo de vehiculo con ese numero
                return false;
            }
            return true;
        }

        /// <summary>
        /// Recupera un tipo de vehiculo dependiendo del id pasado
        /// </summary>
        /// <param name="_recnum"></param>
        /// <returns></returns>
        public static TipoVehiculo getTipoVehiculoByRecnum(int _recnum)
        {
            recso2011DBEntities gestor = claseIntercambio.getGestor();
            return (from t in gestor.TipoVehiculoes
                    where t.recnum == _recnum
                    select t).First<TipoVehiculo>();
        }
    }
}
