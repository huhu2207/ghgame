using Microsoft.Xna.Framework;
using MinGH.ChartImpl;
using MinGH.Config;
using MinGH.EngineExtensions;
using MinGH.Interfaces;

namespace MinGH.GameScreen.SinglePlayer
{
    /// <summary>
    /// Encompasses most of the logic to be done during the single player game for every
    /// update.  This special updater will also autohit any note that passes the center
    /// of the hitbox.
    /// </summary>
    class AutoplayNoteUpdater : INoteUpdater
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
        /// <param name="themeSetting">The current theme setting of the game.</param>
        /// <param name="currentMsec">The current milisecond position the playing song is on.</param>
        /// <param name="spriteSheetSize">The size of an individual note on the sprite sheets (i.e. 100px)</param>
        /// <param name="playerInfo">The player's current status.</param>
        /// <param name="hitBox">The current hit window.</param>
        /// <param name="noteParticleEmitters">Used for the autohit functionality.</param>
        public void updateNotes(Notechart inputNotechart, ref int inputNoteIterator,
                                Note[,] physicalNotes, Rectangle viewportRectangle,
                                float currStep, double currentMsec,
                                int spriteSheetSize, PlayerInformation playerInfo,
                                HorizontalHitBox hitBox, NoteParticleEmitters noteParticleEmitters)
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
                                    physicalNotes[currentNoteset, i].spriteSheetRectangle = 
                                        new Rectangle(physicalNotes[currentNoteset, i].spriteSheetRectangle.X,
                                                      physicalNotes[currentNoteset, i].spriteSheetRectangle.Y + spriteSheetSize,
                                                      physicalNotes[currentNoteset, i].spriteSheetRectangle.Width,
                                                      physicalNotes[currentNoteset, i].spriteSheetRectangle.Height);
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
                                physicalNotes[currentNoteset, i].position3D = new Vector3(physicalNotes[currentNoteset, i].position3D.X, physicalNotes[currentNoteset, i].position3D.Y, -1000f);
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
