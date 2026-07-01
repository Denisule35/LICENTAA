using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Modern.Model;

namespace Modern.ViewModel
{
     public  class EditareArticolCommand:Commandbase
     {
        EditareArticolViewModel _articol;
        
        public EditareArticolCommand(EditareArticolViewModel articol)
        {
            _articol = articol;
        }

        public override void Execute(object parameter)
        {

            if (parameter is Window wind)
            {   
                using (Bazadateconnect bz = new Bazadateconnect())
                {
                    Inventar art = bz.Articole.FirstOrDefault(o => o.Denumire == _articol._articol.denumire);

                    if (art != null)
                    {
                        art.Denumire = _articol.Denumire;
                        art.PretCumparare = _articol.Pretcumparare;
                        art.PretVanzare = _articol.Pretvanzare;
                        art.Stoc = _articol.Stoc;
                    }

                    bz.SaveChanges();
                }

                _articol._articol.denumire = _articol.Denumire;
                _articol._articol.pretcumparare = _articol.Pretcumparare;   
                _articol._articol.pretvazare = _articol.Pretvanzare;
                _articol._articol.stoc = _articol.Stoc; 

                wind.Close();
            }
        }
    }
    
    
    
}
