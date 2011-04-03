using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MinGH.Config;
using MinGH.FRBExtensions;

namespace MinGH.EngineExtensions
{
    /// <summary>
    /// Draws the medians between the note lanes (not including the outside lanes).
    /// </summary>
    public class GuitarLaneSeparators
    {
        /// <summary>
        /// All four lane separators
        /// </summary>
        SteppableSprite[] _laneSeparators;

        /// <summary>
        /// The physical texture for the lane separators.
        /// </summary>
        public Texture2D texture { get; set; }

        public GuitarLaneSeparators(GameConfiguration gameConfig, Texture2D loadedTex)
        {
            _laneSeparators = new SteppableSprite[4];
            texture = loadedTex;
            for (int i = 0; i < _laneSeparators.GetLength(0); i++)
            {
                _laneSeparators[i] = new SteppableSprite();

                _laneSeparators[i].pixelsPerSpriteSheetStepX = loadedTex.Width;
                _laneSeparators[i].pixelsPerSpriteSheetStepY = loadedTex.Height;
                _laneSeparators[i].ScaleX = gameConfig.themeSetting.laneSeparatorSize;
                _laneSeparators[i].ScaleY = gameConfig.themeSetting.fretboardDepth;
                _laneSeparators[i].RotationX = -MathHelper.PiOver2;
                _laneSeparators[i].Position = new Vector3(((i + 1) * gameConfig.themeSetting.laneSizeGuitar) + (i * gameConfig.themeSetting.laneSeparatorSize), 0f, 0f);
            }
        }

        //public void draw(GraphicsDevice device, Matrix viewMatrix, Matrix projectionMatrix)
        //{
        //    foreach (GameObject obj in _laneSeparators)
        //    {
        //        obj.draw(device, viewMatrix, projectionMatrix);
        //    }
        //}
    }
}
