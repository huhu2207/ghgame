// The classes and functions for the various game objects used

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Chart_View
{
    class gameObject
    {
        //Constructor
        public gameObject(Texture2D loadedTex, Rectangle spritePos)
        {
            rotation = 0;
            position = Vector2.Zero;
            sprite = loadedTex;
            center = new Vector2(sprite.Width / 2, sprite.Height / 2);
            velocity = Vector2.Zero;
            alive = false;
            spriteSheetPosition = spritePos;
        }

        //Public data members
        public Texture2D sprite;
        public Vector2 position;
        public float rotation;
        public Vector2 center;
        public Vector2 velocity;
        public bool alive;
        public Rectangle spriteSheetPosition;
    }
}
