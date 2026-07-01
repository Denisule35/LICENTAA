using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using Modern.Model;

namespace Modern.ViewModel
{
    public class TranzactiiViewModel : ViewModelBase
    {

        public ObservableCollection<string> _venituri;

        public ObservableCollection<string> _pierderi;

        public IEnumerable<string> Venituri => _venituri;

        public IEnumerable<string> Pierderi => _pierderi;

        public int _totalVenituri = 0;
        public int TotalVenituri
        {
            get => _totalVenituri;
            set { _totalVenituri = value; OnPropertyChanged(nameof(TotalVenituri)); OnPropertyChanged(nameof(SoldNet)); }
        }

        public int _totalPierderi = 0;
        public int TotalPierderi
        {
            get => _totalPierderi;
            set { _totalPierderi = value; OnPropertyChanged(nameof(TotalPierderi)); OnPropertyChanged(nameof(SoldNet)); }
        }

        public int SoldNet => TotalVenituri - TotalPierderi;


        public LunaFinanataViewModel _finanteViewModel;

        private Brush _gri = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#343131"));
        public Brush Gri
        {
            get => _gri;
            set
            {
                _gri = value;
                OnPropertyChanged(nameof(Gri));
            }
        }


        private Brush _galben = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D8A25E"));
        public Brush Galben
        {
            get => _galben;
            set
            {
                _galben = value;
                OnPropertyChanged(nameof(Galben));
            }
        }

        private Brush _negru = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#242222"));
        public Brush Negru
        {
            get => _negru;
            set
            {
                _negru = value;
                OnPropertyChanged(nameof(Negru));
            }
        }

        private Brush _rosu = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A04747"));
        public Brush Rosu
        {
            get => _rosu;
            set
            {
                _rosu = value;
                OnPropertyChanged(nameof(Rosu));
            }
        }

        public ICommand AdaugaTranzactieSuplimentara { get; set; }

        public TranzactiiViewModel(LunaFinanataViewModel finanteViewModel)
        {
            _finanteViewModel = finanteViewModel;
            _venituri = new ObservableCollection<string>();
            _pierderi = new ObservableCollection<string>();
            AdaugaTranzactieSuplimentara = new DeschidereAdaugareTranzactieSuplimentaraCommand(this);
            

            if (_finanteViewModel._finante._mainviewmodel.elight == true)
            {
                Gri = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#98D2C0"));
                Galben = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#205781"));
                Rosu = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4F959D"));
                Negru = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F6F8D5"));
            }
            else
            {
                Gri = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#343131"));
                Galben = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D8A25E"));
                Rosu = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A04747"));
                Negru = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#242222"));
            }

            using (Bazadateconnect bz = new Bazadateconnect())
            {
                
                var tranzactii = bz.Tranzactii.Where(o => o.Data.Month == _finanteViewModel.Month && o.Data.Year == _finanteViewModel.Year).ToList();

                foreach (var tranzactie in tranzactii)
                {
                    if (tranzactie.EVenit == true)
                    {
                        _venituri.Add($"{tranzactie.Descriere}: {tranzactie.Suma} lei");
                        TotalVenituri += tranzactie.Suma;

                    }
                    else
                    {
                        _pierderi.Add($"{tranzactie.Descriere}: {(tranzactie.Suma)} lei");
                        TotalPierderi += tranzactie.Suma;
                    }
                }

            }
        }
    }
}
