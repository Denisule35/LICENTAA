using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modern.View;

namespace Modern.ViewModel
{
    public class DeschidereDetaliSportivCommand : Commandbase
    {

        OameniViewModel _oameniviewmodel;

        public DeschidereDetaliSportivCommand(OameniViewModel oameniviewmodel)
        {
            this._oameniviewmodel = oameniviewmodel;
        }

        public override void Execute(object parameter)
        {
            DetaliiSportiviView  detali= new DetaliiSportiviView();
            detali.DataContext = new DetaliSportiviViewModel(this._oameniviewmodel);
            detali.ShowDialog();
        }
    }
}
