using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using MinGH.GameStringImpl;

namespace MinGH.GameScreenImpl.BlankScreen
{
    public class BlankScreen : GameScreen
    {
        SpriteBatch spriteBatch;  // Draws the shapes

        GameStringManager strManager = new GameStringManager();
        SpriteFont gameFont;

        public override void Initialize(GraphicsDeviceManager graphics)
        {
            gameScreenType = GameScreenEnum.Blank;
            isActive = false;

            BlankScreenStringInitalizer.initializeStrings(ref strManager, graphics.GraphicsDevice.Viewport.Width,
                               graphics.GraphicsDevice.Viewport.Height);

            spriteBatch = new SpriteBatch(graphics.GraphicsDevice);
        }

        public override void LoadContent(ContentManager content, GraphicsDeviceManager graphics)
        {
            gameFont = content.Load<SpriteFont>("Arial");  // Load the font
        }

        public override void UnloadContent()
        {
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend);
            strManager.DrawStrings(spriteBatch, gameFont);
            spriteBatch.End();
        }
    }
}
