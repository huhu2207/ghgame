using GameEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MinGH.EngineExtensions
{
    /// <summary>
    /// Initializes and maintains the textured game object below the notes that represents
    /// the base of the fretboard.
    /// </summary>
    public class FretboardBackground : GameObject
    {
        public FretboardBackground(Texture2D loadedTex, Effect effectToUse, GraphicsDevice device)
            : base(loadedTex, new Rectangle(0, 0, loadedTex.Width, loadedTex.Height), effectToUse, device)
        {
            pixelsPerSpriteSheetStepX = loadedTex.Width;
            pixelsPerSpriteSheetStepY = loadedTex.Height;
            spriteSheetStepX = 0;
            spriteSheetStepY = 0;
            _scale3D = Vector3.One;
        }

        /// <summary>
        /// The position of this fretboard in 3D space.
        /// </summary>
        //public override Vector3 position3D
        //{
        //    get
        //    {
        //        return _position3D;
        //    }

        //    set
        //    {
        //        _position3D = value;
        //        vertices[0].Position = _position3D;
        //        vertices[1].Position = _position3D + new Vector3(spriteSheet.Width, 0, 0) * new Vector3(_scale3D.X, 1, 1);
        //        vertices[2].Position = _position3D + new Vector3(0, 0, spriteSheet.Height) * new Vector3(1, 1, _scale3D.Z);
        //        vertices[3].Position = _position3D + new Vector3(spriteSheet.Width, 0, spriteSheet.Height) * _scale3D;
        //        vertices[4].Position = _position3D + new Vector3(0, 0, spriteSheet.Height) * new Vector3(1, 1, _scale3D.Z);
        //        vertices[5].Position = _position3D + new Vector3(spriteSheet.Width, 0, 0) * new Vector3(_scale3D.X, 1, 1);
        //    }
        //}

        /// <summary>
        /// The scaling value of this fretboard.
        /// </summary>
        public override Vector3 scale3D
        {
            get
            {
                return _scale3D;
            }
            set
            {
                _scale3D = value;
                float newWidth = pixelsPerSpriteSheetStepX * value.X;
                float newHeight = pixelsPerSpriteSheetStepY * value.Z;

                vertices[0].Position = _position3D + new Vector3(0, 0, -newHeight);
                vertices[1].Position = _position3D + new Vector3(newWidth, 0, 0);
                vertices[2].Position = _position3D;
                vertices[3].Position = _position3D + new Vector3(newWidth, 0, 0);
                vertices[4].Position = _position3D + new Vector3(0, 0, -newHeight);
                vertices[5].Position = _position3D + new Vector3(newWidth, 0, -newHeight);
            }
        }
    }
}
