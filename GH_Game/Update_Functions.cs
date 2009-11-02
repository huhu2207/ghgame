using System.Collections.Generic;
using Microsoft.Xna.Framework;
using GH_Game.Chart;

namespace Chart_View
{
    class Misc_Functions
    {
        // Updates the position of viewable notes and creates/destroys notes when necessary
        public static void Update_Notes(double current_tick, List<Note> curr_chart,
                                        int note_num, ref int iterator_value,
                                        ref gameObject[,] Notes, Rectangle viewportRectangle,
                                        GameTime curr_time, double note_velocity, int note_size)
        {
            // This check is here due to the final increment after the last note.
            // Once the last note passes (these two are equal), we get a out of bounds error.
            if (!(iterator_value == curr_chart.Count))
            {
                // If the current tick > the next note to be drawn...
                if (current_tick >= curr_chart[iterator_value].TickValue)
                {
                    // Look for a spot to draw the note
                    for (int i = 0; i < Notes.GetLength(1); i++)
                    {
                        if (Notes[note_num, i].alive == false)
                        {
                            // And draw it
                            Notes[note_num, i].alive = true;
                            float new_note_pos = 196 + (note_size * note_num);
                            Notes[note_num, i].position = new Vector2(new_note_pos, 0f);
                            break;
                        }
                    }
                    iterator_value++;
                }
            }

            // Update the living note's positions and kill notes that leave the screen
            for (int i = 0; i < Notes.GetLength(1); i++)
            {
                if (Notes[note_num, i].alive == true)
                    Notes[note_num, i].position += new Vector2(0.0f, (float)(curr_time.ElapsedGameTime.TotalMilliseconds / note_velocity));

                if (!viewportRectangle.Contains(new Point((int)Notes[note_num, i].position.X,
                        (int)Notes[note_num, i].position.Y)))
                    Notes[note_num, i].alive = false;
            }
        }

        // Updates the position of viewable notes and creates/destroys notes when necessary
        public static void SUpdate_Notes(List<Note> curr_chart,
                                        int note_num, ref int iterator_value,
                                        ref gameObject[,] Notes, Rectangle viewportRectangle,
                                        GameTime curr_time, double note_velocity, int note_size, uint currentMsec)
        {
            // This check is here due to the final increment after the last note.
            // Once the last note passes (these two are equal), we get a out of bounds error.
            if (!(iterator_value == curr_chart.Count))
            {
                // If the current tick > the next note to be drawn...
                if (currentMsec >= curr_chart[iterator_value].TimeValue)
                {
                    // Look for a spot to draw the note
                    for (int i = 0; i < Notes.GetLength(1); i++)
                    {
                        if (Notes[note_num, i].alive == false)
                        {
                            // And draw it
                            Notes[note_num, i].alive = true;
                            float new_note_pos = 196 + (note_size * note_num);
                            Notes[note_num, i].position = new Vector2(new_note_pos, 0f);
                            break;
                        }
                    }
                    iterator_value++;
                }
            }

            // Update the living note's positions and kill notes that leave the screen
            for (int i = 0; i < Notes.GetLength(1); i++)
            {
                if (Notes[note_num, i].alive == true)
                    Notes[note_num, i].position += new Vector2(0.0f, (float)(curr_time.ElapsedGameTime.TotalMilliseconds / note_velocity));

                if (!viewportRectangle.Contains(new Point((int)Notes[note_num, i].position.X,
                        (int)Notes[note_num, i].position.Y)))
                    Notes[note_num, i].alive = false;
            }
        }

        // Updates the current ticks per milisecond (tpms) if necessary (the new value is returned
        public static void Update_TPMS(double current_tick, ref int iterator_value,
                                       List<BPMChange> BPM_Changes, GameTime gameTime,
                                       ref double current_tpms)
        {
            if (!(iterator_value == BPM_Changes.Count))
            {
                // Update the tpms every "update frame" in case the elapsed gametime is not 16 (running slow)
                // and assign the tpms in accordance to the current bpm and elapsed gametime
                current_tpms = ((BPM_Changes[iterator_value].BPMValue * 192.0) / 60000000.0) * gameTime.ElapsedGameTime.TotalMilliseconds;

                // IF the current bpm change is not the last, then increment the iterator
                // (count is not zero based, and must be decremented by 1)
                if (iterator_value < (BPM_Changes.Count - 1))
                {
                    if ((current_tick >= BPM_Changes[iterator_value + 1].TickValue))
                        iterator_value++;
                }
            }
        }
    }
}
