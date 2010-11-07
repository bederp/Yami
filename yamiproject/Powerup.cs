using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace yamiproject
{
    class Powerup : Sprite
    {
        SoundEffectInstance sound1;
        ColissionLayer collision;
        Point oldpos;
        double movementx = 1;
        double movementy = 0;
        int type;   // 1 - Upshroom 2 - Lvlshroom 3 - Plant 4  - Star
        bool dead = false;
        

        public enum State
        {
            emerge,
            walk,
            stomp,
            dead,
            falling
        }

        public State state = State.emerge;

        public Powerup(SpriteBatch batch, ContentManager manager, ColissionLayer collision, int type, Point position)
            : base(batch, manager, "sprites/powerups")
        {
            FrameAnimation anim = new FrameAnimation(1, 16, 16, 0, 0);
            anim.FramesPerSecond = 6;
            animations.Add("Lvlshroom", anim);
            anim = new FrameAnimation(1, 16, 16, 16, 0);
            animations.Add("Upshroom", anim);
            anim = new FrameAnimation(4, 16, 16, 0, 16);
            animations.Add("Plant", anim);
            anim = new FrameAnimation(4, 16, 16, 0, 32);
            animations.Add("Star", anim);

            this.collision = collision;
            this.type = type;

            switch (type)
            {
                case 1:
                    CurrentAnimationName = "Upshroom";
                    break;
                case 2:
                    CurrentAnimationName = "Lvlshroom";
                    break;
                case 3:
                    CurrentAnimationName = "Plant";
                    break;
                case 4:
                    CurrentAnimationName = "Star";
                    break;
                

            }
            this.position = position;
            this.oldpos = position;

            SoundEffect tmp;
            if (type == 4)
                tmp = manager.Load<SoundEffect>("sounds/starman10");
            else if (type == 2)
                tmp = manager.Load<SoundEffect>("sounds/1up");
            else
                tmp = manager.Load<SoundEffect>("sounds/powerup");

            sound1 = tmp.CreateInstance();
        }

        public override void Update(GameTime time)
        {
            if (state == State.emerge)
            {
                position.Y--;
                if (oldpos.Y - 16 >= position.Y)
                    state = State.falling;
            }
            else if (type == 1)
            {
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
            }
            base.Update(time);

        }

        public override void Colission(int size)
        {
            sound1.Play();
            dead = true;
            visable = false;
            state = State.stomp;
        }

        public void GroundTest()
        {
            if (state == State.stomp || state == State.dead)
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

            if (collision.GetCellIndex(x1, y1) == 1)
                state = State.walk;
            if (collision.GetCellIndex(x2, y1) == 1)
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
            if (state == State.stomp || state == State.dead)
                return;

            int y1 = ((position.Y + Globals.yscanlineoffset) / 16);
            int y2 = ((position.Y + Globals.yscanlineoffset + 16) / 16);
            if (y2 >= 14)
                return;
            int x1 = position.X / 16;
            int x2 = (position.X + 16) / 16;

            if (movementx > 0f)
            {
                if (collision.GetCellIndex(x2, y1) == 1)
                {
                    position.X = (x2 * 16 - 16);
                    movementx *= -1;
                }
            }
            else if (movementx < 0f)
            {
                if (collision.GetCellIndex(x1, y1) == 1)
                {
                    position.X = (x1 * 16 + 16);
                    movementx *= -1;
                }
            }

        }
    }
}
