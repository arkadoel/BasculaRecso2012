using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using project.recso.bascula.data;
using System.Threading;

namespace project.recso.bascula.logic
{
    public static class gestionObras
    {
        /// <summary>
        /// Crea o actualiza una obra en la base de datos
        /// </summary>
        /// <param name="_obra"></param>
        /// <returns></returns>
        public static Boolean mergeOrCreate(Obra _obra)
        {
            recso2011DBEntities gestor = claseIntercambio.getGestor();

            int con = (from e in gestor.Obras
                       where e.recnum == _obra.recnum
                       select e).Count();

            if (con > 0)
            { //actualizar
                Obra obra = (from e in gestor.Obras
                                   where e.recnum == _obra.recnum
                                   select e).First();
               
                obra.licenciaMunicipal = _obra.licenciaMunicipal;
                obra.denominacion = _obra.denominacion;
                obra.localidad = _obra.localidad;
                obra.provincia = _obra.provincia;
                obra.codigoMilena = _obra.codigoMilena;
                obra.finicioObra = _obra.finicioObra;
                obra.ffinObra = _obra.ffinObra;

                                   
                
            }
            else
            { //crear nuevo
                //añadir un nuevo codigo de milena por ser obra nueva
                _obra.codigoMilena = generarCodigoMilena();
                if(_obra.finicioObra==null)
                    {
                        _obra.finicioObra = DateTime.Today.ToShortDateString();
                    }

                //guardar la nueva obra
                gestor.AddToObras(_obra);
            }
            gestor.SaveChanges(System.Data.Objects.SaveOptions.AcceptAllChangesAfterSave);
            return true;  
        }

        public static int generarCodigoMilena()
        {
            int? codigo = 0;
            recso2011DBEntities gestor = claseIntercambio.getGestor();
            codigo = (from ou in gestor.Obras
                      orderby ou.codigoMilena descending
                      select ou).First<Obra>().codigoMilena;
            codigo++;
            return codigo.Value;
        }
        
        /// <summary>
        /// Se marca la fecha de fin para una obra
        /// </summary>
        /// <param name="_idObra"></param>
        /// <returns></returns>
        public static Boolean finDeObraPara(long _idObra)
        {
            recso2011DBEntities gestor = claseIntercambio.getGestor();
            Obra obra = (from e in gestor.Obras
                         where e.recnum == _idObra
                         select e).First();

            obra.ffinObra = DateTime.Today.ToShortDateString();
            gestor.SaveChanges(System.Data.Objects.SaveOptions.AcceptAllChangesAfterSave);
            return true;
        }

        /// <summary>
        /// Lista todas la obras de la base de datos
        /// </summary>
        /// <returns></returns>
        public static List<Obra> listarObras()
        {
            //DateTime hoy = DateTime.Today;

            var lista = from ob in claseIntercambio.getGestor().Obras
                        where ob.ffinObra == null 
                        select ob;
            return lista.ToList<Obra>();
        }

        /// <summary>
        /// Lista las obras filtrando el nombre por el string pasado
        /// </summary>
        /// <param name="_filtro"></param>
        /// <returns></returns>
        public static List<Obra> listarObras(String _filtro)
        {
            List<Obra> obras = listarObras();

            /*List<Obra> result = (from o in obras
                                 where o.denominacion.ToUpper().Contains(_filtro.ToUpper())
                                 select o
                                     ).ToList<Obra>();*/

            List<Obra> listado = new List<Obra>();

            foreach (Obra obra in obras)
            {
                string s1 = claseIntercambio.paraFiltros(obra.denominacion);
                string s2 = claseIntercambio.paraFiltros(_filtro);

                if (s1.Contains(s2))
                {
                    listado.Add(obra);
                }
            }
            return listado;
        }

        /// <summary>
        /// Filtra las obras para una determinada empresa
        /// </summary>
        /// <param name="_idEmpresa"></param>
        /// <returns></returns>
        public static List<Obra> listarObrasDeEmpresa(long _idEmpresa)
        {
            String sql = data.SQL.obrasDeUnaEmpresa.Replace("@recnumEmpresa", _idEmpresa.ToString());

            var obras = claseIntercambio.getGestor().ExecuteStoreQuery<Obra>(sql).ToList<Obra>();
             
            /*
            var lista = from o in claseIntercambio.getGestor().EmpresaEnObras
                                where o.recnumEmpresa == _idEmpresa
                                         select new{
                                             recnumObra = o.recnumObra
                                         };
            List<Obra> obras = new List<Obra>();
            List<Obra> obrasfinal = new List<Obra>();

            foreach(var elem in lista){
                obras.Add(gestionObras.getObraByRecnum(elem.recnumObra));
            }

             obrasfinal = (from t in obras
                         orderby t.denominacion
                         select t).ToList();            
           
            return obrasfinal;*/
            return obras;
        }

