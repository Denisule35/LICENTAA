using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modern.View;

namespace Modern.ViewModel
{
    public class DeschiderePrezentaCommand: Commandbase
    {

        MainWindowViewModel _mainviewmodel;
        public DeschiderePrezentaCommand(MainWindowViewModel mainviewmodel)
        {
            _mainviewmodel = mainviewmodel;
        }
        public override void Execute(object parameter)
        {
            PrezentaView view = new PrezentaView();
            view.DataContext = new PrezentaViewModel(_mainviewmodel);
            view.ShowDialog();
        }
    }
}
