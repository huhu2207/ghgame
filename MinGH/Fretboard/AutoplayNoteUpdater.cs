using Microsoft.Xna.Framework;
using MinGH.ChartImpl;
using MinGH.Config;
using MinGH.EngineExtensions;

namespace MinGH.Fretboard
{
    /// <summary>
    /// See INoteUpdater for more information.
    /// </summary>
    class AutoplayNoteUpdater : INoteUpdater
    {
        public void updateNotes(Notechart inputNotechart, ref int inputNoteIterator,
                                Note[,] physicalNotes, Rectangle viewportRectangle,
                                float currStep, double currentMsec,
                                int spriteSheetSize, PlayerInformation playerInfo,
                                HorizontalHitBox hitBox, NoteParticleEmitters noteParticleEmitters,
                                float noteStartPosition, float timeNotesTakeToPassHitmarker,
                                float currStepPerMilisecond)
        {
            int currentNoteset = 0;
            
            // If the current time > the next note to be drawn...
            while ((inputNoteIterator < inputNotechart.notes.Count) &&
                   (currentMsec + timeNotesTakeToPassHitmarker >= 
                    inputNotechart.notes[inputNoteIterator].TimeValue))
            {
                if (!(inputNoteIterator >= inputNotechart.notes.Count))
                {
                    Point currentRoot = new Point(-1, -1);
                    for (int j = 0; j < inputNotechart.notes[inputNoteIterator].getNoteCount(); j++)
                    {
                        currentNoteset = inputNotechart.notes[inputNoteIterator].getNthNote(j);

                        // Look for a spot to draw the note
                        for (int i = 0; i < physicalNotes.GetLength(1); i++)
                        {
                            if (physicalNotes[currentNoteset, i].alive == false)
                            {
                                // Reset the note and apply the various properties
                                physicalNotes[currentNoteset, i].ResetNote();
                                if (inputNotechart.notes[inputNoteIterator].isHOPO == true)
                                {
                                    // Use the HOPO note skin if note is HOPO
                                    physicalNotes[currentNoteset, i].spriteSheetStepY += 1;
                                }

                                physicalNotes[currentNoteset, i].alive = true;
                                physicalNotes[currentNoteset, i].noteChartIndex = inputNoteIterator;

                                if ((inputNoteIterator != inputNotechart.notes.Count - 1) &&
                                    (inputNotechart.notes[inputNoteIterator + 1].isHOPO == true))
                                {
                                    physicalNotes[currentNoteset, i].precedsHOPO = true;
                                }

                                if (inputNotechart.notes[inputNoteIterator].isChord)
                                {
                                    if (j == inputNotechart.notes[inputNoteIterator].getNoteCount() - 1)
                                    {
                                        physicalNotes[currentNoteset, i].isChordStart = true;
                                    }
                                    else
                                    {
                                        physicalNotes[currentNoteset, i].isPartOfChord = true;
                                    }

                                    if (currentRoot == new Point(-1, -1))
                                    {
                                        physicalNotes[currentNoteset, i].rootNote = currentRoot;
                                        currentRoot = new Point(currentNoteset, i);
                                    }
                                    else
                                    {
                                        physicalNotes[currentNoteset, i].rootNote = currentRoot;
                                        currentRoot = new Point(currentNoteset, i);
                                    }
                                }

                                // Find out where a note should actually be upon spawning
                                float actualPosition = noteStartPosition - (float)((currentMsec + timeNotesTakeToPassHitmarker - inputNotechart.notes[inputNoteIterator].TimeValue) * currStepPerMilisecond);
                                physicalNotes[currentNoteset, i].position3D = new Vector3(physicalNotes[currentNoteset, i].position3D.X, physicalNotes[currentNoteset, i].position3D.Y, -actualPosition);
                                break;
                            }
                        }
                    }
                    inputNoteIterator++;
                }
            }

            // Update the living note's positions and kill notes that leave the screen
            for (int i = 0; i < physicalNotes.GetLength(0); i++)
            {
                for (int j = 0; j < physicalNotes.GetLength(1); j++)
                {
                    if (physicalNotes[i, j].alive == true)
                    {
                        physicalNotes[i, j].position3D += new Vector3(0, 0, currStep);

                        if (physicalNotes[i, j].getCenterPosition().Z >= -hitBox.centerLocation)
                        {
                            noteParticleEmitters.emitterList[i].Trigger(noteParticleEmitters.explosionLocations[i]);
                            playerInfo.hitNote(false, physicalNotes[i, j].pointValue);
                            physicalNotes[i, j].alive = false;
                        }

                        // Actually kill the notes that leave the screen
                        if ((physicalNotes[i, j].position3D.Z > 0) && (physicalNotes[i, j].alive))
                        {
                            physicalNotes[i, j].alive = false;
                        }
                    }
                }
            }
        }                                   
    }
}
