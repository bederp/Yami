﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace yamiproject
{
   public class Globals
   {

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

       public static int ConvertCellToY(int y)
       {
           return (y * 16 - yscanlineoffset);
       }
   }
}
