using System.Windows;
using System.Windows.Input;
using Modern.Model;

namespace Modern.ViewModel
{
    public class AdaugaArticolCommand : Commandbase
    {

        InventarViewModel _inventarviewmodel;
        AdaugareArticolViewModel _articol;

        public AdaugaArticolCommand(AdaugareArticolViewModel articol,InventarViewModel inventarviewmodel)
        {
            this._articol = articol;
            this._inventarviewmodel = inventarviewmodel;
        }

        public override void Execute(object parameter)
        {

            if (parameter is Window wind)
            {

                Inventar art = new()
                {
                    Denumire = _articol.Denumire,
                    PretCumparare = _articol.Pretcumparare,
                    PretVanzare = _articol.Pretvanzare,
                    Stoc = _articol.Stoc,
                    EsteEchipament = _articol.EsteEchipament,

                };

                ArticolViewModel articol = new ArticolViewModel(art.Denumire, art.PretVanzare, art.Stoc, art.PretCumparare, _inventarviewmodel);

                if (_articol.ArticolImage != null)
                {
                    articol.ProfileImage = _articol.ArticolImage;
                    art.ArePoza = true;
                }
                else
                {
                    art.ArePoza = false;
                }

                _inventarviewmodel._articole.Add(articol);




                using (Bazadateconnect bz = new Bazadateconnect())
                {

                    bz.Articole.Add(art);
                    bz.SaveChanges();
                }


                wind.Close();
            }
        }
    }
}