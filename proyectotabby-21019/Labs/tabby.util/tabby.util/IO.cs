using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace tabby.util
{
    public class IO
    {
        /// <summary>
        /// Devuelve la ruta de ejecucion actual
        /// </summary>
        /// <returns>string</returns>
        public static string getCurrentDirectory()
        {
            return System.IO.Directory.GetCurrentDirectory().ToString();
        }

        public static void CopyAll(string SourcePath, string DestinationPath)
        {
            //Now Create all of the directories
            foreach (string dirPath in Directory.GetDirectories(SourcePath, "*.*", SearchOption.AllDirectories))
                Directory.CreateDirectory(dirPath.Replace(SourcePath, DestinationPath));

            //Copy all the files
            foreach (string newPath in Directory.GetFiles(SourcePath, "*.*", SearchOption.AllDirectories))
                File.Copy(newPath, newPath.Replace(SourcePath, DestinationPath));
        }
    }
}
