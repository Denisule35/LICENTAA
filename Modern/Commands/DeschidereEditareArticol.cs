using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modern.View;

namespace Modern.ViewModel
{
    public class DeschidereEditareArticol : Commandbase
    {
        ArticolViewModel _articol;

        public DeschidereEditareArticol(ArticolViewModel articol)
        {
            _articol = articol;
        }

        public override void Execute(object parameter)
        {
            EditareArticolView saba = new EditareArticolView();
            saba.DataContext = new EditareArticolViewModel(_articol);
            saba.ShowDialog();
        }
    }
}
