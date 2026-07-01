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
using Modern.Model;
using Modern.ViewModel;

namespace Modern.View
{
    /// <summary>
    /// Interaction logic for AdaugareAdmin.xaml
    /// </summary>
    public partial class AdaugareAdmin : Window
    {
        public AdaugareAdmin()
        {
            InitializeComponent();
            
        }

        

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is AdaugareAdminVIewModel vm)
            {
                vm.Password = txtPasw.Password;
            }
        }

        private void Anulare(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
