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
    class PlayerInputManager
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
        public static void processPlayerInput(Note[,] physicalNotes,
                                              NoteParticleExplosionEmitters noteParticleExplosionEmitters,
                                              HorizontalHitBox hitBox, PlayerInformation playerInformation,
                                              IKeyboardInputManager keyboardInputManager,
                                              Notechart inputNotechart)
        {
            if (playerInformation.HOPOState)
            {
                if (keyboardInputManager.getHighestHeldKey() != Keys.None)
                {
                    // Strums are ignored when the user is in the HOPO state (i.e. GH5 style)
                    triggerInput(physicalNotes, noteParticleExplosionEmitters, hitBox, keyboardInputManager, playerInformation, inputNotechart, false);
                }
            }
            else
            {
                if ((keyboardInputManager.keyIsHit(KeyboardConfiguration.upStrum) || keyboardInputManager.keyIsHit(KeyboardConfiguration.downStrum)) &&
                    (keyboardInputManager.getHighestHeldKey() != Keys.None))
                {
                    triggerInput(physicalNotes, noteParticleExplosionEmitters, hitBox, keyboardInputManager, playerInformation, inputNotechart, true);
                }
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
        /// <param name="wasStrummed">Whether a strum was executed.</param>
        private static void triggerInput(Note[,] physicalNotes,
                                         NoteParticleExplosionEmitters noteParticleExplosionEmitters,
                                         HorizontalHitBox hitBox, IKeyboardInputManager keyboardInputManager, 
                                         PlayerInformation playerInformation,
                                         Notechart inputNotechart, bool wasStrummed)
        {
            Point currentCenterPoint = new Point();
            int farthestNoteIndex = -1;
            int farthestNoteDistance = -1;
            int farthestNoteColumn = -1;
            int hitNote = -1;
            Keys currentKey = keyboardInputManager.getHighestHeldKey();
         
            // Convert the current key to a note type (maybe make a cast for this?)
            if (currentKey == KeyboardConfiguration.green)
            {
                hitNote = 0;
            }
            else if (currentKey == KeyboardConfiguration.red)
            {
                hitNote = 1;
            }
            else if (currentKey == KeyboardConfiguration.yellow)
            {
                hitNote = 2;
            }
            else if (currentKey == KeyboardConfiguration.blue)
            {
                hitNote = 3;
            }
            else if (currentKey == KeyboardConfiguration.orange)
            {
                hitNote = 4;
            }

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
                    if (keyboardInputManager.keyIsHit(KeyboardConfiguration.getKey(hitNote)) ||
                       (farthestNoteDistance > hitBox.centerLocation) ||
                       (wasStrummed))
                    {
                        // If true, we hit a chord...
                        if (physicalNotes[farthestNoteColumn, farthestNoteIndex].rootNote != new Point(-1, -1))
                        {
                            int chordDegree = 1;
                            Point currentRoot = new Point(farthestNoteColumn, farthestNoteIndex);
                            NoteType chordToHit = new NoteType();
                            NoteType chordYouHit = new NoteType();

                            // Scan backwards and add every note within the chord to the chordToHit variable.
                            while (currentRoot != new Point(-1, -1))
                            {
                                // The X value of a "root" note is the same as the numerical type
                                // (i.e. 0->green, 4->orange).
                                switch (KeyboardConfiguration.getKey(currentRoot.X))
                                {
                                    case KeyboardConfiguration.green:
                                        chordToHit.Green = true;
                                        break;
                                    case KeyboardConfiguration.red:
                                        chordToHit.Red = true;
                                        break;
                                    case KeyboardConfiguration.yellow:
                                        chordToHit.Yellow = true;
                                        break;
                                    case KeyboardConfiguration.blue:
                                        chordToHit.Blue = true;
                                        break;
                                    case KeyboardConfiguration.orange:
                                        chordToHit.Orange = true;
                                        break;
                                }
                                currentRoot = physicalNotes[currentRoot.X, currentRoot.Y].rootNote;
                                chordDegree++;
                            }

                            // Now create a noteType variable (noteYouHit) from what the player has held down
                            // at this point in time.
                            for (int i = 0; i < 5; i++)
                            {
                                if (keyboardInputManager.keyIsHeld(KeyboardConfiguration.getKey(i)))
                                {
                                    switch (i)
                                    {
                                        case 0:
                                            chordYouHit.Green = true;
                                            break;
                                        case 1:
                                            chordYouHit.Red = true;
                                            break;
                                        case 2:
                                            chordYouHit.Yellow = true;
                                            break;
                                        case 3:
                                            chordYouHit.Blue = true;
                                            break;
                                        case 4:
                                            chordYouHit.Orange = true;
                                            break;
                                    }
                                }
                            }

                            // If the user is holding the proper buttons, then explode the note...
                            if (chordToHit.isEqual(chordYouHit))
                            {
                                noteParticleExplosionEmitters.emitterList[farthestNoteColumn].Trigger(noteParticleExplosionEmitters.explosionLocations[farthestNoteColumn]);
                                physicalNotes[farthestNoteColumn, farthestNoteIndex].alive = false;

                                currentRoot = physicalNotes[farthestNoteColumn, farthestNoteIndex].rootNote;
                                while (currentRoot != new Point(-1, -1))
                                {
                                    noteParticleExplosionEmitters.emitterList[currentRoot.X].Trigger(noteParticleExplosionEmitters.explosionLocations[currentRoot.X]);
                                    physicalNotes[currentRoot.X, currentRoot.Y].alive = false;
                                    currentRoot = physicalNotes[currentRoot.X, currentRoot.Y].rootNote;
                                }

                                if (physicalNotes[farthestNoteColumn, farthestNoteIndex].precedsHOPO)
                                {
                                    playerInformation.hitNote(true, Note.pointValue * chordDegree);
                                }
                                else
                                {
                                    playerInformation.hitNote(false, Note.pointValue * chordDegree);
                                }
                            }
                        }
                        else
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
            }
            else
            {
                // Only miss if the player strummed and is NOT in a hopo state (or was in A HOPO state)
                if (wasStrummed && !playerInformation.HOPOState && !playerInformation.leftHOPOState)
                {
                    playerInformation.missNote();
                }
            }
        }
    }
}
