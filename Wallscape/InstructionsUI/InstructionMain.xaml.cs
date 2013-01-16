using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace Wallscape.InstructionsUI
{
    public partial class InstructionMain : PhoneApplicationPage
    {
        public InstructionMain()
        {
            InitializeComponent();
            this.Background = new SolidColorBrush(Colors.Black);
            Background.Opacity = 1.0;
            
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/InstructionsUI/InstructionSettings.xaml", UriKind.Relative));
        }
    }
}