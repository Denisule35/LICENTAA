using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modern.View;

namespace Modern.ViewModel
{
    public class DeschidereAdaugareAdminCommand : Commandbase

    {

        MainWindowViewModel _mainviewmodel;

        public DeschidereAdaugareAdminCommand(MainWindowViewModel mainviewmodel)
        {
            _mainviewmodel = mainviewmodel;
        }

        public override void Execute(object parameter)
        {
            AdaugareAdmin saba = new AdaugareAdmin();
            saba.DataContext = new AdaugareAdminVIewModel(_mainviewmodel);
            saba.ShowDialog();
        }
    }
}
