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
using Wallscape.Models;

namespace Wallscape
{
    public partial class Settings : PhoneApplicationPage
    {
        const int MAX_ACTIONS_PER_TURN = 6;
        const int MIN_ACTIONS_PER_TURN = 1;

        const int MAX_TOKENS_PER_PLAYER = 5;
        const int MIN_TOKENS_PER_PLAYER = 2;

        public Settings()
        {
            InitializeComponent();
            
            actions.Text = MIN_ACTIONS_PER_TURN.ToString();
            tokens.Text = MIN_TOKENS_PER_PLAYER.ToString();
            radioButton2.IsChecked = true;
            player1.MaxLength = 7;
            player2.MaxLength = 7;

            actions.Text = "3";
            tokens.Text = "4";
            radioButton2.IsChecked = true;

            boardThumb.Height = 120;
            boardThumb.Width = 90;
            boardThumb.Margin = new Thickness(30, 70, 0, 0);
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Board.xaml", UriKind.Relative));

            Wallscape.Models.Settings.getInstance().setPlayer1Name(player1.Text);
            Wallscape.Models.Settings.getInstance().setPlayer2Name(player2.Text);
            int action = Convert.ToInt32(actions.Text);
            Wallscape.Models.Settings.getInstance().setActions(action);
            int token = Convert.ToInt32(tokens.Text);
            Wallscape.Models.Settings.getInstance().setTokens(token);

            if (radioButton1.IsChecked == true)
            {
                Wallscape.Models.Settings.getInstance().setWidth(5);
                Wallscape.Models.Settings.getInstance().setHeight(6);
            }
            else if (radioButton2.IsChecked == true)
            {
                Wallscape.Models.Settings.getInstance().setWidth(6);
                Wallscape.Models.Settings.getInstance().setHeight(8);
            }
            else if (radioButton3.IsChecked == true)
            {
                Wallscape.Models.Settings.getInstance().setWidth(7);
                Wallscape.Models.Settings.getInstance().setHeight(9);
            }
        }

        private void default_click(object sender, RoutedEventArgs e)
        {
            actions.Text = "3" ;
            tokens.Text = "4";
            radioButton2.IsChecked = true;
            normal_click(sender, e);
        }
        private void quick_click(object sender, RoutedEventArgs e)
        {
            radioButton1.IsChecked = true;
            actions.Text = "1";
            tokens.Text = "3";
            small_click(sender, e);
        }
        private void epic_click(object sender, RoutedEventArgs e)
        {
            actions.Text = "4";
            tokens.Text = "5";
            radioButton3.IsChecked = true;
            large_click(sender, e);
        }


        private void action_plus(object sender, RoutedEventArgs e)
        {
            int action = Convert.ToInt32(actions.Text) + 1;
            if (action <= MAX_ACTIONS_PER_TURN)
            actions.Text = action.ToString();
        }
        private void action_minus(object sender, RoutedEventArgs e)
        {
            int action = Convert.ToInt32(actions.Text) - 1;
            if (action >= MIN_ACTIONS_PER_TURN)
            actions.Text = action.ToString();
        }
        private void token_plus(object sender, RoutedEventArgs e)
        {
            int token = Convert.ToInt32(tokens.Text) + 1;
            if (token <= MAX_TOKENS_PER_PLAYER)
                tokens.Text = token.ToString();
        }
        private void token_minus(object sender, RoutedEventArgs e)
        {
            int token = Convert.ToInt32(tokens.Text) - 1;
            if (token >= MIN_TOKENS_PER_PLAYER)
                tokens.Text = token.ToString();
        }

        private void large_click(object sender, RoutedEventArgs e)
        {
            boardThumb.Height = 140;
            boardThumb.Width = 110;
            boardThumb.Margin = new Thickness(20,60,0,0);
        }
        private void normal_click(object sender, RoutedEventArgs e)
        {
            boardThumb.Height = 120;
            boardThumb.Width = 90;
            boardThumb.Margin = new Thickness(30, 70, 0, 0);
        }
        private void small_click(object sender, RoutedEventArgs e)
        {
            boardThumb.Height = 100;
            boardThumb.Width = 70;
            boardThumb.Margin = new Thickness(40, 80, 0, 0);
        }
        
        private void tokens_LostFocus(object sender, RoutedEventArgs e)
        {
            int token;
            if (!int.TryParse(tokens.Text.Trim(), out token)) { tokens.Text = MIN_TOKENS_PER_PLAYER.ToString(); }
            else
            {
                token = Convert.ToInt32(tokens.Text);
                if (!(token <= MAX_TOKENS_PER_PLAYER && token >= MIN_TOKENS_PER_PLAYER))
                {
                    tokens.Text = MIN_TOKENS_PER_PLAYER.ToString();
                }
            }
        }

        private void action_LostFocus(object sender, RoutedEventArgs e)
        {
            int action;
            if (!int.TryParse(actions.Text.Trim(), out action)) { actions.Text = MIN_ACTIONS_PER_TURN.ToString(); }
            else
            {
                action = Convert.ToInt32(actions.Text);
                if (!(action <= MAX_ACTIONS_PER_TURN && action >= MIN_ACTIONS_PER_TURN))
                {
                    actions.Text = MIN_ACTIONS_PER_TURN.ToString();
                }
            }
        }
    }
}