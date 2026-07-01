using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Modern.Model;

namespace Modern.ViewModel
{

    public class PieData(string name, double value)
    {
        public string Name { get; set; } = name;
        public double[] Values { get; set; } = [value];
    }


    public class DetaliSportiviViewModel : ViewModelBase
    {

        public OameniViewModel _oameni;

        public ObservableCollection<string> _punctetari;

        public IEnumerable<string> PuncteTari => _punctetari;

            public ObservableCollection<string> _puncteslabe;

        public IEnumerable<string> PuncteSlabe => _puncteslabe;


        private string _puncttaretextbox = string.Empty;

        public string PunctTareTextbox
        {
            get { return _puncttaretextbox; }

            set
            {
                _puncttaretextbox = value;
                OnPropertyChanged(nameof(PunctTareTextbox));

            }


        }


        private string _punctslabtextbox = string.Empty;

        public string PunctSlabTextbox
        {
            get { return _punctslabtextbox; }

            set
            {
                _punctslabtextbox = value;
                OnPropertyChanged(nameof(PunctSlabTextbox));

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


        public ObservableCollection<string> NiveluriExperienta { get; } = new()
{
    "Începător",
    "Intermediar",
    "Avansat",
    "Profesionist",
    "Elit"
};



        private string _nivelExperienta;
        public string NivelExperienta
        {
            get => _nivelExperienta;
            set
            {
                if (_nivelExperienta == value) return;
                _nivelExperienta = value;
                OnPropertyChanged(nameof(NivelExperienta));


                using (Bazadateconnect bz = new Bazadateconnect())
                {
                    Oameni om = bz.Oameni.FirstOrDefault(o => o.Name == _oameni.nume);
                    if (om != null)
                    {
                        om.Nivel = value;
                        bz.SaveChanges();
                    }
                }

            }
        }

        private int _procentajprezenta;

        public int Procentajprezenta
        {
            get => _procentajprezenta;
            set
            {
                _procentajprezenta = value;
                OnPropertyChanged(nameof(ProfileImage));
            }
        }

        public string nume { get; }


        public ICommand AdaugarePoza { get; set; }

        public ICommand AdaugarePunctTare { get; set; }

        public ICommand StergerePunctTare { get; set; }

        public ICommand AdaugarePunctSlab { get; set; }

        public ICommand StergerePunctSlab { get; set; }

        public List<PieData> PieChartt { get; set; } = new();

        private string _selectedPunctTare;
        public string SelectedPunctTare
        {
            get => _selectedPunctTare;
            set
            {
                _selectedPunctTare = value;
                OnPropertyChanged(nameof(SelectedPunctTare));
            }
        }


        private string _selectedPunctSlab;
        public string SelectedPunctSlab
        {
            get => _selectedPunctSlab;
            set
            {
                _selectedPunctSlab = value;
                OnPropertyChanged(nameof(SelectedPunctSlab));

                
            }
        }




        public DetaliSportiviViewModel(OameniViewModel oameni)
        {
            this._oameni = oameni;
            nume = oameni.nume;

            if (_oameni._mainviewmodel.elight == true)
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
                Oameni om = bz.Oameni.FirstOrDefault(o => o.Name == _oameni.nume);
                AdaugarePoza = new AdaugarePozaCommand(this);

                if (om != null)
                {
                    if (om.ArePoza == false)
                    {

                        string imaginifolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "imagini");
                        Directory.CreateDirectory(imaginifolder);
                        string butn = Path.Combine(imaginifolder, "placeholder.png");
                        ProfileImage = new BitmapImage(new Uri(butn));

                    }

                    else
                    {
                        string imaginifolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "imagini");
                        Directory.CreateDirectory(imaginifolder);
                        string[] supportedExtensions = new[] { ".jpg", ".jpeg", ".png", ".jfif" };

                        foreach (var a in supportedExtensions)
                        {
                            string bun = Path.Combine(imaginifolder, _oameni.nume + a);

                            if (File.Exists(bun))
                            {
                                ProfileImage = new BitmapImage(new Uri(bun));
                            }
                        }
                    }
                }

                int totalprezente = 0;
                int totalabsente = 0;

                foreach(var prezenta in bz.Prezenti)
                {
                    List<string> prezenti = prezenta.Prezenti.Split("\n", StringSplitOptions.RemoveEmptyEntries).ToList();

                    if (prezenti.Contains(_oameni.nume))
                    {
                        totalprezente++;
                    }
                    else
                    {
                        totalabsente++;
                    }


                }

                _procentajprezenta = (int)((double)totalprezente / (totalprezente + totalabsente) * 100);

                om.ProcentajPrezenta = _procentajprezenta;

                bz.SaveChanges();

            }



            _punctetari = new ObservableCollection<string>();

            AdaugarePunctTare = new AdaugarePunctTareCommand(this);

            StergerePunctTare = new StergerePunctTareCommand(this);

            AdaugarePunctSlab = new AdaugarePunctSlabCommand(this);

            StergerePunctSlab = new StergerePunctSlabCommand(this);

            _puncteslabe = new ObservableCollection<string>();


            using (Bazadateconnect bz = new Bazadateconnect())
            {
                Oameni om = bz.Oameni.FirstOrDefault(o => o.Name == _oameni.nume);

                if (om.PuncteTari != null)
                {

                    List<string> punctetari = om.PuncteTari.Split("\n", StringSplitOptions.RemoveEmptyEntries).ToList();

                    foreach (var a in punctetari)
                    {
                        _punctetari.Add("+ "+a);
                    }
                }

                if (om.PuncteSlabe != null)
                {

                    List<string> puncteslabe = om.PuncteSlabe.Split("\n", StringSplitOptions.RemoveEmptyEntries).ToList();

                    foreach (var a in puncteslabe)
                    {
                        _puncteslabe.Add("+ " + a);
                    }
                }

                PieChartt.Add(new PieData("Pumni Nimeriti Cap", (double)om.PumniNimeritiCap));
                PieChartt.Add(new PieData("Pumni Nimeriti Corp", (double)om.PumniNimeritiCorp));
                PieChartt.Add(new PieData("Pumni Incasati Cap", (double)om.PumniIncasatiCap));
                PieChartt.Add(new PieData("Pumni Incasati Corp", (double)om.PumniIncasatiCorp));
                PieChartt.Add(new PieData("Pumni Ratati", (double)om.PumniRatati));

                _nivelExperienta = om.Nivel ?? "Începător";
            }
        }
    }
}
