using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MinGH.ChartImpl;
using MinGH.MiscClasses;

namespace MinGH.GameScreen.SinglePlayer
{
    /// <summary>
    /// Encompasses most of the logic for interpreting the user's input during a
    /// single player session.
    /// </summary>
    public class DrumInputManager : IInputManager
    {
        /// <summary>
        /// Uses the current keyboard state to figure out whether the user attempted to
        /// hit a note or not.
        /// </summary>
        /// <param name="physicalNotes">The 2D array of drawable notes.</param>
        /// <param name="noteParticleExplosionEmitters">The project mercury explosion emitters.</param>
        /// <param name="hitBox">The current hit window.</param>
        /// <param name="playerInformation">The player's current status.</param>
        /// <param name="keyboardInputManager">The current state of the keyboard.</param>
        /// <param name="inputNotechart">The Notechart currently being played.</param>
        public void processPlayerInput(Note[,] physicalNotes,
                                       NoteParticleExplosionEmitters noteParticleExplosionEmitters,
                                       HorizontalHitBox hitBox, PlayerInformation playerInformation,
                                       IKeyboardInputManager keyboardInputManager,
                                       Notechart inputNotechart)
        {
                if (keyboardInputManager.getCurrentKeyArray().Length > 0)
                {
                    // Strums are ignored when the user is in the HOPO state (i.e. GH5 style)
                    triggerInput(physicalNotes, noteParticleExplosionEmitters, hitBox, keyboardInputManager, playerInformation, inputNotechart);
                }
        }

        /// <summary>
        /// Figures out whether the user missed, or hit a note.
        /// </summary>
        /// <param name="physicalNotes">The 2D array of drawable notes.</param>
        /// <param name="noteParticleExplosionEmitters">The project mercury explosion emitters.</param>
        /// <param name="hitBox">The current hit window.</param>
        /// <param name="keyboardInputManager">The current state of the keyboard.</param>
        /// <param name="playerInformation">The player's current status.</param>
        /// <param name="inputNotechart">The Notechart currently being played.</param>
        private static void triggerInput(Note[,] physicalNotes,
                                         NoteParticleExplosionEmitters noteParticleExplosionEmitters,
                                         HorizontalHitBox hitBox, IKeyboardInputManager keyboardInputManager, 
                                         PlayerInformation playerInformation,
                                         Notechart inputNotechart)
        {
            Point currentCenterPoint = new Point();
            int farthestNoteIndex = -1;
            int farthestNoteDistance = -1;
            int farthestNoteColumn = -1;
            
            //Keys currentKey = keyboardInputManager.getHighestHeldKey();
            Keys[] currentKeyArray = keyboardInputManager.getCurrentKeyArray();
            int hitNote = -1;

            foreach (Keys currentKey in currentKeyArray)
            {
                hitNote = -1;
                hitNote = KeyboardConfiguration.getDrumNumberFromKey(currentKey);

                // Scan every physical note...
                for (int i = 0; i < physicalNotes.GetLength(0); i++)
                {
                    for (int j = 0; j < physicalNotes.GetLength(1); j++)
                    {
                        if (physicalNotes[i, j].alive)
                        {
                            currentCenterPoint = new Point((int)physicalNotes[i, j].getCenterPosition().X, (int)physicalNotes[i, j].getCenterPosition().Y);

                            // If the current physical note is alive and inside the hitbox...
                            if (hitBox.physicalHitbox.Contains(currentCenterPoint))
                            {
                                // and has the farthest distance from the top
                                if (currentCenterPoint.Y >= farthestNoteDistance)
                                {
                                    // set it to be the note to explode
                                    farthestNoteDistance = currentCenterPoint.Y;
                                    farthestNoteColumn = i;
                                    farthestNoteIndex = j;
                                }
                            }
                        }
                    }
                }

                // If a note was found, process the players input.
                if ((farthestNoteIndex != -1) && (farthestNoteColumn != -1))
                {
                    if (hitNote == farthestNoteColumn)
                    {
                        // Dont hit the note if the player was holding prior to the note entering the hit window
                        // unless they strummed or explicitly hit the note (i.e. he hammered on too early)
                        if (keyboardInputManager.keyIsHit(currentKey) ||
                           (farthestNoteDistance > hitBox.centerLocation))
                        {
                            noteParticleExplosionEmitters.emitterList[farthestNoteColumn].Trigger(noteParticleExplosionEmitters.explosionLocations[farthestNoteColumn]);
                            physicalNotes[farthestNoteColumn, farthestNoteIndex].alive = false;

                            if (physicalNotes[farthestNoteColumn, farthestNoteIndex].precedsHOPO)
                            {
                                playerInformation.hitNote(true, Note.pointValue);
                            }
                            else
                            {
                                playerInformation.hitNote(false, Note.pointValue);
                            }
                        }
                    }
                }
                else
                {
                    // Only miss if the player strummed and is NOT in a hopo state (or was in A HOPO state)
                    if (!playerInformation.HOPOState && !playerInformation.leftHOPOState)
                    {
                        playerInformation.missNote();
                    }
                }
            }
        }
    }
}
