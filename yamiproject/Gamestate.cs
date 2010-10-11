using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using System.Threading;
using Microsoft.Xna.Framework.Input;

namespace yamiproject
{
    class Gamestate
    {
        Intro intro;
        Level level;
        int passedtime = 0;
        ContentManager manager;
        SpriteBatch batch;
        GraphicsDevice graphics;
        SpriteFont font;
        SpriteFont font2;
        string[] hud = new string[] { "MARIO", "WORLD", "TIME" };
        Vector2[] hud_positions = new Vector2[] { new Vector2(24, 8), new Vector2(144, 8), new Vector2(201, 8),
                                                  new Vector2(24, 16),new Vector2(97, 16), new Vector2(153, 16), new Vector2(207, 16)};
        int score = 0;
        int coins = 0;
        int lives = 1;
        int[] world = { 1, 1 };
        int mariotime = 400;

        enum State
        {
            intro,
            demo,
            info,
            level,
            black
        }

        State state;

        public Gamestate(SpriteBatch batch, ContentManager manager, GraphicsDevice graphics)
        {
            this.manager = manager;
            this.batch = batch;
            this.graphics = graphics;
            font = manager.Load<SpriteFont>("fonts/font");
            font2 = manager.Load<SpriteFont>("fonts/font2");
            intro = new Intro(batch, manager);
            state = State.intro;
            intro.oninfo += new EventHandler(intro_oninfo);
            level = new Level(batch, manager, "1-1");
            //level = new Level(batch, manager, "worlds/1-1/"); After implementing IO FOR MAP AND LEVEL

        }


        public void Update(GameTime time)
        {
            KeyboardState key = Keyboard.GetState();
            if (key.IsKeyDown(Keys.OemTilde) == true)
            {
                level.KillSound();
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
                    InfoUpdate(time);
                    break;
                case State.level:
                    level.Update(time);
                    break;
                case State.demo:

                    break;
            }

        }

        public void Draw(GameTime time)
        {
            switch (state)
            {
                case State.intro:
                    intro.Draw(time);
                    break;
                case State.info:
                    InfoDraw(time);
                    break;
                case State.level:
                    level.Draw(time);
                    break;
                case State.demo:

                    break;
            }
            GamestateDraw(time);

        }

        private void InfoUpdate(GameTime time)
        {
            passedtime += time.ElapsedGameTime.Milliseconds;
            Console.WriteLine("game time = " +passedtime);
            if (passedtime >= 2000 ) //2000
            {
                passedtime = 0;
                state = State.level;
            }
        }

        private void InfoDraw(GameTime time)
        {
            batch.Begin();
            graphics.Clear(Color.Black);
            Texture2D mario_hud = manager.Load<Texture2D>("images/mariohud");

            batch.Draw(mario_hud, new Vector2(100, 97) * (float)Globals.Scale, null, Color.White,
                0f, Vector2.Zero, Globals.Scale,
                SpriteEffects.None, 0f);
            
            string tmp = hud[1] + " " + world[0].ToString() + "-" + world[1].ToString();
            batch.DrawString(font, tmp, new Vector2(89, 72) * (float)Globals.Scale,
                    Color.White, 0, Vector2.Zero,
                    Globals.Scale, SpriteEffects.None, 0f);
            
            tmp = "x   " + lives.ToString();
            batch.DrawString(font, tmp, new Vector2(122, 104) * (float)Globals.Scale,
                    Color.White, 0, Vector2.Zero,
                    Globals.Scale, SpriteEffects.None, 0f);
            batch.End();

        }

        private void GamestateDraw(GameTime time)
        {

            batch.Begin();
            for (int i = 0; i < 3; i++)
            {
                batch.DrawString(font, hud[i], hud_positions[i] * Globals.Scale,
                    Color.White, 0, Vector2.Zero,
                    Globals.Scale, SpriteEffects.None, 0f);
            }

            batch.DrawString(font, String.Format("{0:000000.}", score), hud_positions[3] * Globals.Scale,
                    Color.White, 0, Vector2.Zero,
                    Globals.Scale, SpriteEffects.None, 0f);

            String tmp = "x" + String.Format("{0:00.}", coins);
            batch.DrawString(font, tmp, hud_positions[4] * Globals.Scale,
                    Color.White, 0, Vector2.Zero,
                    Globals.Scale, SpriteEffects.None, 0f);
            
            tmp = world[0].ToString() + "-" + world[1].ToString();
            batch.DrawString(font, tmp, hud_positions[5] * Globals.Scale,
                    Color.White, 0, Vector2.Zero,
                    Globals.Scale, SpriteEffects.None, 0f);

            if (state == State.level)
            {
                passedtime += time.ElapsedGameTime.Milliseconds;
                if (mariotime <= 100)
                {
                    if (passedtime >= 250)
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

                batch.DrawString(font, String.Format("{0:000.}", mariotime), hud_positions[6] * Globals.Scale,
                        Color.White, 0, Vector2.Zero,
                        Globals.Scale, SpriteEffects.None, 0f);
            }
            batch.End();
        }

        void intro_oninfo(object sender, EventArgs e)
        {
            mariotime = 400;
            state = State.info;
        }
    }
}
