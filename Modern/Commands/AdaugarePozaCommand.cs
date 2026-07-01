using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using Modern.Model;

namespace Modern.ViewModel
{
    public  class AdaugarePozaCommand : Commandbase
    {

        DetaliSportiviViewModel _detalisportivviewmodel;

        public AdaugarePozaCommand(DetaliSportiviViewModel detali) { 
        this._detalisportivviewmodel = detali;
        }
        public override void Execute(object parameter)
        {

            var dialog = new OpenFileDialog();
            dialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.jfif";

            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                string selectedImagePath = dialog.FileName;
                string extension = Path.GetExtension(selectedImagePath);
                string destinationFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Imagini");
                Directory.CreateDirectory(destinationFolder);
                string destinationPath = Path.Combine(destinationFolder, _detalisportivviewmodel._oameni.nume + extension);

                File.Copy(selectedImagePath, destinationPath, true);

                _detalisportivviewmodel.ProfileImage = new BitmapImage(new Uri(destinationPath));
            }

            using (Bazadateconnect bz = new Bazadateconnect())
            {
                Oameni om = bz.Oameni.FirstOrDefault(o => o.Name == _detalisportivviewmodel._oameni.nume);

                om.ArePoza = true;
                bz.SaveChanges();
            }

        }
    }
}
