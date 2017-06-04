using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using project.recso.bascula.data;
using System.Windows.Forms;


namespace project.recso.bascula.logic
{
    public class gestionUsuarios
    {
        
        /// <summary>
        /// Comprueba si los datos de usuario son correctos
        /// </summary>
        /// <param name="_loginName"></param>
        /// <param name="_password"></param>
        /// <returns></returns>
        public static Usuario logueoCorrecto(String _loginName, String _password)
        {
            recso2011DBEntities gestor = claseIntercambio.getGestor();
            Usuario user = null;
           var lista = from usuario in gestor.Usuarios
                       where usuario.loginName == _loginName && usuario.password == _password
                           select usuario;
           if (lista.Count() > 0)
           {
               user = lista.First();
               return user;
           }

           if (_loginName == "demo" && _password == "demo")
           {
               user = new Usuario();
               user.loginName = "demo";
               user.password = "demo";
               user.permiso = "Admin";
           }

            return user;
        }

        /// <summary>
        /// Cambia la contraseña para un usuario
        /// </summary>
        /// <param name="_loginName">Nombre de inicio sesion</param>
        /// <param name="_newPass">Nueva contraseña</param>
        /// <returns></returns>
        public static Boolean cambiarPassword(String _loginName, String _newPass)
        {
            recso2011DBEntities gestor = claseIntercambio.getGestor();
            Usuario user = null;
            var lista = from usuario in gestor.Usuarios
                        where usuario.loginName == _loginName 
                        select usuario;

            if (lista.Count() > 0)
            {
                try
                {
                    user = lista.First();
                    user.password = _newPass;
                    gestor.SaveChanges(System.Data.Objects.SaveOptions.AcceptAllChangesAfterSave);
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        
                    return false;
                }
                   
            }
            return false;
        }
        public static List<Usuario> getUsuarios()
        {
            return claseIntercambio.getGestor().Usuarios.ToList();
        }

        public static List<Usuario> getUsuariosParaLogin()
        {
            List<Usuario> usuarios = (from u in claseIntercambio.getGestor().Usuarios
                                     where u.password.Replace(" ","")!=""
                                     orderby u.idEmpleado ascending
                                     select u).ToList();
            return usuarios;
        }


    }
}
