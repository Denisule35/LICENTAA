using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modern.View;

namespace Modern.ViewModel
{
    public class DeschidereAdaugareOmCommand : Commandbase
    {

        MainWindowViewModel _mainwindowviewmodel;

       public  DeschidereAdaugareOmCommand(MainWindowViewModel mainwindowviewmodel)
        {
            _mainwindowviewmodel = mainwindowviewmodel;
        }

            public override void Execute(object parameter)
        {
            AdaugareOmViewModel adg = new AdaugareOmViewModel(_mainwindowviewmodel);
            var window = new AdaugareOm();
            window.DataContext = adg;
            window.ShowDialog();
        }
    }
}
