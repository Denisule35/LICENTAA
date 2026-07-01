using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modern.View;

namespace Modern.ViewModel
{
    public class DeschidereAnalizaAiCommand : Commandbase
    {
        MainWindowViewModel _mainviewmodel;
        public DeschidereAnalizaAiCommand(MainWindowViewModel mainviewmodel)
        {
            _mainviewmodel = mainviewmodel;
        }
        public override void Execute(object parameter)
        {
            AnalizaAiView view = new AnalizaAiView();
            view.DataContext = new AnalizaAiViewModel(_mainviewmodel);
            view.ShowDialog();
        }
    }
}
