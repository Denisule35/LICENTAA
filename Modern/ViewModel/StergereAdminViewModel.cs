using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Microsoft.EntityFrameworkCore.Sqlite.Query.Internal;
using Modern.Model;

namespace Modern.ViewModel
{
    public class StergereAdminViewModel:ViewModelBase
    {

        MainWindowViewModel _mainviewmodel;

        public ObservableCollection<AdminiViewModel> _antrenori;

        public IEnumerable<AdminiViewModel> Antrenori => _antrenori;

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


        public StergereAdminViewModel(MainWindowViewModel mainviewmodel)
        {
            _antrenori = new ObservableCollection<AdminiViewModel>();
            this._mainviewmodel = mainviewmodel;
            IaAntrenoriDinBD();

            if (_mainviewmodel.elight == true)
            {
                Gri = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#98D2C0"));
                Galben = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#205781"));
                Rosu = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4F959D"));
                Negru = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F6F8D5"));

                foreach(var a in _antrenori)
                {
                    a.Gri = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#98D2C0"));
                    a.Galben = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#205781"));
                    a.Rosu = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4F959D"));
                    a.Negru = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F6F8D5"));
                }

            }
            else
            {

                foreach(var a in _antrenori)
                {
                    a.Gri = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#343131"));
                    a.Galben = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D8A25E"));
                    a.Rosu = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A04747"));
                    a.Negru = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#242222"));
                }

                Gri = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#343131"));
                Galben = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D8A25E"));
                Rosu = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A04747"));
                Negru = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#242222"));
            }

        }

        public void IaAntrenoriDinBD()
        {

            using (Bazadateconnect bz = new Bazadateconnect())
            {
                foreach (var om in bz.Users)
                {
                    if (om.name.Equals("somo"))
                    {
                        continue;
                    }

                    var nou = new AdminiViewModel(this,om.name);
                    _antrenori.Add(nou);
                    

                }
            }

        }

    }
}
