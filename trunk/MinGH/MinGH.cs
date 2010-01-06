using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MinGH.GameScreen;
using System.Collections.Generic;
using MinGH.GameScreen.MainMenu;
using MinGH.GameScreen.SinglePlayer;

namespace MinGH
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class MinGHMain : Microsoft.Xna.Framework.Game
    {
        // Global Content
        GraphicsDeviceManager graphics;
        bool game = false;

        public MinGHMain()
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
            Window.Title = "MinGH";
            //Components.Add(new SinglePlayerScreen(this, graphics));
            Components.Add(new MainMenuScreen(this, graphics));
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            if (Keyboard.GetState().IsKeyDown(Keys.P) && !game)
            {
                Components.Clear();
                Components.Add(new SinglePlayerScreen(this, graphics));
                game = true;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.P) && game)
            {
                Components.Clear();
                Components.Add(new MainMenuScreen(this, graphics));
                game = false;
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            base.Draw(gameTime);
        }
    }
}
