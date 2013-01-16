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

namespace Wallscape.Models
{
    public class Settings
    {
        static Settings instance = null;

        String player1Name;
        String player2Name;
        int boardWidth;
        int boardHeight;
        int tokensPerPlayer;
        int actionsPerTurn;

        private Settings()
        {
           
        }

        static public Settings getInstance()
        {
            if (instance == null){
                instance = new Settings();
            }
            return instance;
        }

        public String getPlayer1Name(){return player1Name;}
        public String getPlayer2Name(){return player2Name;}
        public int getWidth(){return boardWidth;}
        public int getHeight(){return boardHeight;}
        public int getTokens() {return tokensPerPlayer;}
        public int getActions() { return actionsPerTurn;}

        public void setPlayer1Name(String name){player1Name = name;}
        public void setPlayer2Name(String name){player2Name = name;}
        public void setHeight(int height) { boardHeight = height; }
        public void setWidth(int width) { boardWidth = width; }
        public void setActions(int action) { actionsPerTurn = action; }
        public void setTokens(int token) { tokensPerPlayer = token; }
    }
}
