using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MinGH.Misc_Classes;

namespace MinGH.GameScreenImpl.GameScreenGameplaySinglePlayerImpl
{
    class PlayerInputManager
    {

        public void processPlayerInput(ref Note[,] physicalNotes,
                                       NoteParticleExplosionEmitters noteParticleExplosionEmitters,
                                       HorizontalHitBox hitBox, ref PlayerInformation playerInformation,
                                       bool useStrumming, IKeyboardInputManager keyboardInputManager)
        {
            if (useStrumming == false)
            {
                if (keyboardInputManager.keyIsHit(Keys.Enter) && keyboardInputManager.keyIsHeld(Keys.A))
                {
                    triggerInput(ref physicalNotes, noteParticleExplosionEmitters, hitBox, 0, ref playerInformation);
                }
                if (keyboardInputManager.keyIsHit(Keys.Enter) && keyboardInputManager.keyIsHeld(Keys.S))
                {
                    triggerInput(ref physicalNotes, noteParticleExplosionEmitters, hitBox, 1, ref playerInformation);
                }
                if (keyboardInputManager.keyIsHit(Keys.Enter) && keyboardInputManager.keyIsHeld(Keys.D))
                {
                    triggerInput(ref physicalNotes, noteParticleExplosionEmitters, hitBox, 2, ref playerInformation);
                }
                if (keyboardInputManager.keyIsHit(Keys.Enter) && keyboardInputManager.keyIsHeld(Keys.F))
                {
                    triggerInput(ref physicalNotes, noteParticleExplosionEmitters, hitBox, 3, ref playerInformation);
                }
                if (keyboardInputManager.keyIsHit(Keys.Enter) && keyboardInputManager.keyIsHeld(Keys.G))
                {
                    triggerInput(ref physicalNotes, noteParticleExplosionEmitters, hitBox, 4, ref playerInformation);
                }
            }
            else
            {
                if (keyboardInputManager.keyIsHit(Keys.A))
                {
                    triggerInput(ref physicalNotes, noteParticleExplosionEmitters, hitBox, 0, ref playerInformation);
                }
                if (keyboardInputManager.keyIsHit(Keys.S))
                {
                    triggerInput(ref physicalNotes, noteParticleExplosionEmitters, hitBox, 1, ref playerInformation);
                }
                if (keyboardInputManager.keyIsHit(Keys.D))
                {
                    triggerInput(ref physicalNotes, noteParticleExplosionEmitters, hitBox, 2, ref playerInformation);
                }
                if (keyboardInputManager.keyIsHit(Keys.F))
                {
                    triggerInput(ref physicalNotes, noteParticleExplosionEmitters, hitBox, 3, ref playerInformation);
                }
                if (keyboardInputManager.keyIsHit(Keys.G))
                {
                    triggerInput(ref physicalNotes, noteParticleExplosionEmitters, hitBox, 4, ref playerInformation);
                }
            }
        }

        private void triggerInput(ref Note[,] physicalNotes,
                                  NoteParticleExplosionEmitters noteParticleExplosionEmitters,
                                  HorizontalHitBox hitBox,
                                  int noteColumn, ref PlayerInformation playerInformation)
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
                playerInformation.hitNote();
            }
            else
            {
                playerInformation.missNote();
            }
        }
    }
}
