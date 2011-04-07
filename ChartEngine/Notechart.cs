using System;
using System.Collections.Generic;
using System.IO;
using ChartEngine.Chart;
using ChartEngine.Midi;
using ChartEngine.Shared;

namespace ChartEngine
{
    /// <summary>
    /// The master chart class.  All data pertaining to a particular chart is located here.
    /// </summary>
    public class Notechart
    {
        /// <summary>
        /// Default Constructor.  Should never be used since no input function exists yet.
        /// </summary>
        private Notechart()
        {
            chartInfo = new Info();
            BPMChanges = new List<BPMChange>();
            events = new List<Event>();
            noteCharts = new List<Notes>();
            beatMarkers = new List<Beatmarker>();
        }

        /// <summary>
        /// Creates a chart file from a specified chart selection.
        /// </summary>
        /// <param name="chartSelection">
        /// Information on what specific chart from what specific file
        /// to use.
        /// </param>
        public Notechart(ChartSelection chartSelection)
        {
            BPMChanges = new List<BPMChange>();
            events = new List<Event>();
            noteCharts = new List<Notes>();
            chartInfo = new Info();
            beatMarkers = new List<Beatmarker>();

            if (chartSelection.chartType == "*.chart" && File.Exists(chartSelection.chartPath))
            {
                // Read the whole file into a string
                StreamReader inputStream = new StreamReader(chartSelection.chartPath);
                string inputFile = inputStream.ReadToEnd();

                // Add in all the various chart information
                chartInfo = ChartInfoManager.AddSongInfoFromChart(inputFile);
                BPMChanges = ChartBPMManager.AddBPMChangesFromChart(inputFile);
                events = ChartEventManager.AddEventsFromChart(inputFile);

                // Adds just the expert notechart, can make a sneaky way of doing all avaliable charts later
                noteCharts.Add(ChartNotesManager.GenerateNotechartFromChart(chartSelection));

                // Close the input stream
                inputStream.Close();
            }
            else if (chartSelection.chartType == "*.mid")
            {
                chartInfo = ChartInfoManager.AddSongInfoFromMidi(chartSelection.directory);

                // Try and parse with Toub.  If an exception is thrown, parse with Sanford.
                try
                {
                    noteCharts.Add(ChartMidiManager.ParseMidiInformationToub(chartSelection, chartInfo, BPMChanges));
                    //noteCharts.Add(ChartMidiManager.ParseMidiInformationSanford(chartSelection, chartInfo, BPMChanges));
                }
                catch (Exception e)
                {
                    // Logging plz!
                    e.ToString();
                    noteCharts.Add(ChartMidiManager.ParseMidiInformationSanford(chartSelection, chartInfo, BPMChanges));
                }
            }

            for (int i = 0; i < noteCharts.Count; i++)
            {
                noteCharts[i] = TimeValueManager.GenerateTimeValues(noteCharts[i], BPMChanges,
                                                                         events, chartInfo, beatMarkers);
                if (noteCharts[i].instrument != "Drums")
                {
                    noteCharts[i] = HOPOManager.AssignHOPOS(noteCharts[i], chartInfo);
                }
            }
        }

        /// <summary>
        /// A debugging function that will print out alll relavent data from the chart
        /// onto the console.
        /// </summary>
        public void printInfo()
        {
            Console.WriteLine("Song Name = {0}", chartInfo.songName);
            Console.WriteLine("Artist Name = {0}", chartInfo.artistName);
            Console.WriteLine("Offset = {0}", chartInfo.offset);
            Console.WriteLine("");

            Console.WriteLine("BPM Changes:");
            foreach (BPMChange curr_change in BPMChanges)
            {
                curr_change.print_info();
            }
            Console.WriteLine("");

            Console.WriteLine("Events:");
            foreach (Event curr_event in events)
            {
                curr_event.print_info();
            }
            Console.WriteLine("Press Enter to read in the notes...");
            //Console.ReadLine();

            Console.WriteLine("Notes:");
            foreach (Notes curr_notechart in noteCharts)
            {
                curr_notechart.print_info();
            }
        }

        /// <summary>
        /// Stores metadata on the chart.  See the ChartInfo class for more.
        /// </summary>
        public Info chartInfo { get; set; }

        /// <summary>
        /// A list of every BPM change in the chart.
        /// </summary>
        public List<BPMChange> BPMChanges { get; set; }
		
		/// <summary>
		/// A list of every event in the chart.
		/// </summary>
        public List<Event> events { get; set; }

        /// <summary>
        /// A list of every avaliable notechart (i.e. ExpertSingle, MediumDoubleGuitar).
        /// The string constructor, at the moment, does not intelligently pick out every
        /// valid chart within that particular file.  It only chooses ExpertSingle for now.
        /// </summary>
        public List<Notes> noteCharts { get; set; }

        public List<Beatmarker> beatMarkers { get; set; }
    }
}
