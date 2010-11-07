using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace yamiproject
{
    class Goomba : Sprite
    {
        int type = 7; // Goomba
        public bool dead = false;
        double movementx = -1;
        double movementy = 0;
        Point oldpos;
        SoundEffectInstance sound1;
        ColissionLayer colission;

        public enum State
        {
            stop,
            walk,
            stomp,
            dead,
            falling
        }

        public State state = State.stop;

        public Goomba(SpriteBatch batch, ContentManager manager, ColissionLayer colission, int type, Point position)
            : base(batch, manager, "sprites/goomba")
        {
            FrameAnimation anim = new FrameAnimation(2, 16, 16, 0, 0);
            anim.FramesPerSecond = 6;
            animations.Add("walk", anim);
            anim = new FrameAnimation(1, 16, 16, 32, 0);
            anim.FramesPerSecond = 1;
            animations.Add("stomp", anim);

            CurrentAnimationName = "walk";
            IsAnimating = true;
            this.position = position;
            this.oldpos = position;
            this.colission = colission;

            SoundEffect tmp = manager.Load<SoundEffect>("sounds/stomp");
            sound1 = tmp.CreateInstance();
        }

        public override void Update(GameTime time)
        {
            if (!visable)
                return;
            WallTest();
            GroundTest();

            if (state == State.walk || state == State.falling)
            {
                position.X += (int)movementx;
                position.Y += (int)movementy;
            }
            else if (state == State.stomp)
            {       
                    state = State.dead;
            }
            else if (state == State.dead)
            {
                visable = false;
            }
            base.Update(time);

        }

        public override void Colission(int size)
        {
                sound1.Play();
                dead = true;
                state = State.stomp;
                CurrentAnimationName = "stomp";
        }

        public override void Restart()
        {
            visable = true;
            IsAnimating = true;
            dead = false;
            movementx = -1;
            state = State.stop;
            position = oldpos;
            CurrentAnimationName = "walk";
        }
        public void GroundTest()
        {
            if (state == State.stomp || state == State.dead || state == State.stop)
                return;
            state = State.falling;
            
            int y1 = ((position.Y + Globals.yscanlineoffset + 16) / 16);
            if (y1 > 13)
            {
                if (y1 > 15)
                {
                    state = State.dead;
                }
                return;
            }
            int x1 = position.X / 16;

            int x2 = (position.X + 12) / 16;

            if (colission.GetCellIndex(x1, y1) == 1)
                state = State.walk;
            if (colission.GetCellIndex(x2, y1) == 1)
                state = State.walk;
            if (state == State.walk)
            {
                movementy = 0f;
                position.Y = (y1 * 16) - 16 - Globals.yscanlineoffset;
            }
            else
                movementy += 0.5f;
        }

        public void WallTest()
        {
            if (state == State.stop || state == State.stomp || state == State.dead)
                return;

            int y1 = ((position.Y + Globals.yscanlineoffset) / 16);
            int y2 = ((position.Y + Globals.yscanlineoffset + 16) / 16);
            if (y2 >= 14)
                return;
            int x1 = position.X / 16;
            int x2 = (position.X + 16) / 16;

            if (movementx > 0f)
            {
                if (colission.GetCellIndex(x2, y1) == 1)
                {
                    position.X = (x2 * 16 - 16);
                    movementx *= -1;
                }
            }
            else if (movementx < 0f)
            {
                if (colission.GetCellIndex(x1, y1) == 1)
                {
                    position.X = (x1 * 16 + 16);
                    movementx *= -1;
                }
            }

        }
    }
}
