using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using Modern.Model;

namespace Modern.ViewModel
{
    public class AnalizaAICommand : Commandbase
    {
        OameniViewModel _omviewmodel;

        public AnalizaAICommand(OameniViewModel oameniviewmodel)
        {
            _omviewmodel = oameniviewmodel;
        }

        public override async void Execute(object parameter)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "Video Files|*.mp4;*.avi;*.mov;*.mkv;*.wmv";

            bool? result = dialog.ShowDialog();

            if (result != true)
                return;

            string selectedVideoPath = dialog.FileName;
            string extension = Path.GetExtension(selectedVideoPath);

            string destinationFolder =
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "VideoAi");

            Directory.CreateDirectory(destinationFolder);

            string destinationPath =
                Path.Combine(destinationFolder, _omviewmodel.nume + extension);

            try
            {
                
                string json = await Task.Run(() =>
                {
                    return CSnakesInitialization._module.AnalyzeFight(
                        selectedVideoPath,
                        _omviewmodel.nume,
                        "inamic",
                        destinationPath
                    );
                });

                FightResults? fightResults =
                    JsonSerializer.Deserialize<FightResults>(json);

                if (fightResults == null)
                {
                    MessageBox.Show("Failed to parse AI results.");
                    return;
                }

                if (!fightResults.Results.ContainsKey(_omviewmodel.nume))
                {
                    MessageBox.Show("Fighter not found in results.");
                    return;
                }

                FighterStats myStats =
                    fightResults.Results[_omviewmodel.nume];


                using (Bazadateconnect bz = new Bazadateconnect())
                {
                    Oameni om =
                        bz.Oameni.FirstOrDefault(o => o.Name == _omviewmodel.nume);

                    if (om != null)
                    {
                        om.PumniNimeritiCap += myStats.HeadLanded;
                        om.PumniNimeritiCorp += myStats.BodyLanded;
                        om.PumniIncasatiCap += myStats.HeadReceived;
                        om.PumniIncasatiCorp += myStats.BodyReceived;
                        om.PumniRatati += myStats.TotalMissed;

                        bz.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                MessageBox.Show("AI analysis completed. Results have been saved to the database.");
            }
        }
    }
}