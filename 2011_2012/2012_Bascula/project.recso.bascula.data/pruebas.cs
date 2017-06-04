using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace project.recso.bascula.data
{
    class pruebas
    {
        public void prueba()
        {
            recso2011DBEntities gestor = new recso2011DBEntities();
            var lista = from usuario in gestor.Usuarios
                        select usuario;
        }
    }
}
