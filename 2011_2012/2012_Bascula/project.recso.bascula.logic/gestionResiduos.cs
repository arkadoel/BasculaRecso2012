using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using project.recso.bascula.data;

namespace project.recso.bascula.logic
{
    public class gestionResiduos
    {

        public static class __TIPO_RESIDUO__
        {
            public static String ENTRADA = "ENTRADA";
            public static String SALIDA = "SALIDA";
            public static String TARIFA_PLANA = "TARIFA PLANA";
            public static String TODOS = "TODOS";
        }

        /// <summary>
        /// Actualiza o crea un nuevo residuo en la base de datos
        /// </summary>
        /// <param name="_res"></param>
        /// <returns>true || false</returns>
        public static Boolean mergeOrCreate(Residuo _res)
        {
            
                recso2011DBEntities gestor = claseIntercambio.getGestor();

                int con = (from e in gestor.Residuos
                           where e.recnum == _res.recnum
                           select e).Count();

                if (con > 0)
                { //actualizar
                    Residuo residuo = (from e in gestor.Residuos
                                       where e.recnum == _res.recnum
                                       select e).First();

                    residuo.nombre = _res.nombre;
                    residuo.precio = _res.precio;
                    residuo.ivaAplicado = _res.ivaAplicado;
                    residuo.descripcion = _res.descripcion;
                    residuo.codigoLER = _res.codigoLER;
                    residuo.tipoMaterial = _res.tipoMaterial;
                    residuo.milena = _res.milena;
                }
                else
                { //crear nuevo
                    gestor.AddToResiduos(_res);
                }
                gestor.SaveChanges(System.Data.Objects.SaveOptions.AcceptAllChangesAfterSave);
                return true;               
        }

        /// <summary>
        /// Obtiene un residuo a partir de su numero de registro
        /// </summary>
        /// <param name="_recnum"></param>
        /// <returns>Residuo || null</returns>
        public static Residuo getResiduoByRecnum(Int32 _recnum) {

            recso2011DBEntities gestor = claseIntercambio.getGestor();
            Residuo residuo = null;
            var lista = from e in gestor.Residuos
                       where e.recnum == _recnum
                       select e;
            if (lista.Count() > 0)
            {
                residuo = lista.First<Residuo>();
            }
            else residuo = new Residuo();

            return residuo;
        }

        /// <summary>
        /// Elimina un residuo a partir del numero de registro
        /// </summary>
        /// <param name="_recnum"></param>
        /// <returns>true || false</returns>
        public static Boolean eliminarResiduo(Int32 _recnum)
        {
            recso2011DBEntities gestor = claseIntercambio.getGestor();
            //obtenemos el residuo
            var residuos = (from e in gestor.Residuos
                           where e.recnum == _recnum
                           select e);
            if (residuos.Count() > 0)
            {
                var residuo = residuos.First();
                gestor.DeleteObject(residuo);
                gestor.SaveChanges(System.Data.Objects.SaveOptions.AcceptAllChangesAfterSave);
            }
            else
            {   //no existe ningun residuo con ese numero
                return false;
            }

            return true;
        }

        /// <summary>
        /// Devuelve la lista completa de residuos
        /// </summary>
        /// <returns>List</returns>
        public static List<Residuo> getListaResiduos()
        {
            return getListaResiduos("");
        }

        /// <summary>
        /// Devuelve la lista de residuos dependiendo del tipo de residuo
        /// </summary>
        /// <param name="tipo">ENTRADA, SALIDA o TODOS</param>
        /// <returns></returns>
        public static List<Residuo> getListaResiduos(String tipo)
        {
            recso2011DBEntities gestor = claseIntercambio.getGestor();

            List<Residuo> listado = null;

            if (tipo == __TIPO_RESIDUO__.ENTRADA)
            {
                listado = (from o in gestor.Residuos
                           where o.tipoMaterial == __TIPO_RESIDUO__.ENTRADA
                           select o).ToList<Residuo>();
            }
            else if (tipo == __TIPO_RESIDUO__.SALIDA)
            {
                listado = (from o in gestor.Residuos
                           where o.tipoMaterial == __TIPO_RESIDUO__.SALIDA
                           select o).ToList<Residuo>();
            }
            else if (tipo == __TIPO_RESIDUO__.TARIFA_PLANA)
            {
                listado = (from o in gestor.Residuos
                           where o.tipoMaterial.Contains(__TIPO_RESIDUO__.TARIFA_PLANA)
                           select o).ToList<Residuo>();
            }
            else
            {   //todos
                listado = gestor.Residuos.ToList<Residuo>();
            }
            return listado;
        }
    }
}
