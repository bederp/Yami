using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace yamiproject
{
    class Mario : Sprite
    {
        KeyboardState prevkey;
        float speed = 2f;
        
        public enum State
        {
            stopleft,
            stopright,
            runleft,
            runright,
            jumpleft,
            jumpright,
            sit,
            slide,
            swimleft,
            swimright,
            die,
            falling

        }

        State curstate = State.stopright;

        public State Curstate
        {
            get { return curstate; }
            set
            {
                curstate = value;
                if (curstate != State.falling)
                {
                    IsAnimating = true;
                    CurrentAnimationName = curstate.ToString();
                    Console.WriteLine(curstate.ToString());
                }
                else
                    IsAnimating = false;
            }
        }

        public Mario(SpriteBatch batch, ContentManager manager)
            : base(batch, manager, "sprites/mario1")
        {
            FrameAnimation anim = new FrameAnimation(1, 33, 33, 0, 0);
            animations.Add("stopright", anim);
            anim = new FrameAnimation(1, 33, 33, 33, 0);
            animations.Add("die", anim);
            anim = new FrameAnimation(4, 33, 33, 66, 0);
            animations.Add("runright", anim);
            anim = new FrameAnimation(1, 33, 33, 198, 0);
            animations.Add("jumpright", anim);
            anim = new FrameAnimation(1, 33, 33, 0, 33);
            animations.Add("stopleft", anim);
            anim = new FrameAnimation(4, 33, 33, 33, 33 );
            animations.Add("runleft", anim);
            anim = new FrameAnimation(1, 33, 33, 165 , 33);
            animations.Add("jumpleft", anim);
            anim = new FrameAnimation(1, 33, 33, 0, 66);
            animations.Add("sit", anim);
            anim = new FrameAnimation(1, 33, 33, 33, 66);
            animations.Add("slide", anim);
            anim = new FrameAnimation(4, 33, 33, 66, 66);
            animations.Add("swimright", anim);
            anim = new FrameAnimation(4, 33, 33, 0, 99);
            animations.Add("swimleft", anim);

            CurrentAnimationName = curstate.ToString();
            position = new Vector2(5, 200 - 32); 
        }

        public void Update(GameTime time)
        {

            KeyboardState key = Keyboard.GetState();
            if (key.IsKeyDown(Keys.Right) == true)
            {
                Curstate = State.runright;
                Move(new Point(1, 0));
            }
            else if (key.IsKeyDown(Keys.Left) == true)
            {
                Curstate = State.runleft;
                Move(new Point(-1, 0));
            }
            else if(prevkey.IsKeyDown(Keys.Right) == true)
                Curstate = State.stopright;
            else if (prevkey.IsKeyDown(Keys.Left) == true)
                Curstate = State.stopleft;

            prevkey = key;
            base.Update(time);
        }

        public void Move(Point direction)
        {
            position.X += direction.X * speed;
            position.Y += direction.Y * speed;
        }
    }
}
