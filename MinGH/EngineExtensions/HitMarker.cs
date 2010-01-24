using GameEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MinGH.EngineExtensions
{
    public abstract class HitMarker : GameObject3D
    {
        public HitMarker(Texture2D loadedTex, Rectangle spriteRect, Effect effectToUse, GraphicsDevice device)
            : base(loadedTex, spriteRect, effectToUse, device)
        { }

        public abstract void initalizeHitMarker(int hitMarkerDepth, int hitMarkerSize, int fretboardBorderSize, int laneBorderSize, int laneSize);
    }
}
