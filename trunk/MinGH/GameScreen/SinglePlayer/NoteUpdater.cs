using Microsoft.Xna.Framework;
using MinGH.ChartImpl;
using MinGH.MiscClasses;

namespace MinGH.GameScreen.SinglePlayer
{
    /// <summary>
    /// Encompasses most of the logic to be done during the single player game for every
    /// update.
    /// </summary>
    class NoteUpdater
    {
        /// <summary>
        /// Updates the position of viewable notes and creates/destroys notes when necessary
        /// </summary>
        /// <param name="inputNotechart">The Notechart currently being played.</param>
        /// <param name="inputNoteIterator">Indicates the next note to be drawn.</param>
        /// <param name="physicalNotes">The 2D array of drawable notes.</param>
        /// <param name="viewportRectangle">The rectangle surrounding the game screen.</param>
        /// <param name="currStep">The number of pixels each note on screen must move for
        ///                        this current update.</param>
        /// <param name="noteSize">The size of the note lanes on the background.</param>
        /// <param name="currentMsec">The current milisecond position the playing song is on.</param>
        /// <param name="spriteSheetSize">The size of an individual note on the sprite sheets (i.e. 100px)</param>
        /// <param name="playerInfo">The player's current status.</param>
        /// <param name="hitBox">The current hit window.</param>
        public static void updateNotes(Notechart inputNotechart, ref int inputNoteIterator,
                                       Note[,] physicalNotes, Rectangle viewportRectangle,
                                       float currStep, int noteSize, double currentMsec,
                                       int spriteSheetSize, PlayerInformation playerInfo,
                                       HorizontalHitBox hitBox)
        {
            int currentNoteset = 0;
            
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
                                    // Use the HOPO note skin if note is HOPO
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

                                // TODO: Standardize the note lane size and the "196" pixel space to the left of the lanes
                                float newNotePos = physicalNotes[currentNoteset, i].spriteSheetOffset + 196 + (noteSize * currentNoteset);
                                physicalNotes[currentNoteset, i].position = new Vector2(newNotePos, -spriteSheetSize);
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
                        physicalNotes[i, j].position += new Vector2(0.0f, currStep);
                    }

                    if ((physicalNotes[i, j].getCenterPosition().Y >= hitBox.centerLocation +  hitBox.goodThreshold) &&
                        (physicalNotes[i, j].isUnhittable == false))
                    {
                        playerInfo.missNote(true);
                        physicalNotes[i, j].isUnhittable = true;
                    }

                    // Actually kill the notes that leave the screen
                    if ((viewportRectangle.Height < (int)physicalNotes[i, j].position.Y) && (physicalNotes[i, j].alive))
                    {
                        physicalNotes[i, j].alive = false;
                    }
                }
            }
        }                                   
    }
}
