using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MinGH.GameScreen;
using System.Collections.Generic;
using MinGH.GameScreen.BlankScreen;
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
            //gameScreenList.Add(new GameScreenGameplaySingleplayer());
            //gameScreenList.Add(new BlankScreen());

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
            this.Components.Add(new SinglePlayerScreen(this, graphics));
            this.Components.Add(new BlankScreen(this, graphics));
            //foreach (GameScreen currScreen in gameScreenList)
            //{
            //    currScreen.Initialize(graphics);
            //}
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            //foreach (GameScreen currScreen in gameScreenList)
            //{
            //    if (currScreen.isActive)
            //    {
            //        currScreen.LoadContent(Content, graphics);
            //    }
            //}
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            //foreach (GameScreen currScreen in gameScreenList)
            //{
            //    currScreen.UnloadContent();
            //}
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

            //foreach (GameScreen currScreen in gameScreenList)
            //{
            //    if (currScreen.isActive)
            //    {
            //        currScreen.Update(gameTime);
            //    }
            //}

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //foreach (GameScreen currScreen in gameScreenList)
            //{
            //    if (currScreen.isActive)
            //    {
            //        currScreen.Draw(gameTime);
            //    }
            //}
            base.Draw(gameTime);
        }
    }
}
