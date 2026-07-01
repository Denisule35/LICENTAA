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
    public class LunaFinanataViewModel: ViewModelBase
    {
        public FinanteViewModel _finante;
        public int Year { get; set; }
        public int Month { get; set; }

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

        public string MonthName => new DateTime(Year, Month, 1).ToString("MMMM").ToUpper();


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

        public ICommand DeschidereTranzactii {get; }

        public LunaFinanataViewModel(int year, int month, FinanteViewModel finante)
        {
            Year = year;
            Month = month;
            _finante = finante;
            DeschidereTranzactii = new DeschideTranzactiiCommand(this);
        }
    }
}
