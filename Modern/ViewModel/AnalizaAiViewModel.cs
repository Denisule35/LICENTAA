using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Modern.Model;

namespace Modern.ViewModel
{
    internal class AnalizaAiViewModel: ViewModelBase

    {
        private MainWindowViewModel _mainviewmodel;

        public ObservableCollection<OameniViewModel> _oameni;
        public ICollectionView Oameni { get; }

        private string _filtrare = string.Empty;

        public string Filtru
        {
            get { return _filtrare; }

            set
            {
                _filtrare = value;
                OnPropertyChanged(nameof(Filtru));
                Oameni.Refresh();
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

        

        public AnalizaAiViewModel(MainWindowViewModel mainviewmodel)
        {
            this._mainviewmodel = mainviewmodel;
            _oameni = mainviewmodel._oameni;
            Oameni = new CollectionViewSource { Source = _oameni }.View;
            Oameni.Filter = FiltrareOameni;

            if (mainviewmodel.elight == true)
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

            //IaOameniDinBD();

        }

        private bool FiltrareOameni(object obj)
        {
            if (obj is OameniViewModel oamenii)
            {
                return oamenii.nume.Contains(Filtru, StringComparison.InvariantCultureIgnoreCase);
            }

            return false;
        }

        public void IaOameniDinBD()
        {
            using (Bazadateconnect bz = new Bazadateconnect())
            {

                foreach (var om in bz.Oameni)
                {
                    var nou = new OameniViewModel(om.Name, om.Abonament, _mainviewmodel);
                    _oameni.Add(nou);

                }



            }
        }
    }
}