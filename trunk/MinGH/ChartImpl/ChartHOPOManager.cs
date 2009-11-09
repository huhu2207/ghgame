using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MinGH.ChartImpl
{
    /// <remarks>
    /// Manages the hammeron/pulloff and chord states of each note within a notechart.
    /// </remarks>
    class ChartHOPOManager
    {
        /// <summary>
        /// Scans through a notechart and assigns the HOPO tag to notes that are of a
        /// different type than the previous note, and have a tick difference less than
        /// the specified HOPO tick threshold.
        /// </summary>
        /// <param name="inputNotechart">
        /// Any notechart (expected to not have hammeron information already filled out).
        /// </param>
        /// <returns>
        /// The same notechart with proper hammeron note settings.
        /// </returns>
        public static Notechart AssignHOPOS(Notechart inputNotechart, ChartInfo inputChartInfo)
        {
            Notechart notechartToReturn = inputNotechart;
            int HOPOTickThreshold = (inputChartInfo.resolution * 4) / inputChartInfo.HOPOThreshold;

            ChartNote currentNote = new ChartNote();
            ChartNote nextNote = new ChartNote();
            ChartNote thirdNote = new ChartNote();

            // We stop at (count - 2) due to the currentNote/nextNote/thirdNote setup
            for (int i = 0; i < inputNotechart.notes.Count - 2; i++)
            {
                currentNote = inputNotechart.notes[i];
                nextNote = inputNotechart.notes[i + 1];
                thirdNote = inputNotechart.notes[i + 2];

                // If difference is 0, it is a chord and should not be a hammeron.
                // We need to check the third note in case the next note is the start
                // of a chord.
                if (((nextNote.TickValue - currentNote.TickValue) <= HOPOTickThreshold) &&
                    ((nextNote.TickValue - currentNote.TickValue) != 0) &&
                    (nextNote.noteType != currentNote.noteType) &&
                    (nextNote.TickValue != thirdNote.TickValue))
                {
                    notechartToReturn.notes[i + 1].isHOPO = true;
                }
            }

            // With the 3 note logic above, the last note is never checked to be a hopo
            // so I explicitly check here.
            currentNote = inputNotechart.notes[inputNotechart.notes.Count - 2];
            nextNote = inputNotechart.notes[inputNotechart.notes.Count - 1];

            if (((nextNote.TickValue - currentNote.TickValue) <= HOPOTickThreshold) &&
                ((nextNote.TickValue - currentNote.TickValue) != 0) &&
                (nextNote.noteType != currentNote.noteType))
            {
                notechartToReturn.notes[inputNotechart.notes.Count - 1].isHOPO = true;
            }

            return notechartToReturn;
        }
    }
}
