using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using Wallscape.Models;


namespace Wallscape.Controllers
{
    public class GameControl
    {
        List<Button> moveOptions;
        Token selectedT;
        Button selectedB;
        Map board;
        public int actionsPerTurn;
        public int actionCounter;

        public GameControl()
        {
            moveOptions = new List<Button>();
            board = new Map();
            selectedB = null;
            selectedT = null;
            actionsPerTurn = Wallscape.Models.Settings.getInstance().getActions();
            actionCounter = 0;
        }

        public Map getBoard()
        { return board; }
        public Button getSelectedButton()
        { return selectedB; }

        public List<Button> attemptMove(Button button)
        {
            selectedB = button;
            
            Token selectedToken = board.getTokens()[Convert.ToInt32(button.Name)];
            selectedT = selectedToken;
            int positionX = selectedToken.getX();
            int positionY = selectedToken.getY();
            int player = selectedToken.getPlayer();
            String status = "Safe" + (player+1);

            if (board.at(positionX, positionY + 2) == status && board.at(positionX, positionY + 1) == "Open")
            {
                moveOptions.Add(makeOptionButton(positionX, positionY + 2, player));
            }
            if (board.at(positionX, positionY - 2) == status && board.at(positionX, positionY - 1) == "Open")
            {
                moveOptions.Add(makeOptionButton(positionX, positionY - 2, player));
            }

            if (board.at(positionX, positionY - 1) == "Open" &&
                (board.at(positionX, positionY - 2) == "Free" || board.at(positionX, positionY - 2).Contains("Player")))
            {
                if (board.at(positionX, positionY - 2).Contains("Player")){ 
                   if(board.at(positionX, positionY - 3) == "Open" && board.at(positionX, positionY - 4) == "Free"){
                        moveOptions.Add(makeOptionButton(positionX, positionY - 4, player));
                   }
                   if(board.at(positionX - 1, positionY - 2) == "Open" && board.at(positionX - 2, positionY - 2) == "Free")
                   {
                       moveOptions.Add(makeOptionButton(positionX - 2, positionY - 2, player));
                   }
                   if(board.at(positionX + 1, positionY - 2) == "Open" && board.at(positionX + 2, positionY - 2) == "Free")
                   {
                       moveOptions.Add(makeOptionButton(positionX + 2, positionY - 2, player));
                   }

                }else{
                    moveOptions.Add(makeOptionButton(positionX, positionY - 2, player));
                }
            }
            if (board.at(positionX, positionY + 1) == "Open" && 
                (board.at(positionX, positionY + 2) == "Free" || board.at(positionX, positionY + 2).Contains("Player")))
            {
                if (board.at(positionX, positionY + 2).Contains("Player")){ 
                   if(board.at(positionX, positionY + 3) == "Open" && board.at(positionX, positionY + 4) == "Free"){
                        moveOptions.Add(makeOptionButton(positionX, positionY + 4, player));
                   }
                   if (board.at(positionX - 1, positionY + 2) == "Open" && board.at(positionX - 2, positionY + 2) == "Free")
                   {
                       moveOptions.Add(makeOptionButton(positionX - 2, positionY + 2, player));
                   }
                   if (board.at(positionX + 1, positionY + 2) == "Open" && board.at(positionX + 2, positionY + 2) == "Free")
                   {
                       moveOptions.Add(makeOptionButton(positionX + 2, positionY + 2, player));
                   }
                }else{
                    moveOptions.Add(makeOptionButton(positionX, positionY + 2, player));
                }
            }
            if (board.at(positionX - 1, positionY) == "Open" && 
                (board.at(positionX - 2, positionY) == "Free"|| board.at(positionX - 2, positionY).Contains("Player")))
            {
                if (board.at(positionX - 2, positionY).Contains("Player"))
                {
                    if (board.at(positionX - 3, positionY) == "Open" && board.at(positionX - 4, positionY) == "Free")
                    {
                        moveOptions.Add(makeOptionButton(positionX - 4, positionY, player));
                    }
                    if (board.at(positionX - 2, positionY - 1) == "Open" && board.at(positionX - 2, positionY - 2) == "Free")
                    {
                        moveOptions.Add(makeOptionButton(positionX - 2, positionY - 2, player));
                    }
                    if (board.at(positionX - 2, positionY + 1) == "Open" && board.at(positionX - 2, positionY + 2) == "Free")
                    {
                        moveOptions.Add(makeOptionButton(positionX - 2, positionY + 2, player));
                    }
                }
                else
                {
                    moveOptions.Add(makeOptionButton(positionX - 2, positionY, player));
                }
            }
            if (board.at(positionX + 1, positionY) == "Open" &&
                (board.at(positionX + 2, positionY) == "Free" || board.at(positionX + 2, positionY).Contains("Player")))
            {
                if (board.at(positionX + 2, positionY).Contains("Player"))
                {
                    if (board.at(positionX + 3, positionY) == "Open" && board.at(positionX + 4, positionY) == "Free")
                    {
                        moveOptions.Add(makeOptionButton(positionX + 4, positionY, player));
                    }
                    if (board.at(positionX + 2, positionY - 1) == "Open" && board.at(positionX + 2, positionY - 2) == "Free")
                    {
                        moveOptions.Add(makeOptionButton(positionX + 2, positionY - 2, player));
                    }
                    if (board.at(positionX + 2, positionY + 1) == "Open" && board.at(positionX + 2, positionY + 2) == "Free")
                    {
                        moveOptions.Add(makeOptionButton(positionX + 2, positionY + 2, player));
                    }
                }
                else
                {
                    moveOptions.Add(makeOptionButton(positionX + 2, positionY, player));
                }
            }

            return moveOptions;
        }

