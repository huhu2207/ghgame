using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace Chart_View
{
    class Misc_Functions
    {
        // Updates the position of viewable notes and creates/destroys notes when necessary
        public static void Update_Notes(double current_tick, List<Note> curr_chart, int[] note_iterators,
                                        int iterator_num, gameObject[] Notes)
        {
            // Comparison between the current tick and the next notes
            if (current_tick > curr_chart[note_iterators[iterator_num]].Location)
            {
                // Draw the green IF there is a green to draw (should never fail)
                if (note_iterators[iterator_num] < (curr_chart.Count - 1))
                {
                    // Search for a spot for the new note
                    for (int j = 0; j < Notes.Length; j++)
                    {
                        if (Notes[j].alive == false)
                        {
                            Notes[j].alive = true;
                            Notes[j].position = new Vector2(300f, 0f);
                        }
                    }
                    note_iterators[iterator_num]++;
                }
            }
        }
    }
}
