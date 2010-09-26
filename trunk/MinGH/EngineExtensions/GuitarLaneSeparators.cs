using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameEngine;

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

        public GuitarLaneSeparators(int laneSize, int laneBorderSize, Effect effectToUse, Texture2D loadedTex,
                                    GraphicsDevice graphics, float laneDepth)
        {
            _laneSeparators = new GameObject[4];
            texture = loadedTex;
            for (int i = 0; i < _laneSeparators.GetLength(0); i++)
            {
                _laneSeparators[i] = new GameObject(texture, effectToUse, graphics);

                _laneSeparators[i].pixelsPerSpriteSheetStepX = loadedTex.Width;
                _laneSeparators[i].pixelsPerSpriteSheetStepY = loadedTex.Height;
                _laneSeparators[i].spriteSheetStepX = 0;
                _laneSeparators[i].spriteSheetStepY = 0;
                //_laneSeparators[i].setHeight(laneDepth);
                //_laneSeparators[i].setWidth(laneBorderSize);
                _laneSeparators[i].position3D = new Vector3(((i + 1) * laneSize) + (i * laneBorderSize), 0, 0.1f);
                //_laneSeparators[i].makeHorizontal();

                float top = -laneDepth;
                float bottom = 0;
                float left = ((i + 1) * laneSize) + (i * laneBorderSize);
                float right = ((i + 1) * laneSize) + (i * laneBorderSize) + laneBorderSize;

                //float desiredTop = 0;
                //float desiredBottom = 1;
                //float desiredLeft = 0;
                //float desiredRight = 1;

                _laneSeparators[i].vertices[0].Position = new Vector3(left, 0.1f, top);
                //_laneSeparators[i].vertices[0].TextureCoordinate = new Vector2(desiredLeft, desiredTop);
                _laneSeparators[i].vertices[0].Position = new Vector3(right, 0.1f, bottom);
                //_laneSeparators[i].vertices[0].TextureCoordinate = new Vector2(desiredRight, desiredBottom);
                _laneSeparators[i].vertices[0].Position = new Vector3(left, 0.1f, bottom);
                //_laneSeparators[i].vertices[0].TextureCoordinate = new Vector2(desiredLeft, desiredBottom);
                _laneSeparators[i].vertices[0].Position = new Vector3(right, 0.1f, bottom);
                //_laneSeparators[i].vertices[0].TextureCoordinate = new Vector2(desiredRight, desiredBottom);
                _laneSeparators[i].vertices[0].Position = new Vector3(left, 0.1f, top);
                //_laneSeparators[i].vertices[0].TextureCoordinate = new Vector2(desiredLeft, desiredTop);
                _laneSeparators[i].vertices[0].Position = new Vector3(right, 0.1f, top);
                //_laneSeparators[i].vertices[0].TextureCoordinate = new Vector2(desiredRight, desiredTop);
            }
            //scale3D = Vector3.One;

        }

        public Vector3 scale3D
        {
            get
            {
                return _laneSeparators[0].scale3D;
            }
            set
            {
                for (int i = 0; i < _laneSeparators.GetLength(0); i++)
                {
                    _laneSeparators[i].scale3D = value;
                    
                    //float newWidth = _laneSeparators[i].pixelsPerSpriteSheetStepX * value.X;
                    //float newHeight = _laneSeparators[i].pixelsPerSpriteSheetStepY * value.Z;

                    //vertices[0].Position = _position3D + new Vector3(0, 0, -newHeight);
                    //vertices[1].Position = _position3D + new Vector3(newWidth, 0, 0);
                    //vertices[2].Position = _position3D;
                    //vertices[3].Position = _position3D + new Vector3(newWidth, 0, 0);
                    //vertices[4].Position = _position3D + new Vector3(0, 0, -newHeight);
                    //vertices[5].Position = _position3D + new Vector3(newWidth, 0, -newHeight);
                }
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

        public void draw(GraphicsDevice device, Matrix viewMatrix, Matrix projectionMatrix)
        {
            foreach (GameObject obj in _laneSeparators)
            {
                obj.draw(device, viewMatrix, projectionMatrix);
            }
            //Matrix worldMatrix = Matrix.Identity;
            //myEffect.CurrentTechnique = myEffect.Techniques["Textured"];
            //myEffect.Parameters["xWorld"].SetValue(worldMatrix);
            //myEffect.Parameters["xView"].SetValue(viewMatrix);
            //myEffect.Parameters["xProjection"].SetValue(projectionMatrix);
            //myEffect.Parameters["xTexture"].SetValue(texture);
            //myEffect.Begin();
            //foreach (EffectPass pass in myEffect.CurrentTechnique.Passes)
            //{
            //    pass.Begin();

            //    device.VertexDeclaration = new VertexDeclaration(device, VertexPositionTexture.VertexElements);
            //    for (int i = 0; i < _laneSeparators.GetLength(0); i++)
            //    {
            //        device.DrawUserPrimitives(PrimitiveType.TriangleList, _laneSeparators[i], 0, 2);
            //    }

            //    pass.End();
            //}

            //myEffect.End();
        }
    }
}
