using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Xps;
using System.IO.Packaging;
using System.Windows.Xps.Packaging;
using System.Xml;
using System.Xaml;
using System.IO;
using System.Windows.Markup;

namespace project.recso.bascula.logic
{
    public class generarXPS
    {
        XpsDocument fichero = null;
        XpsDocumentWriter xpsdocw = null;
        Grid hoja = null;

        public void Generacion(String _ruta, Grid _pagina)
        {
            try
            {
                hoja = _pagina;
                fichero = new XpsDocument(_ruta, System.IO.FileAccess.ReadWrite);
                xpsdocw = XpsDocument.CreateXpsDocumentWriter(fichero);
                generarDocumento();
            }
            catch (Exception ex)
            {
            }
            
        }

        private void generarDocumento()
        {
            FixedDocument document = new FixedDocument();


            FixedPage page1 = new FixedPage();
            page1.Width = document.DocumentPaginator.PageSize.Width;
            page1.Height = document.DocumentPaginator.PageSize.Height;

            //clonar grid
            string gridXaml = System.Windows.Markup.XamlWriter.Save(hoja);
            StringReader stringReader = new StringReader(gridXaml);
            XmlReader xmlReader = XmlReader.Create(stringReader);
            Grid visual1 = (Grid)System.Windows.Markup.XamlReader.Load(xmlReader);

            page1.Children.Add(visual1);

            // add the page to the document
            PageContent page1Content = new PageContent();
            ((IAddChild)page1Content).AddChild(page1);
            document.Pages.Add(page1Content);

            xpsdocw.Write(document.DocumentPaginator);
            fichero.Close();



        }


        private void saveVisuals(XpsDocumentWriter xpsdw, List<Visual> vc)
        {
            VisualsToXpsDocument vtoxps = (VisualsToXpsDocument)xpsdw.CreateVisualsCollator();

            foreach (Visual v in vc)
            {
                vtoxps.Write(v);
            }

            vtoxps.EndBatchWrite();
        }
    }
}
