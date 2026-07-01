using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modern.Model;
using Modern.View;

namespace Modern.ViewModel
{
    public class DeschidereStergereAdminCommand : Commandbase
    {

        MainWindowViewModel _mainviewmodel;

        public DeschidereStergereAdminCommand(MainWindowViewModel mainviewmodel)
        {
            _mainviewmodel = mainviewmodel;
        }

        public override void Execute(object parameter)
        {
            StergereAdminView view = new StergereAdminView();
            view.DataContext = new StergereAdminViewModel(_mainviewmodel);
            view.ShowDialog();
        }
    }
}
