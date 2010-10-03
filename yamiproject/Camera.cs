using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace yamiproject
{
    class Camera
    {
        int width;
        int height;
        int speed = 5;
        public Point camerapos;
        KeyboardState prevkey;

        public Camera(int width, int height)
        {
            this.width = width;
            this.height = height;
            camerapos = new Point(0, 0);
        }

        public Matrix TransformMatrix
        {
            get
            {
                return Matrix.CreateTranslation(new Vector3(-camerapos.X, -camerapos.Y, 0));
            }
        }

        public Point Camerapos
        {
            get { return camerapos;}

            set
            {
                Point tmp = new Point();
                if (value.X > width - Globals.mario_res.X)
                    tmp.X = width - (int)Globals.mario_res.X;
                else if (value.X < 0)
                    tmp.X = 0;
                else
                    tmp.X = value.X;

                if (value.Y > height - Globals.mario_res.Y)
                    tmp.Y = height - (int)Globals.mario_res.Y;
                else if (value.Y < 0)
                    tmp.Y = 0;
                else
                    tmp.Y = value.Y;
                camerapos = tmp;
                
            }
        }

        public void Move(Point direction)
        {
            Point tmp = Camerapos;
            tmp.X+= direction.X*speed;
            tmp.Y += direction.Y*speed;
            Camerapos = tmp;
        }

        //public void Update(GameTime time)
        //{
            
        //    KeyboardState key = Keyboard.GetState();
        //    if (key.IsKeyDown(Keys.Right) == true)
        //    {
        //        Move(new Point(1,0));
        //    }
        //    if (key.IsKeyDown(Keys.Left) == true)
        //    {
        //        Move(new Point(-1, 0));
        //    }
        //    if (key.IsKeyDown(Keys.Up) == true)
        //    {
        //        Move(new Point(0, 1));
        //    }
        //    if (key.IsKeyDown(Keys.Down) == true)
        //    {
        //        Move(new Point(0, -1));
        //    }

        //    prevkey = key;
        //}


    }
}
