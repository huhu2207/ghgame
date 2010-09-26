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
        VertexPositionTexture[][] _laneSeparators;

        public GuitarLaneSeparators(int laneSize, int laneBorderSize, Effect effectToUse, Texture2D texture,
                                    GraphicsDevice graphics, float laneDepth)
            : base(texture, new Rectangle(0, 0, texture.Width, texture.Height), effectToUse, graphics)
        {
            _laneSeparators = new VertexPositionTexture[4][];
            myEffect = effectToUse;
            for (int i = 0; i < _laneSeparators.GetLength(0); i++)
            {
                _laneSeparators[i] = new VertexPositionTexture[6];
                float top = -laneDepth;
                float bottom = 0;
                float left = ((i + 1) * laneSize) + (i * laneBorderSize);
                float right = ((i + 1) * laneSize) + (i * laneBorderSize) + laneBorderSize;

                float desiredTop = 0;
                float desiredBottom = 1;
                float desiredLeft = 0;
                float desiredRight = 1;

                _laneSeparators[i][0].Position = new Vector3(left, 0.1f, top);
                _laneSeparators[i][0].TextureCoordinate = new Vector2(desiredLeft, desiredTop);
                _laneSeparators[i][1].Position = new Vector3(right, 0.1f, bottom);
                _laneSeparators[i][1].TextureCoordinate = new Vector2(desiredRight, desiredBottom);
                _laneSeparators[i][2].Position = new Vector3(left, 0.1f, bottom);
                _laneSeparators[i][2].TextureCoordinate = new Vector2(desiredLeft, desiredBottom);
                _laneSeparators[i][3].Position = new Vector3(right, 0.1f, bottom);
                _laneSeparators[i][3].TextureCoordinate = new Vector2(desiredRight, desiredBottom);
                _laneSeparators[i][4].Position = new Vector3(left, 0.1f, top);
                _laneSeparators[i][4].TextureCoordinate = new Vector2(desiredLeft, desiredTop);
                _laneSeparators[i][5].Position = new Vector3(right, 0.1f, top);
                _laneSeparators[i][5].TextureCoordinate = new Vector2(desiredRight, desiredTop);
            }
        }

        //public Vector3 position3D
        //{
        //    get
        //    {
        //        return _position3D;
        //    }

        //    set
        //    {
        //        for (int i = 0; i < _laneSeparators.GetLength(0); i++)
        //        {
        //            float top = -laneDepth;
        //            float bottom = 0;
        //            float left = ((i + 1) * laneSize) + (i * laneBorderSize);
        //            float right = ((i + 1) * laneSize) + (i * laneBorderSize) + laneBorderSize;

        //            float desiredTop = 0;
        //            float desiredBottom = 1;
        //            float desiredLeft = 0;
        //            float desiredRight = 1;

        //            _laneSeparators[i][0].Position = new Vector3(left, 0.1f, top);
        //            _laneSeparators[i][0].TextureCoordinate = new Vector2(desiredLeft, desiredTop);
        //            _laneSeparators[i][1].Position = new Vector3(right, 0.1f, bottom);
        //            _laneSeparators[i][1].TextureCoordinate = new Vector2(desiredRight, desiredBottom);
        //            _laneSeparators[i][2].Position = new Vector3(left, 0.1f, bottom);
        //            _laneSeparators[i][2].TextureCoordinate = new Vector2(desiredLeft, desiredBottom);
        //            _laneSeparators[i][3].Position = new Vector3(right, 0.1f, bottom);
        //            _laneSeparators[i][3].TextureCoordinate = new Vector2(desiredRight, desiredBottom);
        //            _laneSeparators[i][4].Position = new Vector3(left, 0.1f, top);
        //            _laneSeparators[i][4].TextureCoordinate = new Vector2(desiredLeft, desiredTop);
        //            _laneSeparators[i][5].Position = new Vector3(right, 0.1f, top);
        //            _laneSeparators[i][5].TextureCoordinate = new Vector2(desiredRight, desiredTop);
        //        }
        //    }
        //}

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
                for (int i = 0; i < _laneSeparators.GetLength(0); i++)
                {
                    device.DrawUserPrimitives(PrimitiveType.TriangleList, _laneSeparators[i], 0, 2);
                }

                pass.End();
            }

            myEffect.End();
        }
    }
}
