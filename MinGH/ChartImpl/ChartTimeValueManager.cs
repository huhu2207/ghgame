using System.Collections.Generic;

namespace MinGH.ChartImpl
{
    class ChartTimeValueManager
    {
        // Generate the milisecond time values for each note in a notechart
        // by simulating a runthrough of the song using the tick and bpm values
        // -The ref uint is a last minute hack.
        public Notechart GenerateTimeValues(Notechart inputNotechart, List<BPMChange> inputBPMChanges,
                                           List<Event> inputEvents, float chartOffset, ref uint chartLengthMiliseconds)
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
