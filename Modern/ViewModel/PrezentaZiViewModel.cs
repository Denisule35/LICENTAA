using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Modern.Model;

namespace Modern.ViewModel
{
    public class PrezentaZiViewModel : ViewModelBase
    {
        private ZiPrezentaViewModel ziPrezentaViewModel;


        public ObservableCollection<string> _prezenti;

        public IEnumerable<string> Prezenti => _prezenti;

        public ObservableCollection<string> _absenti;

        public IEnumerable<string> Absenti => _absenti;



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

        public PrezentaZiViewModel(ZiPrezentaViewModel ziPrezentaViewModel)
        {
            this.ziPrezentaViewModel = ziPrezentaViewModel;
            _prezenti = new ObservableCollection<string>();
            _absenti = new ObservableCollection<string>();


            using (Bazadateconnect bz = new Bazadateconnect())
            {
                foreach (var prez in bz.Prezenti)

                {

                    if(prez.Data == ziPrezentaViewModel.Data)
                    {
                        List<string> prezenti = prez.Prezenti.Split("\n", StringSplitOptions.RemoveEmptyEntries).ToList();
                        List<string> absenti = prez.Absenti.Split("\n", StringSplitOptions.RemoveEmptyEntries).ToList();

                        _prezenti = new ObservableCollection<string>(prezenti);
                        _absenti = new ObservableCollection<string>(absenti);
                        break;
                    }


                }
            }

        }
    }
}
