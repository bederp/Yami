using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.IO;

namespace yamiproject
{
    class Level
    {
        int currentmap;
        List<Map> maps;

        public Level(SpriteBatch batch, ContentManager manager, string world)
        {
            currentmap = 0;
            this.maps = new List<Map>();
            bool readingmaps = false;

            using (StreamReader reader = new StreamReader("Content/worlds/"+world+"/def.txt"))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine().Trim();

                    if (string.IsNullOrEmpty(line))
                        continue;

                    if (line.Contains("[MAPS]"))
                        readingmaps = true;
                    else if (readingmaps)
                    {
                        Map tmp = new Map(batch, manager, world, line);
                        maps.Add(tmp);
                    }

                }
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
