using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace MinGH.GameScreenImpl
{
	/// <remarks>
	/// A standard implementation of every gamescreen in the game.  Forces each gamescreen
	/// to implement the standard XNA functions.  Note that LoadContent needs a content
	/// manager passed since only the main game loop can directly access it.
	/// </remarks>
    public interface IGameScreen
    {
        // Implement all the standard XNA functions
        void Initialize(GraphicsDeviceManager graphics);
        void LoadContent(ContentManager content, GraphicsDeviceManager graphics);
        void UnloadContent();
        void Update(GameTime gameTime);
        void Draw(GameTime gameTime);
    }
}
