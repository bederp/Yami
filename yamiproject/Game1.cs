using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace yamiproject
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Gamestate gamestate;
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += new EventHandler(Window_ClientSizeChanged);
            graphics.PreferredBackBufferWidth = (int)Globals.mario_res.X;
            graphics.PreferredBackBufferHeight = (int)Globals.mario_res.Y;
            graphics.IsFullScreen = false;
            Window.Title = "Yami 0.01";
            graphics.ApplyChanges();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            
            spriteBatch = new SpriteBatch(GraphicsDevice);
            gamestate = new Gamestate(spriteBatch, this.Content, GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            gamestate.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            gamestate.Draw(gameTime);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            Point point = new Point(1,1);
            
            if (Window.ClientBounds.Width >= (Globals.mario_res.X * 3))
            {
                graphics.PreferredBackBufferWidth = 3 * (int)Globals.mario_res.X;
                point.X = 3;
            }
            else if (Window.ClientBounds.Width >= (Globals.mario_res.X * 2))
            {
                graphics.PreferredBackBufferWidth = 2 * (int)Globals.mario_res.X;
                point.X = 2;
            }
            else
            {
                graphics.PreferredBackBufferWidth = (int)Globals.mario_res.X;
                point.X = 1;
            }
                
            if (Window.ClientBounds.Height >= (Globals.mario_res.Y * 3))
            {
                graphics.PreferredBackBufferHeight = 3 * (int)Globals.mario_res.Y;
                point.Y = 3;
            }
            else if (Window.ClientBounds.Height >= (Globals.mario_res.Y * 2))
            {
                graphics.PreferredBackBufferHeight = 2 * (int)Globals.mario_res.Y;
                point.Y = 2;
            }
            else
            {
                graphics.PreferredBackBufferHeight = (int)Globals.mario_res.Y;
                point.Y = 1;
            }
            Globals.Scale = new Vector2(point.X, point.Y);
            graphics.ApplyChanges();
        }

    }
}
