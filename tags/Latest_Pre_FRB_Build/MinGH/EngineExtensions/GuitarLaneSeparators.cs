using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameEngine;
using MinGH.Config;

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
        GameObject[] _laneSeparators;

        /// <summary>
        /// The physical texture for the lane separators.
        /// </summary>
        public Texture2D texture { get; set; }

        public GuitarLaneSeparators(GameConfiguration gameConfig, Effect effectToUse, Texture2D loadedTex, GraphicsDevice graphics)
        {
            _laneSeparators = new GameObject[4];
            texture = loadedTex;
            for (int i = 0; i < _laneSeparators.GetLength(0); i++)
            {
                _laneSeparators[i] = new GameObject(texture, effectToUse, graphics);

                _laneSeparators[i].pixelsPerSpriteSheetStepX = loadedTex.Width;
                _laneSeparators[i].pixelsPerSpriteSheetStepY = loadedTex.Height;
                _laneSeparators[i].scale3D = new Vector3(gameConfig.themeSetting.laneSeparatorSize, gameConfig.themeSetting.fretboardDepth, 1f);
                _laneSeparators[i].rotation3D = new Vector3(-MathHelper.PiOver2, 0f, 0f);
                _laneSeparators[i].position3D = new Vector3(((i + 1) * gameConfig.themeSetting.laneSizeGuitar) + (i * gameConfig.themeSetting.laneSeparatorSize), 0f, 0f);
            }
        }

        public void draw(GraphicsDevice device, Matrix viewMatrix, Matrix projectionMatrix)
        {
            foreach (GameObject obj in _laneSeparators)
            {
                obj.draw(device, viewMatrix, projectionMatrix);
            }
        }
    }
}
