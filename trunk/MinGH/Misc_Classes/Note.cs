using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MinGH
{
    public class Note : GameObject
    {
        public Note(Texture2D loadedTex, Rectangle spritePos, int offset)
            : base(loadedTex, spritePos, offset)
        {
        }

        public const int pointValue = 50;
    }
}
