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
        int type = 1;  // 1 - coin box  2 - power up box 3 - starmanbox

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

            SoundEffect tmp;
            if (type == 1)
            {
                tmp = manager.Load<SoundEffect>("sounds/coin");
            }
            else
                tmp = manager.Load<SoundEffect>("sounds/powerupa");

            sound1 = tmp.CreateInstance();
            tmp = manager.Load<SoundEffect>("sounds/bump");
            sound2 = tmp.CreateInstance();
        }

        public void  Update(GameTime time)
        {

        }
    }
}
