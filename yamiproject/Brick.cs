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
        int type = 5;

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
        }

        public void  Update(GameTime time)
        {

        }
    }
}
