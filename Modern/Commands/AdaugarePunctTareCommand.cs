using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modern.Model;

namespace Modern.ViewModel
{
    public class AdaugarePunctTareCommand : Commandbase

    {

        DetaliSportiviViewModel _detalisportivviewmodel;

        public AdaugarePunctTareCommand(DetaliSportiviViewModel model)
        {
            this._detalisportivviewmodel = model;
        }

        public override void Execute(object parameter)
        {
            _detalisportivviewmodel._punctetari.Add( "+ " +_detalisportivviewmodel.PunctTareTextbox.ToString());
           

            using (Bazadateconnect bz = new Bazadateconnect())
            {

                Oameni om = bz.Oameni.FirstOrDefault(o => o.Name == _detalisportivviewmodel.nume);

                if(om.PuncteTari == null)
                {
                    om.PuncteTari = _detalisportivviewmodel.PunctTareTextbox + "\n";
                }

                else
                {
                    var temp = om.PuncteTari;
                    om.PuncteTari = temp + _detalisportivviewmodel.PunctTareTextbox + "\n";
                }

                bz.SaveChanges();
            }

            _detalisportivviewmodel.PunctTareTextbox = "";

        }
    }
}
