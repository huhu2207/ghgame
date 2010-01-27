using System.Collections.Generic;
using MinGH.GameScreen;
using Sanford.Multimedia.Midi;

namespace MinGH.ChartImpl
{
    public class ChartMidiManager
    {
        /// <summary>
        /// Creates a notechart from the specified midi path and the actual charttype
        /// (i.e. ExpertSingle from notes.mid).  Due to the overhead necessary to
        /// parse a midi file.  I am going to cram all midi->chart operations into
        /// one function call.
        /// </summary>
        /// <param name="chartSelection">
        /// The information on which particular notechart to use.
        /// </param>
        /// <param name="BPMChanges">The list of BPM changes for this chart.</param>
        /// <param name="chartInfo">The metadata on the chart.</param>
        /// <returns>
        /// A filled out Notechart containing the needed information from the *.mid file.
        /// </returns>
        public static Notechart ParseMidiInformation(ChartSelection chartSelection,
                                                     ChartInfo chartInfo,
                                                     List<BPMChange> BPMChanges)
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

            // Go through each event in the first track (which contains the BPM changes)
            // and parse the resulting string.
            for (int i = 0; i < mySequence[0].Count; i++)
            {
                Track sanTrack = mySequence[0];
                MidiEvent currEvent = sanTrack.GetMidiEvent(i);
                if (currEvent.MidiMessage.MessageType == MessageType.Meta)
                {
                    MetaMessage currMessage = currEvent.MidiMessage as MetaMessage;
                    //currTickValue += Convert.ToUInt32(splitEventString[1]);
                    if (currMessage.MetaType == MetaType.Tempo)
                    {
                        TempoChangeBuilder tempoBuilder = new TempoChangeBuilder(currMessage);
                        int midiBPMChange = tempoBuilder.Tempo;
                        // In midi files, bpm chages are stored as "microseconds per quarter note"
                        // and must be converted to BPM, and then into the non decimal format the game
                        // uses.
                        double currBPMDouble = 60000000 / (double)midiBPMChange;
                        uint BPMToAdd = (uint)(currBPMDouble * 1000);
                        BPMChanges.Add(new BPMChange((uint)currEvent.AbsoluteTicks, (uint)BPMToAdd));
                    }
                }
            }

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
