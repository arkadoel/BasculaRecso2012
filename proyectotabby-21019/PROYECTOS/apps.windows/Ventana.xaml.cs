/*
 * 
 * Usuario: Fer.d.minguela@gmail.com
 * Fecha: 15/04/2013
 * Hora: 17:07
 * 
 * 
 */
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

namespace apps.controls
{
	/// <summary>
	/// Interaction logic for Ventana.xaml
	/// </summary>
	public partial class Ventana : Window
	{
		#region "Controles publicos"
		public string Titulo{
			get{
				return this.Title;
			}
			set{
				this.Title = value;
				this.lblTitulo.Content = this.Title;
			}
		}
		
		public Frame Navegador{
			get{ return this.navegador;}
		}
		
		public Label BtnRestaurar{ 
			get{ return btnRestaurar;} 
		}
		
		#endregion
		public Ventana()
		{
			InitializeComponent();
			lblTitulo.Content = this.Title;
			lblTitulo.FontFamily = apps.fonts.fontManager.getMyAppFonts().First();
			
		}
		
		#region "Control de ventana"
		void moverVentana(object sender, MouseButtonEventArgs e)
		{
			this.DragMove();
		}
		
		void btnCerrar_click(object sender, MouseButtonEventArgs e)
		{
			this.Close();
		}
		
		void BtnRestaurar_click(object sender, MouseButtonEventArgs e)
		{
			if(this.WindowState == WindowState.Maximized){
				this.WindowState = WindowState.Normal;
			}
			else this.WindowState = WindowState.Maximized;
		}
		void BtnMinimizar_click(object sender, MouseButtonEventArgs e)
		{
			this.WindowState = WindowState.Minimized;
		}
		#endregion
	}
}