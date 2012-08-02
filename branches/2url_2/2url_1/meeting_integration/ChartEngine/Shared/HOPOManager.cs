namespace ChartEngine.Shared
{
    /// <summary>
    /// Manages the hammeron/pulloff and chord states of each note within a notechart.
    /// </summary>
    class HOPOManager
    {
        /// <summary>
        /// Scans through a notechart and assigns the HOPO tag to notes that are of a
        /// different type than the previous note, and have a tick difference less than
        /// the specified HOPO tick threshold.
        /// </summary>
        /// <param name="inputNotechart">
        /// Any notechart (expected to not have hammeron information already filled out).
        /// </param>
        /// <param name="inputChartInfo">
        /// All information pertaining to the chart.
        /// </param>
        /// <returns>
        /// The same notechart with proper hammeron note settings.
        /// </returns>
        public static Notes AssignHOPOS(Notes inputNotechart, Info inputChartInfo)
        {
            Notes notechartToReturn = inputNotechart;
            int HOPOTickThreshold = (inputChartInfo.resolution * 4) / inputChartInfo.HOPOThreshold;

            Note currentNote = new Note();
            Note nextNote = new Note();

            // We stop at (count - 2) due to the currentNote/nextNote/thirdNote setup
            for (int i = 0; i < inputNotechart.notes.Count - 1; i++)
            {
                currentNote = inputNotechart.notes[i];
                nextNote = inputNotechart.notes[i + 1];

                if (i == 470)
                {
                    nextNote.ToString();
                }

                // If difference is 0, it is a chord and should not be a hammeron.
                // We need to check the third note in case the next note is the start
                // of a chord.
                if (((nextNote.tickValue - currentNote.tickValue) <= HOPOTickThreshold) &&
                    (!nextNote.noteType.isEqual(currentNote.noteType)) &&
                    (!nextNote.isChord))
                {
                    notechartToReturn.notes[i + 1].isHOPO = true;
                }
            }

            return notechartToReturn;
        }
    }
}
