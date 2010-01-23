using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MinGH.EngineExtensions
{
    class GuitarFretboardBorders : FretboardBorders
    {
        /// <summary>
        /// The verticies of the two fretboard borders.
        /// </summary>
        VertexPositionTexture[][] fretboardBorders;

        public GuitarFretboardBorders(int laneSize, int laneBorderSize, Effect effectToUse, Texture2D texture, GraphicsDevice graphics, int fretboardBorderSize)
            : base(texture, new Rectangle(0, 0, texture.Width, texture.Height), effectToUse, graphics)
        {
            fretboardBorders = new VertexPositionTexture[2][];
            initalizeFretboardBorders(laneSize, fretboardBorderSize, laneBorderSize);
            myEffect = effectToUse;
        }

        /// <summary>
        /// Initializes the position and texture verticies
        /// </summary>
        /// <param name="laneSize">The size of the fretboard lanes.</param>
        /// <param name="laneBorderSize">The size of the fretboard medians.</param>
        public override void initalizeFretboardBorders(int laneSize, int fretboardBorderSize, int laneBorderSize)
        {
            for (int i = 0; i < fretboardBorders.GetLength(0); i++)
            {
                fretboardBorders[i] = new VertexPositionTexture[6];
                float top = -1000;
                float bottom = 0;
                float left = 0;
                float right = 0;

                if (i == 0)
                {
                    left = 0 - fretboardBorderSize;
                    right = 0;
                }
                else if (i == 1)
                {
                    left = (5 * laneSize) + (4 * laneBorderSize);
                    right = (5 * laneSize) + (4 * laneBorderSize) + fretboardBorderSize;
                }

                float desiredTop = 0;
                float desiredBottom = 1;
                float desiredLeft = 0;
                float desiredRight = 1;

                fretboardBorders[i][0].Position = new Vector3(left, 0.1f, top);
                fretboardBorders[i][0].TextureCoordinate = new Vector2(desiredLeft, desiredTop);
                fretboardBorders[i][1].Position = new Vector3(right, 0.1f, bottom);
                fretboardBorders[i][1].TextureCoordinate = new Vector2(desiredRight, desiredBottom);
                fretboardBorders[i][2].Position = new Vector3(left, 0.1f, bottom);
                fretboardBorders[i][2].TextureCoordinate = new Vector2(desiredLeft, desiredBottom);
                fretboardBorders[i][3].Position = new Vector3(right, 0.1f, bottom);
                fretboardBorders[i][3].TextureCoordinate = new Vector2(desiredRight, desiredBottom);
                fretboardBorders[i][4].Position = new Vector3(left, 0.1f, top);
                fretboardBorders[i][4].TextureCoordinate = new Vector2(desiredLeft, desiredTop);
                fretboardBorders[i][5].Position = new Vector3(right, 0.1f, top);
                fretboardBorders[i][5].TextureCoordinate = new Vector2(desiredRight, desiredTop);
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
                for (int i = 0; i < fretboardBorders.GetLength(0); i++)
                {
                    device.DrawUserPrimitives(PrimitiveType.TriangleList, fretboardBorders[i], 0, 2);
                }

                pass.End();
            }

            myEffect.End();
        }
    }
}
