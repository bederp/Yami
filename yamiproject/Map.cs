using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System.IO;

namespace yamiproject
{
    class Map
    {
        Texture2D background;
        SpriteBatch batch;
        ContentManager manager;
        Point size;
        Point start;
        Camera camera;
        List<SoundEffectInstance> sounds;
        List<Sprite> objects;
        Mario mario;
        ColissionLayer colission;

        public Map(SpriteBatch batch, ContentManager manager, string world, string filename)
        {
            this.manager = manager;
            this.batch = batch;
            sounds = new List<SoundEffectInstance>();

            bool readingtStart = false;
            bool readingSounds = false;
            bool readingBackground = false;
            bool readingColission = false;
            bool readingObjects = false;


            using (StreamReader reader = new StreamReader("Content/worlds/"+world+"/"+filename))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine().Trim();

                    if (string.IsNullOrEmpty(line))
                        continue;
                    if(line.Contains("[START]"))
                    {
                        readingtStart = true;
                        readingSounds = false;
                        readingBackground = false;
                        readingColission = false;
                        readingObjects = false;
                    }
                    else if (line.Contains("[SOUNDS]"))
                    {
                        readingtStart = false;
                        readingSounds = true;
                        readingBackground = false;
                        readingColission = false;
                        readingObjects = false;
                    }
                    else if (line.Contains("[BACKGROUND]"))
                    {
                        readingtStart = false;
                        readingSounds = false;
                        readingBackground = true;
                        readingColission = false;
                        readingObjects = false;
                    }
                    else if (line.Contains("[COLISSION]"))
                    {
                        readingtStart = false;
                        readingSounds = false;
                        readingBackground = false;
                        readingColission = true;
                        readingObjects = false;
                    }
                    else if (line.Contains("[OBJECTS]"))
                    {
                        readingtStart = false;
                        readingSounds = false;
                        readingBackground = false;
                        readingColission = false;
                        readingObjects = true;
                    }

                    else if (readingtStart)
                    {
                        string[] tmp = line.Split(' ');
                        start = new Point(int.Parse(tmp[0]), int.Parse(tmp[1]));
                    }
                    else if (readingSounds)
                    {
                        SoundEffect tmp = manager.Load<SoundEffect>("sounds/"+line);
                        SoundEffectInstance sound = tmp.CreateInstance();
                        sounds.Add(sound);
                    }
                    else if (readingBackground)
                    {
                        background = manager.Load<Texture2D>("worlds/" + world + "/" + line);
                    }
                    else if (readingColission)
                    {
                        colission = ColissionLayer.FromFile("worlds/" + world + "/" + line);
                    }
                    else if (readingObjects)
                    {

                    }

                }
            }
            
            size = new Point(this.background.Width, this.background.Height);
            camera = new Camera(size.X, size.Y);
            mario = new Mario(batch, manager, start, colission);
        }
           
        

        public void Update(GameTime time)
        {
            //camera.Update(time);
            mario.Update(time);
            camera.Camerapos = new Vector2(mario.position.X +
                mario.CurrentAnimation.CurrentRect.Width - 128,
                mario.position.Y +
                mario.CurrentAnimation.CurrentRect.Height - 112);
        }

        public void Draw(GameTime time)
        {
            batch.Begin(
                SpriteBlendMode.AlphaBlend,
                SpriteSortMode.Deferred,
                SaveStateMode.None,
                camera.TransformMatrix);

            batch.Draw(background, new Rectangle(0, 0, size.X, size.Y), Color.White);
            mario.Draw(time);
            batch.End();
        }
    }
}
