using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace yamiproject
{
    class Level
    {
        int currentmap;
        List<Map> maps;

        public Level(SpriteBatch batch, ContentManager manager, params string[] maps)
        {
            currentmap = 0;
            this.maps = new List<Map>();
            foreach (string s in maps)
            {
                Map tmp = new Map(batch, manager, s);
                this.maps.Add(tmp);
            }
        }

        public void Update(GameTime time)
        {
            maps[currentmap].Update(time);

        }
        
        public void Draw(GameTime time)
        {
            maps[currentmap].Draw(time);
        }
        
    }
}
