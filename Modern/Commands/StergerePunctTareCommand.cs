using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Modern.Model;

namespace Modern.ViewModel
{
    public class StergerePunctTareCommand : Commandbase
    {

        DetaliSportiviViewModel _detalisportivviewmodel;

        public StergerePunctTareCommand(DetaliSportiviViewModel detalisportivviewmodel)
        {
            _detalisportivviewmodel = detalisportivviewmodel;
        }

        public override void Execute(object parameter)
        {



            _detalisportivviewmodel._punctetari.Remove(_detalisportivviewmodel.SelectedPunctTare);

            using (Bazadateconnect bz = new Bazadateconnect())
            {
                Oameni om = bz.Oameni.FirstOrDefault(o => o.Name == _detalisportivviewmodel.nume);
                om.PuncteTari = "";

                foreach (var a in _detalisportivviewmodel._punctetari)
                {
                    om.PuncteTari += a.Substring(2)+"\n";
                }

                bz.SaveChanges();
            }
        }
    }
}
