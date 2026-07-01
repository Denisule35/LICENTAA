using System.Windows.Input;
using Modern.View;

namespace Modern.ViewModel
{
    public class DeschidePrezentaZiCommand : Commandbase
    {
        private ZiPrezentaViewModel ziPrezentaViewModel;

        public DeschidePrezentaZiCommand(ZiPrezentaViewModel ziPrezentaViewModel)
        {
            this.ziPrezentaViewModel = ziPrezentaViewModel;
        }

        public override void Execute(object parameter)
        {
            
            PrezetaZiView view = new PrezetaZiView();
            view.DataContext = new PrezentaZiViewModel(ziPrezentaViewModel);
            view.ShowDialog();
        }
    }
}