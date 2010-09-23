using GameEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MinGH.EngineExtensions
{
    /// <summary>
    /// The common abstract class that defines the lane separators on the fretboard.
    /// </summary>
    public abstract class LaneSeparators : GameObject3D
    {
        public LaneSeparators(Texture2D loadedTex, Rectangle spriteRect, Effect effectToUse, GraphicsDevice device)
            : base(loadedTex, spriteRect, effectToUse, device)
        { }

        public abstract void initalizeLaneSeparators(int laneSize, int laneBorderSize, float depth);
    }
}
