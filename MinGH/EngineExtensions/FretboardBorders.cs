using GameEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MinGH.EngineExtensions
{
    public abstract class FretboardBorders : GameObject3D
    {
        public FretboardBorders(Texture2D loadedTex, Rectangle spriteRect, Effect effectToUse, GraphicsDevice device)
            : base(loadedTex, spriteRect, effectToUse, device)
        { }

        public abstract void initalizeFretboardBorders(int laneSize, int fretboardBorderSize, int laneBorderSize);
    }
}
