using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modern.View;

namespace Modern.ViewModel
{
    public class DeschidereAdaugareArticolCommand:Commandbase
    {

        InventarViewModel _inventarviewmodel;

        public DeschidereAdaugareArticolCommand(InventarViewModel inventarviewmodel)
        {
            _inventarviewmodel = inventarviewmodel;
        }

        public override void Execute(object parameter)
        {
            AdaugareArticolView saba = new AdaugareArticolView();
            saba.DataContext = new AdaugareArticolViewModel(_inventarviewmodel);
            saba.ShowDialog();
        }
    }
}
