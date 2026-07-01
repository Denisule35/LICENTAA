using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modern.View;

namespace Modern.ViewModel
{
    public class DeschidereInventarCommand:Commandbase
    {
        MainWindowViewModel _mainviewmodel;
        public DeschidereInventarCommand(MainWindowViewModel mainviewmodel)
        {
            _mainviewmodel = mainviewmodel;
        }
        public override void Execute(object parameter)
        {
            InventarView view = new InventarView();
            view.DataContext = new InventarViewModel(_mainviewmodel);
            view.ShowDialog();
        }
    }
}
