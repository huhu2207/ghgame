using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MinGH.ChartImpl;
using MinGH.EngineExtensions;
using MinGH.Config;

namespace MinGH.GameScreen.SinglePlayer
{
    /// <summary>
    /// Encompasses most of the logic for interpreting the user's input during a
    /// single player session via drums.
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
                                       NoteParticleEmitters noteParticleExplosionEmitters,
                                       HorizontalHitBox hitBox, PlayerInformation playerInformation,
                                       IKeyboardInputManager keyboardInputManager,
                                       Notechart inputNotechart)
        {
                if (keyboardInputManager.anyKeyIsHit())
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
                                         NoteParticleEmitters noteParticleExplosionEmitters,
                                         HorizontalHitBox hitBox, IKeyboardInputManager keyboardInputManager, 
                                         PlayerInformation playerInformation,
                                         Notechart inputNotechart)
        {
            List<Keys> currentKeyArray = keyboardInputManager.getHitKeyArray();
            foreach (Keys currentKey in currentKeyArray)
            {
                int hitNote = KeyboardConfiguration.getDrumNumberFromKey(currentKey);
                Vector3 currentCenterPoint = new Vector3();
                int farthestNoteIndex = -1;

                // Since -10000 is an impossible distance away from the screen,
                // it is a good value to start with when comparing how close
                // notes are to the screen.
                float farthestNoteDistance = -10000;

                for (int i = 0; i < physicalNotes.GetLength(1); i++)
                {
                    if ((hitNote > -1) && (physicalNotes[hitNote, i].alive))
                    {
                        currentCenterPoint = physicalNotes[hitNote, i].getCenterPosition();

                        // If the current physical note is alive and inside the hitbox...
                        if (hitBox.Contains(currentCenterPoint.Z))
                        {
                            // and has the farthest distance from the top
                            if (currentCenterPoint.Z >= farthestNoteDistance)
                            {
                                // set it to be the note to explode
                                farthestNoteDistance = currentCenterPoint.Z;
                                farthestNoteIndex = i;
                            }
                        }
                    }
                }
                    
                // If a note was found, process the players input.
                if (farthestNoteIndex != -1)
                {
                    noteParticleExplosionEmitters.emitterList[hitNote].Trigger(noteParticleExplosionEmitters.explosionLocations[hitNote]);
                    physicalNotes[hitNote, farthestNoteIndex].alive = false;

                    playerInformation.hitNote(false, Note.pointValue);
                }
                else
                {
                    playerInformation.missNote();
                }
            }
        }
    }
}
