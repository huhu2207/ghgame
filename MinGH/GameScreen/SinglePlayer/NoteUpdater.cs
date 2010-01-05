using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MinGH.ChartImpl;
using MinGH.Misc_Classes;

namespace MinGH.GameScreen.SinglePlayer
{
    class NoteUpdater
    {
        // Updates the position of viewable notes and creates/destroys notes when necessary
        // NOTE: note_num is a simple 0-4 loop
        // NOTE: ...clean up these parameters sometime
        public static void updateNotes(Notechart inputNotechart, ref int inputNoteIterator,
                                       Note[,] physicalNotes, Rectangle viewportRectangle,
                                       GameTime currTime, double noteVelocity,
                                       int noteSize, double currentMsec,
                                       int spriteSheetSize, PlayerInformation playerInfo,
                                       HorizontalHitBox hitBox)
        {
            int currentNoteset = 0;
            // This check is here due to the final increment after the last note.
            // Once the last note passes (these two are equal), we get a out of bounds error.
            
            // If the current time > the next note to be drawn...
            while ((inputNoteIterator < inputNotechart.notes.Count) &&
                   (currentMsec >= inputNotechart.notes[inputNoteIterator].TimeValue))
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
                                    physicalNotes[currentNoteset, i].spriteSheetRectangle.Y = spriteSheetSize;
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
                                    physicalNotes[currentNoteset, i].isChord = true;
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

                                float newNotePos = physicalNotes[currentNoteset, i].spriteSheetOffset + 196 + (noteSize * currentNoteset);
                                physicalNotes[currentNoteset, i].position = new Vector2(newNotePos, 0f);
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
                        physicalNotes[i, j].position += new Vector2(0.0f, (float)(currTime.ElapsedGameTime.TotalMilliseconds * noteVelocity));
                    }

                    if ((physicalNotes[i, j].getCenterPosition().Y >= hitBox.centerLocation +  hitBox.goodThreshold) &&
                        (physicalNotes[i, j].isUnhittable == false))
                    {
                        playerInfo.missNote();
                        physicalNotes[i, j].isUnhittable = true;
                    }

                    // Actually kill the notes that leave the screen
                    if ((!viewportRectangle.Contains(new Point((int)physicalNotes[i, j].position.X,
                            (int)physicalNotes[i, j].position.Y))) && (physicalNotes[i, j].alive))
                    {
                        physicalNotes[i, j].alive = false;
                    }
                }
            }
        }                                   
    }
}
