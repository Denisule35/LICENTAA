using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modern.Model;

namespace Modern.ViewModel
{
    public class VanzareArticolCommand:Commandbase
    {
        InventarViewModel _inventarviewmodel;

        public VanzareArticolCommand(InventarViewModel inventarviewmodel)
        {
            _inventarviewmodel = inventarviewmodel;
        }


        public override void Execute(object parameter)
        {

            int cantitate = int.Parse(_inventarviewmodel.Cantitate.Trim());

            _inventarviewmodel.SelectedArticol.stoc = _inventarviewmodel.SelectedArticol.stoc - cantitate;

            using (Bazadateconnect bz = new Bazadateconnect())
            {
                Inventar articol = bz.Articole.FirstOrDefault(o => o.Denumire == _inventarviewmodel.SelectedArticol.denumire);
                if (articol != null)
                {
                    articol.Stoc = _inventarviewmodel.SelectedArticol.stoc;
                    
                }

                Tranzactie tranzactie = new Tranzactie
                {
                    Descriere = $"Vanzare {cantitate} bucati din {_inventarviewmodel.SelectedArticol.denumire}",
                    Data = DateTime.Now,
                    Suma = _inventarviewmodel.SelectedArticol.pretvazare * cantitate,
                    EVenit = true,
                    

                };

                bz.Tranzactii.Add(tranzactie);
                bz.SaveChanges();

            }
        }
    }
}
