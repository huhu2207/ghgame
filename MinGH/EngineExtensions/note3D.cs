using GameEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MinGH.EngineExtensions
{
	/// <summary>
	/// An extended class of gameObject3D that contains data and functionality 
	/// specific to the drawable notes within the MinGH game.
	/// </summary>
    public class Note3D : GameObject3D
    {
		/// <summary>
		/// The same constructor overloaded from the gameObject class.  The parameter
		/// descriptions are copied straight from there.
		/// </summary>
		/// <param name="loadedTex">
		/// The whole texture or sprite sheet this object will use.
		/// </param>
		/// <param name="spritePos">
		/// The rectangle in which the first sprite in the sheet is located.
        /// Set this to the size of the texture if sprite sheets are not being used.
		/// </param>
        /// <param name="effectToUse">
        /// The predefined shader effect to use on this note.
        /// </param>
        /// <param name="device">A graphics device.</param>
        public Note3D(Texture2D loadedTex, Rectangle spritePos, Effect effectToUse, GraphicsDevice device)
            : base(loadedTex, spritePos, effectToUse, device)
        {
            noteChartIndex = 0;
            precedsHOPO = false;
            isUnhittable = false;
            isChord = false;
            rootNote = new Point();
            wasTicked = false;
            originalSpritePosition = new Rectangle();
            spriteSheetStep = 100;
            _scale3D = Vector3.One;
        }

        /// <summary>
        /// Resets the note specific information for future use.
        /// </summary>
        public void ResetNote()
        {
            spriteSheetRectangle = originalSpritePosition;
            noteChartIndex = 0;
            precedsHOPO = false;
            isUnhittable = false;
            isChord = false;
            rootNote = new Point(-1 , -1);
            wasTicked = false;
        }

        /// <summary>
        /// Initializes the texture coordinates of this note to the sprite sheet
        /// according to the specified section.
        /// </summary>
        /// <param name="XSheetValue">
        /// The number of steps right until the desired section on the sprite sheet is met.
        /// </param>
        /// <param name="YSheetValue">
        /// The number of steps down until the desired section on the sprite sheet is met.
        /// </param>
        /// <param name="sheetStep">
        /// The number of pixels each sprite sheet step is.
        /// </param>
        public void initalizeTextureCoords(int XSheetValue, int YSheetValue, int sheetStep)
        {
            float desiredTop = YSheetValue / (float)spriteSheet.Height;
            float desiredBottom = (YSheetValue + sheetStep) / (float)spriteSheet.Height;
            float desiredLeft = XSheetValue / (float)spriteSheet.Width;
            float desiredRight = (XSheetValue + sheetStep) / (float)spriteSheet.Width;

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
        /// The position of this note in 3D space.
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
                vertices[0].Position = _position3D + new Vector3(0, spriteSheetStep, 0) * new Vector3(1, _scale3D.Y, 1);
                vertices[1].Position = _position3D + new Vector3(spriteSheetStep, 0, 0) * new Vector3(_scale3D.X, 1, 1);
                vertices[2].Position = _position3D;
                vertices[3].Position = _position3D + new Vector3(spriteSheetStep, 0, 0) * new Vector3(_scale3D.X, 1, 1);
                vertices[4].Position = _position3D + new Vector3(0, spriteSheetStep, 0) * new Vector3(1, _scale3D.Y, 1);
                vertices[5].Position = _position3D + new Vector3(spriteSheetStep, spriteSheetStep, 0) * _scale3D;
            }
        }

        /// <summary>
        /// The scaling value of this note.
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
                vertices[0].Position = _position3D * new Vector3(1, value.Y, 1);
                vertices[1].Position = _position3D * new Vector3(value.X, 1, 1);
                vertices[2].Position = _position3D;
                vertices[3].Position = _position3D * new Vector3(value.X, 1, 1);
                vertices[4].Position = _position3D * new Vector3(1, value.Y, 1);
                vertices[5].Position = _position3D * value;
            }
        }

        /// <summary>
        /// The rectangle surrounding this note's section of the sprite sheet.
        /// </summary>
        public override Rectangle spriteSheetRectangle
        {
            get
            {
                return _spriteSheetRectangle;
            }
            set
            {
                _spriteSheetRectangle = value;
                initalizeTextureCoords(value.X, value.Y, spriteSheetStep);
            }
        }

		/// <summary>
		/// The base point value of a note (this value may be in the wrong place,
		/// but I'll keep it there for now).
		/// </summary>
        public const int pointValue = 50;

        /// <summary>
        /// Where the note is located on the currently playing notechart.
        /// </summary>
        public int noteChartIndex { get; set; }

        /// <summary>
        /// If the note is followed by a hammeron/pulloff.
        /// </summary>
        public bool precedsHOPO { get; set; }

        /// <summary>
        /// If the note is past the timing window and is not hittable
        /// </summary>
        public bool isUnhittable { get; set; }

        /// <summary>
        /// Is this note part of a chord.
        /// </summary>
        public bool isChord { get; set; }

        /// <summary>
        /// The next lower placed note in a chord (emulates a linked list where the highest
        /// note points to the 2nd highest and so on).
        /// NOTE: The point structure is used just to have two int values, not as a real point per se.
        /// NOTE: -1, -1 is used to mean that this note IS the root note (or a single note).
        /// </summary>
        public Point rootNote { get; set; }

        /// <summary>
        /// Just a checker for when tick mode is on.  Don't want the tick to shoot off
        /// repeatedly after passing the center.
        /// </summary>
        public bool wasTicked { get; set; }

        /// <summary>
        /// The original sprite rectangle of a note (mainly used to preserve the 
        /// drum bass note's appearance).
        /// </summary>
        public Rectangle originalSpritePosition { get; set; }
    }
}
