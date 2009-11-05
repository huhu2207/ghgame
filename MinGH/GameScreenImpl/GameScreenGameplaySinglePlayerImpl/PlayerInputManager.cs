﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using MinGH.ChartImpl;
using Microsoft.Xna.Framework;

namespace MinGH.GameScreenImpl.GameScreenGameplaySinglePlayerImpl
{
    class PlayerInputManager
    {

        public void processPlayerInput(ref gameObject[,] physicalNotes,
                                       NoteParticleExplosionEmitters noteParticleExplosionEmitters,
                                       IKeyboardInputManager keyboardInputManager,
                                       HorizontalHitBox hitBox,
                                       int noteColumn)
        {
            Point currentPoint = new Point();
            int farthestNoteIndex = -1;
            int farthestNoteDistance = -1;

            // Scan all noteColumn (green, red, etc.) notes...
            for (int i = 0; i < physicalNotes.GetLength(1); i++)
            {
                currentPoint = new Point((int)physicalNotes[noteColumn, i].getCenterPosition().X, (int)physicalNotes[noteColumn, i].getCenterPosition().Y);
                
                // If the current green note is in the hitBox and is alive
                if (hitBox.physicalHitbox.Contains(currentPoint) && physicalNotes[noteColumn, i].alive)
                {
                    // and has the farthest distance from the top
                    if (currentPoint.Y > farthestNoteDistance)
                    {
                        // set it to be the note to explode
                        farthestNoteDistance = currentPoint.Y;
                        farthestNoteIndex = i;
                    }
                }
            }

            if (farthestNoteIndex != -1)
            {
                noteParticleExplosionEmitters.emitterList[noteColumn].Trigger(noteParticleExplosionEmitters.explosionLocations[noteColumn]);
                physicalNotes[noteColumn, farthestNoteIndex].alive = false;
            }
            else
            {
                // NOTE WAS MISSED
            }
        }
    }
}
