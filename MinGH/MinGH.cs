using FlatRedBall;
using FlatRedBall.Screen;
using Microsoft.Xna.Framework;
using MinGH.Config;
using MinGH.Events;
using MinGH.Screens;
using MinGH.Enum;

namespace MinGH
{
    /// <summary>
    /// This is the main type for the game.
    /// </summary>
    public class MinGH : Microsoft.Xna.Framework.Game
    {
        // Global Content
        GraphicsDeviceManager graphics;
        GameConfiguration gameConfiguration;

        public MinGH()
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
            gameConfiguration = new GameConfiguration("config.xml");
            if (gameConfiguration.fullscreen)
            {
                graphics.ToggleFullScreen();
                graphics.PreferredBackBufferWidth = 1920;
                graphics.PreferredBackBufferHeight = 1080;

            }
            else
            {
                graphics.PreferredBackBufferWidth = 1024;
                graphics.PreferredBackBufferHeight = 768;
            }

            graphics.ApplyChanges();

            Window.Title = "MinGH";
            IsMouseVisible = true;
            FlatRedBallServices.InitializeFlatRedBall(this, graphics);
            ScreenManager.Start(typeof(MainMenu).FullName);

            // Feels sloppy, but is how event listeners/handlers are set
            EventBus<ScreenChange>.EventHandler screenChange = 
                new EventBus<ScreenChange>.EventHandler(ChangeGameState);
            EventBus<ScreenChange>.instance.Event += screenChange;

            base.Initialize();
        }

        private void poop()
        {
            this.Exit();
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
            FlatRedBallServices.Update(gameTime);
            ScreenManager.Activity();
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            FlatRedBallServices.Draw();
            base.Draw(gameTime);
        }

        /// <summary>
        /// The main game flow function.  Is called whenever a new game screen/state
        /// is to be displayed/entered.
        /// </summary>
        /// <param name="newState">The new game state to enter.</param>
        /// <param name="chartToUse">The "optional" chart location (is sent null if not used).</param>
        private void ChangeGameState(object sender, ScreenChange myEvent)
        {
            Screen currScreen = ScreenManager.CurrentScreen;
            GameState newState = myEvent.newState;
            switch (newState)
            {
                case GameState.QuitGame:
                    Exit();
                    break;
                case GameState.MainMenu:
                    currScreen.MoveToScreen(typeof(MainMenu).FullName);
                    break;
                case GameState.Options:
                    currScreen.MoveToScreen(typeof(Options).FullName);
                    break;
                case GameState.SinglePlayer:
                    //EnterNewGameScreen(new SinglePlayerScreen(this, graphics, chartToUse));
                    break;
                case GameState.SongSelection:
                    //EnterNewGameScreen(new SongSelectionScreen(this, graphics));
                    break;
            }
        }
    }
}
