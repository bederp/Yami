using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;



namespace yamiproject
{
    class Intro
    {
        Texture2D welcome;
        Texture2D select;
        Vector2[] select_positions = new Vector2[] { new Vector2(72, 136), new Vector2( 72, 152 ) };
        int select_choice = 0;
        ContentManager manager;
        SpriteBatch batch;
        SoundEffectInstance sound;
        KeyboardState prevkey;
        SpriteFont font;
        string[] hud = new string[] { "1 PLAYER GAME", "2 PLAYER GAME", "TOP-"};
        Vector2[] hud_positions = new Vector2[] { new Vector2(89, 136), new Vector2(89, 153), new Vector2(97, 176) };
        int top = 0;
        Vector2 top_position = new Vector2(136, 176);
        public EventHandler oninfo;

        public Intro(SpriteBatch batch, ContentManager manager)
        {
            this.manager = manager;
            this.batch = batch;
            welcome = manager.Load<Texture2D>("images/welcome");
            select = manager.Load<Texture2D>("images/select");
            font = manager.Load<SpriteFont>("fonts/font");
            SoundEffect tmp = manager.Load<SoundEffect>("sounds/title");
            sound = tmp.CreateInstance();
            sound.IsLooped = true;
            sound.Play();
            

        }

        public void Update(GameTime time)
        {
            KeyboardState key = Keyboard.GetState();
            if (key.IsKeyDown(Keys.S) == true && prevkey.IsKeyDown(Keys.S) == false)
                select_choice = (++select_choice) % 2;

            if (select_choice == 0)
                if (key.IsKeyDown(Keys.Enter) == true)
                {
                    sound.Stop();
                    oninfo(this, null);
                }
            prevkey = key;
            

        }

        public void Draw(GameTime time)
        {

            batch.Begin();
            batch.Draw(welcome, Vector2.Zero * Globals.scale, null, Color.White,
                0f, Vector2.Zero, Globals.scale, 
                SpriteEffects.None, 0f );
            batch.Draw(select, select_positions[select_choice] * Globals.scale, null, Color.White,
                0f, Vector2.Zero, Globals.scale,
                SpriteEffects.None, 0f);

            for (int i = 0; i < 3; i++ )
            {
                batch.DrawString(font, hud[i], hud_positions[i] * Globals.scale,
                    Color.White, 0, Vector2.Zero,
                    Globals.scale, SpriteEffects.None, 0f);
            }

            batch.DrawString(font, String.Format("{0:000000.}", top), top_position * Globals.scale,
                    Color.White, 0, Vector2.Zero,
                    Globals.scale, SpriteEffects.None, 0f);
            batch.End();

        }


        internal void MusicStart()
        {
            sound.Play();
        }
    }
}
