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
    public partial class InstructionSettings : PhoneApplicationPage
    {
        public InstructionSettings()
        {
            InitializeComponent();
        }

        private void startGame_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/InstructionsUI/InstructionBoard.xaml", UriKind.Relative));
        }
    }
}