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
using Wallscape.Controllers;

using System.Diagnostics;

namespace Wallscape
{
    public partial class Board : PhoneApplicationPage
    {
        int BOARD_WIDTH;
        int BOARD_HEIGHT;
        public int TOKEN_SIZE;
        GameControl controller;
        List<Button> player1;
        List<Button> player2;
        Button[,] wallButtons;
        List<Rectangle> walls;

        bool wallClick2;
        int xClick1;
        int yClick1;
        int xClick2;
        int yClick2;
        int turn;

        public Board()
        {
            InitializeComponent();
            BOARD_WIDTH = Wallscape.Models.Settings.getInstance().getWidth();
            BOARD_HEIGHT = Wallscape.Models.Settings.getInstance().getHeight();
            TOKEN_SIZE = 94 - BOARD_HEIGHT * 4;
            xClick1 = 0; yClick1 = 0;  
            initializeGrid();

            controller = new GameControl();
            player1 = new List<Button>();
            player2 = new List<Button>();
            walls = new List<Rectangle>();
            
            turn = -1;
            wallClick2 = false;
            refreshTokens();
            takeTurn();

            //controller.actionCounter = 1;
            Canvas.SetZIndex(rectangle1, -1);
            Canvas.SetZIndex(rectangle2, -1);
            Canvas.SetZIndex(rectangle3, -10);
            player1Name.Text = controller.getPlayers()[0].getName();
            player2Name.Text = controller.getPlayers()[1].getName();
                      
            //Create the wall connector buttons
            createWallButtons();
            
            //Insert the walls on the sides
            createSideWalls();                       
        }

        private void initializeGrid()
        {
            for (int i = 0; i <= BOARD_WIDTH + 1; i++)
            {
                if (i == 0 || i == BOARD_WIDTH + 1)
                {
                    ColumnDefinition c = new ColumnDefinition();
                    c.Width = new GridLength(32.0);
                    ContentPanel.ColumnDefinitions.Add(c);
                }
                else
                {
                    ColumnDefinition c = new ColumnDefinition();
                    ContentPanel.ColumnDefinitions.Add(c);
                }
            }
            for (int i = 0; i <= BOARD_HEIGHT + 1; i++)
            {
                if (i == 0 || i == BOARD_HEIGHT + 1)
                {
                    RowDefinition r = new RowDefinition();
                    r.Height = new GridLength(TOKEN_SIZE + 5);
                    ContentPanel.RowDefinitions.Add(r);
                }
                else
                {
                    RowDefinition r = new RowDefinition();
                    ContentPanel.RowDefinitions.Add(r);
                }
            }
            //ColumnDefinition cc = new ColumnDefinition();
            // GridLength gl = new GridLength();

            // ContentPanel.ColumnDefinitions.Add(cc);
            Grid.SetColumnSpan(rectangle1, BOARD_WIDTH + 2);
            Grid.SetColumnSpan(rectangle2, BOARD_WIDTH + 2);
            Grid.SetColumnSpan(rectangle3, BOARD_WIDTH + 2);
            Grid.SetRowSpan(rectangle3, BOARD_HEIGHT + 2);
            Grid.SetRow(rectangle1, BOARD_HEIGHT + 1);
        }

        private void createWallButtons()
        {
            wallButtons = new Button[BOARD_WIDTH + 1, BOARD_HEIGHT + 1];

            for (int y = 0; y <= BOARD_HEIGHT; y++)
            {
                for (int x = 0; x <= BOARD_WIDTH; x++)
                {
                    Button newButton = new Button();
                    newButton.Height = 54;
                    newButton.Width = 54;
                    newButton.Style = (Style)Application.Current.Resources["RoundButton"];

                    if (x == 0) { newButton.Margin = new Thickness(10); newButton.HorizontalAlignment = HorizontalAlignment.Left; }
                    else if (x == BOARD_WIDTH) { newButton.Margin = new Thickness(10); newButton.HorizontalAlignment = HorizontalAlignment.Right; }
                    else { newButton.Margin = new Thickness(25); }

                    Grid.SetColumn(newButton, x);
                    Grid.SetRow(newButton, y);
                    Grid.SetColumnSpan(newButton, 2);
                    Grid.SetRowSpan(newButton, 2);

                    String name = x.ToString() + y.ToString();
                    newButton.Name = name;
                    wallButtons[x, y] = newButton;

                    newButton.Click += new RoutedEventHandler(makeWall_Click);
                    ContentPanel.Children.Add(newButton);

                }
            }
        }
        
