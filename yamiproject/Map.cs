using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace yamiproject
{
    class Map
    {
        Texture2D background;
        SpriteBatch batch;
        ContentManager manager;
        Point size;
        Camera camera;
        List<Sprite> objects;
        Mario mario;

        public Map(SpriteBatch batch, ContentManager manager, string background)
        {
            this.manager = manager;
            this.batch = batch;
            this.background = manager.Load<Texture2D>(background);
            
            
            size = new Point(this.background.Width, this.background.Height);
            camera = new Camera(size.X, size.Y);
            mario = new Mario(batch, manager);
        }
           
        

        public void Update(GameTime time)
        {
            //camera.Update(time);
            mario.Update(time);

        }

        public void Draw(GameTime time)
        {
            batch.Begin(
                SpriteBlendMode.AlphaBlend,
                SpriteSortMode.Deferred,
                SaveStateMode.None,
                camera.TransformMatrix);

            batch.Draw(background, new Rectangle(
                                            0, 0,
                                            size.X*(int)Globals.scale.X, size.Y*(int)Globals.scale.Y),
                                            Color.White);
            mario.Draw(time);
            batch.End();
        }
    }
}
