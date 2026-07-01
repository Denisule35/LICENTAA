using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Modern.Model;

namespace Modern.ViewModel
{
    class RenoireAbonamentCommand : Commandbase
    {

        OameniViewModel _omviewmodel;

        public RenoireAbonamentCommand(OameniViewModel oameniviewmodel)
        {
            _omviewmodel = oameniviewmodel;
        }
        public override void Execute(object parameter)
        {


            _omviewmodel.data = DateOnly.FromDateTime(DateTime.Now);
            if (_omviewmodel._mainviewmodel.elight == true)
            {
                _omviewmodel.expirat = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F6F8D5"));
            }
            else
            {
                _omviewmodel.expirat = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D8A25E"));
            }


            using (Bazadateconnect bz = new Bazadateconnect())
            {
                Oameni om = bz.Oameni.FirstOrDefault(o => o.Name == _omviewmodel.nume);

                if (om != null)
                {
                    om.Abonament = DateOnly.FromDateTime(DateTime.Now);
                }

                Tranzactie tran = new Tranzactie
                {
                    Descriere = $"Renoire abonament pentru {_omviewmodel.nume}",
                    Data = DateTime.Now,
                    Suma = _omviewmodel._mainviewmodel._pretabonament,
                    EVenit = true,

                };

                bz.Tranzactii.Add(tran);

                bz.SaveChanges();
            }

        }
    }
}
