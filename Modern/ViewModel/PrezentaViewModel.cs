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
    public class PrezentaViewModel: ViewModelBase
    {

        public MainWindowViewModel _mainviewmodel;

        public ObservableCollection<ZiPrezentaViewModel> _zileprezenta;

        public IEnumerable<ZiPrezentaViewModel> Prezente => _zileprezenta;

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

        public PrezentaViewModel(MainWindowViewModel mainviewmodel)
        {
            _zileprezenta = new ObservableCollection<ZiPrezentaViewModel>();
            _mainviewmodel = mainviewmodel;

            if (_mainviewmodel.elight == true)
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
                foreach(var prez in bz.Prezenti)
                {
                    ZiPrezentaViewModel zi = new ZiPrezentaViewModel(this)
                    {
                        Data = prez.Data,
                    };

                    _zileprezenta.Add(zi);
                }
            }
        }

    }
}
