using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.GameStringImpl
{
	/// <summary>
	/// A generic class that encapsulates the process of drawing strings on an XNA screen
	/// </summary>
    public class GameString
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public GameString()
        {
            value = "";
            position = new Vector2();
            alive = false;
            scale = new Vector2(1.0f, 1.0f);
            color = Color.Black;
        }

        /// <summary>
        /// Constuctor that takes a position and color.  The actual value of the string
        /// is not set here due to cases where one may not know what to put into the
        /// string during initialization (such as game performance info).
        /// </summary>
        /// <param name="newPos">The position this string will be drawn.</param>
        /// <param name="newColor">The color given to the string</param>
        public GameString(Vector2 newPos, Color newColor)
        {
            position = newPos;
            color = newColor;
            alive = true;
            scale = new Vector2(1.0f, 1.0f);
            value = "default";
        }

        /// <summary>
        /// Constuctor that takes a position and color.  The actual value of the string
        /// is set here.
        /// </summary>
        /// <param name="newPos">The position this string will be drawn.</param>
        /// <param name="newColor">The color given to the string.</param>
        /// <param name="newValue">The string value to use.</param>
        public GameString(Vector2 newPos, Color newColor, string newValue)
        {
            position = newPos;
            color = newColor;
            alive = true;
            scale = new Vector2(1.0f, 1.0f);
            value = newValue;
        }

        /// <summary>
        /// Draws the string with the current settings provided a sprite batch and
        /// a sprite font to use.
        /// </summary>
        /// <param name="game_font">
        /// The font this string will draw in.
        /// </param>
        /// <param name="spriteBatch">
        /// The spritebatch used to physically draw the string.
        /// </param>
        public void draw (SpriteFont gameFont, SpriteBatch spriteBatch)
        {
            if (alive)
            {
                Vector2 curr_origin = gameFont.MeasureString(this.value) / 2;
                spriteBatch.DrawString(gameFont, this.value, this.position, this.color,
                                       0, curr_origin, this.scale, SpriteEffects.None, 0.5f);
            }
        }

		/// <summary>
		/// The actual value of the string.
		/// </summary>
        public string value { get; set; }
		
		/// <summary>
		/// The position of the string on screen when drawn.
		/// </summary>
        public Vector2 position { get; set; }
		
		/// <summary>
		/// The scaling size of the string when drawn.
		/// </summary>
        public Vector2 scale { get; set; }
		
		/// <summary>
		/// A simple boolean for the programmer to use if they want the string to only show
		/// during certian circumstances.
		/// </summary>
        public bool alive { get; set; }
		
		/// <summary>
		/// The actual color of the string when drawn.
		/// </summary>
        public Color color { get; set; }
    }
}
