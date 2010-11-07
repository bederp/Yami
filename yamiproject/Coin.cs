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
    class Coin : Sprite
    {
        SoundEffectInstance sound1;

        public Coin(SpriteBatch batch, ContentManager manager, int type, Point position)
            : base(batch, manager, "sprites/coin2")
        {
            FrameAnimation anim = new FrameAnimation(3, 16, 16, 0, 0);
            anim.FramesPerSecond = 6;
            animations.Add("Coin", anim);

            CurrentAnimationName = "Coin";
            this.position = position;

            SoundEffect tmp = manager.Load<SoundEffect>("sounds/coin");
            sound1 = tmp.CreateInstance();
        }

        public override void Update(GameTime time)
        {
            base.Update(time);

        }

        public override void Colission(int size)
        {
            Score.AddCoin();
            Score.AddScore(200);
            sound1.Play();
            visable = false;
        }

        public override void Restart()
        {
            visable = true;
        }
    }
}
