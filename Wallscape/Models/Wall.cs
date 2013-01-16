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
    public class Wall
    {
        private int startX;
        private int startY;
        private int endX;
        private int endY;

        public Wall(int x1, int x2, int y1, int y2)
        { 
            startX = x1;
            startY = y1;
            endX = x2;
            endY = y2;
        }

        public void setSX(int sx)
        {
            startX = sx;
        }
        public void setSY(int sy)
        {
            startY = sy;
        }
        public void setEX(int ex)
        {
            endX = ex;
        }
        public void setEY(int ey)
        {
            endY = ey;
        }
        public int getSX()
        {
            return startX;
        }
        public int getSY()
        {
            return startY;
        }
        public int getEX()
        {
            return endX;
        }
        public int getEY()
        {
            return endY;
        }
    }
}
