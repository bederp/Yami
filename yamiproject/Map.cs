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
        Texture2D flag;
        SpriteBatch batch;
        ContentManager manager;
        Point size;
        Point start;
        bool killsound = false;
        Camera camera;
        List<Globals.Teleport> teleports;
        List<SoundEffectInstance> sounds;
        List<Sprite> staticobjects;
        List<Sprite> moveableobjects;
        Mario mario;
        Point flagposition;
        Point flagpositionbackup;
        bool flagexists = false;
        public bool flaggodown = false;
        ColissionLayer colission;

        public Map(SpriteBatch batch, ContentManager manager, string world, string filename)
        {
            this.manager = manager;
            this.batch = batch;
            teleports = new List<Globals.Teleport>();
            sounds = new List<SoundEffectInstance>();
            staticobjects = new List<Sprite>();
            moveableobjects = new List<Sprite>();
            bool readingtStart = false;
            bool readingSounds = false;
            bool readingBackground = false;
            bool readingColission = false;
            bool readingObjects = false;
            bool readingFlag = false;
            bool readingTeleports = false;


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
                        readingFlag = false;
                        readingTeleports = false;
                    }
                    else if (line.Contains("[FLAG]"))
                    {
                        readingtStart = false;
                        readingSounds = true;
                        readingBackground = false;
                        readingColission = false;
                        readingObjects = false;
                        readingFlag = true;
                        readingTeleports = false;
                    }
                    else if (line.Contains("[TELEPORTS]"))
                    {
                        readingtStart = false;
                        readingSounds = false;
                        readingBackground = false;
                        readingColission = false;
                        readingObjects = false;
                        readingFlag = false;
                        readingTeleports = true;
                    }
                    else if (line.Contains("[SOUNDS]"))
                    {
                        readingtStart = false;
                        readingSounds = true;
                        readingBackground = false;
                        readingColission = false;
                        readingObjects = false;
                        readingFlag = false;
                        readingTeleports = false;
                    }
                    else if (line.Contains("[BACKGROUND]"))
                    {
                        readingtStart = false;
                        readingSounds = false;
                        readingBackground = true;
                        readingColission = false;
                        readingObjects = false;
                        readingFlag = false;
                        readingTeleports = false;
                    }
                    else if (line.Contains("[COLISSION]"))
                    {
                        readingtStart = false;
                        readingSounds = false;
                        readingBackground = false;
                        readingColission = true;
                        readingObjects = false;
                        readingFlag = false;
                        readingTeleports = false;
                    }
                    else if (line.Contains("[OBJECTS]"))
                    {
                        readingtStart = false;
                        readingSounds = false;
                        readingBackground = false;
                        readingColission = false;
                        readingObjects = true;
                        readingFlag = false;
                        readingTeleports = false;
                    }

                    else if (readingtStart)
                    {
                        string[] tmp = line.Split(' ');
                        start = new Point(int.Parse(tmp[0]), int.Parse(tmp[1]));
                    }
                    else if (readingFlag)
                    {
                        flagexists = true;
                        string[] tmp = line.Split(' ');
                        flagposition = new Point(int.Parse(tmp[0]), int.Parse(tmp[1]));
                        flagpositionbackup = new Point(int.Parse(tmp[0]), int.Parse(tmp[1]));
                    }
                    else if (readingTeleports)
                    {
                        
                        string[] tmp = line.Split(' ');
                        Globals.Teleport tmp2 = new Globals.Teleport(int.Parse(tmp[0]),
                                                                        int.Parse(tmp[1]), 
                                                                        int.Parse(tmp[2]),
                                                                        new Point(int.Parse(tmp[3]), int.Parse(tmp[4])));
                        teleports.Add(tmp2);
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

            flag = manager.Load<Texture2D>("images/flag");
            size = new Point(this.background.Width, this.background.Height);
            camera = new Camera(size.X, size.Y);
            mario = new Mario(batch, manager, start, colission, staticobjects, moveableobjects);
        }    

        public void Update(GameTime time)
        {
            mario.Update(time);

            if (mario.position.X >= flagposition.X && flagexists)
                mario.Flagpole(flagposition.X);
            FlagUpdate();

            TeleportsCheck();

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

        private void FlagUpdate()
        {
            if (flagexists)
            {
                if (flaggodown)
                {
                    if (flagposition.Y < 169)
                    {
                        flagposition.Y += 2;
                    }
                    else
                    {
                        flagposition.Y = 169;
                        flaggodown = false;
                    }
                }
            }
        }

        private void TeleportsCheck()
        {
            foreach (Globals.Teleport s in teleports)
            {
                if (s.type == 1)
                {
                    if (mario.position.X >= s.x && mario.position.X <= s.x + 10)
                    {

                        if (mario.ducking)
                        {
                            if (s.where == 9)
                            {
                                Gamestate.delaytime = 2200;
                                Gamestate.BackgroundMusicStop();
                                Gamestate.state = Gamestate.State.rickroll;
                                return;
                            }
                            
                            Console.WriteLine("On teleport to map{0}", s.where);
                            SoundEffect tmp = manager.Load<SoundEffect>("sounds/pipe");
                            tmp.Play();
                            Gamestate.GetCurrentLevel().SwitchMap(s.where, s.mariopos);
                        }
                    }
                }
                else if (s.type == 2)
                {
                    if (mario.position.X == s.x)
                    {

                        if (mario.Movementx>0 && mario.curstate2 == Mario.State.standing)
                        {
                            Console.WriteLine("On teleport to map{0}", s.where);
                            SoundEffect tmp = manager.Load<SoundEffect>("sounds/pipe");
                            tmp.Play();
                            Gamestate.GetCurrentLevel().SwitchMap(s.where, s.mariopos);
                        }
                    }
                }
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
            if(flagexists)
                batch.Draw(flag, new Vector2(flagposition.X, flagposition.Y), Color.White);

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
            flaggodown = false;
            flagposition = flagpositionbackup;
            mario.Restart(start);
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

        internal void StartFlag()
        {
            Gamestate.timerstopped = true;
            KillSound();
            SoundEffect tmp = manager.Load<SoundEffect>("sounds/goal");
            tmp.Play();
            flaggodown = true;
        }

        public void SetMarioPosition(Point pos)
        {
            mario.position = pos;
        }

        internal void StartSound()
        {
            sounds[0].Play();
        }
    }
}
