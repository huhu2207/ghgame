using Microsoft.Xna.Framework;
using MinGH.Config;

namespace MinGH.GameScreen.SinglePlayer
{
    /// <summary>
    /// A thin horizontal rectangle that is used to detect the accuracy of the users input
    /// while playing MinGH.
    /// </summary>
    class HorizontalHitBox
    {
		/// <summary>
		/// The actual bounding rectangle of the hitbox.
		/// </summary>
        public Rectangle physicalHitbox;
		
		/// <summary>
		/// The center of the hitbox.  Note that since this is a horiziontal hitbox, we only
		/// need to concern ourselves with the Y value since the bounding rectangle will
		/// span the entire screen on the X axis.
		/// </summary>
        public int centerLocation { get; set; }
		
		/// <summary>
		/// How many pixels away from the center value can the note be hit and considered
		/// a perfect hit.  This is a grading mechanism for the user's accuracy.
		/// Note that these thresholds apply to both sides of the center point, so the
		/// "real" threshold for a perfect hit is 50 pixels.  This applies for all 3
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
        /// range.  The actual bounding rectangle will have a total height of
        /// goodThreshold * 2.
        /// </summary>
        public int goodThreshold  { get; set; }

        /// <summary>
        /// The base size of the good threshold before the note velocity is taken into
        /// consideration.
        /// </summary>
        public const int goodThresholdConstant = 140;

        /// <summary>
        /// Default Constructor.  Doesn't really serve any purpose, but will be kept for
        /// consistency's sake
        /// </summary>
        public HorizontalHitBox()
        {
            physicalHitbox = new Rectangle();
            centerLocation = 0;
        }

        /// <summary>
        /// The real constructor that generate the bounding rectangle when given the total
        /// size of the game screen.  The hit box will be centered at the 85% mark towards
        /// the bottom of the screen.
        /// </summary>
        /// <param name="gameScreenRectangle">
        /// The dimensions for the entire screen (i.e. 800x600).  This is usally gotten
        /// from a graphics object.
        /// </param>
        public HorizontalHitBox(Rectangle gameScreenRectangle, SpeedModValue currHyperspeed)
        {
            // The hit box center is at 85% towards the bottom.
            // NOTE: we convert to int...this may introduce slight error on some resolutions
            centerLocation = (int)(gameScreenRectangle.Height * 0.85);

            goodThreshold = (int)(currHyperspeed.noteVelocityMultiplier * goodThresholdConstant);
            greatThreshold = goodThreshold / 2;
            perfectThreshold = greatThreshold / 2;

            physicalHitbox = new Rectangle
            {
                Width = gameScreenRectangle.Width,
                Height = goodThreshold * 2,
                Y = centerLocation - (goodThreshold),
                X = 0
            };
        }
    }
}
