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
        bool killsound = false;
        Camera camera;
        List<SoundEffectInstance> sounds;
        List<Sprite> staticobjects;
        List<Sprite> moveableobjects;
        Mario mario;
        ColissionLayer colission;

        public Map(SpriteBatch batch, ContentManager manager, string world, string filename)
        {
            this.manager = manager;
            this.batch = batch;
            sounds = new List<SoundEffectInstance>();
            staticobjects = new List<Sprite>();
            moveableobjects = new List<Sprite>();
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
                        ObjectsLayer tmp = ObjectsLayer.FromFile("worlds/" + world + "/" + line);
                        
                        for (int x = 0; x < tmp.width; x++)
                        {
                            for (int y = 0; y < tmp.height; y++)
                            {
                                if (tmp.GetCellIndex(x, y) == 0)
                                    continue;
                                if (tmp.GetCellIndex(x, y) > 0 && tmp.GetCellIndex(x, y) < 5)
                                {
                                    Box tmp3 = new Box(batch, manager, tmp.GetCellIndex(x, y), new Point(Globals.ConvertCellToX(x), Globals.ConvertCellToY(y)));
                                    staticobjects.Add(tmp3);
                                }
                                else if (tmp.GetCellIndex(x, y) > 4 && tmp.GetCellIndex(x, y) < 7)
                                {
                                    Brick tmp3 = new Brick(batch, manager, tmp.GetCellIndex(x, y), new Point(Globals.ConvertCellToX(x), Globals.ConvertCellToY(y)));
                                    staticobjects.Add(tmp3);
                                }

                            }
                        }
 

                    }

                }
            }
            
            size = new Point(this.background.Width, this.background.Height);
            camera = new Camera(size.X, size.Y);
            mario = new Mario(batch, manager, start, colission, staticobjects, moveableobjects);
        }
           
        

        public void Update(GameTime time)
        {
            mario.Update(time);

            if(!killsound)
                if (sounds[1].State == SoundState.Stopped)
                    if (sounds[0].State == SoundState.Stopped)
                        sounds[0].Play();
            camera.Camerapos = new Vector2(mario.position.X +
                mario.CurrentAnimation.CurrentRect.Width - 128,
                mario.position.Y +
                mario.CurrentAnimation.CurrentRect.Height - 112);

            mario.minx = (int)camera.Camerapos.X;
            foreach (Sprite s in staticobjects)
            {
                s.Update(time);
            }

        }

        public void Draw(GameTime time)
        {
            batch.Begin(
                SpriteBlendMode.AlphaBlend,
                SpriteSortMode.Deferred,
                SaveStateMode.None,
                camera.TransformMatrix);

            batch.Draw(background, new Rectangle(0, 0, size.X, size.Y), Color.White);
            
            foreach (Sprite s in staticobjects)
            {
                s.Draw(time);
            }
            mario.Draw(time);
            
            batch.End();
        }

        internal void KillSound()
        {
            killsound = true;
            foreach (SoundEffectInstance s in sounds)
            {
                s.Stop();
            }
        }

        public void MapRestart()
        {
            Gamestate.Restart();
            killsound = false;
            mario.curstate2 = Mario.State.falling;
            mario.orientation = true;
            mario.beginjump = true;
            mario.deathfall = false;
            mario.position = start;
            mario.timeofdeath = 0;
            camera.camerapos = new Vector2(0f, 0f);
            sounds[0].Pitch = 0f;
            sounds[0].Play();

            foreach (Sprite s in staticobjects)
            {
                s.Restart();
            }
        }

        public void MapSpeedUp()
        {
            sounds[0].Stop();
            sounds[0].Pitch = 0.6f;
            sounds[1].Play();
        }

        internal void TimeUp()
        {
            mario.curstate2 = Mario.State.die;
        }
    }
}