        private void createSideWalls()
        {
            //Make the side walls
            for (int y = 0; y < BOARD_HEIGHT; y++)
            {
                yClick1 = y;
                //computeWall(y + 1, 0);
                drawWall(y + 1, 0);
                xClick1 = BOARD_WIDTH;
                //computeWall(y + 1, BOARD_WIDTH-1);
                drawWall(y + 1, BOARD_WIDTH);
                xClick1 = 0;
            }
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            wallClick2 = false;
            refreshTokens();
            int saveTurn = turn;
            int saveAction = controller.actionCounter;
            takeTurn();
            controller.actionCounter = saveAction;
            turn = saveTurn;
            //controller.actionCounter--;
        }   

        private void makeWall_Click(object sender, RoutedEventArgs e)
        {
            SolidColorBrush _whiteBrush = MainPage.GetColorFromHexa("#FFFFFFFF");
            wallButtons[xClick1, yClick1].BorderBrush = _whiteBrush;
            wallButtons[xClick2, yClick2].BorderBrush = _whiteBrush;

            deselect_Token(controller.getSelectedButton(), e);
            if (wallClick2)
            {
                xClick2 = Grid.GetColumn((Button)sender);
                yClick2 = Grid.GetRow((Button)sender);
                               
                //Compute where to place the wall (4 cases)
                if (computeWall(yClick2, xClick2))
                {
                    drawWall(yClick2, xClick2);
                    takeTurn();
                }
                else
                {
                    SolidColorBrush _highlightBrush = MainPage.GetColorFromHexa("#FF861A1A");                
                    //Turn Labels
                    TURNLABEL.Foreground = _highlightBrush;
                    TURNLABEL.Text = "INVALID WALL!";
                    wallButtons[xClick1, yClick1].BorderBrush = _highlightBrush;
                    wallButtons[xClick2, yClick2].BorderBrush = _highlightBrush;
                    //actionLeft.Text = "
                }

                wallClick2 = false;
            }
            else
            {
                wallClick2 = true;
                xClick1 = Grid.GetColumn((Button)sender);
                yClick1 = Grid.GetRow((Button)sender);

                SolidColorBrush _selectBrush = MainPage.GetColorFromHexa("#61FFFFFF");
                wallButtons[xClick1, yClick1].BorderBrush = _selectBrush;
            }
        }

        // Will check if wall can follow from previous click either:
        /*
         * Same Row
         *      Col+1 
         *      Col-1
         * Same Col
         *      Row+1
         *      Row-1
         */
        private void drawWall(int row, int col)
        {
            Color _buttonHighlightColor = new Color();
            SolidColorBrush _highlightBrush = null;
            // Orange = active
            _buttonHighlightColor.R = 255;
            _buttonHighlightColor.G = 255;
            _buttonHighlightColor.B = 255;
            _buttonHighlightColor.A = 185;
            _highlightBrush = new SolidColorBrush(_buttonHighlightColor);

            Rectangle wall = new Rectangle();
            wall.Margin = new Thickness(-2.5);
            wall.Stroke = _highlightBrush;
            wall.StrokeThickness = 7;
            Canvas.SetZIndex(wall, -10);

            if (col == xClick1)
            {
                if (row == yClick1 + 1)
                {
                    wall.Height = (700 - (2 * TOKEN_SIZE)) / (BOARD_HEIGHT);
                    wall.Width = 7;
                    wall.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                    wall.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;

                    Grid.SetRow(wall, row);
                    Grid.SetColumn(wall, col);
                    walls.Add(wall);
                    ContentPanel.Children.Add(wall);
                }
                else if (row == yClick1 - 1)
                {
                    wall.Height = (700 - (2 * TOKEN_SIZE)) / (BOARD_HEIGHT);
                    wall.Width = 7;
                    wall.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                    wall.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;

                    Grid.SetRow(wall, row + 1);
                    Grid.SetColumn(wall, col);
                    walls.Add(wall);
                    ContentPanel.Children.Add(wall);
                }
            }
            if (row == yClick1)
            {
                if (col == xClick1 + 1)
                {
                    wall.Height = 7;
                    wall.Width = 480 / (BOARD_WIDTH + 1);
                    wall.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
                    Canvas.SetZIndex(wall, -1);

                    Grid.SetRow(wall, row);
                    Grid.SetColumn(wall, col);
                    walls.Add(wall);
                    ContentPanel.Children.Add(wall);
                }
                else if (col == xClick1 - 1)
                {
                    wall.Height = 7;
                    wall.Width = 480 / (BOARD_WIDTH + 1);
                    wall.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;

                    Grid.SetRow(wall, row);
                    Grid.SetColumn(wall, col + 1);
                    walls.Add(wall);
                    ContentPanel.Children.Add(wall);
                }
            }

           // return false;
        }
        private bool computeWall(int row, int col)
        {
            if (col == xClick1){
                if (row == yClick1 + 1){
                    return controller.addWall(xClick1, yClick1, 1);
                }
                else if (row == yClick1 - 1){
                    return controller.addWall(xClick1, yClick1, 2);
                }
            }
            else if (row == yClick1){
                if (col == xClick1 + 1){
                    return controller.addWall(xClick1, yClick1, 4);
                }
                else if (col == xClick1 - 1){
                    return controller.addWall(xClick1, yClick1, 3);
                }
            }

            return false;
        }
       
