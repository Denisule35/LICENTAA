using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Modern.View
{
    /// <summary>
    /// Interaction logic for DetaliiSportiviView.xaml
    /// </summary>
    public partial class DetaliiSportiviView : Window
    {
        public DetaliiSportiviView()
        {
            InitializeComponent();
            var screen = SystemParameters.WorkArea;
            this.Left = screen.Right; // starts off-screen right
            this.Top = (screen.Height - this.Height) / 2;
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var screen = SystemParameters.WorkArea;
            double targetLeft = (screen.Width - this.Width) / 2;

            // Animate window Left from screen.Right → centered
            var anim = new DoubleAnimation
            {
                From = screen.Right,
                To = targetLeft,
                Duration = new Duration(TimeSpan.FromSeconds(0.55)),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };
            this.BeginAnimation(Window.LeftProperty, anim);

            // Also trigger the inner border slide + fade storyboard
            var sb = (Storyboard)FindResource("SlideInFromRight");
            sb.Begin();
        }

        private void Iesire(object sender, RoutedEventArgs e)
        {
            // Slide out to the right on close
            var screen = SystemParameters.WorkArea;
            var anim = new DoubleAnimation
            {
                To = screen.Right,
                Duration = new Duration(TimeSpan.FromSeconds(0.35)),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseIn }
            };
            anim.Completed += (s, _) => this.Close();
            this.BeginAnimation(Window.LeftProperty, anim);

            // Fade out
            var fadeOut = new DoubleAnimation
            {
                To = 0,
                Duration = new Duration(TimeSpan.FromSeconds(0.3))
            };
            RootBorder.BeginAnimation(OpacityProperty, fadeOut);
        }
    

       

       
    }
}
