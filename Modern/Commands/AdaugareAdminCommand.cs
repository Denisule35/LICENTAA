using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Modern.Model;

namespace Modern.ViewModel
{
    public class AdaugareAdminCommand : Commandbase
    {
        AdaugareAdminVIewModel _viewmodel;

        public AdaugareAdminCommand(AdaugareAdminVIewModel viemodel)
        {
            _viewmodel = viemodel;
        }

        public override void Execute(object parameter)
        {

            if (parameter is Window wind) {

                string username, parola;
                username = _viewmodel.Username;
                parola = _viewmodel.Password;

                User nou = new User
                {
                    name = username,
                    password = parola,
                };

                using (Bazadateconnect bazadedate = new Bazadateconnect())
                {
                    bazadedate.Add(nou);
                    bazadedate.SaveChanges();
                }

                wind.Close();

            }
            
        }
    }
}
