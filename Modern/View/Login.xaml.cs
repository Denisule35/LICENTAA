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
   
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();

            DataContext = new LoginViewModel();
            
        }

        private void Miscare_Ecran(object sender, MouseButtonEventArgs e)
        {

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();

            }
        }

        private void Butonminimize(object sender, RoutedEventArgs e)
        {

            WindowState = WindowState.Minimized;
        }

        private void Butonclose(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        

        private void MutalaParola(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                txtPasw.Focus();
            }
        }

       

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is LoginViewModel vm)
            {
                vm.Password = txtPasw.Password;
            }
        }
    }
}
