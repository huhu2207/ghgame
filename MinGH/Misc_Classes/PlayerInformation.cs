
namespace MinGH.Misc_Classes
{
	/// <remarks>
	/// Contains all information on a player during a MinGH session.
	/// </remarks>
    class PlayerInformation
    {
		/// <summary>
		/// The maximum allowed health.
		/// </summary>
        public const int maxHealth = 100;
		
		/// <summary>
		/// How many sequential notes must be hit before the multiplier is increased
		/// </summary>
        public const int multiplierThreshold = 10;
		
		/// <summary>
		/// The highest possible multiplier this player can obtain.
		/// </summary>
        public const int maxMultiplier = 4;
		
		/// <summary>
		/// The penalty to the players health when a note is missed (or the player attempts
		/// to hit a note that is not there).
		/// </summary>
        public const int missHealthPenalty = 8;
		
		/// <summary>
		/// The reward a player gets added to his current health when a note is successfully
		/// hit.
		/// </summary>
        public const int hitHealthReward = 2;
		
		/// <summary>
		/// The players current health.  If this number drops below 1, the player loses.
		/// </summary>
        public int currentHealth;
		
		/// <summary>
		/// The current amount of notes the player has hit without miss.
		/// </summary>
        public int currentCombo;
		
		/// <summary>
		/// The current multiplier the player has.  The multiplier will multiply the note's
		/// base score and add the resulting value to the users current score.
		/// </summary>
        public int currentMultiplier;
		
		/// <summary>
		/// The current score the user has.
		/// </summary>
        public uint currentScore;

        /// <summary>
        /// If the user is currently playing hopos (and is not missing any).
        /// </summary>
        public bool inHOPOState;

        /// <summary>
        /// The index of the next hammer on note (so the player doesnt hammer on notes
        /// he shouldnt be allowed to).
        /// </summary>
        public int nextExpectedHammerOnIndex;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public PlayerInformation()
        {
            currentHealth = maxHealth / 2;
            currentCombo = 0;
            currentMultiplier = 1;
            currentScore = 0;
            inHOPOState = false;
        }

		
		/// <summary>
		/// If the program deems the player missed a note, this function should be called.
		/// </summary>
        public void missNote()
        {
            inHOPOState = false;
            currentCombo = 0;
            currentMultiplier = 1;
            currentHealth -= missHealthPenalty;
        }

		
		/// <summary>
		/// If the program deems the player hit a note, this function should be called.
		/// </summary>
        public void hitNote(bool nextNoteisHOPO)
        {
            currentCombo++;
            inHOPOState = nextNoteisHOPO;

            if (currentHealth < maxHealth)
            {
                currentHealth = (currentHealth + hitHealthReward);

                // Quick safety check in case the current hit pushes us over the max
                if (currentHealth > maxHealth)
                {
                    currentHealth = maxHealth;
                }
            }

            // Calculate the new multiplier
            if (currentMultiplier < maxMultiplier)
            {
                currentMultiplier = (currentCombo / multiplierThreshold) + 1;
            }

            currentScore += (uint)(currentMultiplier * (int)Note.pointValue);
        }
    }
}
