using GameEngine.GameStringImpl;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MinGH.Config;
using MinGH.Enum;

namespace MinGH.GameScreen.MainMenu
{
    /// <summary>
    /// The main menu screen for the game.
    /// </summary>
    public class MainMenuScreen : DrawableGameComponent
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        MinGHMain gameReference;  // The refrence to the game itself, used for changing the state.

        Menu mainMenu;
        SpriteFont gameFont;
        IKeyboardInputManager keyboardInputManager = new KeyboardInputManager();

        public MainMenuScreen(MinGHMain game, GraphicsDeviceManager graph)
            : base((Game)game)
        {
            gameReference = game;
            graphics = graph;
        }

        public override void Initialize()
        {
            mainMenu = new Menu("Main Menu", new Vector2(graphics.GraphicsDevice.Viewport.Width / 2f, graphics.GraphicsDevice.Viewport.Height / 4f));
            
            mainMenu.titleScaling = new Vector2(5.0f, 5.0f);
            mainMenu.entryScaling = new Vector2(2.0f, 2.0f);

            mainMenu.AddEntry("Start");
            mainMenu.AddEntry("Options");
            mainMenu.AddEntry("Quit");

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
                mainMenu.SelectNextEntry(graphics.GraphicsDevice.Viewport.Height);
            }
            else if (keyboardInputManager.keyIsHit(Keys.Up) || keyboardInputManager.keyIsHit(KeyboardConfiguration.upStrum))
            {
                mainMenu.SelectPreviousEntry(graphics.GraphicsDevice.Viewport.Height);
            }

            if (keyboardInputManager.keyIsHit(Keys.Enter) || keyboardInputManager.keyIsHit(KeyboardConfiguration.greenFret))
            {
                switch (mainMenu.currentlySelectedEntry)
                {
                    case 1:
                        gameReference.ChangeGameState(GameStateEnum.SongSelection, null);
                        break;
                    case 2:
                        //TODO: Options Menu
                        break;
                    case 3:
                        gameReference.ChangeGameState(GameStateEnum.QuitGame, null);
                        break;
                }
            }

            if (keyboardInputManager.keyIsHit(Keys.Escape))
            {
                gameReference.ChangeGameState(GameStateEnum.QuitGame, null);
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
    }
}
