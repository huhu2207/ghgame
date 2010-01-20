using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.GameStringImpl
{
	/// <summary>
	/// A manager class that allows for easy access and management of multiple gamestrings.
	/// </summary>
    public class GameStringManager
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public GameStringManager()
        {
            strings = new List<GameString>();
        }

        /// <summary>
        /// Draws every string the string list has that is currently alive.
        /// </summary>
        /// <param name="spriteBatch">
        /// The sprite batch used to draw the strings.
        /// </param>
        /// <param name="game_font">
        /// The font each string will use.
        /// </param>
        public void DrawStrings(SpriteBatch spriteBatch, SpriteFont game_font)
        {
            foreach (GameString input in strings)
            {
				if (input.alive == true)
				{
					input.Draw(game_font, spriteBatch);
				}
            }
        }

        /// <summary>
        /// Adds a new string to the manager's list.
        /// </summary>
        /// <param name="input">
        /// The new string to add to the list.
        /// </param>
        public void Add(GameString input)
        {
            strings.Add(input);
        }

        /// <summary>
        /// Accesses the string list and sets it a new value.
        /// </summary>
        /// <param name="place">
        /// The index where the string to be replaced is located.  As of now, the
        /// programmer must keep track of which strings are for what.
        /// </param>
        /// <param name="input">
        /// The new value for the desired string.
        /// </param>
        public void SetString(int place, string input)
        {
            strings[place].value = input;
        }

        /// <summary>
        /// Sets the color of a specified string.
        /// </summary>
        /// <param name="place">The place of the string to change.</param>
        /// <param name="inputColor">The new color of the string.</param>
        public void SetColor(int place, Color inputColor)
        {
            strings[place].color = inputColor;
        }

        /// <summary>
        /// Sets the position of a specified string.
        /// </summary>
        /// <param name="place">The place of the string to change.</param>
        /// <param name="newPosition">The new position of the string.</param>
        public void SetStringPosition(int place, Vector2 newPosition)
        {
            strings[place].position = newPosition;
        }

        /// <summary>
        /// Gets the number of different strings within the string manager.
        /// </summary>
        /// <returns>Number of strings.</returns>
        public int GetNumberOfStrings()
        {
            return strings.Count;
        }

		/// <summary>
		/// A list of gamestrings.
		/// </summary>
        public List<GameString> strings { get; set; }
    }
}
