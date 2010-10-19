using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using System.Threading;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace yamiproject
{
    static class Gamestate
    {
        static Intro intro;
        static Level level;
        static int passedtime = 0;
        static SoundEffectInstance gameover;
        static ContentManager manager;
        static SpriteBatch batch;
        static GraphicsDevice graphics;
        static SpriteFont font;
        static SpriteFont font2;
        static string[] hud = new string[] { "MARIO", "WORLD", "TIME" };
        static Vector2[] hud_positions = new Vector2[] { new Vector2(24, 8), new Vector2(144, 8), new Vector2(201, 8),
                                                  new Vector2(24, 16),new Vector2(97, 16), new Vector2(153, 16), new Vector2(207, 16)};
        static int[] world = { 1, 1 };
        public static int mariotime;
        static bool speedup = false;
        static public bool timeup = false;

        public enum State
        {
            intro,
            info,
            level,
            gemeover
        }

        public static State state;


        public static void Update(GameTime time)
        {
            KeyboardState key = Keyboard.GetState();
            if (key.IsKeyDown(Keys.OemTilde) == true)
            {
                level.GetCurrentMap().KillSound();
                intro.MusicStart();
                state = State.intro;
                level = new Level(batch, manager, "1-1");
            }
            
            switch (state) 
            {
                case State.intro:             
                    intro.Update(time);
                    break;
                case State.info:
                    GameInfoUpdate(time);
                    break;
                case State.level:
                    level.Update(time);
                    break;
                case State.gemeover:
                    GameoverUpdate(time);
                    break;
            }
            GameStateUpdate(time);
        }

        private static void GameStateUpdate(GameTime time)
        {
            if (state == State.level)
            {
                passedtime += time.ElapsedGameTime.Milliseconds;
                if (mariotime <= 100)
                {
                    if (mariotime <= 0)
                    {
                        if (!timeup)
                        {
                            timeup = true;
                            level.GetCurrentMap().TimeUp();
                        }
                        return;
                    }
                    if (!speedup)
                    {
                        speedup = true;
                        level.GetCurrentMap().MapSpeedUp();
                    }
                    if (passedtime >= 250)//250
                    {
                        passedtime = 0;
                        mariotime--;
                    }
                }
                else if (passedtime >= 500)
                {
                    passedtime = 0;
                    mariotime--;
                }
            }
        }

        public static void GameoverUpdate(GameTime time)
        {
            
            gameover.Play();
            passedtime += time.ElapsedGameTime.Milliseconds;
            if (passedtime >= 3500) //3500
            {
                passedtime = 0;
                Score.SetTop();
                intro.MusicStart();
                state = State.intro;
            }
        }

        public static void Draw(GameTime time)
        {
            switch (state)
            {
                case State.intro:
                    intro.Draw(time);
                    break;
                case State.info:
                    GameInfoDraw(time);
                    break;
                case State.level:
                    level.Draw(time);
                    break;
                case State.gemeover:
                    GameoverDraw(time);
                    break;
            }
            GamestateDraw(time);

        }

        public static void GameoverDraw(GameTime time)
        {
            batch.Begin();
            graphics.Clear(Color.Black);
            
            batch.DrawString(font, "GAME OVER", new Vector2(100, 100) * (float)Globals.Scale,
                    Color.White, 0, Vector2.Zero,
                    Globals.Scale, SpriteEffects.None, 0f);
            batch.End();
        }

        private static void GameInfoUpdate(GameTime time)
        {
            passedtime += time.ElapsedGameTime.Milliseconds;
            if (passedtime >= 2000 ) //2000
            {
                passedtime = 0;
                level.GetCurrentMap().MapRestart();
                state = State.level;
            }
        }

        private static void GameInfoDraw(GameTime time)
        {
            batch.Begin();
            graphics.Clear(Color.Black);
            Texture2D mario_hud = manager.Load<Texture2D>("images/mariohud");

            batch.Draw(mario_hud, new Vector2(95, 97) * (float)Globals.Scale, null, Color.White,
                0f, Vector2.Zero, Globals.Scale,
                SpriteEffects.None, 0f);
            
            string tmp = hud[1] + " " + world[0].ToString() + "-" + world[1].ToString();
            batch.DrawString(font, tmp, new Vector2(89, 72) * (float)Globals.Scale,
                    Color.White, 0, Vector2.Zero,
                    Globals.Scale, SpriteEffects.None, 0f);
            
            tmp = "x   " + Score.lives.ToString();
            batch.DrawString(font, tmp, new Vector2(117, 104) * (float)Globals.Scale,
                    Color.White, 0, Vector2.Zero,
                    Globals.Scale, SpriteEffects.None, 0f);
            batch.End();

        }

        private static void GamestateDraw(GameTime time)
        {

            batch.Begin();
            for (int i = 0; i < 3; i++)
            {
                batch.DrawString(font, hud[i], hud_positions[i] * Globals.Scale,
                    Color.White, 0, Vector2.Zero,
                    Globals.Scale, SpriteEffects.None, 0f);
            }

            batch.DrawString(font, String.Format("{0:000000.}", Score.score), hud_positions[3] * Globals.Scale,
                    Color.White, 0, Vector2.Zero,
                    Globals.Scale, SpriteEffects.None, 0f);

            String tmp = "x" + String.Format("{0:00.}", Score.coins);
            batch.DrawString(font, tmp, hud_positions[4] * Globals.Scale,
                    Color.White, 0, Vector2.Zero,
                    Globals.Scale, SpriteEffects.None, 0f);
            
            tmp = world[0].ToString() + "-" + world[1].ToString();
            batch.DrawString(font, tmp, hud_positions[5] * Globals.Scale,
                    Color.White, 0, Vector2.Zero,
                    Globals.Scale, SpriteEffects.None, 0f);

            if (state == State.level)
            {
                batch.DrawString(font, String.Format("{0:000.}", mariotime), hud_positions[6] * Globals.Scale,
                        Color.White, 0, Vector2.Zero,
                        Globals.Scale, SpriteEffects.None, 0f);
            }
            batch.End();
        }

        public static void Initialize(SpriteBatch batch, ContentManager manager, GraphicsDevice graphics)
        {
            Gamestate.manager = manager;
            Gamestate.batch = batch;
            Gamestate.graphics = graphics;
            font = manager.Load<SpriteFont>("fonts/font");
            font2 = manager.Load<SpriteFont>("fonts/font2");
            intro = new Intro(batch, manager);
            state = State.intro;
            level = new Level(batch, manager, "1-1");
            SoundEffect tmp = manager.Load<SoundEffect>("sounds/gameover");
            gameover = tmp.CreateInstance();
        }

        public static void StartNewGame(int select_choice)
        {
            if (select_choice == 0) // 1 Player Game
            {
                mariotime = 105;//400
                Score.Reset();
                state = State.info;
            }
        }

        public static void BackgroundMusicStop()
        {
            level.GetCurrentMap().KillSound();
        }

        public static void Restart()
        {
            speedup = false;
            timeup = false;
            passedtime = 0;
            mariotime = 105;//400
        }

        public static void GameOver()
        {
            Restart();
            state = State.gemeover;
        }

        public static void GameInfo()
        {
            Restart();
            state = State.info;
        }
    }
}
