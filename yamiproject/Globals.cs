using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace yamiproject
{
   public class Globals
   {
       public struct Teleport
       {
           public int x, type, where;
           public Point mariopos;

           public Teleport(int p1, int p2, int p3, Point tmp)
           {
               x = p1;
               type = p2;
               where = p3;
               mariopos = tmp;
           }
       }

       
       public static Point mario_res = new Point(256, 224);
       public static Point title = new Point(16, 16);
       public static int yscanlineoffset = 8;
       public static int scale = 2;

       public static int Scale
       {
            get { return scale; }
            set 
            {
                if (value > 3)
                    scale = 3;
                else if (value < 1)
                    scale = 1;
                else
                    scale = value;
            }
       }

       public static Point ConvertPositionToCell(Point position)
       {
           return new Point(
               position.X/title.X,
               (position.Y+yscanlineoffset)/title.Y);
       }

       public static int ConvertCellToX(int x)
       {
           return (x*16);
       }

       public static int Width()
       {
           return mario_res.X* scale;
       }

       public static int Height()
       {
           return mario_res.Y * scale;
       }

       public static int ConvertCellToY(int y)
       {
           return (y * 16 - yscanlineoffset);
       }
   }
}
