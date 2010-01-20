using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Toub.Sound.Midi;

namespace MinGH.ChartImpl
{
	/// <sumamry>
	/// A manager class that will read and store all BPM changes in a chart in an
	/// organized manner.
	/// </summary>
    class ChartBPMManager
    {
        /// <summary>
        /// Opens a specified chart and reads in all the valid BPM changes 
        /// (i.e. 23232 = B 162224) and returns a populated list.
        /// </summary>
        /// <param name="inputFile">
        /// The whole *.chart file stored in one massive string.
        /// </param>
        /// <returns>
        /// A list containing every valid BPM change from the chart.  Due to the nature
        /// of the *.chart specification, these BPM changes will be in proper order.
        /// </returns>
        public static List<BPMChange> AddBPMChangesFromChart(string inputFile)
        {
            List<BPMChange> BPMChangeListToReturn = new List<BPMChange>();

            // Single out the BPM section via regular expressions
            string pattern = Regex.Escape("[") + "SyncTrack]\\s*" + Regex.Escape("{") + "[^}]*";
            Match matched_section = Regex.Match(inputFile, pattern);

            // Create the stream from the singled out section of the input string
            StringReader pattern_stream = new StringReader(matched_section.ToString());
            string current_line = "";
            string[] parsed_line;

            while ((current_line = pattern_stream.ReadLine()) != null)
            {
                // Trim and split the line to retrieve information
                current_line = current_line.Trim();
                parsed_line = current_line.Split(' ');

                // If a valid change is found, add it to the list
                if (parsed_line.Length == 4)
                {
                    if (parsed_line[2] == "B")
                        BPMChangeListToReturn.Add(new BPMChange(Convert.ToUInt32(parsed_line[0]), Convert.ToUInt32(parsed_line[3])));
                }
            }

            // Close the string stream
            pattern_stream.Close();

            return BPMChangeListToReturn;
        }

        /// <summary>
        /// Generates a list of BPM changes from a midi source.
        /// </summary>
        /// <param name="midiFilePath">The directory in which the midi is held.</param>
        /// <returns>A filled out list of BPM changes.</returns>
        public static List<BPMChange> AddBPMChangesFromMidi(string midiFilePath)
        {
            List<BPMChange> listToReturn = new List<BPMChange>();
            MidiSequence mySequence = MidiSequence.Import(midiFilePath + "\\notes.mid");
            MidiTrack[] myTracks = mySequence.GetTracks();
            MidiTrack trackToUse = new MidiTrack();
            uint currTickValue = 0;

            // Go through each event in the first track (which contains the BPM changes)
            // and parse the resulting string.
            for (int i = 0; i < myTracks[0].Events.Count; i++)
            {
                MidiEvent currEvent = myTracks[0].Events[i];
                string eventString = currEvent.ToString();
                string[] splitEventString = eventString.Split('\t');

                // Since ticks are stored relative to each other (e.g. 300 ticks
                // until next note), we must maintain the total tick amout.
                currTickValue += Convert.ToUInt32(splitEventString[1]);
                if (splitEventString[0] == "Tempo")
                {
                    // In midi files, bpm chages are stored as "microseconds per quarter note"
                    // and must be converted to BPM, and then into the non decimal format the game
                    // uses.
                    double currBPMDouble = 60000000 / Convert.ToDouble(splitEventString[3]);
                    uint BPMToAdd = (uint)(currBPMDouble * 1000);
                    listToReturn.Add(new BPMChange(currTickValue, BPMToAdd));
                }
            }

            return listToReturn;
        }
    }
}
