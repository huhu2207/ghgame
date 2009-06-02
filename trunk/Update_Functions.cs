using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Chart_View
{
    class Misc_Functions
    {
        // Updates the position of viewable notes and creates/destroys notes when necessary
        public static void Update_Notes(double current_tick, List<Note> curr_chart,
                                        int note_num, ref int iterator_value,
                                        ref gameObject[,] Notes)
        {
            // This check is here due to the final increment after the last note.
            // Once the last note passes (these two are equal), we get a out of bounds error.
            if (!(iterator_value == curr_chart.Count))
            {
                // If the current tick > the next note to be drawn...
                if (current_tick >= curr_chart[iterator_value].Location)
                {
                    // Look for a spot to draw the note
                    for (int i = 0; i < Notes.GetLength(1); i++)
                    {
                        if (Notes[note_num, i].alive == false)
                        {
                            // And draw it
                            Notes[note_num, i].alive = true;
                            float new_note_pos = 196 + (75 * note_num);
                            Notes[note_num, i].position = new Vector2(new_note_pos, 0f);
                            break;
                        }
                    }
                    iterator_value++;
                }
            }
        }

        // Updates the current ticks per milisecond (tpms) if necessary (the new value is returned
        public static void Update_TPMS(double current_tick, ref int iterator_value,
                                       List<BPM_Change> BPM_Changes, double game_time_msec,
                                       ref double current_tpms)
        {
            if (!(iterator_value == BPM_Changes.Count))
            {
                // Update the bpm every "update frame" in case the elapsed gametime is not 16 (running slow)
                // and assign the tpms in accordance to the current bpm and elapsed gametime
                current_tpms = ((BPM_Changes[iterator_value].Value / 6000000.0) * 192.0) * (game_time_msec / 10.0);

                // IF the current bpm change is not the last, then increment the iterator
                // (count is not zero based, and must be decremented by 1)
                if (iterator_value < (BPM_Changes.Count - 1))
                {
                    if ((current_tick >= BPM_Changes[iterator_value + 1].Location))
                        iterator_value++;
                }
            }
        }
    }
}
