using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Modern.ViewModel
{
    public class EditareArticolViewModel: ViewModelBase
    {

        public ArticolViewModel _articol;

        string _denumire;
        bool _schimbat;
        public string Denumire
        {
            get { return _denumire; }
            set
            {
                _denumire = value;

                _schimbat = _denumire != _articol.denumire;

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
            set
            {
                _pretvanzare = value;
                OnPropertyChanged(nameof(Pretvanzare));
            }
        }

        int _pretcumparare;

        public int Pretcumparare
        {
            get { return _pretcumparare; }
            set
            {
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

       
        public ICommand EditeazaArticol { get; }
        public ICommand EditarePozaArticol { get; }

        public bool _denumireschimbata;
        public EditareArticolViewModel(ArticolViewModel articol)
        {
            _articol = articol;
            _articolImage = articol.ProfileImage;
            _denumire= articol.denumire;
            _pretvanzare = articol.pretvazare;
            _pretcumparare = articol.pretcumparare;
            _stoc = articol.stoc;
            EditarePozaArticol = new EditarePozaArticolCommand(this);
            EditeazaArticol = new EditareArticolCommand(this);
        }

    }
}
