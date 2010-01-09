// The classes and functions for the various game objects used
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MinGH.MiscClasses
{
    /// <remarks>
    /// The wrapper class that allows for easy creation and manipulation of visible
    /// objects on the game screen.  Note this class has functionality for sprite sheets.
    /// </remarks>
    public class GameObject
    {
        /// <summary>
        /// Creats a game object from a texture, rectangle and offset.
        /// </summary>
        /// <param name="loadedTex">
        /// The whole texture or sprite sheet this object will use.
        /// </param>
        /// <param name="spriteRect">
        /// The rectangle in which the first sprite in the sheet is located.
        /// Set this to the size of the texture if sprite sheets are not being used.
        /// </param>
        /// <param name="offset">
        /// The padding on the left side of the spritesheet.  Use this if all the
        /// sprites appear to be pushed to the right slightly.
        /// </param>
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

		/// <summary>
		/// Gets the center location of the sprite at its current location on the screen.
		/// Simply gets the position (top left of the sprite rectangle) and adds the
		/// distance to the center of the sprite rectangle to it.
		/// </summary>
		/// <returns>
		/// The center position of the sprite.
		/// </returns>
        public Vector2 getCenterPosition()
        {
            return new Vector2(position.X + center.X, position.Y + center.Y);
        }

        /// <summary>
        /// The physical texture this object will use.  This can be a sprite or a
        /// spritesheet.
        /// </summary>
        public Texture2D spriteSheet;
		
		/// <summary>
		/// The position this game object is currently located.  This position is where
		/// the top left of the sprite is to be drawn.
		/// </summary>
        public Vector2 position;
		
		/// <summary>
		/// The rotation the sprite currently has.
		/// </summary>
        public float rotation;
		
		/// <summary>
		/// The center point of the sprite texture used.  This is really calculated using
		/// the provided sprite sheet rectangle.
		/// </summary>
        public Vector2 center;
		
		/// <summary>
		/// The speed and direction in which the sprite is to be currently moving
		/// </summary>
        public Vector2 velocity;
		
		/// <summary>
		/// A boolean value that dictates wether this sprite is to be drawn or not.
		/// </summary>
        public bool alive;
		
		/// <summary>
		/// The rectangle that encompasses the desire section of the sprite sheet that
		/// this object will currently use.  If it is set to the size of the sprite sheet
		/// this will operate as a normal texture.
		/// </summary>
        public Rectangle spriteSheetRectangle;
		
		/// <summary>
		/// Informs the program that there is a padding on the left side of the sprite sheet.
		/// Use this if all the sprites appear to be slightly moved to the right.
		/// </summary>
        public int spriteSheetOffset;
    }
}
