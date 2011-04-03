using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MinGH.Config;
using MinGH.FRBExtensions;

namespace MinGH.EngineExtensions
{
    /// <summary>
    /// Initializes and maintains the two borders on each side of the fretboard.
    /// </summary>
    class GuitarFretboardBorders
    {
        public SteppableSprite[] fretboardBorders { get; set; }

        public Texture2D texture { get; set; }

        public GuitarFretboardBorders(Texture2D texture, GameConfiguration gameConfig)
        {
            //fretboardBorders = new VertexPositionTexture[2][];
            //initalizeFretboardBorders(laneSize, fretboardBorderSize, laneBorderSize, laneDepth);
            fretboardBorders = new SteppableSprite[2];

            for (int i = 0; i < fretboardBorders.GetLength(0); i++)
            {
                fretboardBorders[i] = new SteppableSprite();
                fretboardBorders[i].pixelsPerSpriteSheetStepX = texture.Width;
                fretboardBorders[i].pixelsPerSpriteSheetStepY = texture.Height;
                fretboardBorders[i].spriteSheetStepX = 0;
                fretboardBorders[i].spriteSheetStepY = 0;
                fretboardBorders[i].ScaleX = gameConfig.themeSetting.fretboardBorderSize;
                fretboardBorders[i].ScaleY = gameConfig.themeSetting.fretboardDepth;
                fretboardBorders[i].RotationX = -MathHelper.PiOver2;
            }

            fretboardBorders[0].Position = new Vector3(-gameConfig.themeSetting.fretboardBorderSize, 0f, 0f);
            fretboardBorders[1].Position = new Vector3(gameConfig.themeSetting.laneSizeGuitar * 5 + gameConfig.themeSetting.laneSeparatorSize * 4, 0f, 0f);
        }

        //public void draw(GraphicsDevice device, Matrix viewMatrix, Matrix projectionMatrix)
        //{
        //    foreach (GameObject obj in _fretboardBorders)
        //    {
        //        obj.draw(device, viewMatrix, projectionMatrix);
        //    }
        //}
    }
}
