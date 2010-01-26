using System;
using System.IO;
using System.Text.RegularExpressions;
using MinGH.GameScreen;
//using Toub.Sound.Midi;
using Sanford.Multimedia.Midi;

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
        /// <param name="chartSelection">
        /// The information on which particular notechart to use.
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
                notechartToReturn.notes.Add(new NotechartNote());
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
                                        notechartToReturn.notes.Add(new NotechartNote(Convert.ToUInt32(parsed_line[0]),
                                                                                  Convert.ToInt32(parsed_line[4]),
                                                                                  0));
                                        break;

                                    case 1:
                                        notechartToReturn.notes.Add(new NotechartNote(Convert.ToUInt32(parsed_line[0]),
                                                                                  Convert.ToInt32(parsed_line[4]),
                                                                                  1));
                                        break;

                                    case 2:
                                        notechartToReturn.notes.Add(new NotechartNote(Convert.ToUInt32(parsed_line[0]),
                                                                                  Convert.ToInt32(parsed_line[4]),
                                                                                  2));
                                        break;

                                    case 3:
                                        notechartToReturn.notes.Add(new NotechartNote(Convert.ToUInt32(parsed_line[0]),
                                                                                  Convert.ToInt32(parsed_line[4]),
                                                                                  3));
                                        break;

                                    case 4:
                                        notechartToReturn.notes.Add(new NotechartNote(Convert.ToUInt32(parsed_line[0]),
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
                            notechartToReturn.SPNotes.Add(new NotechartNote(Convert.ToUInt32(parsed_line[0]),
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
        /// (i.e. ExpertSingle from notes.mid).  Due to the overhead necessary to
        /// parse a midi file.  I am going to cram all midi->chart operations into
        /// one function call.
        /// </summary>
        /// <param name="chartSelection">
        /// The information on which particular notechart to use.
        /// </param>
        /// <returns>
        /// A filled out Notechart containing the needed information from the *.mid file.
        /// </returns>
        public static Notechart GenerateNotechartFromMidi(ChartSelection chartSelection,
                                                          ChartInfo chartInfo)
        {
            Notechart notechartToReturn = new Notechart();
            notechartToReturn.instrument = chartSelection.instrument;
            notechartToReturn.difficulty = chartSelection.difficulty;

            // The following two switch's are used to get the proper midi terminology for
            // the selected track and difficulty.
            string instrumentPart = null;
            int greenKey = 0;
            int redKey = 0;
            int yellowKey = 0;
            int blueKey = 0;
            int orangeKey = 0;

            switch (chartSelection.instrument)
            {
                case "Single":
                    instrumentPart = "PART GUITAR";
                    break;
                case "DoubleGuitar":
                    instrumentPart = "PART GUITAR COOP";
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
                    greenKey = 96;
                    redKey = 97;
                    yellowKey = 98;
                    blueKey = 99;
                    orangeKey = 100;
                    break;
                case "Hard":
                    greenKey = 84;
                    redKey = 85;
                    yellowKey = 86;
                    blueKey = 87;
                    orangeKey = 88;
                    break;
                case "Medium":
                    greenKey = 72;
                    redKey = 73;
                    yellowKey = 74;
                    blueKey = 75;
                    orangeKey = 76;
                    break;
                case "Easy":
                    greenKey = 60;
                    redKey = 61;
                    yellowKey = 62;
                    blueKey = 63;
                    orangeKey = 64;
                    break;
                default:
                    greenKey = 96;
                    redKey = 97;
                    yellowKey = 98;
                    blueKey = 99;
                    orangeKey = 100;
                    break;
            }

            Sequence mySequence = new Sequence(chartSelection.directory + "\\notes.mid");
            Track trackToUse = new Track();
            chartInfo.resolution = mySequence.Division;

            // Find the specified instrument's track
            for (int i = 0; i < mySequence.Count; i++)
            {
                Track sanTrack = mySequence[i];
                for (int j = 0; j < sanTrack.Count; j++)
                {
                    MidiEvent currEvent = sanTrack.GetMidiEvent(j);

                    if (currEvent.MidiMessage.MessageType == MessageType.Meta)
                    {
                        MetaMessage currMessage = currEvent.MidiMessage as MetaMessage;
                        if (currMessage.MetaType == MetaType.TrackName)
                        {
                            MetaTextBuilder trackName = new MetaTextBuilder(currMessage);

                            // -If we come across a "T1 GEMS" track, we're in GH1 territory.
                            // -GH2 has both PART BASS and PART RHYTHM (one or the other depending
                            //  on the chart).  This is the only game that has this as far as I know.
                            if ((trackName.Text == instrumentPart) || (trackName.Text == "T1 GEMS") ||
                                ((trackName.Text == "PART RHYTHM") && (instrumentPart == "PART BASS")))
                            {
                                trackToUse = sanTrack;
                            }
                        }
                    }
                }
            }

            NotechartNote currNote = new NotechartNote();
            bool blankNote = true;
            // Scan through and record every note specific to the selected difficulty
            for (int i = 0; i < trackToUse.Count; i++)
            {
                MidiEvent currEvent = trackToUse.GetMidiEvent(i);

                // We need to specify wether a note is blank or not so we don't add
                // blank notes from other difficulties into the chart, but if we have
                // a filled out note, any nonzero tick value means we are moving to a
                // new note, so we must cut our ties and add this note to the chart.
                if ((currEvent.DeltaTicks != 0) && !blankNote)
                {
                    notechartToReturn.notes.Add(currNote);
                    currNote = new NotechartNote();
                    blankNote = true;
                }

                if (currEvent.MidiMessage.MessageType == MessageType.Channel)
                {
                    ChannelMessage currMessage = currEvent.MidiMessage as ChannelMessage;
                    if (currMessage.Command == ChannelCommand.NoteOn)
                    {
                        // Only consider notes within the octave our difficulty is in.
                        if (((currMessage.Data1 == greenKey) || (currMessage.Data1 == redKey) ||
                            (currMessage.Data1 == yellowKey) || (currMessage.Data1 == blueKey) ||
                            (currMessage.Data1 == orangeKey)) && (currMessage.Data2 != 0))
                        {
                            // If it's a new note, we need to setup the tick value of it.
                            if (blankNote)
                            {
                                //currNote.TickValue = totalTickValue;
                                currNote.TickValue = (uint)currEvent.AbsoluteTicks;
                                blankNote = false;
                            }
                            if (currMessage.Data1 == greenKey) { currNote.addNote(0); }
                            else if (currMessage.Data1 == redKey) { currNote.addNote(1); }
                            else if (currMessage.Data1 == yellowKey) { currNote.addNote(2); }
                            else if (currMessage.Data1 == blueKey) { currNote.addNote(3); }
                            else if (currMessage.Data1 == orangeKey) { currNote.addNote(4); }
                        }
                    }

                }
            }

            return notechartToReturn;
        }
    }
}
