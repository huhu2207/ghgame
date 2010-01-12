using System.Collections.Generic;

namespace MinGH.ChartImpl
{
	/// <summary>
	/// A manager class that calculates the milisecond value for each note within a notechart.
	/// </summary>
    class ChartTimeValueManager
    {
        /// <summary>
        /// Simulates a chart playthrough and adds a milisecond value to each note and event within the
        /// current notechart. This is necessary because the *.chart file specification does not
        /// provide the user a specific time value.  It works by calulating how many ticks are to pass
        /// for every milisecond in relation to the current BPM (see formula below), and adds that to a
        /// total tick value.  Every iteration, 1 is added to the current milisecond.  When a note or event's
        /// tick value becomes less than the total tick value, its milisecond value is set to the current
        /// milisecond. 
        /// </summary>
        /// <param name="inputNotechart">
        /// The notechart that will be scanned.  A pass by refrence may be more efficent...
        /// </param>
        /// <param name="inputBPMChanges">
        /// The list of BPM changes that apply to the notechart.
        /// </param>
        /// <param name="inputEvents">
        /// The event list that will be scanned.  This list also gets its time values calculated, so a pass
        /// by refrence is necrssary.
        /// </param>
        /// <param name="chartInfo">
        /// The information (particularly the offset and milisecond chart length) of the chart.
        /// </param>
        /// <returns>
        /// A notechart that is the same as the input notechart, but every note has a milisecond value filled out.
        /// </returns>
        public static Notechart GenerateTimeValues(Notechart inputNotechart, List<BPMChange> inputBPMChanges,
                                           List<Event> inputEvents, ChartInfo chartInfo)
        {
            double currentTick = 0.0;
            double currentTicksPerMilisecond = 0.0;
            uint currentMilisecond = (uint)(chartInfo.offset * 1000);  // Convert the chart offset into flat miliseconds

            int notechartIterator = 0;
            int SPNoteIterator = 0;
            int eventIterator = 0;
            int BPMChangeIterator = 0;

            EndofChartCondition endofChartCondition = new EndofChartCondition();
            Notechart noteChartToReturn = inputNotechart;

            // Keep working until no more events or notes are found
            while (endofChartCondition)
            {
                // Update the event time values
                if (eventIterator < inputEvents.Count)
                {
                    if (currentTick >= inputEvents[eventIterator].TickValue)
                    {
                        inputEvents[eventIterator].TimeValue = currentMilisecond;
                        eventIterator++;
                    }
                }
                else
                {
                    endofChartCondition.noMoreEvents = true;
                }

                // Update the notes themselves
                if (notechartIterator < inputNotechart.notes.Count)
                {
                    while ((notechartIterator < inputNotechart.notes.Count) && (currentTick >= inputNotechart.notes[notechartIterator].TickValue))
                    {
                        inputNotechart.notes[notechartIterator].TimeValue = currentMilisecond;
                        notechartIterator++;
                    }
                }
                else
                {
                    endofChartCondition.noMoreNotes = true;
                }

                // Update the Star Power notes
                if (SPNoteIterator < inputNotechart.SPNotes.Count)
                {
                    if (currentTick >= inputNotechart.SPNotes[SPNoteIterator].TickValue)
                    {
                        inputNotechart.SPNotes[SPNoteIterator].TimeValue = currentMilisecond;
                        SPNoteIterator++;
                    }
                }
                else
                {
                    endofChartCondition.noMoreSPNotes = true;
                }

                // Update the BPM changes
                if (!(BPMChangeIterator == inputBPMChanges.Count))
                {
                    // -TODO: 192 is an assumed magic number.  Edit Chart class so it contains the resolution of the
                    //        chart (192 is the standard, but others exist).
                    currentTicksPerMilisecond = ((inputBPMChanges[BPMChangeIterator].BPMValue * 192.0) / 60000000.0);

                    // IF the current bpm change is not the last, then increment the iterator
                    // (count is not zero based, and must be decremented by 1)
                    if (BPMChangeIterator < (inputBPMChanges.Count - 1))
                    {
                        if ((currentTick >= inputBPMChanges[BPMChangeIterator + 1].TickValue))
                        {
                            BPMChangeIterator++;
                        }
                    }
                }

                currentTick += currentTicksPerMilisecond;
                currentMilisecond++;
            }
            chartInfo.chartLengthMiliseconds = currentMilisecond;
            return noteChartToReturn;
        }
    }
}
