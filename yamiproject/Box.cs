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
    class Box : Sprite
    {
        SoundEffectInstance sound1;
        SoundEffectInstance sound2;
        int type;  // 1 - coin box  2 - power up box 3 - starmanbox 4  - invisible box
        bool usedup = false;
        Point oldpos;

        SpriteBatch batch;
        ContentManager manager;

        public enum State
        {
            stop,
            jump,
            injump,
            offjump
        }

        State state = State.stop;

        public Box(SpriteBatch batch, ContentManager manager, int type, Point position)
            : base(batch, manager, "sprites/box")
        {
            FrameAnimation anim = new FrameAnimation(3, 16, 16, 0, 0);
            anim.FramesPerSecond = 6;
            animations.Add("box1", anim);
            anim = new FrameAnimation(1, 16, 16, 48, 0);
            anim.FramesPerSecond = 1;
            animations.Add("box2", anim);

            CurrentAnimationName = "box1";
            this.position = position;

            this.batch = batch;
            this.manager = manager;

            SoundEffect tmp;
            if (type == 1)
            {
                tmp = manager.Load<SoundEffect>("sounds/coin");
            }
            else
            {
                tmp = manager.Load<SoundEffect>("sounds/powerupa");
            }
            this.type = type;
            if (type == 4)
                visable = false;

            sound1 = tmp.CreateInstance();
            tmp = manager.Load<SoundEffect>("sounds/bump");
            sound2 = tmp.CreateInstance();
        }

        public override void Update(GameTime time)
        {
            if (state == State.stop)
            {
            }
            else if (state == State.jump)
            {
                oldpos = position;
                state = State.injump;
            }
            else if (state == State.injump)
            {
                while (oldpos.Y - 4 < position.Y)
                    position.Y--;
                if (oldpos.Y - 4 >= position.Y)
                {
                    usedup = true;
                    CurrentAnimationName = "box2";
                    state = State.offjump;
                }

            }
            else if (state == State.offjump)
            {
                while (oldpos.Y > position.Y)
                    position.Y++;
                if (oldpos.Y <= position.Y)
                {
                    position.Y = oldpos.Y;
                    state = State.stop;
                }
            }
            base.Update(time);

        }

        public override void Colission(int size)
        {
            if (type == 4)
                visable = true;
            if (!usedup)
            {
                if (type == 1)
                {
                    Score.AddCoin();

                    Score.AddScore(200);
                }
                else if (type == 2)
                {
                    if (size ==  0)
                    {
                        Powerup tmp3 = new Powerup(
                            batch,
                            manager,
                            Gamestate.GetCurrentMap().colission,
                            1,
                            position);

                        Gamestate.GetCurrentMap().moveableobjects.Add(tmp3);
                    }
                }
                sound1.Play();
                state = State.jump;
            }
            else
            {
                sound2.Play();
            }
        }

        public override void Restart()
        {
            if (type == 4)
                visable = false;

            usedup = false;
            CurrentAnimationName = "box1";
        }
    }
}
