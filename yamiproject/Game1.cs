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
            graphics.PreferredBackBufferWidth = (int)Globals.mario_res.X*Globals.Scale;
            graphics.PreferredBackBufferHeight = (int)Globals.mario_res.Y*Globals.Scale;
            graphics.IsFullScreen = false;
            Window.Title = "Yami 0.05";
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
            Gamestate.Initialize(spriteBatch, this.Content, GraphicsDevice);

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

            Gamestate.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            Gamestate.Draw(gameTime);
            base.Draw(gameTime);
        }

        void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            int scale = 1;
            
            if (Window.ClientBounds.Width >= (Globals.mario_res.X * 3))
            {
                graphics.PreferredBackBufferWidth = 3 * Globals.mario_res.X;
                graphics.PreferredBackBufferHeight = 3 * Globals.mario_res.Y;
                scale = 3;
            }
            else if (Window.ClientBounds.Width >= (Globals.mario_res.X * 2))
            {
                graphics.PreferredBackBufferWidth = 2 * Globals.mario_res.X;
                graphics.PreferredBackBufferHeight = 2 * Globals.mario_res.Y;
                scale = 2;
            }
            else
            {
                graphics.PreferredBackBufferWidth = Globals.mario_res.X;
                graphics.PreferredBackBufferHeight = Globals.mario_res.Y;
                scale = 1;
            }

            Globals.Scale = scale;

            graphics.ApplyChanges();
        }

    }
}
