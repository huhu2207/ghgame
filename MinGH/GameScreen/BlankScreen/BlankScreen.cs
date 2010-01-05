using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using MinGH.GameStringImpl;

namespace MinGH.GameScreen.BlankScreen
{
    public class BlankScreen : DrawableGameComponent
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;  // Draws the shapes

        GameStringManager strManager = new GameStringManager();
        SpriteFont gameFont;

        public BlankScreen(Game game, GraphicsDeviceManager graph)
            : base(game)
        {
            graphics = graph;
        }

        public override void Initialize()
        {
            strManager = BlankScreenStringInitalizer.initializeStrings(graphics.GraphicsDevice.Viewport.Width,
                                    graphics.GraphicsDevice.Viewport.Height);

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
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend);
            strManager.DrawStrings(spriteBatch, gameFont);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
