using System.Collections.Generic;

namespace MinGH.ChartImpl
{
	/// <remarks>
	/// A manager class that calculates the milisecond value for each note within a notechart.
	/// </remarks>
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
        /// <param name="chartOffset">
        /// The general offset of the chart in seconds.  This value will be first converted to flat miliseconds
        /// and then added to the current milisecond.  This effectively adds the offset to every note and event.
        /// See the ChartInfo class for more information on the offset.
        /// </param>
        /// <param name="chartLengthMiliseconds">
        /// This is the total length of the chart as a whole.  This value also needs to be returned,
        /// so it is passed by refrenced.
        /// </param>
        /// <returns>
        /// A notechart that is the same as the input notechart, but every note has a milisecond value filled out.
        /// </returns>
        public Notechart GenerateTimeValues(Notechart inputNotechart, List<BPMChange> inputBPMChanges,
                                           ref List<Event> inputEvents, float chartOffset, ref uint chartLengthMiliseconds)
        {
            double currentTick = 0.0;
            double currentTicksPerMilisecond = 0.0;
            uint currentMilisecond = (uint)(chartOffset * 1000);  // Convert the chart offset into flat miliseconds

            int[] notechartIterators = {0, 0, 0, 0, 0, 0};
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

                // Update the notes themselves (have to specifiy each note set)
                if (notechartIterators[0] < inputNotechart.greenNotes.Count)
                {
                    if (currentTick >= inputNotechart.greenNotes[notechartIterators[0]].TickValue)
                    {
                        inputNotechart.greenNotes[notechartIterators[0]].TimeValue = currentMilisecond;
                        notechartIterators[0]++;
                    }
                }
                else
                {
                    endofChartCondition.noMoreGreenNotes = true;
                }

                // Update the notes themselves (have to specifiy each note set)
                if (notechartIterators[1] < inputNotechart.redNotes.Count)
                {
                    if (currentTick >= inputNotechart.redNotes[notechartIterators[1]].TickValue)
                    {
                        inputNotechart.redNotes[notechartIterators[1]].TimeValue = currentMilisecond;
                        notechartIterators[1]++;
                    }
                }
                else
                {
                    endofChartCondition.noMoreRedNotes = true;
                }

                // Update the notes themselves (have to specifiy each note set)
                if (notechartIterators[2] < inputNotechart.yellowNotes.Count)
                {
                    if (currentTick >= inputNotechart.yellowNotes[notechartIterators[2]].TickValue)
                    {
                        inputNotechart.yellowNotes[notechartIterators[2]].TimeValue = currentMilisecond;
                        notechartIterators[2]++;
                    }
                }
                else
                {
                    endofChartCondition.noMoreYellowNotes = true;
                }

                // Update the notes themselves (have to specifiy each note set)
                if (notechartIterators[3] < inputNotechart.blueNotes.Count)
                {
                    if (currentTick >= inputNotechart.blueNotes[notechartIterators[3]].TickValue)
                    {
                        inputNotechart.blueNotes[notechartIterators[3]].TimeValue = currentMilisecond;
                        notechartIterators[3]++;
                    }
                }
                else
                {
                    endofChartCondition.noMoreBlueNotes = true;
                }

                // Update the notes themselves (have to specifiy each note set)
                if (notechartIterators[4] < inputNotechart.orangeNotes.Count)
                {
                    if (currentTick >= inputNotechart.orangeNotes[notechartIterators[4]].TickValue)
                    {
                        inputNotechart.orangeNotes[notechartIterators[4]].TimeValue = currentMilisecond;
                        notechartIterators[4]++;
                    }
                }
                else
                {
                    endofChartCondition.noMoreOrangeNotes = true;
                }

                // Update the notes themselves (have to specifiy each note set)
                if (notechartIterators[5] < inputNotechart.SPNotes.Count)
                {
                    if (currentTick >= inputNotechart.SPNotes[notechartIterators[5]].TickValue)
                    {
                        inputNotechart.SPNotes[notechartIterators[5]].TimeValue = currentMilisecond;
                        notechartIterators[5]++;
                    }
                }
                else
                {
                    endofChartCondition.noMoreSPNotes = true;
                }

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
            chartLengthMiliseconds = currentMilisecond;
            return noteChartToReturn;
        }
    }
}
