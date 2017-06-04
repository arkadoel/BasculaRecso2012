using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using project.recso.bascula.data;

namespace project.recso.bascula.logic
{
    public class gestionEmpresas
    {
        /// <summary>
        /// Guarda los datos o crea nueva empresa en la base de datos
        /// </summary>
        /// <param name="_emp"></param>
        /// <returns></returns>
        public static Boolean mergeOrCreate(Empresa _emp)
        {
            recso2011DBEntities gestor = claseIntercambio.getGestor();

            int cont = (from e in gestor.Empresas
                        where e.recnum == _emp.recnum
                        select e).Count();

            if (cont > 0)
            {
                Empresa emp = (from e in gestor.Empresas
                               where e.recnum == _emp.recnum
                               select e).First<Empresa>();

                emp.cif = _emp.cif;
                emp.codigoMilena = _emp.codigoMilena;
                emp.cuentaBancaria = _emp.cuentaBancaria;
                emp.direccion = _emp.direccion;
                emp.email = _emp.email;
                emp.localidad = _emp.localidad;
                emp.nombre = _emp.nombre;
                emp.provincia = _emp.provincia;
                emp.razonSocial = _emp.razonSocial;
                emp.telefono = _emp.telefono;
                emp.tipoDeEmpresa= _emp.tipoDeEmpresa;
                emp.tipoDePago = _emp.tipoDePago;
                emp.esmoroso = _emp.esmoroso;

            }
            else
            {
                gestor.AddToEmpresas(_emp);
            }
            gestor.SaveChanges(System.Data.Objects.SaveOptions.AcceptAllChangesAfterSave);
            return true;
        }

        public static Empresa getRECSO()
        {
            return (from r in claseIntercambio.getGestor().Empresas
                    where r.nombre.Replace(" ", "").ToLower().Contains("recicladossostenibles")
                    select r).First();
        }

        /// <summary>
        /// Lista las empresas existentes
        /// </summary>
        /// <returns></returns>
        public static List<Empresa> listarEmpresas()
        {
            recso2011DBEntities gestor = claseIntercambio.getGestor();

            return gestor.Empresas.ToList<Empresa>();
        }

        /// <summary>
        /// Lista las empresas existentes
        /// </summary>
        /// <returns></returns>
        public static List<Empresa> listarEmpresas(String _filtro)
        {
            recso2011DBEntities gestor = claseIntercambio.getGestor();
            List<Empresa> lista = (from emp in gestor.Empresas
                                  where emp.nombre.Contains(_filtro) == true
                                  select emp).ToList<Empresa>();

            return lista;
        }
       

        /// <summary>
        /// Elimina los datos de una empresa
        /// </summary>
        /// <param name="recnum"></param>
        /// <returns></returns>
        public static Boolean eliminarEmpresa(long recnum)
        {
            recso2011DBEntities gestor = claseIntercambio.getGestor();
            Empresa emp = (from e in gestor.Empresas
                           where e.recnum == recnum
                           select e).First<Empresa>();

            if (emp != null)
            {
                gestor.Empresas.DeleteObject(emp);
                gestor.SaveChanges(System.Data.Objects.SaveOptions.AcceptAllChangesAfterSave);
            }
            else return false;
            return true;
        }

        /// <summary>
        /// devuelve los datos de una empresa
        /// </summary>
        /// <param name="recnum"></param>
        /// <returns></returns>
        public static Empresa getEmpresaByRecnum(long recnum)
        {
            recso2011DBEntities gestor = claseIntercambio.getGestor();
            Empresa emp = null;

            var empresas = from e in gestor.Empresas
                           where e.recnum == recnum
                           select e;

            if(empresas.Count()>0){
                emp = empresas.First<Empresa>();
            }
            

            if (emp != null)
            {
                return emp;
            }
            else return null;
        }

        /// <summary>
        /// Se asocia una obra a la empresa actual
        /// </summary>
        /// <param name="_asoc"></param>
        public static void asociarEmpresaAObra(EmpresaEnObra _asoc)
        {
            recso2011DBEntities gestor = claseIntercambio.getGestor();

            gestor.AddToEmpresaEnObras(_asoc);
            gestor.SaveChanges(System.Data.Objects.SaveOptions.AcceptAllChangesAfterSave);
        }
    }
}
