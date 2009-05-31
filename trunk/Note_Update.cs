using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Chart_View
{
    class Misc_Functions
    {
        // Updates the position of viewable notes and creates/destroys notes when necessary
        public static void Update_Notes(ref double current_tick, ref List<Note> curr_chart,
                                        int note_num, ref int iterator_value,
                                        ref gameObject[] Notes)
        {
            // This check is here due to the final increment after the last note.
            // Once the last note passes (these two are equal), we get a out of bounds error.
            if (!(iterator_value == curr_chart.Count))
            {
                // Comparison between the current tick and the next notes
                if (current_tick >= curr_chart[iterator_value].Location)
                {
                    // Search for a spot for the new note
                    for (int j = 0; j < Notes.Length; j++)
                    {
                        if (Notes[j].alive == false)
                        {
                            Notes[j].alive = true;

                            // note_num is passed so checking which chart we are really looking at 
                            switch (note_num)
                            {
                                case 0:
                                    Notes[j].position = new Vector2(200f, 0f);
                                    break;
                                case 1:
                                    Notes[j].position = new Vector2(275f, 0f);
                                    break;
                                case 2:
                                    Notes[j].position = new Vector2(350f, 0f);
                                    break;
                                case 3:
                                    Notes[j].position = new Vector2(425f, 0f);
                                    break;
                                case 4:
                                    Notes[j].position = new Vector2(500f, 0f);
                                    break;
                            }
                            break;
                        }
                    }
                    iterator_value++;
                }
            }
        }
    }
}
