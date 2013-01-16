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
using Wallscape.InstructionsUI;

namespace Wallscape
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            //gamePage = new Board();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //App.Current.RootVisual = new Board();
            NavigationService.Navigate(new Uri("/Settings.xaml", UriKind.Relative));
            //NavigationService.Navigate(new Uri("/Page1.xaml", UriKind.Relative));
            
       //     (App.Current.RootVisual as PhoneApplicationFrame).Navigate(
        //                            new Uri("/Board.xaml", UriKind.Relative)); 
        }

        public static SolidColorBrush GetColorFromHexa(string hexaColor)
        {
            return new SolidColorBrush(
                Color.FromArgb(
                    Convert.ToByte(hexaColor.Substring(1, 2), 16),
                    Convert.ToByte(hexaColor.Substring(3, 2), 16),
                    Convert.ToByte(hexaColor.Substring(5, 2), 16),
                    Convert.ToByte(hexaColor.Substring(7, 2), 16)
                )
            );
        }

        private void button1_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/InstructionsUI/InstructionMain.xaml", UriKind.Relative));
        }
    }
}