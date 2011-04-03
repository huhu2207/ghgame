using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameEngine;
using MinGH.Config;

namespace MinGH.EngineExtensions
{
    /// <summary>
    /// Initializes and maintains the two borders on each side of the fretboard.
    /// </summary>
    class DrumFretboardBorder
    {
        GameObject[] _fretboardBorders;

        public Texture2D texture { get; set; }

        public DrumFretboardBorder(Effect effectToUse, Texture2D texture, GraphicsDevice graphics, GameConfiguration gameConfig)
        {
            //fretboardBorders = new VertexPositionTexture[2][];
            //initalizeFretboardBorders(laneSize, fretboardBorderSize, laneBorderSize, laneDepth);
            _fretboardBorders = new GameObject[2];

            for (int i = 0; i < _fretboardBorders.GetLength(0); i++)
            {
                _fretboardBorders[i] = new GameObject(texture, effectToUse, graphics);
                _fretboardBorders[i].pixelsPerSpriteSheetStepX = texture.Width;
                _fretboardBorders[i].pixelsPerSpriteSheetStepY = texture.Height;
                _fretboardBorders[i].spriteSheetStepX = 0;
                _fretboardBorders[i].spriteSheetStepY = 0;
                _fretboardBorders[i].scale3D = new Vector3(gameConfig.themeSetting.fretboardBorderSize, gameConfig.themeSetting.fretboardDepth, 1f);
                _fretboardBorders[i].rotation3D = new Vector3(-MathHelper.PiOver2, 0f, 0f);
            }

            _fretboardBorders[0].position3D = new Vector3(-gameConfig.themeSetting.fretboardBorderSize, 0f, 0f);
            _fretboardBorders[1].position3D = new Vector3(gameConfig.themeSetting.laneSizeDrums * 4 + gameConfig.themeSetting.laneSeparatorSize * 3, 0f, 0f);
        }

        public void draw(GraphicsDevice device, Matrix viewMatrix, Matrix projectionMatrix)
        {
            foreach (GameObject obj in _fretboardBorders)
            {
                obj.draw(device, viewMatrix, projectionMatrix);
            }
        }
    }
}
