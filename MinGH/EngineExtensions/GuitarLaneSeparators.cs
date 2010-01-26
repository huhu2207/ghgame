using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MinGH.EngineExtensions
{
    /// <summary>
    /// Draws the medians between the note lanes (not including the outside lanes).
    /// </summary>
    public class GuitarLaneSeparators : LaneSeparators
    {
        /// <summary>
        /// The verticies of all 4 lane separators.
        /// </summary>
        VertexPositionTexture[][] laneSeparators;

        public GuitarLaneSeparators(int laneSize, int laneBorderSize, Effect effectToUse, Texture2D texture, GraphicsDevice graphics)
            : base(texture, new Rectangle(0, 0, texture.Width, texture.Height), effectToUse, graphics)
        {
            laneSeparators = new VertexPositionTexture[4][];
            initalizeLaneSeparators(laneSize, laneBorderSize);
            myEffect = effectToUse;
        }

        /// <summary>
        /// Initializes the position and texture verticies
        /// </summary>
        /// <param name="laneSize">The size of the fretboard lanes.</param>
        /// <param name="laneBorderSize">The size of the fretboard medians.</param>
        public override void initalizeLaneSeparators(int laneSize, int laneBorderSize)
        {
            for (int i = 0; i < laneSeparators.GetLength(0); i++)
            {
                laneSeparators[i] = new VertexPositionTexture[6];
                float top = -1000;
                float bottom = 0;
                float left = ((i + 1) * laneSize) + (i * laneBorderSize);
                float right = ((i + 1) * laneSize) + (i * laneBorderSize) + laneBorderSize;

                float desiredTop = 0;
                float desiredBottom = 1;
                float desiredLeft = 0;
                float desiredRight = 1;

                laneSeparators[i][0].Position = new Vector3(left, 0.1f, top);
                laneSeparators[i][0].TextureCoordinate = new Vector2(desiredLeft, desiredTop);
                laneSeparators[i][1].Position = new Vector3(right, 0.1f, bottom);
                laneSeparators[i][1].TextureCoordinate = new Vector2(desiredRight, desiredBottom);
                laneSeparators[i][2].Position = new Vector3(left, 0.1f, bottom);
                laneSeparators[i][2].TextureCoordinate = new Vector2(desiredLeft, desiredBottom);
                laneSeparators[i][3].Position = new Vector3(right, 0.1f, bottom);
                laneSeparators[i][3].TextureCoordinate = new Vector2(desiredRight, desiredBottom);
                laneSeparators[i][4].Position = new Vector3(left, 0.1f, top);
                laneSeparators[i][4].TextureCoordinate = new Vector2(desiredLeft, desiredTop);
                laneSeparators[i][5].Position = new Vector3(right, 0.1f, top);
                laneSeparators[i][5].TextureCoordinate = new Vector2(desiredRight, desiredTop);
            }
        }

        public override void draw(GraphicsDevice device, Matrix viewMatrix, Matrix projectionMatrix)
        {
            Matrix worldMatrix = Matrix.Identity;
            myEffect.CurrentTechnique = myEffect.Techniques["Textured"];
            myEffect.Parameters["xWorld"].SetValue(worldMatrix);
            myEffect.Parameters["xView"].SetValue(viewMatrix);
            myEffect.Parameters["xProjection"].SetValue(projectionMatrix);
            myEffect.Parameters["xTexture"].SetValue(spriteSheet);
            myEffect.Begin();
            foreach (EffectPass pass in myEffect.CurrentTechnique.Passes)
            {
                pass.Begin();

                device.VertexDeclaration = new VertexDeclaration(device, VertexPositionTexture.VertexElements);
                for (int i = 0; i < laneSeparators.GetLength(0); i++)
                {
                    device.DrawUserPrimitives(PrimitiveType.TriangleList, laneSeparators[i], 0, 2);
                }

                pass.End();
            }

            myEffect.End();
        }
    }
}
