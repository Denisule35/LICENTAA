using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

namespace Modern.ViewModel
{
    public class EditarePozaArticolCommand : Commandbase
    {


        EditareArticolViewModel _articol;
        bool _schimbat;

        public EditarePozaArticolCommand(EditareArticolViewModel articol)
        {
            this._articol = articol;

        }
        public override void Execute(object parameter)
        {

            var dialog = new OpenFileDialog();
            dialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.jfif;*.webp";

            if (dialog.ShowDialog() != true)
                return;

            string selectedImagePath = dialog.FileName;
            string extension = Path.GetExtension(selectedImagePath);

            string destinationFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "imagini");
            Directory.CreateDirectory(destinationFolder);

           
            var oldFiles = Directory.GetFiles(destinationFolder, _articol.Denumire + ".*");
            foreach (var file in oldFiles)
                File.Delete(file);

            string destinationPath = Path.Combine(destinationFolder, _articol.Denumire + extension);

            File.Copy(selectedImagePath, destinationPath, true);

            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.UriSource = new Uri(destinationPath);
            bitmap.EndInit();
            bitmap.Freeze();

            _articol.ArticolImage = bitmap;
            _articol._articol.ProfileImage = bitmap;
        }
    }
}
