/*
 * Created by SharpDevelop.
 * User: develop
 * Date: 16/04/2013
 * Time: 11:16
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace calculadora
{
	/// <summary>
	/// Interaction logic for About.xaml
	/// </summary>
	public partial class About : Page
	{
		public About()
		{
			InitializeComponent();
		}

        private void label1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("explorer", "http://ferdminguela.blogspot.com.es");
        }

        private void label5_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("explorer", "http://acidrums.deviantart.com/");
        }
	}
}