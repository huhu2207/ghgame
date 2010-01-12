using Microsoft.Xna.Framework;
using MinGH.Enum;
using MinGH.GameScreen;
using MinGH.GameScreen.MainMenu;
using MinGH.GameScreen.SinglePlayer;
using MinGH.GameScreen.SongSelection;

namespace MinGH
{
    /// <summary>
    /// This is the main type for the game.
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

        /// <summary>
        /// The main game flow function.  Is called whenever a new game screen/state
        /// is to be displayed/entered.
        /// </summary>
        /// <param name="newState">The new game state to enter.</param>
        /// <param name="chartToUse">The "optional" chart location (is sent null if not used).</param>
        public void ChangeGameState(GameStateEnum newState, ChartLocation chartToUse)
        {
            switch (newState)
            {
                case GameStateEnum.QuitGame:
                    Exit();
                    break;
                case GameStateEnum.MainMenu:
                    EnterNewGameScreen(new MainMenuScreen(this, graphics));
                    break;
                case GameStateEnum.SinglePlayer:
                    EnterNewGameScreen(new SinglePlayerScreen(this, graphics, chartToUse));
                    break;
                case GameStateEnum.SongSelection:
                    EnterNewGameScreen(new SongSelectionScreen(this, graphics));
                    break;
            }
        }

        /// <summary>
        /// Removes all current components from the component list and adds a new
        /// screen ontop of the list.
        /// TODO: This method removes ALL game components, which can be a problem if 
        /// I want to use random components outside of the screen spectrum.  
        /// Components.Remove() did not work from what I tried.
        /// </summary>
        /// <param name="newScreen">The new screen to display.</param>
        private void EnterNewGameScreen(DrawableGameComponent newScreen)
        {
            Components.Clear();
            Components.Add(newScreen);
        }
    }
}
