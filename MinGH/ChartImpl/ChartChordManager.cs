using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MinGH.ChartImpl
{
    class ChartChordManager
    {
        /// <summary>
        /// Scans therough the input notechart and assigns notes that have the same
        /// tick value as the note after it as a chord note.  These will not ever be
        /// hammer ons and can only be hit when all notes on a chord are hit at once.
        /// </summary>
        /// <param name="inputNotechart">
        /// Any notechart (expected to not have chord information already filled out).
        /// </param>
        /// <returns>
        /// The same notechart with proper note chord settings.
        /// </returns>
        public static Notechart AssignChords(Notechart inputNotechart)
        {
            Notechart notechartToReturn = inputNotechart;

            ChartNote currentNote = new ChartNote();
            ChartNote nextNote = new ChartNote();

            // We stop at (count - 1) due to the currentNote/nextNote setup
            for (int i = 0; i < inputNotechart.notes.Count - 1; i++)
            {
                currentNote = inputNotechart.notes[i];
                nextNote = inputNotechart.notes[i + 1];

                // If difference is 0, it is a chord.
                if ((nextNote.TickValue - currentNote.TickValue) == 0)
                {
                    notechartToReturn.notes[i].isChord = true;
                    notechartToReturn.notes[i + 1].isChord = true;
                }
            }

            return notechartToReturn;
        }
    }
}
