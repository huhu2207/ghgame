using System;
using System.IO;
using System.Text.RegularExpressions;
using Toub.Sound.Midi;
using MinGH.GameScreen;

namespace MinGH.ChartImpl
{
    /// <summary>
    /// A manager class that reads in all notes from a specific notechart within a *.chart file.
    /// </summary>
    class ChartNotechartManager
    {
        /// <summary>
        /// Creates a notechart from the specified file and the actual charttype
        /// (i.e. ExpertSingle from Freebird.chart)
        /// </summary>
        /// <param name="chartname">
        /// The specific notechart to be taken from the *.chart file (i.e. ExpertSingle).
        /// </param>
        /// <param name="inputString">
        /// The whole *.chart file stored in one massive string.
        /// </param>
        /// <returns>
        /// A filled out Notechart containing the needed information from the *.chart file
        /// </returns>
        public static Notechart GenerateNotechartFromChart(ChartSelection chartSelection)
        {
            string chartname = chartSelection.difficulty + chartSelection.instrument;
            StreamReader inputStream = new StreamReader(chartSelection.chartPath);
            string inputString = inputStream.ReadToEnd();

            // Single out the specified section via regular expressions
            string pattern = Regex.Escape("[") + chartname + "]\\s*" + Regex.Escape("{") + "[^}]*";
            Match matched_section = Regex.Match(inputString, pattern);

            // Create the stream from the singled out section of the input string
            StringReader pattern_stream = new StringReader(matched_section.ToString());
            string current_line = "";
            string[] parsed_line;

            // Create the resulting notechart and prep for input
            Notechart notechartToReturn = new Notechart(chartSelection.difficulty, chartSelection.instrument);

            //If specific notechart is not found, return a generic one
            if (!(matched_section.Success))
            {
                notechartToReturn.notes.Add(new ChartNote());
            }

            // Else, read in all the chart information
            else
            {
                int i = 0;
                while ((current_line = pattern_stream.ReadLine()) != null)
                {
                    // Trim and split the line to retrieve information
                    current_line = current_line.Trim();
                    parsed_line = current_line.Split(' ');

                    // If a valid note is found, add it to the list
                    if (parsed_line.Length == 5)
                    {
                        if (parsed_line[2] == "N")
                        {
                            if ((notechartToReturn.notes.Count > 0) &&
                                (Convert.ToUInt32(parsed_line[0]) == notechartToReturn.notes[notechartToReturn.notes.Count - 1].TickValue))
                            {
                                switch (Convert.ToInt32(parsed_line[3]))
                                {
                                    case 0:
                                        notechartToReturn.notes[notechartToReturn.notes.Count - 1].addNote(0);
                                        break;

                                    case 1:
                                        notechartToReturn.notes[notechartToReturn.notes.Count - 1].addNote(1);
                                        break;

                                    case 2:
                                        notechartToReturn.notes[notechartToReturn.notes.Count - 1].addNote(2);
                                        break;

                                    case 3:
                                        notechartToReturn.notes[notechartToReturn.notes.Count - 1].addNote(3);
                                        break;

                                    case 4:
                                        notechartToReturn.notes[notechartToReturn.notes.Count - 1].addNote(4);
                                        break;

                                    default:
                                        Console.WriteLine("ERROR: Invalid Note Detcted.  Skipping...");
                                        break;
                                }
                            }
                            else
                            {
                                // Find out which note the current line is, and add it to the respective list
                                switch (Convert.ToInt32(parsed_line[3]))
                                {
                                    case 0:
                                        notechartToReturn.notes.Add(new ChartNote(Convert.ToUInt32(parsed_line[0]),
                                                                                  Convert.ToInt32(parsed_line[4]),
                                                                                  0));
                                        break;

                                    case 1:
                                        notechartToReturn.notes.Add(new ChartNote(Convert.ToUInt32(parsed_line[0]),
                                                                                  Convert.ToInt32(parsed_line[4]),
                                                                                  1));
                                        break;

                                    case 2:
                                        notechartToReturn.notes.Add(new ChartNote(Convert.ToUInt32(parsed_line[0]),
                                                                                  Convert.ToInt32(parsed_line[4]),
                                                                                  2));
                                        break;

                                    case 3:
                                        notechartToReturn.notes.Add(new ChartNote(Convert.ToUInt32(parsed_line[0]),
                                                                                  Convert.ToInt32(parsed_line[4]),
                                                                                  3));
                                        break;

                                    case 4:
                                        notechartToReturn.notes.Add(new ChartNote(Convert.ToUInt32(parsed_line[0]),
                                                                                  Convert.ToInt32(parsed_line[4]),
                                                                                  4));
                                        break;

                                    default:
                                        Console.WriteLine("ERROR: Invalid Note Detcted.  Skipping...");
                                        break;
                                }
                            }
                        }
                        // Also check for SP notes
                        else if (parsed_line[2] == "S")
                            notechartToReturn.SPNotes.Add(new ChartNote(Convert.ToUInt32(parsed_line[0]),
                                                                        Convert.ToInt32(parsed_line[4]),
                                                                        5));
                    }
                    i++;
                }
            }

            // Close the string stream
            pattern_stream.Close();
            return notechartToReturn;
        }

