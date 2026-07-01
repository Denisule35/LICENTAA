using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Modern.View;

namespace Modern.ViewModel
{
    public class DeschideFinanateCommand : Commandbase
    {

        MainWindowViewModel _mainviewmodel;

        public DeschideFinanateCommand(MainWindowViewModel mainviewmodel)
        {
            _mainviewmodel = mainviewmodel;
        }


        public override void Execute(object parameter)
        {
            FinanteView  saba = new FinanteView();
            saba.DataContext = new FinanteViewModel(_mainviewmodel);
            saba.ShowDialog();
        }
    }
}
