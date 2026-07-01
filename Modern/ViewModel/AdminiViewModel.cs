using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace Modern.ViewModel
{
    public class AdminiViewModel: ViewModelBase

    {

        public StergereAdminViewModel _stergereadminviewmodel;

        public string nume { get; }

        public ICommand StergereAntrenor { get; }


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


        public AdminiViewModel(StergereAdminViewModel adaugareadminviewmodel,string nume)
        {
            this._stergereadminviewmodel = adaugareadminviewmodel;
            this.nume = nume;
            StergereAntrenor = new StergereAntrenorCommand(this);
        }
        
        

    }
}
