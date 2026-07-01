using System.Windows;
using System.Windows.Input;
using Modern.Model;

namespace Modern.ViewModel
{
    internal class AdaugaTranzactieSuplimentaraCommand : Commandbase
    {
        private AdaugareTranzactieSuplimentaraViewModel adaugareTranzactieSuplimentaraViewModel;

        public AdaugaTranzactieSuplimentaraCommand(AdaugareTranzactieSuplimentaraViewModel adaugareTranzactieSuplimentaraViewModel)
        {
            this.adaugareTranzactieSuplimentaraViewModel = adaugareTranzactieSuplimentaraViewModel;
        }

        public override void Execute(object parameter)
        {

            if (parameter is Window window)
            {




                int year = adaugareTranzactieSuplimentaraViewModel._tranzactiiViewModel._finanteViewModel.Year;
                int month = adaugareTranzactieSuplimentaraViewModel._tranzactiiViewModel._finanteViewModel.Month;

                DateTime data = new DateTime(year, month, DateTime.Now.Day);

                using (Bazadateconnect bz = new Bazadateconnect())
                {
                    Tranzactie tranzactie = new Tranzactie
                    {
                        Descriere = adaugareTranzactieSuplimentaraViewModel.Descriere,
                        EVenit = adaugareTranzactieSuplimentaraViewModel.EsteVenit,
                        Suma = adaugareTranzactieSuplimentaraViewModel._suma,
                        Data = data

                    };

                    if (tranzactie.EVenit == true)
                    {
                        adaugareTranzactieSuplimentaraViewModel._tranzactiiViewModel._venituri.Add($"{tranzactie.Descriere}: {tranzactie.Suma} lei");
                        adaugareTranzactieSuplimentaraViewModel._tranzactiiViewModel._totalVenituri += tranzactie.Suma;
                    }

                    else
                    {
                        adaugareTranzactieSuplimentaraViewModel._tranzactiiViewModel._pierderi.Add($"{tranzactie.Descriere}: {tranzactie.Suma} lei");
                        adaugareTranzactieSuplimentaraViewModel._tranzactiiViewModel._totalPierderi += tranzactie.Suma;
                        
                    }

                    

                    bz.Tranzactii.Add(tranzactie);
                    bz.SaveChanges();
                }

                window.Close();
            }
        }
    }
}

