using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MinGH.ChartImpl;
using MinGH.Misc_Classes;

namespace MinGH.GameScreenImpl.GameScreenGameplaySinglePlayerImpl
{
    class NoteUpdater
    {
        // Updates the position of viewable notes and creates/destroys notes when necessary
        // NOTE: note_num is a simple 0-4 loop
        // NOTE: ...clean up these parameters sometime
        public void updateNotes(Notechart inputNotechart, ref int inputNoteIterator,
                                ref Note[,] physicalNotes, Rectangle viewportRectangle,
                                GameTime currTime, double noteVelocity,
                                int noteSize, double currentMsec, PlayerInformation playerInformation,
                                int spriteSheetSize)
        {
            int currentNoteset = 0;
            // This check is here due to the final increment after the last note.
            // Once the last note passes (these two are equal), we get a out of bounds error.
            if (!(inputNoteIterator >= inputNotechart.notes.Count))
            {
                // If the current time > the next note to be drawn...
                while (currentMsec >= inputNotechart.notes[inputNoteIterator].TimeValue)
                {
                    currentNoteset = (int)inputNotechart.notes[inputNoteIterator].noteType;

                    // Look for a spot to draw the note
                    for (int i = 0; i < physicalNotes.GetLength(1); i++)
                    {
                        if (physicalNotes[currentNoteset, i].alive == false)
                        {
                            physicalNotes[currentNoteset, i].noteType = inputNotechart.notes[inputNoteIterator].noteType;

                            if (inputNotechart.notes[inputNoteIterator].isHOPO == true)
                            {
                                // Move the sprite sheet rectangle to the hopo texture immediately below
                                // the normal note texture.
                                physicalNotes[currentNoteset, i].spriteSheetRectangle.Y = spriteSheetSize;
                                physicalNotes[currentNoteset, i].isHopo = true;
                            }
                            else if(inputNotechart.notes[inputNoteIterator].isChord == true)
                            {
                                physicalNotes[currentNoteset, i].spriteSheetRectangle.Y = spriteSheetSize * 2;
                                physicalNotes[currentNoteset, i].isChord = true;
                            }
                            else
                            {
                                // Reset the note texture back to the basic note texture
                                physicalNotes[currentNoteset, i].spriteSheetRectangle.Y = 0;
                            }

                            if (inputNotechart.notes[inputNoteIterator + 1].isHOPO == true)
                            {
                                physicalNotes[currentNoteset, i].precedesHopo = true;
                            }

                            physicalNotes[currentNoteset, i].alive = true;
                            float newNotePos = physicalNotes[currentNoteset, i].spriteSheetOffset + 196 + (noteSize * currentNoteset);
                            physicalNotes[currentNoteset, i].position = new Vector2(newNotePos, 0f);
                            break;
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

                    // Kill any notes that managed to get past the previous check and left the screen
                    // Also tell the player he missed a note.
                    if ((!viewportRectangle.Contains(new Point((int)physicalNotes[i, j].position.X,
                            (int)physicalNotes[i, j].position.Y))) && (physicalNotes[i, j].alive))
                    {
                        playerInformation.missNote();
                        physicalNotes[i, j].alive = false;
                    }
                }
            }
        }                                   
    }
}
