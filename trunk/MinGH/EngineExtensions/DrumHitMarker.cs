using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MinGH.EngineExtensions
{
    public class DrumHitMarker : HitMarker
    {
        /// <summary>
        /// The verticies of the hitmarker.
        /// </summary>
        private VertexPositionTexture[] hitMarker;

        public DrumHitMarker(int hitMarkerDepth, int hitMarkerSize, int laneSize, int laneBorderSize, int fretboardBorderSize, Effect effectToUse, Texture2D texture, GraphicsDevice graphics)
            : base(texture, new Rectangle(0, 0, texture.Width, texture.Height), effectToUse, graphics)
        {
            hitMarker = new VertexPositionTexture[6];
            initalizeHitMarker(hitMarkerDepth, hitMarkerSize, fretboardBorderSize, laneBorderSize, laneSize);
            myEffect = effectToUse;
        }

        /// <summary>
        /// Initalizes the position and appearance of the hit marker according to the
        /// user input.
        /// </summary>
        /// <param name="hitMarkerDepth">The distance in which the marker is positioned.</param>
        /// <param name="hitMarkerSize">The height of the hit marker.</param>
        /// <param name="fretboardBorderSize">The size of the fretboard boarders.</param>
        /// <param name="laneBorderSize">The size of the lane borders.</param>
        /// <param name="laneSize">The size of the lanes themselves.</param>
        public override void initalizeHitMarker(int hitMarkerDepth, int hitMarkerSize, int fretboardBorderSize, int laneBorderSize, int laneSize)
        {
            float top = -hitMarkerDepth - hitMarkerSize;
            float bottom = -hitMarkerDepth;
            float left = 0 - fretboardBorderSize;
            float right = (4 * laneSize) + (3 * laneBorderSize) + fretboardBorderSize;

            float desiredTop = 0;
            float desiredBottom = 1;
            float desiredLeft = 0;
            float desiredRight = 1;

            hitMarker[0].Position = new Vector3(left, 0.1f, top);
            hitMarker[0].TextureCoordinate = new Vector2(desiredLeft, desiredTop);
            hitMarker[1].Position = new Vector3(right, 0.1f, bottom);
            hitMarker[1].TextureCoordinate = new Vector2(desiredRight, desiredBottom);
            hitMarker[2].Position = new Vector3(left, 0.1f, bottom);
            hitMarker[2].TextureCoordinate = new Vector2(desiredLeft, desiredBottom);
            hitMarker[3].Position = new Vector3(right, 0.1f, bottom);
            hitMarker[3].TextureCoordinate = new Vector2(desiredRight, desiredBottom);
            hitMarker[4].Position = new Vector3(left, 0.1f, top);
            hitMarker[4].TextureCoordinate = new Vector2(desiredLeft, desiredTop);
            hitMarker[5].Position = new Vector3(right, 0.1f, top);
            hitMarker[5].TextureCoordinate = new Vector2(desiredRight, desiredTop);
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
                device.DrawUserPrimitives(PrimitiveType.TriangleList, hitMarker, 0, 2);

                pass.End();
            }

            myEffect.End();
        }
    }
}
