using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace Modern.ViewModel
{
    public class AdaugareTranzactieSuplimentaraViewModel:ViewModelBase
    {

        public TranzactiiViewModel _tranzactiiViewModel;

        private string _descriere;

        public string Descriere
        {
            get { return _descriere; }
            set
            {
                _descriere = value;
                OnPropertyChanged(nameof(Descriere));
            }
        }

        private bool _esteVenit;

        public bool EsteVenit
        {
            get { return _esteVenit; }
            set
            {
                _esteVenit = value;
                OnPropertyChanged(nameof(EsteVenit));
            }
        }

        public int _suma;

        public string Suma
        {
            get => _suma == 0 ? "" : _suma.ToString();

            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    _suma = 0;
                    OnPropertyChanged(nameof(Suma));
                    return;
                }

                if (int.TryParse(value, out int pret))
                {
                    _suma = pret;
                    OnPropertyChanged(nameof(Suma));
                }
            }
        }



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


        public AdaugareTranzactieSuplimentaraViewModel(TranzactiiViewModel tranzactiiViewModel)
        {
            _tranzactiiViewModel = tranzactiiViewModel;
                AdaugaTranzactieSuplimentara = new AdaugaTranzactieSuplimentaraCommand(this);

            if (_tranzactiiViewModel._finanteViewModel._finante._mainviewmodel.elight == true)
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
        }
    }
}
