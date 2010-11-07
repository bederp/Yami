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
        List<Sprite> staticobjects;
        List<Sprite> movableobjects;
        float movementx = 0f;
        float movementy = 0f;
        int jumpstarty;
        bool turbo = false;
        public bool beginjump = true;
        public bool orientation = true;
        public int minx = 0;
        public int timeofdeath = 0;
        public bool deathjump = true;
        bool reachedend = false;
        bool reacheddown = false;
        bool jumpfromflagpole = false;
        public bool ducking = false;
        bool invincible = false;
        int invincibletime = 0;
        int feetoffset = 16;
        int rightoffset = 16;
        int flagpole1 = 169;
        Point centeroffset = new Point(8, 8);

        public enum Animation
        {
            stopleft,
            stopright,
            runleft,
            runright,
            jumpleft,
            jumpright,
            duck,
            slideright,
            slideleft,
            swimleft,
            swimright,
            frictionright,
            frictionleft,
            die
        }

        public enum State
        {
            falling,
            standing,
            jump,
            flagpole,
            die, 
            duck
        }

        public enum Size
        {
            small,
            big,
            fire,
        }

        Animation curstate = Animation.stopright;
        public State curstate2 = State.standing;
        public Size curstate3 = Size.small;


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
        


        public Mario(SpriteBatch batch, ContentManager manager, Point position, ColissionLayer colission, List<Sprite> staticobjects, List<Sprite> movableobjects)
            : base(batch, manager, "sprites/marios")
        {
           
            FrameAnimation anim = new FrameAnimation(3, 15, 16, 0, 0);
            anim.FramesPerSecond = 20;
            animations.Add("runright", anim);
            anim = new FrameAnimation(3, 15, 16, 0, 17);
            anim.FramesPerSecond = 20;
            animations.Add("runleft", anim);
            anim = new FrameAnimation(1, 16, 16, 48, 0);
            anim.FramesPerSecond = 20;
            animations.Add("jumpright", anim);
            anim = new FrameAnimation(1, 16, 16, 48, 17);
            anim.FramesPerSecond = 20;
            animations.Add("jumpleft", anim);
            anim = new FrameAnimation(1, 16, 16, 66, 0);
            anim.FramesPerSecond = 20;
            animations.Add("frictionright", anim);
            anim = new FrameAnimation(1, 16, 16, 66, 17);
            anim.FramesPerSecond = 20;
            animations.Add("frictionleft", anim);
            anim.FramesPerSecond = 20;
            anim = new FrameAnimation(1, 16, 16, 80 , 0);
            anim.FramesPerSecond = 20;
            animations.Add("stopright", anim);
            anim = new FrameAnimation(1, 16, 16, 80, 17);
            anim.FramesPerSecond = 20;
            animations.Add("stopleft", anim);
            anim = new FrameAnimation(1, 16, 16, 0, 34);
            anim.FramesPerSecond = 20;
            animations.Add("die", anim);
            anim = new FrameAnimation(1, 16, 16, 16, 34);
            animations.Add("slideright", anim);
            anim = new FrameAnimation(1, 16, 16, 32, 34);
            animations.Add("slideleft", anim);
            //anim = new FrameAnimation(4, 33, 33, 66, 66);
            //animations.Add("swimright", anim);
            //anim = new FrameAnimation(4, 33, 33, 0, 99);
            //animations.Add("swimleft", anim);

            CurrentAnimationName = "stopright";
            this.position = position;
            this.colission = colission;
            this.staticobjects = staticobjects;
            this.movableobjects = movableobjects;
            
            sounds = new List<SoundEffectInstance>();
            
            SoundEffect tmp = manager.Load<SoundEffect>("sounds/jump");
            SoundEffectInstance sound = tmp.CreateInstance();
            sounds.Add(sound);
            tmp = manager.Load<SoundEffect>("sounds/death");
            sound = tmp.CreateInstance();
            sounds.Add(sound);
        }

        public override void Update(GameTime time)
        {
            Invincible();
            Death(time);
            KeyboardInput();  
            Move();
            AnimationMachine();
            WallTest();
            GroundTest();
            Jump();
            HeadTest();
            CollisionTest();
            Console.WriteLine(curstate2);
            Console.WriteLine(position);
            Console.WriteLine(ducking);
            

            base.Update(time);
        }

        private void Invincible()
        {
            if (invincibletime > 0)
            {
                invincibletime--;

            }
            else
                invincible = false;
        }

        private void CollisionTest()
        {
            foreach (Sprite s in movableobjects)
            {

                Vector2 mariocenter1 = new Vector2(position.X+8, position.Y+8);
                Vector2 mariocenter2 = new Vector2(position.X+8, position.Y+24);
                Vector2 spritecenter = new Vector2(s.position.X + 8, s.position.Y + 8);
                bool colided = false;

                if (curstate3 == Size.small)
                {
                    if (Vector2.Distance(mariocenter1, spritecenter) < 16)
                        colided = true;
                }
                else
                    if (Vector2.Distance(mariocenter1, spritecenter) < 16 || Vector2.Distance(mariocenter2, spritecenter) < 16)
                        colided = true;

                if (colided == true)
                    {
                        if (s is Goomba)
                        {
                            Goomba tmp = s as Goomba;
                            if (!tmp.dead && curstate2 != State.die)
                            {
                                if (position.Y + feetoffset < s.position.Y + 16)
                                {
                                    movementy = -2.0f;
                                    s.Colission((int)curstate3);
                                }
                                else if (invincible == true)
                                {

                                }
                                else if (!(curstate3 == Size.small))
                                {
                                    SetSize(Size.small);
                                    invincibletime = 60 * 4;
                                    invincible = true;
                                }
                                else
                                    curstate2 = State.die;
                            }
                        }
                        else if (s is Coin)
                        {
                            Coin tmp = s as Coin;
                            if (tmp.visable && curstate2 != State.die)
                            {
                                s.Colission((int)curstate3);
                            }
                        }

                        else if (s is Powerup)
                        {
                            Powerup tmp = s as Powerup;
                            if (tmp.visable && curstate2 != State.die)
                            {
                                s.Colission((int)curstate3);
                                Powerup();
                            }
                        }
                    }
            }
        }

        private void Powerup()
        {
            if (curstate3 == Size.small)
            {
                SetSize(Size.big);
            }
            else if (curstate3 == Size.big)
            {
                SetSize(Size.fire);
            }
            else if (curstate3 == Size.fire)
            {
                Score.AddScore(1000);
            }

        }

        public void SetSize(Mario.Size size)
        {
            if (size == Size.small)
            {
                position.Y += 16;
                feetoffset = 16;
                flagpole1 = 169;
                centeroffset = new Point(8, 8);
                curstate3 = Size.small;
                ReloadTexture("marios");
                animations.Clear();
                FrameAnimation anim = new FrameAnimation(3, 15, 16, 0, 0);
                anim.FramesPerSecond = 20;
                animations.Add("runright", anim);
                anim = new FrameAnimation(3, 15, 16, 0, 17);
                anim.FramesPerSecond = 20;
                animations.Add("runleft", anim);
                anim = new FrameAnimation(1, 16, 16, 48, 0);
                anim.FramesPerSecond = 20;
                animations.Add("jumpright", anim);
                anim = new FrameAnimation(1, 16, 16, 48, 17);
                anim.FramesPerSecond = 20;
                animations.Add("jumpleft", anim);
                anim = new FrameAnimation(1, 16, 16, 66, 0);
                anim.FramesPerSecond = 20;
                animations.Add("frictionright", anim);
                anim = new FrameAnimation(1, 16, 16, 66, 17);
                anim.FramesPerSecond = 20;
                animations.Add("frictionleft", anim);
                anim.FramesPerSecond = 20;
                anim = new FrameAnimation(1, 16, 16, 80, 0);
                anim.FramesPerSecond = 20;
                animations.Add("stopright", anim);
                anim = new FrameAnimation(1, 16, 16, 80, 17);
                anim.FramesPerSecond = 20;
                animations.Add("stopleft", anim);
                anim = new FrameAnimation(1, 16, 16, 0, 34);
                anim.FramesPerSecond = 20;
                animations.Add("die", anim);
                anim = new FrameAnimation(1, 16, 16, 16, 34);
                animations.Add("slideright", anim);
                anim = new FrameAnimation(1, 16, 16, 32, 34);
                animations.Add("slideleft", anim);
            }
            else if (size == Size.big)
            {
                position.Y -= 16;
                feetoffset = 32;
                flagpole1 = 169-16;
                centeroffset = new Point(8, 16);
                curstate3 = Size.big;
                ReloadTexture("mariob");
                animations.Clear();
                FrameAnimation anim = new FrameAnimation(3, 16, 32, 0, 0);
                anim.FramesPerSecond = 20;
                animations.Add("runright", anim);
                anim = new FrameAnimation(3, 16, 32, 0, 32);
                anim.FramesPerSecond = 20;
                animations.Add("runleft", anim);
                anim = new FrameAnimation(1, 16, 32, 48, 0);
                anim.FramesPerSecond = 20;
                animations.Add("jumpright", anim);
                anim = new FrameAnimation(1, 16, 32, 48, 32);
                anim.FramesPerSecond = 20;
                animations.Add("jumpleft", anim);
                anim = new FrameAnimation(1, 16, 32, 64, 0);
                anim.FramesPerSecond = 20;
                animations.Add("frictionright", anim);
                anim = new FrameAnimation(1, 16, 32, 64, 32);
                anim.FramesPerSecond = 20;
                animations.Add("frictionleft", anim);
                anim.FramesPerSecond = 20;
                anim = new FrameAnimation(1, 16, 32, 80, 0);
                anim.FramesPerSecond = 20;
                animations.Add("stopright", anim);
                anim = new FrameAnimation(1, 16, 32, 80, 32);
                anim.FramesPerSecond = 20;
                animations.Add("stopleft", anim);
                anim = new FrameAnimation(1, 16, 32, 0, 64);
                anim.FramesPerSecond = 20;
                animations.Add("duck", anim);
                anim = new FrameAnimation(1, 16, 32, 16, 64);
                animations.Add("slideright", anim);
                anim = new FrameAnimation(1, 16, 32, 32, 64);
                animations.Add("slideleft", anim);
            }
        }

        public void Flagpole(int maxx)
        {
                if (!reachedend)
                {
                    reachedend = true;
                    position.X = maxx;
                    movementx = 0f;
                    movementy = 0f;
                    curstate2 = State.flagpole;
                    CurrentAnimationName = Animation.slideright.ToString();
                    Gamestate.GetCurrentMap().StartFlag();
                }

                else
                {
                    if (!reacheddown)
                    {
                        if (position.Y < flagpole1)
                        {
                            position.Y += 2;
                        }
                        else
                        {
                            position.Y = flagpole1;
                            reacheddown = true;
                        }
                    }
                    else if (jumpfromflagpole)
                    {
                        CurrentAnimationName = Animation.runright.ToString();

                        if (position.Y < 200 - feetoffset)
                            position.Y += 2;
                        else
                            position.Y = 200 - feetoffset;

                        if (position.X < 3265)
                            position.X += 2;
                        else if(visable == true)
                        {
                            visable = false;
                            Gamestate.CoinsToScore();
                        }
                    }
                    else if (!Gamestate.GetCurrentMap().flaggodown)
                                        {
                                            position.X += 16;
                                            CurrentAnimationName = Animation.slideleft.ToString();
                                            jumpfromflagpole = true;
                                        }
                }

        }

        public void Death(GameTime time)
        {
            if (curstate2 != State.die)
                return;
            Gamestate.BackgroundMusicStop();
            movementx = 0f;
            sounds[1].Play();
            timeofdeath += time.ElapsedGameTime.Milliseconds;

            int y1 = ((position.Y + Globals.yscanlineoffset + 16) / 16);
            if (y1 < 17)
            {
                if (deathjump)
                {
                    movementy = -3f;
                    deathjump = false;
                }
                else 
                {
                    movementy += 0.2f;
                }

            }
            if (timeofdeath > 2500)
            {
                Score.Death();
                SetSize(Size.small);
                if (Score.lives > 0)
                    Gamestate.GameInfo();
                else
                    Gamestate.GameOver();
            }
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

            if (curstate2 == State.flagpole)
                return;

            if (curstate2 == State.die)
            {
                CurrentAnimationName = Animation.die.ToString();
            }
            else if (ducking == true && !(curstate3 == Size.small))
            {
                CurrentAnimationName = Animation.duck.ToString();
            }
            else if (curstate2 == State.jump)
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
            if (curstate2 == State.die  || curstate2 == State.flagpole)
                return;
            KeyboardState key = Keyboard.GetState();
            turbo = false;
            if (!(curstate3 == Size.small) && ducking == true)
            {
                Frictionx();
            }
            else if (key.IsKeyDown(Keys.Right) == true)
            {
                if (key.IsKeyDown(Keys.D) == true)
                {
                    turbo = true;
                    Movementx += 0.3f;
                }

                Movementx += 0.2f;
            }
            else if (key.IsKeyDown(Keys.Left) == true)
            {
                if (key.IsKeyDown(Keys.D) == true)
                {
                    turbo = true;
                    Movementx -= 0.3f;
                }
                Movementx -= 0.2f;
            }
            else
                Frictionx();

            if (key.IsKeyDown(Keys.F) == true)
            {
                if(curstate2 == State.standing)
                    curstate2 = State.jump;
            }

            if (key.IsKeyDown(Keys.Down) == true)
            {
                if (curstate2 == State.standing)
                    ducking = true;
            }
            else
                ducking = false;


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
            if(curstate2 == State.jump || curstate2 == State.duck || curstate2 == State.die || curstate2 == State.flagpole)
                return;
            curstate2 = State.falling;
            int y1 = ((position.Y + Globals.yscanlineoffset + feetoffset) / 16);
            if (y1 > 13)
            {
                if (y1 > 15)
                {
                    beginjump = true;
                    curstate2 = State.die;
                }
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
                position.Y = (y1 * 16) - feetoffset - Globals.yscanlineoffset;
            }
            else
                movementy += 0.5f;
            string tmp = curstate2.ToString() + position.ToString() + " x1=" + x1 + " x2=" + x2 + " y1=" + y1;
            //Console.WriteLine(tmp);

        }

        public void WallTest()
        {
            if (Movementx == 0f || curstate2 == State.flagpole)
                return;

            int y1 = ((position.Y + Globals.yscanlineoffset) / 16);
            int y2 = ((position.Y + Globals.yscanlineoffset + feetoffset/2) / 16);
            if (y2 >= 14 || y1 <0)
                return;
            int x1 = position.X/16;
            int x2 = (position.X + rightoffset)/16;

            if (Movementx > 0f)
            {
                if (colission.GetCellIndex(x2, y1) == 1 || colission.GetCellIndex(x2, y2) == 1)
                {
                    position.X = (x2 * 16 - rightoffset);
                    Movementx = 0f;
                }      
            }
            else if (Movementx < 0f)
            {
                if (colission.GetCellIndex(x1, y1) == 1 || colission.GetCellIndex(x1, y2) == 1)
                {
                    position.X = (x1 * 16 + rightoffset);
                    Movementx = 0f;
                }
            }

        }

        public void HeadTest()
        {
            if (curstate2 != State.jump)
                return;

           int x1 = position.X+centeroffset.X;
           int y1 = position.Y-1;

           foreach (Sprite s in staticobjects)
           {
               if((Globals.ConvertPositionToCell(s.position) == Globals.ConvertPositionToCell(new Point(x1, y1))))
               {
                   s.Colission((int)curstate3);
                   sounds[0].Stop();
                   curstate2 = State.falling;
                   movementy = 0f;
                   beginjump = true;
               }
           }
        }

        public void Restart(Point start)
        {
            movementx = 0f;
            movementy = 0f;
            minx = 0;
            curstate2 = Mario.State.falling;
            orientation = true;
            beginjump = true;
            position = start;
            timeofdeath = 0;
            turbo = false;
            beginjump = true;
            reachedend = false;
            reacheddown = false;
            jumpfromflagpole = false;
            visable = true;
            ducking = false;
            deathjump = true;
        }

    }
}
