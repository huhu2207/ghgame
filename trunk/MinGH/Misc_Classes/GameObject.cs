// The classes and functions for the various game objects used

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MinGH
{
    // This gameobject is geared towards a spritesheet system
    public class GameObject
    {
        //Constructor
        public GameObject(Texture2D loadedTex, Rectangle spritePos)
        {
            rotation = 0;
            position = Vector2.Zero;
            spriteSheet = loadedTex;
            velocity = Vector2.Zero;
            alive = false;
            spriteSheetPosition = spritePos;
            center = new Vector2(spritePos.Width / 2, spritePos.Height / 2);
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
        public Rectangle spriteSheetPosition;
    }
}
