using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace MinGH.GameScreenImpl
{
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
