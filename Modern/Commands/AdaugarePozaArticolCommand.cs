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
    public class AdaugarePozaArticolCommand : Commandbase
    {

        AdaugareArticolViewModel _articol;

        public AdaugarePozaArticolCommand(AdaugareArticolViewModel articol)
        {
            this._articol = articol;
        }
        public override void Execute(object parameter)
        {

            var dialog = new OpenFileDialog();
            dialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.jfif;*.webp";

            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                string selectedImagePath = dialog.FileName;
                string extension = Path.GetExtension(selectedImagePath);
                string destinationFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "imagini");
                Directory.CreateDirectory(destinationFolder);
                string destinationPath = Path.Combine(destinationFolder, _articol.Denumire + extension);

                File.Copy(selectedImagePath, destinationPath, true);

                _articol.ArticolImage = new BitmapImage(new Uri(destinationPath));
            }

           

        }
    }
    
}
