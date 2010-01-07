using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MinGH.GameStringImpl;

namespace MinGH.GameScreen.MainMenu
{
    public class MainMenuScreen : DrawableGameComponent
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;  // Draws the shapes
        MinGHMain gameReference;

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

            if (keyboardInputManager.keyIsHit(Keys.Down))
            {
                mainMenu.SelectNextEntry();
            }
            else if (keyboardInputManager.keyIsHit(Keys.Up))
            {
                mainMenu.SelectPreviousEntry();
            }

            if (keyboardInputManager.keyIsHit(Keys.Enter))
            {
                switch (mainMenu.currentlySelectedEntry)
                {
                    case 1:
                        gameReference.ChangeGameState(1);
                        break;
                    case 2:
                        //gameReference.ChangeGameState(0);
                        break;
                    case 3:
                        gameReference.ChangeGameState(-1);
                        break;
                }
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
