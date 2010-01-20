using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MinGH.Config;
using MinGH.Enum;
using MinGH.GameStringImpl;
using MinGH.MiscClasses;

namespace MinGH.GameScreen.SongSelection
{
    /// <summary>
    /// Finds and displays every chart within the "./songs" directory.
    /// Selecting a song will proceed to the single player game screen
    /// using the selected song.
    /// </summary>
    public class SongSelectionScreen : DrawableGameComponent
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        MinGHMain gameReference;
        List<ChartLocation> chartPaths;
        GameConfiguration gameConfiguration;

        MenuSet songSelectionMenuSet;
        SpriteFont gameFont;
        IKeyboardInputManager keyboardInputManager;

        public SongSelectionScreen(MinGHMain game, GraphicsDeviceManager graph)
            : base((Game)game)
        {
            gameReference = game;
            graphics = graph;
            songSelectionMenuSet = new MenuSet();
            keyboardInputManager = new KeyboardInputManager();
        }

        public override void Initialize()
        {
            gameConfiguration = new GameConfiguration("./config.xml");
            chartPaths = ChartFinder.GenerateAllChartPaths(gameConfiguration.songDirectory);

            Menu songSelectionMenu = new Menu("Song Selection", new Vector2(graphics.GraphicsDevice.Viewport.Width / 2f, graphics.GraphicsDevice.Viewport.Height / 4f));
            songSelectionMenu.titleScaling = new Vector2(5.0f, 5.0f);
            songSelectionMenu.entryScaling = new Vector2(2.0f, 2.0f);
            songSelectionMenu.scrollable = true;
            for (int i = 0; i < chartPaths.Count; i++)
            {
                songSelectionMenu.AddEntry(chartPaths[i].directory);
            }
            songSelectionMenuSet.AddMenu(songSelectionMenu);

            Menu instrumentSelectionMenu = new Menu("Instrument Selection", new Vector2(graphics.GraphicsDevice.Viewport.Width / 2f, graphics.GraphicsDevice.Viewport.Height / 4f));
            instrumentSelectionMenu.titleScaling = new Vector2(5.0f, 5.0f);
            instrumentSelectionMenu.entryScaling = new Vector2(2.0f, 2.0f);
            instrumentSelectionMenu.scrollable = true;
            instrumentSelectionMenu.AddEntry("Single Player Guitar");
            instrumentSelectionMenu.AddEntry("Co-op Guitar");
            instrumentSelectionMenu.AddEntry("Co-op Rhythm/Bass");
            instrumentSelectionMenu.AddEntry("Drums");
            songSelectionMenuSet.AddMenu(instrumentSelectionMenu);

            spriteBatch = new SpriteBatch(graphics.GraphicsDevice);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            gameFont = Game.Content.Load<SpriteFont>("Arial");  // Load the font

            base.LoadContent();
        }

        protected override void UnloadContent()
        {
        }

        public override void Update(GameTime gameTime)
        {
            keyboardInputManager.processKeyboardState(Keyboard.GetState());

            // Menu navagation logic
            if (keyboardInputManager.keyIsHit(Keys.Down) || keyboardInputManager.keyIsHit(KeyboardConfiguration.downStrum))
            {
                songSelectionMenuSet.SelectNextEntryInCurrentMenu(graphics.GraphicsDevice.Viewport.Height);
            }
            else if (keyboardInputManager.keyIsHit(Keys.Up) || keyboardInputManager.keyIsHit(KeyboardConfiguration.upStrum))
            {
                songSelectionMenuSet.SelectPreviousEntryInCurrentMenu(graphics.GraphicsDevice.Viewport.Height);
            }
            if (keyboardInputManager.keyIsHit(Keys.Enter) || keyboardInputManager.keyIsHit(KeyboardConfiguration.greenFret))
            {
                //gameReference.ChangeGameState(GameStateEnum.SinglePlayer, chartPaths[songSelectionMenu.currentlySelectedEntry - 1]);
                songSelectionMenuSet.SelectNextMenu();
            }

            // Go back to the main menu if escape is hit
            if (keyboardInputManager.keyIsHit(Keys.Escape))
            {
                //gameReference.ChangeGameState(GameStateEnum.MainMenu, null);
                songSelectionMenuSet.SelectPreviousMenu();
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteBlendMode.AlphaBlend);
            songSelectionMenuSet.Draw(spriteBatch, gameFont);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
