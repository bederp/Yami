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
        public Vector2 camerapos;

        public Camera(int width, int height)
        {
            this.width = width;
            this.height = height;
            camerapos = new Vector2(0, 0);
        }

        public Matrix TransformMatrix
        {
            get
            {
                return Matrix.CreateScale(Globals.scale) * Matrix.CreateTranslation(new Vector3(-camerapos.X * Globals.scale, -camerapos.Y * Globals.scale, 0));
            }
        }

        public Vector2 Camerapos
        {
            get { return camerapos;}

            set
            {
                Vector2 tmp = new Vector2();
                if (value.X < Camerapos.X)
                    return;
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
