using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tabby.tpm.core
{
    public class ZipManager
    {
        public void CreateZipFile(List<string> items, string destination)
        {
            using (Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile())
            {
                // Loop through all the items
                foreach (string item in items)
                {
                    // If the item is a file
                    if (System.IO.File.Exists(item))
                    {
                        // Add the file in the root folder inside our zip file
                        zip.AddFile(item, "");
                    }
                    // if the item is a folder    
                    else if (System.IO.Directory.Exists(item))
                    {
                        // Add the folder in our zip file with the folder name as its name
                        zip.AddDirectory(item, new System.IO.DirectoryInfo(item).Name);
                    }
                    

                }
                // Finally save the zip file to the destination we want
                zip.Save(destination);
            }
        }

        public void ExtractZipFile(string zipFileLocation, string destination)
        {
            using (Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile(zipFileLocation))
            {
                zip.ExtractAll(destination);
            }
        }
        
    }
}