        public class obraReducida
        {
            public String denominacion { get; set; }
            public long recnum { get; set; }
            public obraReducida(string dem, long num)
            {
                denominacion = dem;
                recnum = num;
            }
        }

        /// <summary>
        /// Filtra las obras para una determinada empresa
        /// </summary>
        /// <param name="_idEmpresa"></param>
        /// <returns></returns>
        public static List<obraReducida> listarObrasDeEmpresaFormatoReducido(long _idEmpresa)
        {

            var obra2 = listarObrasDeEmpresa(_idEmpresa);
            List<obraReducida> obras = new List<obraReducida>();

            foreach (Obra obra in obra2)
            {
                obras.Add(new obraReducida(obra.denominacion, obra.recnum));
            }
            

            /*
            var lista = from o in claseIntercambio.getGestor().EmpresaEnObras
                        where o.recnumEmpresa == _idEmpresa
                        select new
                        {
                            recnumObra = o.recnumObra
                        };
            
            obraReducida nuevaObra = null;

            foreach (var elem in lista)
            {
                var ou = (from t in claseIntercambio.getGestor().Obras
                      where t.recnum == elem.recnumObra
                      select new
                      {
                          denominacion = t.denominacion,
                          recnum = t.recnum
                      });
                nuevaObra = new obraReducida(ou.First().denominacion, ou.First().recnum);

                obras.Add(nuevaObra);
            }*/

            return obras;
        }

        /// <summary>
        /// Devuelve una obra encontrada mediante su numero de obra
        /// </summary>
        /// <param name="_idObra"></param>
        /// <returns></returns>
        public static Obra getObraByRecnum(long _idObra)
        {
            Obra obra = null;
            var obras = from e in claseIntercambio.getGestor().Obras
                         where e.recnum == _idObra
                         select e;

            if (obras.Count() > 0)
            {
                obra = obras.First();
            }
            else obra = new Obra();

            return obra;
        }

        /// <summary>
        /// Filtra las obras para una determinada empresa filtrando
        /// </summary>
        /// <param name="_idEmpresa"></param>
        /// <returns></returns>
        public static List<Obra> listarObrasDeEmpresa(long _idEmpresa, String _filtro)
        {
            List<Obra> lista = (from o in gestionObras.listarObrasDeEmpresa( _idEmpresa)
                                    where o.denominacion.Contains(_filtro)
                                    select o).ToList<Obra>();
            return lista;
        }

        public static void eliminarObraDe(Obra _obra, long _idEmpresa)
        {
            recso2011DBEntities gestor = claseIntercambio.getGestor();
            List<Obra> obrasActuales = gestionObras.listarObrasDeEmpresa(_idEmpresa);
            EmpresaEnObra emp = null;

            emp = (from t in gestor.EmpresaEnObras
                   where t.recnumObra == _obra.recnum && t.recnumEmpresa == _idEmpresa
                   select t).First();
            gestor.EmpresaEnObras.DeleteObject(emp);
            gestor.SaveChanges(System.Data.Objects.SaveOptions.AcceptAllChangesAfterSave);
        }

        /// <summary>
        /// Guarda el listado de obras para una empresa en la base de datos
        /// </summary>
        /// <param name="_listado"></param>
        /// <param name="_idEmpresa"></param>
        public static void guardarListadoObrasEmpresa(List<Obra> _listado, long _idEmpresa)
        {
            recso2011DBEntities gestor =  claseIntercambio.getGestor();
            List<Obra> obrasActuales = gestionObras.listarObrasDeEmpresa(_idEmpresa);
            EmpresaEnObra emp = null;


            //borrar las obras
            foreach (Obra obra in obrasActuales)
            {

               var empes = (from t in gestor.EmpresaEnObras
                           where t.recnumObra == obra.recnum && t.recnumEmpresa == _idEmpresa
                           select t);
                foreach(var borrar in empes)
                gestor.EmpresaEnObras.DeleteObject(borrar);

                gestor.SaveChanges(System.Data.Objects.SaveOptions.AcceptAllChangesAfterSave);
            
            }

            //agregar las obras como nuevas
            foreach (Obra obra in _listado)
            {
                emp = new EmpresaEnObra();
                emp.recnumObra = obra.recnum;
                emp.recnumEmpresa = _idEmpresa;

                //FECHA INICIO NO PUEDE SER NULL
                if (obra.finicioObra == null)
                {
                    obra.finicioObra = DateTime.Today.ToShortDateString();
                }
               
                emp.fechaInicio = obra.finicioObra;
                if (obra.ffinObra != null) emp.fechaFin = obra.ffinObra;
                else emp.fechaFin = "";

                gestor.AddToEmpresaEnObras(emp);
                gestor.SaveChanges(System.Data.Objects.SaveOptions.AcceptAllChangesAfterSave);
            
            }


        }

        
    }
}
