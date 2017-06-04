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
using System.Windows.Shapes;

namespace project.recso.bascula.wpf.controls
{
    /// <summary>
    /// Interaction logic for ChildWin.xaml
    /// </summary>
    public partial class ChildWin : UserControl
    {
        Point m_start;
        Vector m_startOffset;
        Page vParent;

        public Grid GetControles()
        {
            return this.Ventana;
        }
        public void setControles(Grid _grid)
        {
            this.Ventana = _grid;
        }

        public ChildWin(Page _win)
        {
            InitializeComponent();
            vParent = _win;
               
        }

        private void Ventana_MouseDown(object sender, MouseButtonEventArgs e)
        {
            m_start = e.GetPosition(vParent);
            m_startOffset = new Vector(tt.X, tt.Y);
            Ventana.CaptureMouse();
        }

        private void Ventana_MouseMove(object sender, MouseEventArgs e)
        {
            if (Ventana.IsMouseCaptured)
            {
                Vector offset = Point.Subtract(e.GetPosition(vParent), m_start);

                tt.X = m_startOffset.X + offset.X;
                tt.Y = m_startOffset.Y + offset.Y;
            }
        }

        private void Ventana_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Ventana.ReleaseMouseCapture();
        }

        private void image1_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            
           
        }

        
        private void image1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Hidden;

            this.UserControl_Unloaded(new object(), new RoutedEventArgs());
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            GC.Collect();

        }
    }
}
