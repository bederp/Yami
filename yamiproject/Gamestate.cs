using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace yamiproject
{
    class Gamestate
    {
        Intro intro;
        ContentManager manager;
        SpriteBatch batch;
        GraphicsDevice graphics;
        SpriteFont font;
        SpriteFont font2;
        Vector2 scale;
        string[] hud = new string[] { "MARIO", "WORLD", "TIME" };
        Vector2[] hud_positions = new Vector2[] { new Vector2(24, 8), new Vector2(144, 8), new Vector2(201, 8),
                                                  new Vector2(24, 16),new Vector2(97, 16), new Vector2(153, 16), new Vector2(207, 16)};
        int score = 0;
        int coins = 0;
        int lives = 1;
        int[] world = { 1, 1 };
        int time = 400;

        enum State
        {
            intro,
            demo,
            info,
            game
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
            

        }

        public void Update(GameTime time)
        {
            
            switch (state) 
            {
                case State.intro:             
                    intro.Update(time);
                    break;
                case State.info:

                    break;
                case State.game:

                    break;
                case State.demo:

                    break;
            }

        }

        public void Draw(GameTime time)
        {
            scale = new Vector2(Globals.scale.X, Globals.scale.Y);
            switch (state)
            {
                case State.intro:
                    intro.Draw(time);
                    break;
                case State.info:
                    InfoDraw();
                    break;
                case State.game:

                    break;
                case State.demo:

                    break;
            }
            GamestateDraw(time);

        }

        private void InfoDraw()
        {
            graphics.Clear(Color.Black);
            Texture2D mario_hud = manager.Load<Texture2D>("images/mariohud");

            batch.Draw(mario_hud, new Vector2(100, 97) * scale, null, Color.White,
                0f, Vector2.Zero, scale,
                SpriteEffects.None, 0f);
            
            string tmp = hud[1] + " " + world[0].ToString() + "-" + world[1].ToString();
            batch.DrawString(font, tmp, new Vector2(89, 72) * scale,
                    Color.White, 0, Vector2.Zero,
                    scale, SpriteEffects.None, 0f);
            
            tmp = "x   " + lives.ToString();
            batch.DrawString(font, tmp, new Vector2(122, 104) * scale,
                    Color.White, 0, Vector2.Zero,
                    scale, SpriteEffects.None, 0f);

        }

        private void GamestateDraw(GameTime time)
        {
            
            
            for (int i = 0; i < 3; i++)
            {
                batch.DrawString(font, hud[i], hud_positions[i] * scale,
                    Color.White, 0, Vector2.Zero,
                    scale, SpriteEffects.None, 0f);
            }

            batch.DrawString(font, String.Format("{0:000000.}", score), hud_positions[3]* scale,
                    Color.White, 0, Vector2.Zero,
                    scale, SpriteEffects.None, 0f);

            String tmp = "x" + String.Format("{0:00.}", coins);
            batch.DrawString(font, tmp,  hud_positions[4] * scale,
                    Color.White, 0, Vector2.Zero,
                    scale, SpriteEffects.None, 0f);
            
            tmp = world[0].ToString() + "-" + world[1].ToString();
            batch.DrawString(font, tmp, hud_positions[5] * scale,
                    Color.White, 0, Vector2.Zero,
                    scale, SpriteEffects.None, 0f);
            
            if(state == State.game) 
                batch.DrawString(font, String.Format("{0:000000.}", time), hud_positions[6] * scale,
                        Color.White, 0, Vector2.Zero,
                        scale, SpriteEffects.None, 0f);
        }

        void intro_oninfo(object sender, EventArgs e)
        {
            state = State.info;
        }
    }
}
