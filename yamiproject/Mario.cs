using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace yamiproject
{
    class Mario : Sprite
    {
        KeyboardState prevkey;
        ColissionLayer colission;
        List<SoundEffectInstance> sounds;
        float movementx = 0f;
        float movementy = 0f;
        int jumpstarty;
        bool turbo = false;
        bool beginjump = true;
        bool orientation = true;
        int minx = 0;
        
        public float Movementx
        {
            get { return movementx; }
            set
            {
                if (turbo)
                {
                    if (value > 3.0f)
                        movementx = 3.0f;
                    else if (value < -3.0f)
                        movementx = -3.0f;
                    else
                        movementx = value;
                }
                else
                {
                    if (value > 2.0f)
                        movementx = 2.0f;
                    else if (value < -2.0f)
                        movementx = -2.0f;
                    else
                        movementx = value;
                }
            }
        }

        public void Frictionx()
        {
            if (curstate2 != State.jump && curstate2 != State.falling)
            {
                if (movementx > 0)
                    movementx -= 0.1f;
                else if (movementx < 0)
                    movementx += 0.1f;
                if (Movementx < 0.01 && Movementx > -0.01)
                    Movementx = 0;
            }
        }
        public enum Animation
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
            die
        }
        public enum State
        {
            falling,
            standing,
            jump,
            die
        }

        Animation curstate = Animation.stopright;
        State curstate2 = State.standing;



        public Mario(SpriteBatch batch, ContentManager manager, Point position, ColissionLayer colission)
            : base(batch, manager, "sprites/marios")
        {
           
            FrameAnimation anim = new FrameAnimation(3, 15, 16, 0, 0);
            anim.FramesPerSecond = 20;
            animations.Add("runright", anim);
            anim = new FrameAnimation(3, 15, 16, 0, 17);
            anim.FramesPerSecond = 20;
            animations.Add("runleft", anim);
            anim = new FrameAnimation(1, 16, 16, 45, 0);
            anim.FramesPerSecond = 20;
            animations.Add("jumpright", anim);
            anim = new FrameAnimation(1, 16, 16, 45, 17);
            anim.FramesPerSecond = 20;
            animations.Add("jumpleft", anim);
            anim = new FrameAnimation(1, 13, 16, 61, 0);
            anim.FramesPerSecond = 20;
            animations.Add("slideright", anim);
            anim = new FrameAnimation(1, 13, 16, 61, 17);
            anim.FramesPerSecond = 20;
            animations.Add("slideleft", anim);
            anim.FramesPerSecond = 20;
            anim = new FrameAnimation(1, 12, 16, 74 , 0);
            anim.FramesPerSecond = 20;
            animations.Add("stopright", anim);
            anim = new FrameAnimation(1, 12, 16, 74, 17);
            anim.FramesPerSecond = 20;
            animations.Add("stopleft", anim);
            //anim = new FrameAnimation(1, 33, 33, 33, 66);
            //animations.Add("slide", anim);
            //anim = new FrameAnimation(4, 33, 33, 66, 66);
            //animations.Add("swimright", anim);
            //anim = new FrameAnimation(4, 33, 33, 0, 99);
            //animations.Add("swimleft", anim);

            CurrentAnimationName = "stopright";
            this.position = position;
            this.colission = colission;
            sounds = new List<SoundEffectInstance>();
            
            SoundEffect tmp = manager.Load<SoundEffect>("sounds/jump");
            SoundEffectInstance sound = tmp.CreateInstance();
            sounds.Add(sound);
        }

        public void  Update(GameTime time)
        {
            KeyboardInput();
            Jump();
            AnimationMachine();
            Move();

            WallTest();
            GroundTest();
            
            if (position.X - (Globals.mario_res.X / 2) + Globals.title.X > minx)
                minx = position.X - (Globals.mario_res.X / 2) +Globals.title.X;
            base.Update(time);
        }

        private void Move()
        {
            if (position.X + (int)movementx > minx)
            {
                position.X += (int)movementx;
                
            }
            position.Y += (int)movementy;
        }

        public void AnimationMachine()
        {
            IsAnimating = true;
            if (curstate2 == State.jump)
            {
                if (orientation)
                    CurrentAnimationName = Animation.jumpright.ToString();
                else
                    CurrentAnimationName = Animation.jumpleft.ToString();
            }
            else if (curstate2 == State.falling)
                IsAnimating = false;
            else if (movementx > 0f)
            {
                CurrentAnimationName = Animation.runright.ToString();
                orientation = true;
            }
            else if (movementx < 0f)
            {
                CurrentAnimationName = Animation.runleft.ToString();
                orientation = false;
            }
            else if (orientation)
                CurrentAnimationName = Animation.stopright.ToString();
            else if (!orientation)
                CurrentAnimationName = Animation.stopleft.ToString();


        }

        private void KeyboardInput()
        {
            KeyboardState key = Keyboard.GetState();
            turbo = false;
            if (key.IsKeyDown(Keys.Right) == true)
            {
                if (key.IsKeyDown(Keys.D) == true)
                {
                    turbo = true;
                    Movementx += 0.2f;
                }

                Movementx += 0.1f;
            }
            else if (key.IsKeyDown(Keys.Left) == true)
            {
                if (key.IsKeyDown(Keys.D) == true)
                {
                    turbo = true;
                    Movementx -= 0.2f;
                }
                Movementx -= 0.1f;
            }
            else
                Frictionx();

            if (key.IsKeyDown(Keys.F) == true)
            {
                if(curstate2 == State.standing)
                    curstate2 = State.jump;
            }

            prevkey = key;
        }

        private void Jump()
        {
            if(curstate2 == State.jump)
            {
                if(beginjump)
                {
                    sounds[0].Play();
                    jumpstarty = position.Y;
                    movementy = -5f;
                    beginjump = false;
                }
                
                if(jumpstarty-position.Y<50 + 16)
                movementy+=0.1f;
                else
                {
                   movementy = 0f;
                   beginjump = true;
                   curstate2 = State.falling;
                }
            }

        }

        public void GroundTest()
        {
            if(curstate2 == State.jump)
                return;
            curstate2 = State.falling;
            int y1 = ((position.Y + Globals.yscanlineoffset + 16) / 16);
            if (y1 > 13)
            {
                if (y1 >= 14)
                    curstate2 = State.die;
                return;
            }
            int x1 = position.X / 16;
            
            int x2 = (position.X + 12) / 16;

            if (colission.GetCellIndex(x1, y1) == 1)
                curstate2 = State.standing;
            if (colission.GetCellIndex(x2, y1) == 1)
                curstate2 = State.standing;
            if (curstate2 == State.standing)
            {
                movementy = 0f;
                position.Y = (y1 * 16) - 16 - Globals.yscanlineoffset;
            }
            else
                movementy += 0.5f;
            string tmp = curstate2.ToString() + position.ToString() + " x1=" + x1 + " x2=" + x2 + " y1=" + y1;
            Console.WriteLine(tmp);

        }

        public void WallTest()
        {
            if (Movementx == 0f )
                return;

            int y1 = ((position.Y + Globals.yscanlineoffset) / 16);
            int y2 = ((position.Y + Globals.yscanlineoffset + 16) / 16);
            if (y2 >= 14)
                return;
            int x1 = position.X/16;
            int x2 = (position.X + 16)/16;

            if (Movementx > 0f)
            {
                if (colission.GetCellIndex(x2, y1) == 1)
                {
                    position.X = (x2 * 16 - 16);
                    Movementx = 0f;
                }      
            }
            else if (Movementx < 0f)
            {
                if (colission.GetCellIndex(x1, y1) == 1)
                {
                    position.X = (x1 * 16 + 16);
                    Movementx = 0f;
                }
            }

        }

    }
}
