﻿using System.Collections.Generic;
using GameEngine.GameStringImpl;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MinGH.Config;
using MinGH.Enum;

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
        List<ChartSelection> chartPaths;
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

            // This list of ChartSelections will only have the path and directory properties
            // filled out.  You need to expilicitly define the remaining properties.
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

            Menu difficultySelectionMenu = new Menu("Difficulty Selection", new Vector2(graphics.GraphicsDevice.Viewport.Width / 2f, graphics.GraphicsDevice.Viewport.Height / 4f));
            difficultySelectionMenu.titleScaling = new Vector2(5.0f, 5.0f);
            difficultySelectionMenu.entryScaling = new Vector2(2.0f, 2.0f);
            difficultySelectionMenu.scrollable = true;
            difficultySelectionMenu.AddEntry("Expert");
            difficultySelectionMenu.AddEntry("Hard");
            difficultySelectionMenu.AddEntry("Medium");
            difficultySelectionMenu.AddEntry("Easy");
            songSelectionMenuSet.AddMenu(difficultySelectionMenu);

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
                if (songSelectionMenuSet.currentlySelectedMenu == songSelectionMenuSet.menus.Count - 1)
                {
                    ChartSelection chartToUse = new ChartSelection(chartPaths[songSelectionMenuSet.menus[0].currentlySelectedEntry - 1]);

                    switch (songSelectionMenuSet.menus[1].currentlySelectedEntry)
                    {
                        case 1:
                            chartToUse.instrument = "Single";
                            break;
                        case 2:
                            chartToUse.instrument = "DoubleGuitar";
                            break;
                        case 3:
                            chartToUse.instrument = "DoubleBass";
                            break;
                        case 4:
                            chartToUse.instrument = "Drums";
                            break;
                        default:
                            chartToUse.instrument = "Single";
                            break;
                    }

                    switch (songSelectionMenuSet.menus[2].currentlySelectedEntry)
                    {
                        case 1:
                            chartToUse.difficulty = "Expert";
                            break;
                        case 2:
                            chartToUse.difficulty = "Hard";
                            break;
                        case 3:
                            chartToUse.difficulty = "Medium";
                            break;
                        case 4:
                            chartToUse.difficulty = "Easy";
                            break;
                        default:
                            chartToUse.difficulty = "Expert";
                            break;
                    }

                    gameReference.ChangeGameState(GameStateEnum.SinglePlayer, chartToUse);
                }
                else
                {
                    songSelectionMenuSet.SelectNextMenu();
                }
            }

            // Go back to the main menu if escape is hit
            if (keyboardInputManager.keyIsHit(Keys.Escape))
            {
                if (songSelectionMenuSet.currentlySelectedMenu == 0)
                {
                    gameReference.ChangeGameState(GameStateEnum.MainMenu, null);
                }
                else
                {
                    songSelectionMenuSet.SelectPreviousMenu();
                }
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
