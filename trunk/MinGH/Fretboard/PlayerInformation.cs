namespace MinGH.GameScreen
{
	/// <summary>
	/// Contains all information on a player during a MinGH session.
	/// </summary>
    public class PlayerInformation
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
        public const int missHealthPenalty = 3;
		
		/// <summary>
		/// The reward a player gets added to his current health when a note is successfully
		/// hit.
		/// </summary>
        public const int hitHealthReward = 2;
		
		/// <summary>
		/// The players current health.  If this number drops below 1, the player loses.
		/// </summary>
        public int currentHealth { get; set; }
		
		/// <summary>
		/// The current amount of notes the player has hit without miss.
		/// </summary>
        public int currentCombo { get; set; }
		
		/// <summary>
		/// The current multiplier the player has.  The multiplier will multiply the note's
		/// base score and add the resulting value to the users current score.
		/// </summary>
        public int currentMultiplier { get; set; }
		
		/// <summary>
		/// The current score the user has.
		/// </summary>
        public uint currentScore { get; set; }

        /// <summary>
        /// If the user is currently playing hopos (and is not missing any).
        /// </summary>
        public bool HOPOState { get; set; }

        /// <summary>
        /// Is true when the player hits a HOPO that precedes a normal note.  This
        /// allows the program to tell the input manager to ignore strums even
        /// when the player left the HOPO state without missing a note, else
        /// there will be misses when a player strums the last note of a 
        /// HOPO string.
        /// </summary>
        public bool leftHOPOState { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public PlayerInformation()
        {
            currentHealth = maxHealth / 2;
            currentCombo = 0;
            currentMultiplier = 1;
            currentScore = 0;
            HOPOState = false;
            leftHOPOState = false;
        }

		
		/// <summary>
		/// Sets the player state to if they missed a note.
		/// </summary>
        public void missNote()
        {
            HOPOState = false;
            currentCombo = 0;
            currentMultiplier = 1;
            currentHealth -= missHealthPenalty;
        }

		
		/// <summary>
        /// If the program deems the player hit a note, this function should be called.
		/// </summary>
		/// <param name="newHOPOState">The new hopo state the player will be in after hitting the note.</param>
		/// <param name="pointValue">The flat point value of the note hit.</param>
        public void hitNote(bool newHOPOState, int pointValue)
        {
            currentCombo++;
            if (HOPOState && !newHOPOState)
            {
                leftHOPOState = true;
            }
            else
            {
                leftHOPOState = false;
            }

            HOPOState = newHOPOState;

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

            currentScore += (uint)(currentMultiplier * pointValue);
        }
    }
}
