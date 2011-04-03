using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FlatRedBall;
using MinGH.FRBExtensions;

namespace MinGH.EngineExtensions
{
	/// <summary>
	/// An extended class of gameObject3D that contains data and functionality 
	/// specific to the drawable notes within the MinGH game.
	/// </summary>
    public class Note
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
        /// <param name="device">
        /// A graphics device.
        /// </param>
        public Note(Texture2D loadedTex, Rectangle spritePos, Effect effectToUse, GraphicsDevice device)
        {
            sprite = new SteppableSprite();
            noteChartIndex = 0;
            precedsHOPO = false;
            isUnhittable = false;
            isChordStart = false;
            isPartOfChord = false;
            rootNote = new Point();
            wasTicked = false;
            originalSpriteStepX = 0;
            originalSpriteStepY = 0;
            pointValue = 50;
        }

        /// <summary>
        /// Resets the note specific information for future use.
        /// </summary>
        public void ResetNote()
        {
            sprite.spriteSheetStepX = originalSpriteStepX;
            sprite.spriteSheetStepY = originalSpriteStepY;
            noteChartIndex = 0;
            precedsHOPO = false;
            isUnhittable = false;
            isChordStart = false;
            isPartOfChord = false;
            rootNote = new Point(-1 , -1);
            wasTicked = false;
            pointValue = 50;
        }

        public SteppableSprite sprite { get; set; }

		/// <summary>
		/// The base point value of a note (this value may be in the wrong place,
		/// but I'll keep it there for now).
		/// </summary>
        public int pointValue { get; set; }

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
        /// Is the note the starting note of a chord
        /// </summary>
        public bool isChordStart { get; set; }

        /// <summary>
        /// Is this note part of a chord.
        /// </summary>
        public bool isPartOfChord { get; set; }

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
        /// The original sprite step of this particular note
        /// </summary>
        public int originalSpriteStepX { get; set; }
        public int originalSpriteStepY { get; set; }
    }
}
