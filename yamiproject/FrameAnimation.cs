using System;
using Microsoft.Xna.Framework;

namespace yamiproject
{
   public class FrameAnimation : ICloneable
    {
        Rectangle[] frames;
        int currentframe = 0;
        float framelength = .5f;
        float timer = 0;

        public int FramesPerSecond
        {
            get
            {
                return (int)1f / (int)framelength;
            }
            set
            {
                framelength = 1f / value;
            }
        }

        public Rectangle CurrentRect
        {
            get { return frames[currentframe]; }

        }

        public int CurrentFrame
        {
            get { return currentframe; }
            set
            {
                currentframe = value;
            }
        }

        public FrameAnimation(
            int numberofframes,
            int framewidth,
            int frameheight,
            int xoffset,
            int yoffset)
        {
            frames = new Rectangle[numberofframes];

            for (int i = 0; i < numberofframes; i++)
            {
                Rectangle rect = new Rectangle();
                rect.Width = framewidth;
                rect.Height = frameheight;
                rect.X = xoffset + i * framewidth;
                rect.Y = yoffset;

                frames[i] = rect;
            }
        }

        private FrameAnimation()
        {
        }
        public void Update(GameTime time)
        {
            timer += (float)time.ElapsedGameTime.TotalSeconds;
            if (timer >= framelength)
            {
                timer = 0f;
                CurrentFrame = (CurrentFrame + 1) % frames.Length; 
            }
        }

        #region ICloneable Members

        public object Clone()
        {
            FrameAnimation anim = new FrameAnimation();
            anim.framelength = framelength;
            anim.frames = frames;
            return anim;
        }

        #endregion
    }
}
