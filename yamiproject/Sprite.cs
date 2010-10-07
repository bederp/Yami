using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace yamiproject
{
   public class Sprite
    {
        public Dictionary<string, FrameAnimation> animations =
            new Dictionary<string, FrameAnimation>();
        string currentanimation = null;
        public Point position = Point.Zero;
        bool animating = true;
        Texture2D texture;
        SpriteBatch batch;
        ContentManager manager;

        public bool IsAnimating
        {
            get { return animating; }
            set { animating = value; }
        }

        public FrameAnimation CurrentAnimation
        {
            get
            {
                if(!string.IsNullOrEmpty(currentanimation))
                    return animations[currentanimation];
                else
                    return null;
            }
        }

        public string CurrentAnimationName
        {
            get { return currentanimation; }
            set
            {
                if(animations.ContainsKey(value))
                    currentanimation = value;
            }
        }

        public Sprite(SpriteBatch batch, ContentManager manager,  string texture)
        {
            this.texture = manager.Load<Texture2D>(texture);
            this.batch = batch;
            this.manager = manager;
        }

        public void Update(GameTime time)
        {
            if (!IsAnimating)
                return;
            FrameAnimation animation = CurrentAnimation;

            if (animation != null)
                animation.Update(time);
        }

        public void Draw(GameTime time)
        {
            FrameAnimation animation = CurrentAnimation;
            if (animation != null)
            {
                Rectangle tmp = new Rectangle(
                    (int)position.X,
                    (int)position.Y,
                    animation.CurrentRect.Width,
                    animation.CurrentRect.Height);

                batch.Draw(texture,
                    tmp,
                    animation.CurrentRect,
                    Color.White);
            }
                
        }
    }
}
