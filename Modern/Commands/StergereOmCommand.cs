using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Modern.Model;

namespace Modern.ViewModel
{
    class StergereOmCommand : Commandbase
    {

        OameniViewModel _omviewmodel;

        public StergereOmCommand(OameniViewModel omviewmodel)
        {
            _omviewmodel = omviewmodel;
        }

        public override void Execute(object parameter)
        {
            
            _omviewmodel._mainviewmodel._oameni.Remove(_omviewmodel);

            using (Bazadateconnect bz = new Bazadateconnect())
            {

                Oameni om = bz.Oameni.FirstOrDefault(o => o.Name == _omviewmodel.nume);

                if (om != null)
                {
                    bz.Oameni.Remove(om);
                    bz.SaveChanges();
                }


            }

        }
    }
}
