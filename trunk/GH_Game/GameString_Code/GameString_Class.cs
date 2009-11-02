using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MinGH
{
    class GameString
    {
        // Default Constructor
        public GameString()
        {
            value = "";
            position = new Vector2();
            alive = false;
            scale = new Vector2(1.0f, 1.0f);
            color = Color.Black;
        }

        // Typical Constructor (string value is added later)
        public GameString(Vector2 new_pos, Color new_color)
        {
            position = new_pos;
            color = new_color;
            alive = true;
            scale = new Vector2(1.0f, 1.0f);
            value = "default";
        }

        // Draws a GameString onto the screen
        public void Draw (SpriteFont game_font, SpriteBatch spriteBatch)
        {
            Vector2 curr_origin = game_font.MeasureString(this.value) / 2;
            spriteBatch.DrawString(game_font, this.value, this.position, this.color,
                                   0, curr_origin, this.scale, SpriteEffects.None, 0.5f);
        }

        public string value;
        public Vector2 position;
        public Vector2 scale;
        public bool alive;
        public Color color;
    }
}
