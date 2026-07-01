using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Modern.Model;

namespace Modern.ViewModel
{
    public class AdaugareOmCommand : Commandbase
    {

        AdaugareOmViewModel _adaugareomviewmodel;

        public AdaugareOmCommand(AdaugareOmViewModel adaugareOmViewModel)
        {
            _adaugareomviewmodel = adaugareOmViewModel;
        }

        public override void Execute(object parameter)
        {
            if(parameter is Window wind)
            {

                 OameniViewModel om = new OameniViewModel(_adaugareomviewmodel.Username,DateOnly.FromDateTime(DateTime.Now),_adaugareomviewmodel._mainwindowviewmodel);

                if (_adaugareomviewmodel._mainwindowviewmodel.elight == true)
                {
                    om.Gri = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#98D2C0"));
                    om.Galben = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#205781"));
                    om.Rosu = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4F959D"));
                    om.Negru = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F6F8D5"));
                    if (DateOnly.FromDateTime(DateTime.Today) > om.data.AddMonths(1))
                    {
                        om.expirat = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#205781"));
                    }
                    else
                    {
                        om.expirat = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F6F8D5"));
                    }
                }
                else
                {
                    om.Gri = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#343131"));
                    om.Galben = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D8A25E"));
                    om.Rosu = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A04747"));
                    om.Negru = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#242222"));
                    if (DateOnly.FromDateTime(DateTime.Today) > om.data.AddMonths(1))
                    {
                        om.expirat = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A04747"));
                    }
                    else
                    {
                        om.expirat = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D8A25E"));
                    }
                }


                Oameni bzom = new Oameni()
                {

                    Name = om.nume,
                    Abonament = om.data,
                    ArePoza = false,
                    PumniNimeritiCap = 0,
                    PumniNimeritiCorp = 0,
                    PumniIncasatiCap = 0,
                    PumniIncasatiCorp = 0,
                    PumniRatati = 0,
                    ProcentajPrezenta = 0,
                    NumarVicotorii = 0,
                    NumarInfrangeri = 0,
                    NumarRemize = 0,


                };
                using (Bazadateconnect bazadedate = new Bazadateconnect())
                {
                    bazadedate.Oameni.Add(bzom);
                    

                    Tranzactie tranz = new Tranzactie()
                    {
                        Data = DateTime.Now,
                        Suma = _adaugareomviewmodel._mainwindowviewmodel._pretabonament,
                        EVenit = true,
                        Descriere = $"Un nou membru a fost inscris : {om.nume} Abonament:{_adaugareomviewmodel._mainwindowviewmodel._pretabonament}"

                    };

                    bazadedate.Tranzactii.Add(tranz);
                    bazadedate.SaveChanges();
                }

                _adaugareomviewmodel._mainwindowviewmodel._oameni.Add(om);

                wind.Close();

            }
        }
    }
}
