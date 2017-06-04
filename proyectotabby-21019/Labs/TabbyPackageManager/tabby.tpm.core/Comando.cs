using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tabby.tpm.core
{
    public class Comando
    {
        /// <summary>
        /// Nombre de la orden
        /// </summary>
        public String Orden { get; set; }
        /// <summary>
        /// Lista de argumentos del comando
        /// </summary>
        public List<string> Args { get; set; }

    }
}
