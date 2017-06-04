/*
 * 
 * Usuario: Fer.d.minguela@gmail.com
 * Fecha: 04/15/2013
 * Hora: 17:37
 * 
 * 
 */
using System;
using System.Runtime.Hosting;
using System.Windows;

namespace apps
{
	/// <summary>
	/// Description of IO.
	/// </summary>
	public class IO
	{
        /// <summary>
        /// Devuelve la ruta actual en donde se ubica el ensamblado
        /// </summary>
		public static string DirectorioActual{
			get{
				return System.IO.Directory.GetCurrentDirectory().ToString();
			}
			set{}
		}
		
		public IO()
		{
			
		}
	}
}