        private Button makeOptionButton(int x, int y, int player)
        {
            int gridX = ((x+1)  / 2 );
            int gridY = (y) / 2;
            Button newButton = new Button();
            newButton.Height = 65;
            newButton.Width = 65;
            newButton.Style = (Style)Application.Current.Resources["RoundButton"];
            newButton.Margin = new Thickness(0);

            Grid.SetColumn(newButton, gridX);
            Grid.SetRow(newButton, gridY);

            SolidColorBrush _borderBrush = MainPage.GetColorFromHexa("#21FFFFFF"); 
            newButton.Background = _borderBrush;
            if (player == 0)
            {   // Blue = P1
                SolidColorBrush _highlightBrush = MainPage.GetColorFromHexa("#51C5D118"); 
                newButton.Background = _highlightBrush;
            }
            else if (player == 1)
            {   // Yellow = P1
                SolidColorBrush _highlightBrush = MainPage.GetColorFromHexa("#5129C7F5"); 
                newButton.Background = _highlightBrush;
            }
            return newButton;
        }

        public void executeMove(Button move)
        {
            selectedB.Visibility = Visibility.Collapsed;
            cancelMove();
            int gridY = Grid.GetRow(move);
            int gridX = Grid.GetColumn(move);
            int posx = ((gridX) * 2)-1;
            int posy = gridY * 2;

            Token t = board.getTokens()[selectedT.getId()];
            board.set(t.getX(), t.getY(), "Free");

            if (board.at(posx, posy).Contains("Safe"))
            {
                selectedT.setSafe(true);
            }
            else
            {
                board.set(posx, posy, "Player" + (t.getPlayer() + 1));
            }
            t.setY(posy);
            t.setX(posx);

        }

        public void cancelMove()
        {
            foreach (Button b in moveOptions)
            {
                b.Visibility = Visibility.Collapsed;
            }
            moveOptions.RemoveRange(0, moveOptions.Count);
            selectedB = null;
        }

        public List<Player> getPlayers() { return board.getPlayers(); }
        
        public bool addWall(int xClick, int yClick, int type) { 
             return board.addWall(xClick, yClick, type); 
        }
        
        public void manageTurn()
        {
        }    
    }
}
