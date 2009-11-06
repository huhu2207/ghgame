using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MinGH
{
    public class Note : GameObject
    {
        public Note(Texture2D loadedTex, Rectangle spritePos)
            : base(loadedTex, spritePos)
        {
        }

        public const int pointValue = 50;
    }
}
