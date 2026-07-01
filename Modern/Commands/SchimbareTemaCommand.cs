using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
namespace Modern.ViewModel
{
    class SchimbareTemaCommand : Commandbase
    {

        MainWindowViewModel _mainviewmodel;

        public SchimbareTemaCommand(MainWindowViewModel mainviewmodel)
        {
            _mainviewmodel = mainviewmodel;
        }

        public override void Execute(object parameter)
        {
            


            if (_mainviewmodel.elight == false)
            {
                _mainviewmodel.Gri= new SolidColorBrush((Color)ColorConverter.ConvertFromString("#98D2C0"));
                _mainviewmodel.Galben = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#205781"));
                _mainviewmodel.Rosu = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4F959D"));
                _mainviewmodel.Negru = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F6F8D5"));

                _mainviewmodel.elight = true;
                

                foreach(var a in _mainviewmodel._oameni)
                {
                    a.Rosu = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4F959D"));
                    a.Negru = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F6F8D5"));
                    a.Galben  = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#205781"));
                    a.Gri = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#98D2C0"));
                    if(DateOnly.FromDateTime(DateTime.Today) > a.data.AddMonths(1))
                    {
                       a.expirat= new SolidColorBrush((Color)ColorConverter.ConvertFromString("#205781"));
                    }
                    else
                    {
                        a.expirat = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F6F8D5"));
                    }
                }
            }
            else
            {
                _mainviewmodel.Gri = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#343131"));
                _mainviewmodel.Galben = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D8A25E"));
                _mainviewmodel.Rosu = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A04747"));
                _mainviewmodel.Negru = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#242222"));

                

                foreach (var a in _mainviewmodel._oameni)
                {
                    a.Gri = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#343131"));
                    a.Galben = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D8A25E"));
                    a.Rosu = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A04747"));
                    a.Negru = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#242222"));
                    if (DateOnly.FromDateTime(DateTime.Today) > a.data.AddMonths(1))
                    {
                        a.expirat = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A04747"));
                    }
                    else
                    {
                        a.expirat = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D8A25E"));
                    }

                }

                _mainviewmodel.elight = false;

            }
        }
    }
}
