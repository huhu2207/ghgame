using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace Chart_View
{
    class GameStringManager
    {
        // Default Constructor
        public GameStringManager()
        {
            strings = new List<GameString>();
        }

        // Draws every gamestring contained within the list
        public void DrawStrings(SpriteBatch spriteBatch, SpriteFont game_font)
        {
            foreach (GameString input in strings)
            {
                input.Draw(game_font, spriteBatch);
            }
        }

        // Adds a gamestring into the list
        public void Add(GameString input)
        {
            strings.Add(input);
        }

        // Sets a specific string's value according to the input
        public void Set_String(int place, string input)
        {
            strings[place].value = input;
        }

        private List<GameString> strings;
    }
}
