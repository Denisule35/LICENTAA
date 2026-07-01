using System.Windows.Input;
using Modern.View;

namespace Modern.ViewModel
{
    internal class DeschideTranzactiiCommand : Commandbase
    {
        private LunaFinanataViewModel lunaFinanataViewModel;

        public DeschideTranzactiiCommand(LunaFinanataViewModel lunaFinanataViewModel)
        {
            this.lunaFinanataViewModel = lunaFinanataViewModel;
        }

        public override void Execute(object parameter)
        {
            TranzactiiView saba = new TranzactiiView();
            saba.DataContext = new TranzactiiViewModel(lunaFinanataViewModel);
            saba.ShowDialog();
        }
    }
}