using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MinGH.Misc_Classes;
using MinGH.ChartImpl;
using System.Collections.Generic;

namespace MinGH.GameScreenImpl.GameScreenGameplaySinglePlayerImpl
{
    class PlayerInputManager
    {
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
                    triggerInput(physicalNotes, noteParticleExplosionEmitters, hitBox, keyboardInputManager, playerInformation, inputNotechart, false);
                }
            }
            else
            {
                if (keyboardInputManager.keyIsHit(KeyboardConfiguration.strum) &&
                    (keyboardInputManager.getHighestHeldKey() != Keys.None))
                {
                    triggerInput(physicalNotes, noteParticleExplosionEmitters, hitBox, keyboardInputManager, playerInformation, inputNotechart, true);
                }
            }
        }

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
            List<Point> hitboxNoteIndicies = new List<Point>();  // I use point as a 2 int struct

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
                            // Store every note in the hitbox incase a chord is hit
                            hitboxNoteIndicies.Add(new Point(i, j));

                            // and has the farthest distance from the top
                            if (currentCenterPoint.Y > farthestNoteDistance)
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
                    // unless they strummed (or explicitly hit the note)
                    if (keyboardInputManager.keyIsHit(KeyboardConfiguration.getKey(hitNote)) ||
                       (farthestNoteDistance > hitBox.centerLocation) ||
                       (wasStrummed))
                    {
                        noteParticleExplosionEmitters.emitterList[farthestNoteColumn].Trigger(noteParticleExplosionEmitters.explosionLocations[farthestNoteColumn]);
                        physicalNotes[farthestNoteColumn, farthestNoteIndex].alive = false;

                        if (physicalNotes[farthestNoteColumn, farthestNoteIndex].precedsHOPO)
                        {
                            playerInformation.hitNote(true);
                        }
                        else
                        {
                            playerInformation.hitNote(false);
                        }
                    }
                }
            }
            else
            {
                if (wasStrummed)
                {
                    playerInformation.missNote();
                }
            }
        }
    }
}
