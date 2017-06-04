using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Xml.Linq;

namespace tabby.tpm.core
{
    public class installPkg
    {
        private Comando comando;
        private string tmpDir = @".\tmp";

        public installPkg(Comando _comando)
        {
            comando = _comando;

            string rutaPaquete=comando.Args[1].ToString();

            //eliminar y crear tmpDir
            Console.WriteLine("Desempaquetando directorio temporal...");
            if (Directory.Exists(tmpDir) == true)
            {
                Directory.Delete(tmpDir, true);
            }

            //crear temp dir
            Directory.CreateDirectory(tmpDir);

            //Descomprimir paquete
            ZipManager zip = new ZipManager();
            zip.ExtractZipFile(rutaPaquete, tmpDir);
            

            //ejecutar preInstall si existe
            Console.WriteLine("Ejecutando preinstalacion...");
            string preInstall =tmpDir + @"\CONTROL\preInstall";
            if (File.Exists(preInstall + ".exe") == true)
            {
                Process pInst = new Process();
                pInst.StartInfo.FileName = preInstall + ".exe";
                // pInst.StartInfo.Arguments = "";
                pInst.StartInfo.UseShellExecute = false;
                pInst.StartInfo.RedirectStandardOutput = true;
                pInst.StartInfo.CreateNoWindow = true;
                pInst.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

                pInst.Start();
               /* while (!pInst.HasExited)
                {
                    Console.Write(".");
                    // wait one second
                    Thread.Sleep(1000);
                }*/
                Console.WriteLine(pInst.StandardOutput.ReadToEnd());
                pInst.WaitForExit();
                pInst.Close();
                pInst.Dispose();

                //Leer ruta de instalacion
                Console.WriteLine("Leyendo ruta de destino...");
                string installDir="";
                XDocument doc = XDocument.Load(tmpDir + @"\CONTROL\info.xml") ;
                var paths = from c in doc.Elements("package")
                                 select c.Element("installDir").Value;
                if (paths.Count() > 0) installDir = paths.First();

                //copiar release a directorio de salida
                Console.WriteLine("Copiando archivos de instalacion...");
                if (Directory.GetFiles(installDir, "*.*", SearchOption.AllDirectories).Count() > 0)
                {
                    Directory.Delete(installDir, true);
                }

                tabby.util.IO.CopyAll(tmpDir + @"\RELEASE", installDir);
                Console.WriteLine("Instalacion terminada");

                //comprimir el paquete a la carpeta de paquetes instalados
                string dirInstalados = tabby.util.IO.getCurrentDirectory() + @"\installed";
                Console.WriteLine("Pasando paquete a 'paquetes instalados'...");
                FileInfo paquete = new FileInfo(rutaPaquete);

                if (Directory.Exists(dirInstalados) == false) Directory.CreateDirectory(dirInstalados);
                string[] stokenizer = paquete.Name.Split(' ');
                string nombreApp =  


                System.IO.File.Copy(rutaPaquete, dirInstalados + @"\" + paquete.Name);

            }


        }


    }
}