        public Button makeToken(int x,int y)
        {
            int gridX = ((x + 1) / 2);
            int gridY = y / 2;
            Button newButton = new Button();
            newButton.Height = TOKEN_SIZE;
            newButton.Width = TOKEN_SIZE;
            newButton.Style = (Style)Application.Current.Resources["RoundButton"];
            newButton.Margin = new Thickness(0);

            Grid.SetColumn(newButton, gridX);
            Grid.SetRow(newButton, gridY);

            newButton.Click += new RoutedEventHandler(select_Token);
            ContentPanel.Children.Add(newButton);

            return newButton;
        }        
        //Token moving actions
        private void select_Token(object sender, RoutedEventArgs e)
        {
            wallClick2 = false;
            SolidColorBrush _whiteBrush = MainPage.GetColorFromHexa("#FFFFFFFF");
            wallButtons[xClick1, yClick1].BorderBrush = _whiteBrush;
            wallButtons[xClick2, yClick2].BorderBrush = _whiteBrush;

            Button selectedToken = (Button)sender;
            selectedToken.Click += new RoutedEventHandler(deselect_Token);
            selectedToken.Click -= new RoutedEventHandler(select_Token);

            foreach (Button other in player1) {
                if(selectedToken.Name != other.Name)
                { other.IsEnabled = false; }
            }
            foreach (Button other in player2)
            {
                if (selectedToken.Name != other.Name)
                { other.IsEnabled = false; }
            }
            //int counter = 0;
            //debug.Text = counter.ToString();
            //debug.TextWrapping = TextWrapping.Wrap;
            foreach (Button option in controller.attemptMove(selectedToken))
            {
                //counter++;
                //debug.Text = counter.ToString();
                // debug.Text += " " + Grid.GetColumn(option) +","+Grid.GetRow(option);
                option.Click += new RoutedEventHandler(make_Move);
                option.Width = TOKEN_SIZE;
                option.Height = TOKEN_SIZE;
                ContentPanel.Children.Add(option);
            }
        }
        private void make_Move(object sender, RoutedEventArgs e)
        {
            controller.executeMove((Button)sender);
            refreshTokens();
            takeTurn();
        }
        private void deselect_Token(object sender, RoutedEventArgs e)
        {
            if (controller.getSelectedButton() != null)
            {
                controller.cancelMove();
                ((Button)sender).Click += new RoutedEventHandler(select_Token);
                ((Button)sender).Click -= new RoutedEventHandler(deselect_Token);

                if (turn % 2 == 0)
                {
                    foreach (Button other in player1) { other.IsEnabled = true; }
                    foreach (Button other in player2) { other.IsEnabled = false; }
                }
                else
                {
                    foreach (Button other in player1) { other.IsEnabled = false; }
                    foreach (Button other in player2) { other.IsEnabled = true; }
                }
            }
        }

