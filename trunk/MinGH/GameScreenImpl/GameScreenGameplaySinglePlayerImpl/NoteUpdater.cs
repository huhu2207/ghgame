﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using MinGH.ChartImpl;
using ProjectMercury.Emitters;

namespace MinGH.GameScreenImpl.GameScreenGameplaySinglePlayerImpl
{
    class NoteUpdater
    {
        // Updates the position of viewable notes and creates/destroys notes when necessary
        // NOTE: note_num is a simple 0-4 loop
        // NOTE: ...clean up these parameters sometime
        public void updateNotes(Notechart inputNotechart, ref int[] inputNoteIterators,
                                ref gameObject[,] physicalNotes, Rectangle viewportRectangle,
                                GameTime currTime, double noteVelocity,
                                int noteSize, double currentMsec,
                                NoteParticleExplosionEmitters noteParticleExplosionEmitters, int hitBarYValue)
        {
            List<Note> currentNoteList = new List<Note>();

            // Change the current note list to look at according to the current loop iteration
            for (int currentNoteset = 0; currentNoteset < 5; currentNoteset++)
            {
                switch (currentNoteset)
                {
                    case 0:
                        currentNoteList = inputNotechart.greenNotes;
                        break;
                    case 1:
                        currentNoteList = inputNotechart.redNotes;
                        break;
                    case 2:
                        currentNoteList = inputNotechart.yellowNotes;
                        break;
                    case 3:
                        currentNoteList = inputNotechart.blueNotes;
                        break;
                    case 4:
                        currentNoteList = inputNotechart.orangeNotes;
                        break;
                    default:
                        break;
                }

                // This check is here due to the final increment after the last note.
                // Once the last note passes (these two are equal), we get a out of bounds error.
                if (!(inputNoteIterators[currentNoteset] == currentNoteList.Count))
                {
                    // If the current tick > the next note to be drawn...
                    if (currentMsec >= currentNoteList[inputNoteIterators[currentNoteset]].TimeValue)
                    {
                        // Look for a spot to draw the note
                        for (int i = 0; i < physicalNotes.GetLength(1); i++)
                        {
                            if (physicalNotes[currentNoteset, i].alive == false)
                            {
                                // And draw it
                                physicalNotes[currentNoteset, i].alive = true;
                                float new_note_pos = 196 + (noteSize * currentNoteset);
                                physicalNotes[currentNoteset, i].position = new Vector2(new_note_pos, 0f);
                                break;
                            }
                        }
                        inputNoteIterators[currentNoteset]++;
                    }
                }

                // Update the living note's positions and kill notes that leave the screen
                for (int i = 0; i < physicalNotes.GetLength(1); i++)
                {
                    if (physicalNotes[currentNoteset, i].alive == true)
                    {
                        physicalNotes[currentNoteset, i].position += new Vector2(0.0f, (float)(currTime.ElapsedGameTime.TotalMilliseconds * noteVelocity));
                    }

                    //// Kill note if it passes the hit line and EXPLODE
                    //// Adjust the y value by a magic number to give it a slightly earlier hit
                    //if ((physicalNotes[currentNoteset, i].position.Y > (hitBarYValue - 12)) &&
                    //    (physicalNotes[currentNoteset, i].alive == true))
                    //{
                    //    noteParticleExplosionEmitters.emitterList[currentNoteset].Trigger(noteParticleExplosionEmitters.explosionLocations[currentNoteset]);
                    //    physicalNotes[currentNoteset, i].alive = false;
                    //}

                    // Kill any notes that managed to get past the previous check and left the screen
                    if (!viewportRectangle.Contains(new Point((int)physicalNotes[currentNoteset, i].position.X,
                            (int)physicalNotes[currentNoteset, i].position.Y)))
                    {
                        physicalNotes[currentNoteset, i].alive = false;
                    }
                }
            }
        }
    }
}
