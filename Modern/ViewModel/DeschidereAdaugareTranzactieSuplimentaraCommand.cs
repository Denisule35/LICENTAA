using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modern.View;

namespace Modern.ViewModel
{
    public class DeschidereAdaugareTranzactieSuplimentaraCommand : Commandbase
    {
        private TranzactiiViewModel tranzactiiViewModel;

        public DeschidereAdaugareTranzactieSuplimentaraCommand(TranzactiiViewModel tranzactiiViewModel)
        {
            this.tranzactiiViewModel = tranzactiiViewModel;
        }

        public override void Execute(object parameter)
        {
            AdaugareTranzactieSuplimentara saba = new AdaugareTranzactieSuplimentara();
            saba.DataContext = new AdaugareTranzactieSuplimentaraViewModel(tranzactiiViewModel);
            saba.ShowDialog();
        }
    }
}
