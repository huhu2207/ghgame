using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MinGH.Misc_Classes;

namespace MinGH.Misc_Classes
{
    class PlayerInformation
    {
        public const int maxHealth = 100;
        public const int multiplierThreshold = 10;  // How many sequential note hits before multiplier is incremented
        public const int maxMultiplier = 4;
        public const int missHealthPenalty = 8;
        public const int hitHealthReward = 2;

        public int currentHealth;
        public int currentCombo;
        public int currentMultiplier;
        public uint currentScore;

        // Default Constructor
        public PlayerInformation()
        {
            currentHealth = maxHealth / 2;
            currentCombo = 0;
            currentMultiplier = 1;
            currentScore = 0;
        }

        public void missNote()
        {
            currentCombo = 0;
            currentMultiplier = 1;
            currentHealth -= missHealthPenalty;
        }

        public void hitNote()
        {
            currentCombo++;

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
