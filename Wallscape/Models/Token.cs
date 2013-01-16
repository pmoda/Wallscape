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
    public class Token
    {
        private int x;
        private int y;
        private int player;
        private bool safe; 

        public static int tokenNum = 0;
        private int id;
        public Token(int playerId)
        {
            int TOKEN_NUMBER = Settings.getInstance().getTokens() * 2;
            player = playerId;            
            safe = false;
            id = tokenNum % TOKEN_NUMBER;
            tokenNum++;
        }

        public int getId() { return id; }

        public bool getSafe() { return safe; }
        public void setSafe(bool nsafe) { safe = nsafe; }

        public int getX() { return x; }
        public int getY() { return y; }
        public void setX(int nx) { x = nx; }
        public void setY(int ny) { y = ny; }

        public int getPlayer() { return player; }
        public void setPlayer(int playerId) { player = playerId; }
    }
}
