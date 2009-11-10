using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MinGH.Misc_Classes;
using MinGH.ChartImpl;

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
            if (playerInformation.inHOPOState)
            {
                if (keyboardInputManager.getHighestHeldKey() != Keys.None)
                {
                    triggerInput(physicalNotes, noteParticleExplosionEmitters, hitBox, keyboardInputManager.getHighestHeldKey(), playerInformation, inputNotechart, false);
                }
            }
            else
            {
                if (keyboardInputManager.keyIsHit(KeyboardConfiguration.strum) &&
                    (keyboardInputManager.getHighestHeldKey() != Keys.None))
                {
                    triggerInput(physicalNotes, noteParticleExplosionEmitters, hitBox, keyboardInputManager.getHighestHeldKey(), playerInformation, inputNotechart, true);
                }
            }
        }

        private static void triggerInput(Note[,] physicalNotes,
                                         NoteParticleExplosionEmitters noteParticleExplosionEmitters,
                                         HorizontalHitBox hitBox,
                                         Keys currentKey, PlayerInformation playerInformation,
                                         Notechart inputNotechart, bool wasStrummed)
        {
            Point currentCenterPoint = new Point();
            int farthestNoteIndex = -1;
            int farthestNoteDistance = -1;
            int farthestNoteColumn = -1;
            int currentNote = -1;

            // Convert the current key to a note type (maybe make a cast for this?)
            if (currentKey == KeyboardConfiguration.green)
            {
                currentNote = 0;
            }
            else if (currentKey == KeyboardConfiguration.red)
            {
                currentNote = 1;
            }
            else if (currentKey == KeyboardConfiguration.yellow)
            {
                currentNote = 2;
            }
            else if (currentKey == KeyboardConfiguration.blue)
            {
                currentNote = 3;
            }
            else if (currentKey == KeyboardConfiguration.orange)
            {
                currentNote = 4;
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

            if ((farthestNoteIndex != -1) && (farthestNoteColumn != -1))
            {
                if (currentNote == farthestNoteColumn)
                {
                    noteParticleExplosionEmitters.emitterList[farthestNoteColumn].Trigger(noteParticleExplosionEmitters.explosionLocations[farthestNoteColumn]);
                    physicalNotes[farthestNoteColumn, farthestNoteIndex].alive = false;

                    if (physicalNotes[farthestNoteColumn, farthestNoteIndex].precedsHOPO == true)
                    {
                        playerInformation.inHOPOState = true;
                    }
                    else
                    {
                        playerInformation.inHOPOState = false;
                    }

                    playerInformation.hitNote();
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
