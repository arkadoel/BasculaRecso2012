/*
 * 
 * Usuario: Fer.d.minguela@gmail.com
 * Fecha: 04/15/2013
 * Hora: 17:33
 * 
 * 
 */
using System;
using System.Linq;
using System.Windows.Media;
using System.Collections.Generic;

namespace apps.fonts
{
	/// <summary>
	/// Gestion de fuentes
	/// </summary>
	public class fontManager
	{
		public fontManager()
		{
			
		}
		

        /// <summary>
        /// Extrae todas las fuentes del directorio Fuentes alojado en la misma carpeta
        /// que la aplicacion.
        /// </summary>
        /// <returns></returns>
		public static List<FontFamily> getMyAppFonts(){
			List<FontFamily> fuentes = Fonts.GetFontFamilies("file:///"+ apps.IO.DirectorioActual +"/Fuentes/").ToList();
        	
			return fuentes;
		}
	}
}