        private void takeTurn()
        {
            TURNLABEL.Text = "OPERATION";
            if (controller.actionCounter < controller.actionsPerTurn)
            {
                if (turn < 0) { turn++; controller.actionCounter = (controller.actionsPerTurn + 1) / 2; }
                else { controller.actionCounter++; }
            }
            else
            {
                controller.actionCounter = 1;
                turn++;
            }
            actionLeft.Text = controller.actionCounter + " / " + controller.actionsPerTurn;
            if (turn % 2 == 0)
            {
                foreach (Button other in player1) { other.IsEnabled = true; }
                foreach (Button other in player2) { other.IsEnabled = false; }
                SolidColorBrush _highlightBrush = MainPage.GetColorFromHexa("#6129C7F5");
                player2tokens.Foreground = _highlightBrush;
                player2Name.Foreground = _highlightBrush;
                _highlightBrush = MainPage.GetColorFromHexa("#FFC5D118");
                player1tokens.Foreground = _highlightBrush;
                player1Name.Foreground = _highlightBrush;
                
                //Turn Labels
                TURNLABEL.Foreground = _highlightBrush;
                TURNLABEL.Text = "OPERATION";
                p1TurnArrow.Foreground = _highlightBrush;
                p1TurnArrow.Text = "<";
                p2TurnArrow.Text = "";
                actionLeft.Foreground = _highlightBrush;
            }
            else
            {
                foreach (Button other in player1) { other.IsEnabled = false; }
                foreach (Button other in player2) { other.IsEnabled = true; }
                SolidColorBrush _highlightBrush = MainPage.GetColorFromHexa("#61C5D118");
                player1tokens.Foreground = _highlightBrush;
                player1Name.Foreground = _highlightBrush;
                _highlightBrush = MainPage.GetColorFromHexa("#FF29C7F5");
                player2tokens.Foreground = _highlightBrush;
                player2Name.Foreground = _highlightBrush;

                //Turn Labels
                TURNLABEL.Foreground = _highlightBrush;
                TURNLABEL.Text = "OPERATION";
                p2TurnArrow.Foreground = _highlightBrush;
                p2TurnArrow.Text = ">";
                p1TurnArrow.Text = "";
                actionLeft.Foreground = _highlightBrush;
            }

            foreach (Player p in controller.getPlayers())
            {
                if (p.hasWon())
                {
                    MessageBox.Show(p.getName() + " has escaped to safety!", "VICTORY", MessageBoxButton.OK);
                    foreach (Button b1 in player1) { b1.Visibility = Visibility.Collapsed; }
                    foreach (Button b2 in player2) { b2.Visibility = Visibility.Collapsed; }
                    foreach (Rectangle r in walls) { r.Visibility = Visibility.Collapsed; }

                    controller = new GameControl();
                    player1 = new List<Button>();
                    player2 = new List<Button>();
                    walls = new List<Rectangle>();

                    //turn = -1;
                    turn++;
                    controller.actionCounter++;
                    wallClick2 = false;
                    refreshTokens();
                    takeTurn();
                    createSideWalls();
                }
            }
            wallClick2 = false;
        }
        public void refreshTokens()
        {
            foreach (Button b in player1) { b.Visibility = Visibility.Collapsed; }
            foreach (Button b in player2) { b.Visibility = Visibility.Collapsed; }
            player1.RemoveRange(0, player1.Count);
            player2.RemoveRange(0, player2.Count);
            player1tokens.Text = "";
            player2tokens.Text = "";

            foreach (Player p in controller.getPlayers())
            {
                foreach (Token t in p.getTokens())
                {
                    if (!t.getSafe())
                    {
                        Button newToken = makeToken(t.getX(), t.getY());
                        newToken.Name = t.getId().ToString();
                        if (p.getId() == 0){
                            // Gold = P1
                            SolidColorBrush _highlightBrush = MainPage.GetColorFromHexa("#FFC5D118"); ;
                            newToken.Background = _highlightBrush;
                            newToken.Padding = new Thickness(0);
                            newToken.FontSize = 16;
                            newToken.Content = "G";
                            player1.Add(newToken);
                            player1tokens.Text += " - ";
                        }
                        else if (p.getId() == 1){
                            // Blue = P2
                            SolidColorBrush _highlightBrush = MainPage.GetColorFromHexa("#FF29C7F5"); ;
                            newToken.Background = _highlightBrush;
                            newToken.Padding = new Thickness(0);
                            newToken.FontSize = 16;
                            newToken.Content = "B";
                            player2.Add(newToken);
                            player2tokens.Text += " - ";
                        }
                    }
                    else{
                        if (p.getId() == 0) { player1tokens.Text += " O "; }
                        if (p.getId() == 1) { player2tokens.Text = " O " + player2tokens.Text; }
                    }
                }
            }
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}