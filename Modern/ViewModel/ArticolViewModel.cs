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
    public class ArticolViewModel : ViewModelBase
    {

         string _denumire { get; set; }

        public string denumire
        {
            get { return _denumire; }
            set
            {
                _denumire = value;
                OnPropertyChanged(nameof(denumire));

            }
        }
       
         int _pretvazare { get; set; }

       public int pretvazare
        {
            get { return _pretvazare; }
            set
            {
                _pretvazare = value;
                OnPropertyChanged(nameof(pretvazare));
            }
        }


         int _stoc { get; set; }

        public int stoc
        {
            get { return _stoc; }
            set
            {
                _stoc = value;
                OnPropertyChanged(nameof(stoc));
            }
        }

         int _pretcumparare { get; set; }

        public int pretcumparare
        {
            get { return _pretcumparare; }
            set
            {
                _pretcumparare = value;
                OnPropertyChanged(nameof(pretcumparare));
            }
        }


        public InventarViewModel _inventarviewmodel;


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


        private BitmapImage _profileImage;

        public BitmapImage ProfileImage
        {
            get => _profileImage;
            set
            {
                _profileImage = value;
                OnPropertyChanged(nameof(ProfileImage));
            }
        }

        public ICommand StergereArticol { get; }

        public ICommand EditareArticol { get; }


        public ArticolViewModel(string denumire, int pretvazare, int stoc, int pretcumparare,InventarViewModel inventarviewmodel)
        {
            this.denumire = denumire;
            this.pretvazare = pretvazare;
            this.stoc = stoc;
            this.pretcumparare = pretcumparare;
            this._inventarviewmodel = inventarviewmodel;

            StergereArticol= new StergereArticolCommand(this, inventarviewmodel);
            EditareArticol = new DeschidereEditareArticol(this);

        }

    }
    
}
