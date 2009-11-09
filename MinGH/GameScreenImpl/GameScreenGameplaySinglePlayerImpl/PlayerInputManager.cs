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
                                              bool useStrumming, IKeyboardInputManager keyboardInputManager,
                                              Notechart inputNotechart)
        {
            if (useStrumming == true)
            {
                if (keyboardInputManager.keyIsHit(Keys.Enter) && keyboardInputManager.keyIsHeld(Keys.G))
                {
                    triggerInput(physicalNotes, noteParticleExplosionEmitters, hitBox, 4, playerInformation);
                }
                else if (keyboardInputManager.keyIsHit(Keys.Enter) && keyboardInputManager.keyIsHeld(Keys.F))
                {
                    triggerInput(physicalNotes, noteParticleExplosionEmitters, hitBox, 3, playerInformation);
                }
                else if (keyboardInputManager.keyIsHit(Keys.Enter) && keyboardInputManager.keyIsHeld(Keys.D))
                {
                    triggerInput(physicalNotes, noteParticleExplosionEmitters, hitBox, 2, playerInformation);
                }
                else if (keyboardInputManager.keyIsHit(Keys.Enter) && keyboardInputManager.keyIsHeld(Keys.S))
                {
                    triggerInput(physicalNotes, noteParticleExplosionEmitters, hitBox, 1, playerInformation);
                }
                else if (keyboardInputManager.keyIsHit(Keys.Enter) && keyboardInputManager.keyIsHeld(Keys.A))
                {
                    triggerInput(physicalNotes, noteParticleExplosionEmitters, hitBox, 0, playerInformation);
                }
            }
            else
            {
                if (keyboardInputManager.keyIsHit(Keys.A))
                {
                    triggerInput(physicalNotes, noteParticleExplosionEmitters, hitBox, 0, playerInformation);
                }
                if (keyboardInputManager.keyIsHit(Keys.S))
                {
                    triggerInput(physicalNotes, noteParticleExplosionEmitters, hitBox, 1, playerInformation);
                }
                if (keyboardInputManager.keyIsHit(Keys.D))
                {
                    triggerInput(physicalNotes, noteParticleExplosionEmitters, hitBox, 2, playerInformation);
                }
                if (keyboardInputManager.keyIsHit(Keys.F))
                {
                    triggerInput(physicalNotes, noteParticleExplosionEmitters, hitBox, 3, playerInformation);
                }
                if (keyboardInputManager.keyIsHit(Keys.G))
                {
                    triggerInput(physicalNotes, noteParticleExplosionEmitters, hitBox, 4, playerInformation);
                }
            }
        }

        private static void triggerInput(Note[,] physicalNotes,
                                         NoteParticleExplosionEmitters noteParticleExplosionEmitters,
                                         HorizontalHitBox hitBox,
                                         int noteColumn, PlayerInformation playerInformation)
        {
            Point currentCenterPoint = new Point();
            int farthestNoteIndex = -1;
            int farthestNoteDistance = -1;

            // Scan all noteColumn (green, red, etc.) notes...
            for (int i = 0; i < physicalNotes.GetLength(1); i++)
            {
                currentCenterPoint = new Point((int)physicalNotes[noteColumn, i].getCenterPosition().X, (int)physicalNotes[noteColumn, i].getCenterPosition().Y);

                // If the current green note is in the hitBox and is alive
                if (hitBox.physicalHitbox.Contains(currentCenterPoint) && physicalNotes[noteColumn, i].alive)
                {
                    // and has the farthest distance from the top
                    if (currentCenterPoint.Y > farthestNoteDistance)
                    {
                        // set it to be the note to explode
                        farthestNoteDistance = currentCenterPoint.Y;
                        farthestNoteIndex = i;
                    }
                }
            }

            if (farthestNoteIndex != -1)
            {
                noteParticleExplosionEmitters.emitterList[noteColumn].Trigger(noteParticleExplosionEmitters.explosionLocations[noteColumn]);
                physicalNotes[noteColumn, farthestNoteIndex].alive = false;
                playerInformation.hitNote(false);
            }
            else
            {
                playerInformation.missNote();
            }
        }
    }
}
