using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modern.Model;

namespace Modern.ViewModel
{
    public class StergereAntrenorCommand : Commandbase
    {

        AdminiViewModel _adminviewmodel;

        public StergereAntrenorCommand(AdminiViewModel adminviewmodel)
        {
            _adminviewmodel = adminviewmodel;
        }

        public override void Execute(object parameter)
        {
            _adminviewmodel._stergereadminviewmodel._antrenori.Remove(_adminviewmodel);


            using (Bazadateconnect bz = new Bazadateconnect())
            {

                User om = bz.Users.FirstOrDefault(u => u.name == _adminviewmodel.nume);

                if (om != null)
                {
                    bz.Users.Remove(om);
                    bz.SaveChanges();
                }

            }

        }
    }
}
