using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace MinGH.GameScreenImpl
{
	/// <remarks>
	/// A standard implementation of every gamescreen in the game.  Forces each gamescreen
	/// to implement the standard XNA functions.  Note that LoadContent needs a content
	/// manager passed since only the main game loop can directly access it.
	/// </remarks>
    public abstract class GameScreen
    {
        // Implement all the standard XNA functions
        public abstract void Initialize(GraphicsDeviceManager graphics);
        public abstract void LoadContent(ContentManager content, GraphicsDeviceManager graphics);
        public abstract void UnloadContent();
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(GameTime gameTime);

        public bool isActive;
        public GameScreenEnum gameScreenType;
    }
}