        /// <summary>
        /// Creates a notechart from the specified midi path and the actual charttype
        /// (i.e. ExpertSingle from notes.mid)
        /// </summary>
        /// <param name="chartName">
        /// The specific notechart to be taken from the *.chart file (i.e. ExpertSingle).
        /// </param>
        /// <param name="midiFilePath">
        /// The path to the midi file being used.
        /// </param>
        /// <param name="chartInfo">
        /// The info on the chart.  This is passed since some information can only
        /// be obtained via the midi file.
        /// </param>
        /// <returns>
        /// A filled out Notechart containing the needed information from the *.mid file.
        /// </returns>
        public static Notechart GenerateNotechartFromMidi(ChartSelection chartSelection)
        {
            Notechart notechartToReturn = new Notechart();
            notechartToReturn.instrument = chartSelection.instrument;
            notechartToReturn.difficulty = chartSelection.difficulty;

            // The following two switch's are used to get the proper midi terminology for
            // the selected track and difficulty.
            string instrumentPart = null;
            string greenKey = null;
            string redKey = null;
            string yellowKey = null;
            string blueKey = null;
            string orangeKey = null;

            switch (chartSelection.instrument)
            {
                case "Single":
                    instrumentPart = "PART GUITAR";
                    break;
                case "DoubleBass":
                    instrumentPart = "PART BASS";
                    break;
                case "Drums":
                    instrumentPart = "PART DRUMS";
                    break;
                default:
                    instrumentPart = "PART GUITAR";
                    break;
            }

            switch (chartSelection.difficulty)
            {
                case "Expert":
                    greenKey = "C8";
                    redKey = "C#8";
                    yellowKey = "D8";
                    blueKey = "D#8";
                    orangeKey = "E8";
                    break;
                case "Hard":
                    greenKey = "C7";
                    redKey = "C#7";
                    yellowKey = "D7";
                    blueKey = "D#7";
                    orangeKey = "E7";
                    break;
                case "Medium":
                    greenKey = "C6";
                    redKey = "C#6";
                    yellowKey = "D6";
                    blueKey = "D#6";
                    orangeKey = "E6";
                    break;
                case "Easy":
                    greenKey = "C5";
                    redKey = "C#5";
                    yellowKey = "D5";
                    blueKey = "D#5";
                    orangeKey = "E5";
                    break;
                default:
                    greenKey = "C8";
                    redKey = "C#8";
                    yellowKey = "D8";
                    blueKey = "D#8";
                    orangeKey = "E8";
                    break;
            }

            MidiSequence mySequence = MidiSequence.Import(chartSelection.directory + "\\notes.mid");
            MidiTrack[] myTracks = mySequence.GetTracks();
            MidiTrack trackToUse = new MidiTrack();

            // Find the specified instrument's track
            foreach (MidiTrack currTrack in myTracks)
            {
                string trackHeader = currTrack.Events[0].ToString();
                string[] splitHeader = trackHeader.Split('\t');

                if (splitHeader[3] == instrumentPart)
                {
                    trackToUse = currTrack;
                }
            }

            uint totalTickValue = 0;
            uint currTickValue = 0;
            ChartNote currNote = new ChartNote();
            bool blankNote = true;
            // Scan through and record every note specific to the selected difficulty
            for (int i = 0; i < trackToUse.Events.Count; i++)
            {
                string currEvent = trackToUse.Events[i].ToString();
                string[] splitEvent = currEvent.Split('\t');
                currTickValue = Convert.ToUInt32(splitEvent[1]);
                totalTickValue += currTickValue;

                // We need to specify wether a note is blank or not so we don't add
                // blank notes from other difficulties into the chart, but if we have
                // a filled out note, any nonzero tick value means we are moving to a
                // new note, so we must cut our ties and add this note to the chart.
                if ((currTickValue != 0) && !blankNote)
                {
                    notechartToReturn.notes.Add(currNote);
                    currNote = new ChartNote();
                    blankNote = true;
                }

                // The "0x64" I think means "not was hit."  There is another
                // set of notes that use "0x00" that all appear slightly after
                // the "0x64" notes.
                if ((splitEvent[0] == "NoteOn") && (splitEvent[4] == "0x64"))
                {
                    // Only consider notes within the octave our difficulty is in.
                    if ((splitEvent[3] == greenKey) || (splitEvent[3] == redKey) ||
                        (splitEvent[3] == yellowKey) || (splitEvent[3] == blueKey) ||
                        (splitEvent[3] == orangeKey))
                    {
                        // If it's a new note, we need to setup the tick value of it.
                        if (blankNote)
                        {
                            currNote.TickValue = totalTickValue;
                            blankNote = false;
                        }
                        if (splitEvent[3] == greenKey) { currNote.addNote(0); }
                        else if (splitEvent[3] == redKey) { currNote.addNote(1); }
                        else if (splitEvent[3] == yellowKey) { currNote.addNote(2); }
                        else if (splitEvent[3] == blueKey) { currNote.addNote(3); }
                        else if (splitEvent[3] == orangeKey) { currNote.addNote(4); }
                    }
                }
            }

            return notechartToReturn;
        }
    }
}
