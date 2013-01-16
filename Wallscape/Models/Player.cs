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

namespace Wallscape.Models
{
    public class Player
    {
        const int PLAYER_NUMBER = 2;
        List<Token> tokens;
        
        static int num = 0;
        String name;
        private int id;

        public Player()
        {
            tokens = new List<Token>();
            id = (num++ % PLAYER_NUMBER);
            name = "P" + id;
        }

        public void setId(int nid)
        {
            id = nid;
        }
        public int getId()
        {
            return id;
        }
        public void setName(String sName)
        {
            name = sName;
        }
        public String getName()
        {
            return name;
        }
        public List<Token> getTokens()
        {
            return tokens;
        }

        public void addToken(Token token)
        {
            token.setPlayer(this.id);
            tokens.Add(token);
        }

        public bool hasWon()
        {
            foreach(Token t in tokens)
            {
                if (!t.getSafe())
                    return false;
            }
            return true;
        }

    }
}
