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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Modern.View
{
    /// <summary>
    /// Interaction logic for EditareArticolView.xaml
    /// </summary>
    public partial class EditareArticolView : Window
    {
        public EditareArticolView()
        {
            InitializeComponent();
        }

        private void Iesire(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
