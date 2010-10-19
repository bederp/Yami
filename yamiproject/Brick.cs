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
    class Brick : Sprite
    {
        SoundEffectInstance sound1;
        SoundEffectInstance sound2;
        Point oldpos;
        int type = 5;           // 5 - brick   6 - coinsbrick

        public enum State
        {
            stop,
            jump,
            injump,
            offjump
        }

        State state = State.stop;

        public Brick(SpriteBatch batch, ContentManager manager, int type, Point position)
            : base(batch, manager, "sprites/brick")
        {
            FrameAnimation anim = new FrameAnimation(1, 16, 16, 0, 0);
            anim.FramesPerSecond = 15;
            animations.Add("brick", anim);

            CurrentAnimationName = "brick";
            IsAnimating = false;
            this.position = position;

            SoundEffect tmp = manager.Load<SoundEffect>("sounds/bump");
            sound1 = tmp.CreateInstance();
            tmp = manager.Load<SoundEffect>("sounds/breakblock");
            sound2 = tmp.CreateInstance();
        }

        public override void Update(GameTime time)
        {
            if (state == State.jump)
            {
                oldpos = position;
                state = State.injump;
            }
            else if (state == State.injump)
            {
                while (oldpos.Y - 4 < position.Y)
                    position.Y--;
                if (oldpos.Y - 4 >= position.Y)
                    state = State.offjump;

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
            if (size == 0)
            {
                sound1.Play();
                state = State.jump;
            }

            if (size == 1)
            {
                sound2.Play();
            }
        }

        public override void Restart()
        {

        }
    }
}
