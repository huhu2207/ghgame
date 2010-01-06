using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using MinGH.GameStringImpl;

namespace MinGH.GameScreen.MainMenu
{
    public class MainMenuScreen : DrawableGameComponent
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;  // Draws the shapes

        Menu mainMenu;
        SpriteFont gameFont;

        public MainMenuScreen(Game game, GraphicsDeviceManager graph)
            : base(game)
        {
            graphics = graph;
        }

        public override void Initialize()
        {
            mainMenu = new Menu("Main Menu", graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height);

            mainMenu.titleScaling = new Vector2(5.0f, 5.0f);
            mainMenu.entryScaling = new Vector2(2.0f, 2.0f);

            mainMenu.AddEntry("one");
            mainMenu.AddEntry("two");
            mainMenu.AddEntry("three");
            mainMenu.AddEntry("poop");

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
