using GameEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MinGH.EngineExtensions
{
    /// <summary>
    /// Initializes and maintains the textured game object below the notes that represents
    /// the base of the fretboard.
    /// </summary>
    public class FretboardBackground : GameObject
    {
        public FretboardBackground(Texture2D loadedTex, Effect effectToUse, GraphicsDevice device, float scale)
            : base(loadedTex, effectToUse, device)
        {
            pixelsPerSpriteSheetStepX = loadedTex.Width;
            pixelsPerSpriteSheetStepY = loadedTex.Height;
            spriteSheetStepX = 0;
            spriteSheetStepY = 0;
        }
    }
}
