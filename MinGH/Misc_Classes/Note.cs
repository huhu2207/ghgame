using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MinGH.ChartImpl;

namespace MinGH
{
	/// <remarks>
	/// An extended class of gameObject that contains data and functionality 
	/// specific to the drawable notes within the MinGH game.
	/// </remarks>
    public class Note : GameObject
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
        public Note(Texture2D loadedTex, Rectangle spritePos, int offset)
            : base(loadedTex, spritePos, offset)
        {
            noteChartIndex = 0;
            precedsHOPO = false;
            isUnhittable = false;
            isChord = false;
        }

        /// <summary>
        /// Resets the note specific information for future use.
        /// </summary>
        public void ResetNote()
        {
            noteChartIndex = 0;
            precedsHOPO = false;
            isUnhittable = false;
            isChord = false;
            spriteSheetRectangle.Y = 0;
        }

		/// <summary>
		/// The base point value of a note (this value may be in the wrong place,
		/// but I'll keep it there for now).
		/// </summary>
        public const int pointValue = 50;

        /// <summary>
        /// Where the note is located on the currently playing notechart.
        /// </summary>
        public int noteChartIndex;

        /// <summary>
        /// If the note is followed by a hammeron/pulloff.
        /// </summary>
        public bool precedsHOPO;

        /// <summary>
        /// If the note is past the timing window and is not hittable
        /// </summary>
        public bool isUnhittable;

        /// <summary>
        /// Is this note part of a chord.
        /// </summary>
        public bool isChord;
    }
}
