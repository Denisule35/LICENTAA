using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Modern.Model;

namespace Modern.ViewModel
{
    public class AdaugareArticolViewModel: ViewModelBase
    {
             InventarViewModel _inventarviewmodel;

        ArticolViewModel _articolnou;

         string _denumire;

        public string Denumire
        {
            get { return _denumire; }
            set
            {
                _denumire = value;
                OnPropertyChanged(nameof(Denumire));
            }
        }

        int _stoc;

        public int Stoc
        {
            get { return _stoc; }

            set
            {
                _stoc = value;
                OnPropertyChanged(nameof(Stoc));
            }
        }


        int _pretvanzare;

        public int Pretvanzare
        {
            get { return _pretvanzare; }
            set             {
                _pretvanzare = value;
                OnPropertyChanged(nameof(Pretvanzare));
            }
        }

        int _pretcumparare;

        public int Pretcumparare
        {
            get { return _pretcumparare; }
            set {
                _pretcumparare = value;
                OnPropertyChanged(nameof(Pretcumparare));
            }

        }

        public bool _esteechipament;

        public bool EsteEchipament
        {
            get { return _esteechipament; }
            set
            {
                _esteechipament = value;
                OnPropertyChanged(nameof(EsteEchipament));
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


        private BitmapImage _articolImage;

        public BitmapImage ArticolImage
        {
            get => _articolImage;
            set
            {
                _articolImage = value;
                OnPropertyChanged(nameof(ArticolImage));
            }
        }


        public ICommand AdaugaArticol { get; }
        public ICommand AdaugaPozaArticol { get; }

        public AdaugareArticolViewModel(InventarViewModel inventaviewmodel)
            {
                _inventarviewmodel = inventaviewmodel;
            AdaugaPozaArticol = new AdaugarePozaArticolCommand(this);
                AdaugaArticol = new AdaugaArticolCommand(this, _inventarviewmodel);

            if (_inventarviewmodel._mainviewmodel.elight == true)
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
