using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modern.Model;

namespace Modern.ViewModel
{
    public class AdaugarePunctSlabCommand : Commandbase
    {

        DetaliSportiviViewModel _detalisportivviewmodel;

        public AdaugarePunctSlabCommand(DetaliSportiviViewModel model)
        {
            this._detalisportivviewmodel = model;
        }

        public override void Execute(object parameter)
        {
            _detalisportivviewmodel._puncteslabe.Add("+ " + _detalisportivviewmodel.PunctSlabTextbox.ToString());


            using (Bazadateconnect bz = new Bazadateconnect())
            {

                Oameni om = bz.Oameni.FirstOrDefault(o => o.Name == _detalisportivviewmodel.nume);

                if (om.PuncteSlabe == null)
                {
                    om.PuncteSlabe = _detalisportivviewmodel.PunctSlabTextbox + "\n";
                }

                else
                {
                    var temp = om.PuncteSlabe;
                    om.PuncteSlabe = temp + _detalisportivviewmodel.PunctSlabTextbox + "\n";
                }

                bz.SaveChanges();
            }

            _detalisportivviewmodel.PunctSlabTextbox = "";
        }
    }
}
