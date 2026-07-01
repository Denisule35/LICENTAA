using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace Modern.ViewModel
{
    public class AdaugareOmViewModel:ViewModelBase
    {
        public MainWindowViewModel _mainwindowviewmodel;
       

        private string _username;

        public string Username
        {
            get { return _username; }

            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
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


        public ICommand AdgOm {get;}

        public AdaugareOmViewModel(MainWindowViewModel mainwindowviewmodel)
        {
            _mainwindowviewmodel = mainwindowviewmodel;
            AdgOm = new AdaugareOmCommand(this);

            if (_mainwindowviewmodel.elight == true)
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
