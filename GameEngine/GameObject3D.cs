using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine
{
    /// <summary>
    /// A game object to be drawn in a 3D space.
    /// </summary>
    public class GameObject3D
    {
        public VertexDeclaration texturedVertexDeclaration;
        public VertexPositionTexture[] vertices;
        public Effect myEffect;

        /// <summary>
        /// The position this game object is currently located.  This position is where
        /// the top left of the sprite is to be drawn.
        /// </summary>
        public Vector3 position3D;

        /// <summary>
        /// The rectangle that encompasses the desire section of the sprite sheet that
        /// this object will currently use.  If it is set to the size of the sprite sheet
        /// this will operate as a normal texture.
        /// </summary>
        public Rectangle spriteSheetRectangle;

        /// <summary>
        /// The physical texture this object will use.  This can be a sprite or a
        /// spritesheet.
        /// </summary>
        public Texture2D spriteSheet { get; set; }

        /// <summary>
        /// The center point of the sprite texture used.  This is really calculated using
        /// the provided sprite sheet rectangle.
        /// </summary>
        public Vector3 center3D;

        /// <summary>
        /// The speed and direction in which the sprite is to be currently moving
        /// </summary>
        public Vector3 velocity3D;

        /// <summary>
        /// The current scaling for this GameObject.
        /// </summary>
        public Vector3 scale3D;

        /// <summary>
        /// A boolean value that dictates wether this sprite is to be drawn or not.
        /// </summary>
        public bool alive { get; set; }

        public void updateVerticies(float step)
        {
            for (int i = 0; i < vertices.GetLength(0); i++)
            {
                vertices[i].Position.Z += step;
            }
        }

        public void draw(GraphicsDevice device, Matrix viewMatrix, Matrix projectionMatrix)
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

                device.VertexDeclaration = texturedVertexDeclaration;
                device.DrawUserPrimitives(PrimitiveType.TriangleList, vertices, 0, 2);

                pass.End();
            }
            myEffect.End();
        }

        public GameObject3D(Texture2D loadedTex, Rectangle spriteRect, Effect effectToUse, GraphicsDevice device)
        {
            vertices = new VertexPositionTexture[6];
            spriteSheet = loadedTex;
            spriteSheetRectangle = spriteRect;
            myEffect = effectToUse;
            position3D = Vector3.Zero;
            center3D = Vector3.Zero;
            velocity3D = Vector3.Zero;
            scale3D = Vector3.One;
            texturedVertexDeclaration = new VertexDeclaration(device, VertexPositionTexture.VertexElements);
        }

        public Vector3 getCenterPosition()
        {
            return new Vector3(position3D.X + (center3D.X * scale3D.X),
                               position3D.Y + (center3D.Y * scale3D.Y),
                               position3D.Z + (center3D.Z * scale3D.Z));
        }
    }
}
