using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modern.View;
using System.Windows;
using Modern.Model;

namespace Modern.ViewModel
{
     public class LoginCommand : Commandbase

    {

        private readonly LoginViewModel _viewModel;

        public LoginCommand(LoginViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override void Execute(object parameter)
        {

            if (parameter is Window loginWindow)
            {

                var username = _viewModel.Username;
                var parola = _viewModel.Password;

                if (username == null || parola == null)
                {
                    return;
                }

                string good = username.ToString();
                good = good.Trim();

                using (Bazadateconnect context = new Bazadateconnect())
                {

                    bool userfound = context.Users.Any(user => user.name == good && user.password == parola);

                    
                    


                    if (userfound)
                    {
                        MainWindow mainWindow = new MainWindow();

                        mainWindow.Show();
                        loginWindow.Close();
                    }
                    else
                    {
                        _viewModel.Msgeroare = "Usernameul sau Parola este grestia";
                    }

                }


            }

         }


      }
    
}
