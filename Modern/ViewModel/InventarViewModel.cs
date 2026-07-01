using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Modern.Model;

namespace Modern.ViewModel
{
    public class InventarViewModel:ViewModelBase
    {

        public MainWindowViewModel _mainviewmodel;


        public ObservableCollection<ArticolViewModel> _articole;

        public IEnumerable<ArticolViewModel> Articole => _articole;

        string _cantitate = string.Empty;

        public string Cantitate
        {
            get { return _cantitate; }
            set
            {
                _cantitate = value;
                OnPropertyChanged(nameof(Cantitate));
            }
        }


        public ICollectionView Articolee { get; }

        private string _filtrare = string.Empty;

        public string Filtru
        {
            get { return _filtrare; }

            set
            {
                _filtrare = value;
                OnPropertyChanged(nameof(Filtru));
                Articolee?.Refresh();
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


        private ArticolViewModel _selectedArticol;

        public ArticolViewModel SelectedArticol
        {
            get => _selectedArticol;
            set
            {
                _selectedArticol = value;
                OnPropertyChanged(nameof(SelectedArticol));
            }
        }




        public ICommand AdaugareArticol { get; }
        public ICommand VanzareArticol { get; }

        public ICommand CumparareArticol { get; }

        public InventarViewModel(MainWindowViewModel mainviewmodel)
        {
            _mainviewmodel = mainviewmodel;
            _articole = new ObservableCollection<ArticolViewModel>();
           
            IaArticoledinDb();
            Articolee = new CollectionViewSource { Source = _articole }.View;
            Articolee.Filter = FiltrareArticole;
            


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

            AdaugareArticol = new DeschidereAdaugareArticolCommand(this);
            CumparareArticol = new CumparareArticolCommand(this);
            VanzareArticol = new VanzareArticolCommand(this);
        }


        private bool FiltrareArticole(object obj)
        {
            if (obj is ArticolViewModel art)
            {
                return art.denumire.Contains(Filtru, StringComparison.InvariantCultureIgnoreCase);
            }

            return false;
        }

        public void IaArticoledinDb()
        {

            using (Bazadateconnect bz = new Bazadateconnect())
            {

                foreach (var articol in bz.Articole)
                {
                    var nou = new ArticolViewModel(articol.Denumire,articol.PretVanzare,articol.Stoc,articol.PretCumparare,this);

                    if(_mainviewmodel.elight == true)
                    {
                        nou.Gri = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#98D2C0"));
                        nou.Galben = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#205781"));
                        nou.Rosu = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4F959D"));
                        nou.Negru = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F6F8D5"));
                    }
                    else
                        {
                            nou.Gri = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#343131"));
                            nou.Galben = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D8A25E"));
                            nou.Rosu = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A04747"));
                            nou.Negru = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#242222"));

                    }

                    if (articol.ArePoza == false)
                    {

                        string imaginifolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "imagini");
                        Directory.CreateDirectory(imaginifolder);
                        string butn = Path.Combine(imaginifolder, "placeholderarticole.png");
                        nou.ProfileImage = new BitmapImage(new Uri(butn));



                    }

                    else
                    {
                        string imaginifolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "imagini");
                        Directory.CreateDirectory(imaginifolder);
                        string[] supportedExtensions = new[] { ".jpg", ".jpeg", ".png", ".jfif",".webp" };

                        foreach (var a in supportedExtensions)
                        {
                            string bun = Path.Combine(imaginifolder, nou.denumire + a);

                            if (File.Exists(bun))
                            {

                                var bitmap = new BitmapImage();
                                bitmap.BeginInit();
                                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                                bitmap.UriSource = new Uri(bun);
                                bitmap.EndInit();
                                bitmap.Freeze();

                                nou.ProfileImage = bitmap;
                            }
                        }
                    }


                        _articole.Add(nou);

                }



            }
        }

    }
}
