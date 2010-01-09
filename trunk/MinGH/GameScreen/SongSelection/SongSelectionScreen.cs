using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MinGH.Enum;
using MinGH.GameStringImpl;
using MinGH.Misc_Classes;
using System.IO;
using System.Collections.Generic;

namespace MinGH.GameScreen.MainMenu
{
    public class SongSelectionScreen : DrawableGameComponent
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;  // Draws the shapes
        MinGHMain gameReference;

        Menu mainMenu;
        SpriteFont gameFont;
        IKeyboardInputManager keyboardInputManager = new KeyboardInputManager();

        public SongSelectionScreen(MinGHMain game, GraphicsDeviceManager graph)
            : base((Game)game)
        {
            gameReference = game;
            graphics = graph;
        }

        public override void Initialize()
        {
            List<string> chartPaths = GenerateAllChartPaths("./Songs");

            mainMenu = new Menu("Song Selection", new Vector2(graphics.GraphicsDevice.Viewport.Width / 2f, graphics.GraphicsDevice.Viewport.Height / 4f));
            
            mainMenu.titleScaling = new Vector2(5.0f, 5.0f);
            mainMenu.entryScaling = new Vector2(2.0f, 2.0f);

            foreach (string currChart in chartPaths)
            {
                mainMenu.AddEntry(currChart);
            }

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

            if (keyboardInputManager.keyIsHit(Keys.Down) || keyboardInputManager.keyIsHit(KeyboardConfiguration.downStrum))
            {
                mainMenu.SelectNextEntry();
            }
            else if (keyboardInputManager.keyIsHit(Keys.Up) || keyboardInputManager.keyIsHit(KeyboardConfiguration.upStrum))
            {
                mainMenu.SelectPreviousEntry();
            }

            if (keyboardInputManager.keyIsHit(Keys.Enter) || keyboardInputManager.keyIsHit(KeyboardConfiguration.green))
            {
                //switch (mainMenu.currentlySelectedEntry)
                //{
                    
                //}
            }

            if (keyboardInputManager.keyIsHit(Keys.Escape))
            {
                gameReference.ChangeGameState(GameStateEnum.MainMenu);
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteBlendMode.AlphaBlend);
            mainMenu.Draw(spriteBatch, gameFont);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private List<string> GenerateAllChartPaths(string songDirectory)
        {
            List<string> listOfCharts = new List<string>();

            try
            {
                SearchDirectoryForCharts(songDirectory, listOfCharts);
            }
            catch (IOException e)
            {
                e.ToString();
                //TODO: Add some logging for exceptions.
            }

            return listOfCharts;
        }

        private List<string> SearchDirectoryForCharts(string directory, List<string> listOfCharts)
        {
            foreach (string currDirectory in Directory.GetDirectories(directory))
            {
                foreach (string currFile in Directory.GetFiles(currDirectory, "*.chart"))
                {
                    listOfCharts.Add(currFile);
                }
                SearchDirectoryForCharts(currDirectory, listOfCharts);
            }
            return listOfCharts;
        }
    }
}
