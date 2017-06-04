using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace preinstall
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Presintalacion del programa");
            if (Directory.Exists(@"C:\tabby") == false)
            {
                Console.WriteLine("Creando directorio tabby");
                Directory.CreateDirectory(@"C:\tabby");

            }
            if (Directory.Exists(@"C:\tabby\Calculadora") == false)
            {
                Console.WriteLine("Creando directorio calculadora");
                Directory.CreateDirectory(@"C:\tabby\Calculadora");

            }



        }
    }
}
