﻿using System.Collections.Generic;
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

        Menu songSelectionMenu;
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
            gameConfiguration = new GameConfiguration("./config.xml");
            chartPaths = ChartFinder.GenerateAllChartPaths(gameConfiguration.songDirectory);

            songSelectionMenu = new Menu("Song Selection", new Vector2(graphics.GraphicsDevice.Viewport.Width / 2f, graphics.GraphicsDevice.Viewport.Height / 4f));
            
            songSelectionMenu.titleScaling = new Vector2(5.0f, 5.0f);
            songSelectionMenu.entryScaling = new Vector2(2.0f, 2.0f);
            songSelectionMenu.scrollable = true;

            for (int i = 0; i < chartPaths.Count; i++)
            {
                songSelectionMenu.AddEntry(chartPaths[i].directory);
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

            // Menu navagation logic
            if (keyboardInputManager.keyIsHit(Keys.Down) || keyboardInputManager.keyIsHit(KeyboardConfiguration.downStrum))
            {
                songSelectionMenu.SelectNextEntry(graphics.GraphicsDevice.Viewport.Height);
            }
            else if (keyboardInputManager.keyIsHit(Keys.Up) || keyboardInputManager.keyIsHit(KeyboardConfiguration.upStrum))
            {
                songSelectionMenu.SelectPreviousEntry(graphics.GraphicsDevice.Viewport.Height);
            }
            if (keyboardInputManager.keyIsHit(Keys.Enter) || keyboardInputManager.keyIsHit(KeyboardConfiguration.green))
            {
                gameReference.ChangeGameState(GameStateEnum.SinglePlayer, chartPaths[songSelectionMenu.currentlySelectedEntry - 1]);
            }

            // Go back to the main menu if escape is hit
            if (keyboardInputManager.keyIsHit(Keys.Escape))
            {
                gameReference.ChangeGameState(GameStateEnum.MainMenu, null);
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteBlendMode.AlphaBlend);
            songSelectionMenu.Draw(spriteBatch, gameFont);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}