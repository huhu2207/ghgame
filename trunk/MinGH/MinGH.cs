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
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        public void ChangeGameState(int code)
        {
            switch (code)
            {
                case -1:
                    Exit();
                    break;
                case 0:
                    EnterNewGameScreen(new MainMenuScreen(this, graphics));
                    break;
                case 1:
                    EnterNewGameScreen(new SinglePlayerScreen(this, graphics));
                    break;
            }
        }

        private void EnterNewGameScreen(DrawableGameComponent newScreen)
        {
            Components.Clear();
            Components.Add(newScreen);
        }
    }
}
