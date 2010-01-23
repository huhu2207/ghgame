using GameEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MinGH.EngineExtensions
{
	/// <summary>
	/// An extended class of gameObject that contains data and functionality 
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
		/// <param name="offset">
		/// The padding on the left side of the spritesheet.  Use this if all the
        /// sprites appear to be pushed to the right slightly.
		/// </param>
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

        public void initalizeVerticies(int XSheetValue, int YSheetValue, int sheetStep)
        {
            float desiredTop = (XSheetValue * sheetStep) / (float)spriteSheet.Height;
            float desiredBottom = ((XSheetValue + 1) * sheetStep) / (float)spriteSheet.Height;
            float desiredLeft = (YSheetValue * sheetStep) / (float)spriteSheet.Width;
            float desiredRight = ((YSheetValue + 1) * sheetStep) / (float)spriteSheet.Width;

            vertices[0].Position = new Vector3(-10f, 10f, 0f);
            vertices[0].TextureCoordinate.X = desiredLeft;
            vertices[0].TextureCoordinate.Y = desiredTop;

            vertices[1].Position = new Vector3(10f, -10f, 0f);
            vertices[1].TextureCoordinate.X = desiredRight;
            vertices[1].TextureCoordinate.Y = desiredBottom;

            vertices[2].Position = new Vector3(-10f, -10f, 0f);
            vertices[2].TextureCoordinate.X = desiredLeft;
            vertices[2].TextureCoordinate.Y = desiredBottom;

            vertices[3].Position = new Vector3(10f, -10f, 0f);
            vertices[3].TextureCoordinate.X = desiredRight;
            vertices[3].TextureCoordinate.Y = desiredBottom;

            vertices[4].Position = new Vector3(-10f, 10f, 0f);
            vertices[4].TextureCoordinate.X = desiredLeft;
            vertices[4].TextureCoordinate.Y = desiredTop;

            vertices[5].Position = new Vector3(10f, 10f, 0f);
            vertices[5].TextureCoordinate.X = desiredRight;
            vertices[5].TextureCoordinate.Y = desiredTop;
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
