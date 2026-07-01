using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modern.Model;

namespace Modern.ViewModel
{
    public class StergereArticolCommand : Commandbase
    {

            InventarViewModel _inventarviewmodel;
        ArticolViewModel _articol;

        public StergereArticolCommand(ArticolViewModel articol, InventarViewModel inventarviewmodel)
        {
            _articol = articol;
            _inventarviewmodel = inventarviewmodel;
        }
        public override void Execute(object parameter)
        {
            _inventarviewmodel._articole.Remove(_articol);

            using (Bazadateconnect bz = new Bazadateconnect())
            {

                Inventar articol = bz.Articole.FirstOrDefault(o => o.Denumire == _articol.denumire);

                if (articol != null)
                {
                    bz.Articole.Remove(articol);
                    bz.SaveChanges();
                }


            }

        }
    }
}
