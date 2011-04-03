using Microsoft.Xna.Framework;
using MinGH.Config;

namespace MinGH.Fretboard
{
    /// <summary>
    /// An imaginary horizontal rectangle that is used to detect the accuracy
    /// of the users input while playing MinGH.
    /// </summary>
    public class HorizontalHitBox
    {	
		/// <summary>
		/// The center of the hitbox. We only need to concern ourselves with one dimension, 
        /// since notes will only come from one direction.
		/// </summary>
        public int centerLocation { get; set; }
		
		/// <summary>
		/// How many pixels away from the center value can the note be hit and considered
		/// a perfect hit.  This is a grading mechanism for the user's accuracy.
		/// Note that these thresholds apply to both sides of the center point, so if
        /// the value is 10, the REAL perfect window is 20 pixels.  This applies for all 3
		/// grading values.
		/// </summary>
        public int perfectThreshold { get; set; }
		
		/// <summary>
		/// How many pixels away from the center value can the note be hit and considered
		/// a great hit.  This is a grading mechanism for the user's accuracy.
		/// </summary>
        public int greatThreshold { get; set; }

        /// <summary>
        /// The final threshold before the player will simply miss the note.  This value is
        /// used when generating the actual hitbox.  Since no note can be hit outside this
        /// range.  The actual hitbox will have a total size of goodThreshold * 2.
        /// </summary>
        public int goodThreshold  { get; set; }

        /// <summary>
        /// The base size of the hitbox before the note velocity is taken into
        /// consideration.
        /// </summary>
        public const int hitBoxSize = 50;

        /// <summary>
        /// Default Constructor.  Doesn't really serve any purpose, but will be kept for
        /// consistency's sake
        /// </summary>
        public HorizontalHitBox()
        {
            centerLocation = 0;
        }

        /// <summary>
        /// The real constructor that generate the bounding rectangle when given the total
        /// size of the game screen.  The hit box will be centered around a specified point
        /// which is expected to be the center of the drawn hitmarker.
        /// </summary>
        public HorizontalHitBox(int hitMarkerCenter, int currMSTillHit)
        {
            centerLocation = hitMarkerCenter;

            goodThreshold = 2000 / currMSTillHit * hitBoxSize;
            greatThreshold = goodThreshold / 2;
            perfectThreshold = greatThreshold / 2;
        }

        /// <summary>
        /// A simple check to see if a 1 dimensional point (along the Z axis) is
        /// contained within the hitbox.
        /// </summary>
        public bool Contains(float point)
        {
            return (point > (-centerLocation - goodThreshold) &&
                    point < (-centerLocation + goodThreshold));
        }
    }
}
