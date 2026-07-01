using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modern.Model;

namespace Modern.ViewModel
{
    class StergerePunctSlabCommand : Commandbase
    {

        DetaliSportiviViewModel _detalisportivviewmodel;

        public StergerePunctSlabCommand(DetaliSportiviViewModel detalisportivviewmodel)
        {
            _detalisportivviewmodel = detalisportivviewmodel;
        }

        public override void Execute(object parameter)
        {

            _detalisportivviewmodel._puncteslabe.Remove(_detalisportivviewmodel.SelectedPunctSlab);

            using (Bazadateconnect bz = new Bazadateconnect())
            {
                Oameni om = bz.Oameni.FirstOrDefault(o => o.Name == _detalisportivviewmodel.nume);
                om.PuncteSlabe = "";

                foreach (var a in _detalisportivviewmodel._puncteslabe)
                {
                    om.PuncteSlabe += a.Substring(2) + "\n";
                }

                bz.SaveChanges();
            }

        }
    }
}
