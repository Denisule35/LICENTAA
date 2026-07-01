using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Modern.View
{
    /// <summary>
    /// Interaction logic for PrezentaView.xaml
    /// </summary>
    public partial class PrezentaView : Window
    {
        public PrezentaView()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var sb = (Storyboard)FindResource("FadeIn");
            sb.Begin();
        }

        private void Iesire(object sender, RoutedEventArgs e)
        {
            Close();
        }

        

    }
}
