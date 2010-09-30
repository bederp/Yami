using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace yamiproject
{
   public class Globals
    {

       public static Vector2 mario_res = new Vector2(256, 224);
       public static Vector2 scale = new Vector2(1, 1);

       public static Vector2 Scale
        {
            get { return scale; }
            set { scale = value; }
        }
    }
}
