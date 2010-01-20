using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using MinGH.Extensions;
using Toub.Sound.Midi;

namespace MinGH.ChartImpl
{
	/// <summary>
	/// A manager class that will read and store all Events in a chart in an
	/// organized manner.
	/// </summary>
    class ChartEventManager
    {
        /// <summary>
        /// Opens a specified chart and reads in all the valid events 
        /// (i.e 14208 = E "section Verse 1a") and returns a populated list.
        /// </summary>
        /// <param name="input_string">
        /// The whole *.chart file stored in one massive string.
        /// </param>
        /// <returns>
        /// A list containing every valid event from the chart.  Due to the nature
        /// of the *.chart specification, these events will be in proper order.
        /// </returns>
        public static List<ChartEvent> AddEventsFromChart(string input_string)
        {
            List<ChartEvent> eventListToReturn = new List<ChartEvent>();

            // Single out the event section via regular expressions
            string pattern = Regex.Escape("[") + "Events]\\s*" + Regex.Escape("{") + "[^}]*";
            Match matched_section = Regex.Match(input_string, pattern);

            // Create the stream from the singled out section of the input string
            StringReader pattern_stream = new StringReader(matched_section.ToString());
            string current_line = "";
            string[] parsed_line;

            while ((current_line = pattern_stream.ReadLine()) != null)
            {
                // Trim and split the line to retrieve information
                current_line = current_line.Trim();
                parsed_line = current_line.Split(' ');

                // If a valid event is found, add it to the list
                if (parsed_line.Length >= 4)
                {
                    if (parsed_line[2] == "E")
                    {
                        eventListToReturn.Add(new ChartEvent(Convert.ToUInt32(parsed_line[0]),
                                              ProperStringCreator.createProperString(parsed_line.SubArray(3, parsed_line.Length))));
                    }
                }
            }

            // Close the string stream
            pattern_stream.Close();

            return eventListToReturn;
        }

        public static List<ChartEvent> AddEventsFromMidi(string midiFilePath, ChartInfo chartInfo)
        {
            List<ChartEvent> listToReturn = new List<ChartEvent>();
            MidiSequence mySequence = MidiSequence.Import(midiFilePath + "\\notes.mid");
            chartInfo.resolution = mySequence.Division;
            MidiTrack[] myTracks = mySequence.GetTracks();
            MidiTrack trackToUse = new MidiTrack();

            // Find the specified instrument's track
            foreach (MidiTrack currTrack in myTracks)
            {
                string trackHeader = currTrack.Events[0].ToString();
                string[] splitHeader = trackHeader.Split('\t');

                if (splitHeader[3] == "EVENTS")
                {
                    trackToUse = currTrack;
                }
            }

            uint currTickValue = 0;

            // Go through each event in the first track (which contains the BPM changes)
            // and parse the resulting string.
            for (int i = 0; i < trackToUse.Events.Count; i++)
            {
                MidiEvent currEvent = trackToUse.Events[i];
                string eventString = currEvent.ToString();
                string[] splitEventString = eventString.Split('\t');

                // Since ticks are stored relative to each other (e.g. 300 ticks
                // until next note), we must maintain the total tick amout.
                currTickValue += Convert.ToUInt32(splitEventString[1]);
                if (splitEventString[0] == "Tempo")
                {
                    if (splitEventString[3] == "[end]")
                    {
                        // TODO: Use this with the time manager to get the REAL end
                    }
                    //listToReturn.Add(new BPMChange(currTickValue, BPMToAdd));
                }
            }

            return listToReturn;
        }
    }
}
