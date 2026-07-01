using System.Configuration;
using System.Data;
using System.IO;
using System.Net;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Modern.Model;

namespace Modern
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            
            DatabaseFacade facade =new DatabaseFacade(new Bazadateconnect());
            facade.EnsureCreated();

            User somo = new User
            {
                name = "somo",
                password = "kickbox"
            };

           

            using (Bazadateconnect bz = new Bazadateconnect())
            {
                if (!bz.Users.Any())
                {
                    bz.Users.Add(somo);
                    bz.SaveChanges();
                }
            }

            

                CSnakesInitialization.InitializeSnakes();

            if (!CSnakesInitialization.VerifyCudaAvailability())
            {
                CSnakesInitialization.InstallPyTorchWithCuda();
            }



        }
    }

}
