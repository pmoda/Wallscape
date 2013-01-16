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


//Represents a model of the map//
//Safe1 = the goal area for Player 1 (Bottom)
//Safe2 = the goal area for Player 2 (Top)
//Open = No wall has been placed yet
//Closed = Wall is built, can not cross
//Player1 = Space is occupied by another token
//Player2 = Space is occupied by another token
//Free = Unoccupied, token can navigate there
//Void = Unreachable square
//Button = space where the click virtually occurs
//Special = special square where something happens (futur)

namespace Wallscape.Models
{
    public class Map
    {
        int width;
        int height;

        String[,] board;
        int[,] coverageCheck;
        List<Wall> walls;
        List<Player> players;
        List<Token> tokens;

        public Map()
        {
            walls = new List<Wall>();
            players = new List<Player>();
            tokens = new List<Token>();
            Token.tokenNum = 0;

            Player p1 = new Player();
            Player p2 = new Player();
            p1.setName(Settings.getInstance().getPlayer1Name());
            p2.setName(Settings.getInstance().getPlayer2Name());
            players.Add(p1);
            players.Add(p2);

            //width = 13;
            //height = 19;

            width = (Settings.getInstance().getWidth() * 2) + 1;
            height = (Settings.getInstance().getHeight() * 2) + 3;
            board = new String[width+2, height+2];
            initializeBoard();
            placeTokens();

            coverageCheck = new int[width, height];
            resetCovereage();

        }

