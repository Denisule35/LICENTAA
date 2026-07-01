using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace Modern.ViewModel
{
    public class OameniViewModel:ViewModelBase
    {
        public MainWindowViewModel _mainviewmodel;
        public string nume { get; }

        private DateOnly _data;
        public DateOnly data
        {
            get => _data;
            set
            {
                _data = value;
                OnPropertyChanged(nameof(data));
            }
        }

        private SolidColorBrush _expirat;
        public SolidColorBrush expirat
        {
            get => _expirat;
            set
            {
                _expirat = value;
                OnPropertyChanged(nameof(expirat));
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


        public  ICommand StergereOm {  get; }

        public ICommand RenoireAbonament {  get; }

        public ICommand DeschidereDetaliSportiv { get; }

        public ICommand AnalizaAI { get; }

        public OameniViewModel(string nume, DateOnly data,MainWindowViewModel mainviewmodel)
        {
            this.nume = nume;
            this.data = data;
            _mainviewmodel = mainviewmodel;

            if (DateOnly.FromDateTime(DateTime.Today) > data.AddMonths(1))
            {

                
                expirat = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A04747")); 
            }
            else
            {
                expirat = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D8A25E"));
            }

            StergereOm = new StergereOmCommand(this);
            RenoireAbonament = new RenoireAbonamentCommand(this);
            DeschidereDetaliSportiv = new DeschidereDetaliSportivCommand(this);
            AnalizaAI = new AnalizaAICommand(this);

        } 
    }
}
