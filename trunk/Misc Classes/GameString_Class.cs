using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Chart_View
{
    class GameString
    {
        // Default Constructor
        public GameString()
        {
            value = "";
            position = new Vector2();
            alive = false;
            color = Color.Black;
        }

        // Typical Constructor (string value is added later)
        public GameString(Vector2 new_pos, Color new_color)
        {
            position = new_pos;
            color = new_color;
            alive = true;
            value = "default";
        }

        public string value;
        public Vector2 position;
        public bool alive;
        public Color color;
    }
}
