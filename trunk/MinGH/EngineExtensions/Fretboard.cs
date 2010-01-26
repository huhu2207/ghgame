using GameEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MinGH.EngineExtensions
{
    /// <summary>
    /// Initializes and maintains the textured game object below the notes that represents
    /// the base of the fretboard.
    /// </summary>
    public class Fretboard : GameObject3D
    {
        public Fretboard(Texture2D loadedTex, Effect effectToUse, GraphicsDevice device)
            : base(loadedTex, new Rectangle(0, 0, loadedTex.Width, loadedTex.Height), effectToUse, device)
        {
            initalizeTextureCoords();
            _scale3D = Vector3.One;
            spriteSheetStep = 100;
        }

        /// <summary>
        /// Sets up the texture coordniates to completly fill the texture.
        /// </summary>
        public void initalizeTextureCoords()
        {
            float desiredTop = 0;
            float desiredBottom = 1;
            float desiredLeft = 0;
            float desiredRight = 1;

            vertices[0].TextureCoordinate.X = desiredLeft;
            vertices[0].TextureCoordinate.Y = desiredTop;

            vertices[1].TextureCoordinate.X = desiredRight;
            vertices[1].TextureCoordinate.Y = desiredBottom;

            vertices[2].TextureCoordinate.X = desiredLeft;
            vertices[2].TextureCoordinate.Y = desiredBottom;

            vertices[3].TextureCoordinate.X = desiredRight;
            vertices[3].TextureCoordinate.Y = desiredBottom;

            vertices[4].TextureCoordinate.X = desiredLeft;
            vertices[4].TextureCoordinate.Y = desiredTop;

            vertices[5].TextureCoordinate.X = desiredRight;
            vertices[5].TextureCoordinate.Y = desiredTop;
        }

        /// <summary>
        /// The position of this fretboard in 3D space.
        /// </summary>
        public override Vector3 position3D
        {
            get
            {
                return _position3D;
            }

            set
            {
                _position3D = value;
                vertices[0].Position = _position3D;
                vertices[1].Position = _position3D + new Vector3(spriteSheet.Width, 0, 0) * new Vector3(_scale3D.X, 1, 1);
                vertices[2].Position = _position3D + new Vector3(0, 0, spriteSheet.Height) * new Vector3(1, 1, _scale3D.Z);
                vertices[3].Position = _position3D + new Vector3(spriteSheet.Width, 0, spriteSheet.Height) * _scale3D;
                vertices[4].Position = _position3D + new Vector3(0, 0, spriteSheet.Height) * new Vector3(1, 1, _scale3D.Z);
                vertices[5].Position = _position3D + new Vector3(spriteSheet.Width, 0, 0) * new Vector3(_scale3D.X, 1, 1);
            }
        }

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
                vertices[0].Position = _position3D * new Vector3(1, 1, value.Z);
                vertices[1].Position = _position3D * new Vector3(value.X, 1, 1);
                vertices[2].Position = _position3D;
                vertices[3].Position = _position3D * new Vector3(value.X, 1, 1);
                vertices[4].Position = _position3D * new Vector3(1, 1, value.Z);
                vertices[5].Position = _position3D * value;
            }
        }
    }
}
