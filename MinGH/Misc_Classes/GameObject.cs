// The classes and functions for the various game objects used
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MinGH
{
    // This gameobject is geared towards a spritesheet system
    public class GameObject
    {
        //Constructor
        public GameObject(Texture2D loadedTex, Rectangle spriteRect, int offset)
        {
            rotation = 0;
            position = Vector2.Zero;
            spriteSheet = loadedTex;
            velocity = Vector2.Zero;
            alive = false;
            spriteSheetRectangle = spriteRect;
            center = new Vector2(spriteRect.Width / 2, spriteRect.Height / 2);
            spriteSheetOffset = offset;
        }

        public Vector2 getCenterPosition()
        {
            return new Vector2(position.X + center.X, position.Y + center.Y);
        }

        //Public data members
        public Texture2D spriteSheet;
        public Vector2 position;
        public float rotation;
        public Vector2 center;
        public Vector2 velocity;
        public bool alive;
        public Rectangle spriteSheetRectangle;
        public int spriteSheetOffset;  // Used to easily align sprites in game
    }
}
