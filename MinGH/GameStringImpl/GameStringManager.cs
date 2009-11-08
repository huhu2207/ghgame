using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace MinGH.GameStringImpl
{
	/// <remarks>
	/// A manager class that allows for easy access and management of multiple gamestrings.
	/// </remarks>
    class GameStringManager
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
        public void Set_String(int place, string input)
        {
            strings[place].value = input;
        }

		/// <summary>
		/// A list of gamestrings.
		/// </summary>
        private List<GameString> strings;
    }
}
