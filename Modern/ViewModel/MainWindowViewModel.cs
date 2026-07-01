using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using Modern.Model;

namespace Modern.ViewModel
{
    public class MainWindowViewModel:ViewModelBase

    {
        public  ObservableCollection<OameniViewModel> _oameni;
        public ICollectionView Oameni { get; }

        private string _filtrare=string.Empty;

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

        private Brush _gri= new SolidColorBrush((Color)ColorConverter.ConvertFromString("#343131"));
        public Brush Gri
        {
            get => _gri;
            set
            {
                _gri = value;
                OnPropertyChanged(nameof(Gri));
            }
        }


        private Brush _galben= new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D8A25E"));
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


         public List<OameniViewModel> _prezenti { get; set; }

        public ICommand DeschidereAdgOm {  get; }
        public ICommand SchimbareTema {  get; }

        public ICommand DeschidereAdgAdmin { get; }
        public ICommand DeschidereStergereAdmin { get; }

        public ICommand DeschidereAnalizaAi { get; }

        public ICommand DeschidereInventar { get; }

        public ICommand DeschiderePrezenta { get; }

        public ICommand DeschidereFinante { get; }

        public bool elight = false;


        public int _pretabonament;

        public string PretAbonament
        {
            get => _pretabonament == 0 ? "" : _pretabonament.ToString();

            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    _pretabonament = 0;
                    OnPropertyChanged(nameof(PretAbonament));
                    return;
                }

                if (int.TryParse(value, out int pret))
                {
                    _pretabonament = pret;
                    OnPropertyChanged(nameof(PretAbonament));
                }
            }
        }

        public List<string> _absenti { get; set; }

        HttpListener _listner;
        HttpServer _server;
        public MainWindowViewModel()
        {

            _oameni = new ObservableCollection<OameniViewModel>();
            _prezenti = new List<OameniViewModel>();
            _absenti = new List<string>();

            IaOameniDinBD();
            CrearePrezentaAzi();

            Oameni = CollectionViewSource.GetDefaultView(_oameni);
            Oameni.Filter = FiltrareOameni;
            DeschidereAdgOm=new DeschidereAdaugareOmCommand(this);
            SchimbareTema = new SchimbareTemaCommand(this);
            DeschidereAdgAdmin= new DeschidereAdaugareAdminCommand(this);
            DeschidereStergereAdmin = new DeschidereStergereAdminCommand(this);
            DeschidereAnalizaAi = new DeschidereAnalizaAiCommand(this);
            DeschidereInventar = new DeschidereInventarCommand(this);
            DeschiderePrezenta = new DeschiderePrezentaCommand(this);
            DeschidereFinante = new DeschideFinanateCommand(this);

            _listner = new HttpListener();
            _server = new HttpServer(_listner,this);
            _server.Start();
        }

        private bool FiltrareOameni(object obj)
        {
            if(obj is OameniViewModel oamenii)
            {
                return oamenii.nume.Contains(Filtru,StringComparison.InvariantCultureIgnoreCase);
            }

            return false;
        }

        
        public void CrearePrezentaAzi()
        {
            using(Bazadateconnect bz = new Bazadateconnect())
            {
                string absenti = string.Empty;

                foreach (var om in _oameni)
                {
                    _absenti.Add(om.nume);
                    absenti += om.nume + "\n";
                }

                var prezenta = new Prezenta
                {
                    Data = DateOnly.FromDateTime(DateTime.Today),
                    Prezenti = string.Empty,
                    Absenti = absenti
                };

                if (!bz.Prezenti.Any())
                {
                    bz.Prezenti.Add(prezenta);
                    bz.SaveChanges();
                    return;
                }


                foreach(var prez in bz.Prezenti)
                {
                   if(prez.Data == DateOnly.FromDateTime(DateTime.Today))
                    {
                        return;
                    }
                }
                bz.Prezenti.Add(prezenta);
                bz.SaveChanges();
            }
        }

        public void IaOameniDinBD()
        {
            using (Bazadateconnect bz = new Bazadateconnect())
            {

                foreach (var om in bz.Oameni)
                {
                    var nou = new OameniViewModel(om.Name, om.Abonament,this);
                    _oameni.Add(nou);
                    
                }

                

            }
        }
         

    }
}