        public void resetCovereage()
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    coverageCheck[i, j] = 0;
                }
            }
        }

        public void placeToken(int x, int y, int p)
        {
            if (x >= 0 && y >= 0 && x < board.GetLength(0) && y < board.GetLength(1))
            {
                board[x, y] = "Player" + p.ToString(); 

                Token newToken = new Token(p);
                newToken.setX(x);
                newToken.setY(y);
                players[p].addToken(newToken);
                tokens.Add(newToken);
            }
        }

        public void placeTokens()
        {
            int counter = Settings.getInstance().getTokens();
            int oddOffset = Settings.getInstance().getWidth() % 2;
            int mid = Settings.getInstance().getWidth() + oddOffset;

            placeToken(mid - 1, 2, 0);
            placeToken(mid + 1, 2, 0);
            placeToken(mid - 1, height - 3, 1);
            placeToken(mid + 1, height - 3, 1);

            if (counter >= 3){
                placeToken(mid - 3, 2, 0);
                placeToken(mid - 3, height - 3, 1);
            }
            if (counter >= 4)
            {
                placeToken(mid + 3, 2, 0);
                placeToken(mid + 3, height - 3, 1);
            }
            if (counter >= 5)
            {
                placeToken(mid - 5, 2, 0);
                placeToken(mid - 5, height - 3, 1);
            }
        }

        public List<Player> getPlayers()
        {
            return players;
        }
        public List<Token> getTokens()
        {
            return tokens;
        }
        //Type 1 upward
        //Type 2 downward
        //Type 3 right
        //Type 4 left
        public bool addWall(int xClick, int yClick, int type){
            
            int modelClickX = xClick * 2;
            int modelClickY = (yClick * 2) + 1;

            if (type == 1){
                return notDeadEnd(modelClickX, modelClickY + 1);
            }
            if (type == 2){
                return notDeadEnd(modelClickX, modelClickY - 1);
            }
            if (type == 3){
                return notDeadEnd(modelClickX - 1, modelClickY);
            }
            if (type == 4){
                return notDeadEnd(modelClickX + 1, modelClickY);
            }
            
            return false;
        }
        private bool notDeadEnd(int x, int y)
        {
            if(board[x, y] == "Closed")
                return false;
            board[x, y] = "Closed";
            foreach (Token t in tokens)
            {
                resetCovereage();
                if (!wayOutBFS(t.getX(), t.getY(), t.getPlayer()))
                {
                    board[x, y] = "Open";
                    return false;
                }
            }
            resetCovereage();
            return true;
        }

        private bool wayOutBFS(int x, int y, int player)
        {
            //Debug.WriteLine("START OF METHOD " + x + "," + y);
            coverageCheck[x, y] = 1;
            List<int[]> queue = new List<int[]>();
            int[] initial = new int[] { x, y };
            String safe = "Safe" + (player + 1);
            queue.Add(initial);

            while (queue.Count != 0)
            {
                int xCurrent = queue[0][0];
                int yCurrent = queue[0][1];
                queue.RemoveAt(0);

                //Debug.WriteLine("Checking at " + xCurrent + "," + yCurrent);

                coverageCheck[xCurrent, yCurrent] = 1;
                if (board[xCurrent, yCurrent] == safe)
                {
                    return true;
                }
                else if(!board[xCurrent, yCurrent].Contains("Safe")){
                    if (board[xCurrent - 1, yCurrent] != "Closed" && coverageCheck[xCurrent - 2, yCurrent] == 0){
                        queue.Add(new int[] { xCurrent - 2, yCurrent });
                    }
                    if (board[xCurrent + 1, yCurrent] != "Closed" && coverageCheck[xCurrent + 2, yCurrent] == 0){
                        queue.Add(new int[] { xCurrent + 2, yCurrent });
                    }
                    if (board[xCurrent, yCurrent - 1] != "Closed" && coverageCheck[xCurrent, yCurrent - 2] == 0){
                        queue.Add(new int[] { xCurrent, yCurrent - 2 });
                    }
                    if (board[xCurrent, yCurrent + 1] != "Closed" && coverageCheck[xCurrent, yCurrent + 2] == 0){
                        queue.Add(new int[] { xCurrent, yCurrent + 2 });
                    }   
                }
            }
            return false;
        }
               
        public void set(int x, int y, String status)
        {
            board[x, y] = status;
        }

        //This will set all the default values inside the board for the start of the sam
        //And sets the 4 tokens for each player
        public String at(int x, int y)
        {
            if (x >= width || x < 0) { return "Void"; }
            if (y > height || y < 0) { return "Void"; }
            return board[x,y];
        }

        private void initializeBoard()
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (x % 2 == 1)
                    {
                        if (y == 0 || y == height - 1)
                        {
                            if (y == 0){ board[x,y] = "Safe2"; }
                            if (y == height - 1){ board[x, y] = "Safe1"; }
                        }
                        else if (y % 2 == 1)
                        {
                            board[x,y] = "Open";
                        }
                        else if (y % 2 == 0)
                        {
                            board[x, y] = "Free";
                         /*   if(y == 2){                               
                                if (x >= 3 && x <= 9)
                                {
                                    board[x, y] = "Player1";
                                    Token newToken = new Token(players[0].getId());
                                    newToken.setX(x);
                                    newToken.setY(y);
                                    tokens.Add(newToken);
                                    players[0].addToken(newToken);
                                }
                            }
                            else if (y == height - 3)
                            {
                                if (x >= 3 && x <= 9)
                                {
                                    board[x, y] = "Player2";
                                    Token newToken = new Token(players[1].getId());
                                    newToken.setX(x);
                                    newToken.setY(y);
                                    players[1].addToken(newToken);
                                    tokens.Add(newToken);
                                }
                            }
                            else { board[x, y] = "Free"; }  
                        */
                        }
                    }
                    else if (x % 2 == 0)
                    {
                        if (y == 0 || y == height - 1)
                        {
                            board[x,y] = "Void";
                        }
                        else if (y % 2 == 1)
                        {
                            board[x,y] = "Button";
                        }
                        else if (y % 2 == 0)
                        {
                            if(x==0||x==width-1) board[x,y] = "Closed";
                            else board[x, y] = "Open";
                        }
                    }
                }
            }
        }//end of initialize board


    }
}
