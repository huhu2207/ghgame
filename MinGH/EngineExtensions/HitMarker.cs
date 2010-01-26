using GameEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MinGH.EngineExtensions
{
    /// <summary>
    /// The common abstract class that defines the hitmarker on the fretboard.
    /// </summary>
    public abstract class HitMarker : GameObject3D
    {
        public HitMarker(Texture2D loadedTex, Rectangle spriteRect, Effect effectToUse, GraphicsDevice device)
            : base(loadedTex, spriteRect, effectToUse, device)
        { }

        /// <summary>
        /// Initalizes the position and appearance of the hit marker according to the
        /// user input.
        /// </summary>
        /// <param name="hitMarkerDepth">The distance in which the marker is positioned.</param>
        /// <param name="hitMarkerSize">The height of the hit marker.</param>
        /// <param name="fretboardBorderSize">The size of the fretboard boarders.</param>
        /// <param name="laneBorderSize">The size of the lane borders.</param>
        /// <param name="laneSize">The size of the lanes themselves.</param>
        public abstract void initalizeHitMarker(int hitMarkerDepth, int hitMarkerSize, int fretboardBorderSize, int laneBorderSize, int laneSize);
    }
}
