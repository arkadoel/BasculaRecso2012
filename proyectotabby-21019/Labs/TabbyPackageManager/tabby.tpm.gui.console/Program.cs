using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace tabby.tpm.gui.console
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Count() > 0)
            {
                string paramFirst = args.First();

                switch (paramFirst)
                {
                    case "install-pkg":
                        tpm.core.Comando cmd = new core.Comando();
                        cmd.Orden = "install-pkg";
                        cmd.Args = args.ToList<string>();
                        new tpm.core.installPkg(cmd);
                        break;
                    case "--help":
                        printHelp();
                        break;
                    default:
                        Console.WriteLine("Comando no reconocido\r\n\r\n\r\n");
                        printHelp();
                        break;
                }
            }
            else printHelp();

            Console.WriteLine("PULSE ENTER PARA CONTINUAR...");
            Console.Read();
        }

        static void printHelp()
        {            
            StreamReader fich = new StreamReader(@".\help.txt");
            while (fich.Peek() != -1)
            {
                Console.WriteLine(fich.ReadLine());
            }

            fich.Close();
            Console.WriteLine("");
        }
    }
}
