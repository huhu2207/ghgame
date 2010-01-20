using System;
using System.Collections.Generic;
using System.IO;
using Toub.Sound.Midi;
using MinGH.GameScreen;

namespace MinGH.ChartImpl
{
    /// <summary>
    /// The master chart class.  All data pertaining to a particular chart is located here.
    /// </summary>
    class Chart
    {
        /// <summary>
        /// Default Constructor.  Should never be used since no input function exists yet.
        /// </summary>
        public Chart()
        {
            chartInfo = new ChartInfo();
            BPMChanges = new List<BPMChange>();
            events = new List<Event>();
            noteCharts = new List<Notechart>();
        }

        /// <summary>
        /// Creates a chart file from a specified input filename.
        /// </summary>
        /// <param name="filename">
        /// The path to a valid *.chart file.  The constructor creates the
        /// input stream using this string.  If midi is used, a path to the
        /// directory in which the chart is located must be passed.
        /// </param>
        /// <param name="filetype">
        /// The type of chart being used (*.chart and *.mid are currently supported).
        /// NOTE: If you use a chart filetype, you need to supply the full chart path
        /// including the chart filename.  If you use a midi filetype, you need to
        /// specify only the directory in which the midi file is located.
        /// </param>
        public Chart(string chartLocation, string filetype)
        {
            BPMChanges = new List<BPMChange>();
            events = new List<Event>();
            noteCharts = new List<Notechart>();

            if (filetype == "*.chart" && File.Exists(chartLocation))
            {
                // Read the whole file into a string
                StreamReader inputStream = new StreamReader(chartLocation);
                string inputFile = inputStream.ReadToEnd();

                // Add in all the various chart information
                chartInfo = ChartInfoManager.AddSongInfoFromChart(inputFile);
                BPMChanges = ChartBPMManager.AddBPMChangesFromChart(inputFile);
                events = ChartEventManager.AddEventsFromChart(inputFile);

                // Adds just the expert notechart, can make a sneaky way of doing all avaliable charts later
                noteCharts.Add(ChartNotechartManager.GenerateNotechartFromChart("ExpertDoubleBass", inputFile));
                for (int i = 0; i < noteCharts.Count; i++)
                {
                    noteCharts[i] = ChartTimeValueManager.GenerateTimeValues(noteCharts[i], BPMChanges,
                                     events, chartInfo);

                    noteCharts[i] = ChartHOPOManager.AssignHOPOS(noteCharts[i], chartInfo);
                }

                // Close the input stream
                inputStream.Close();
            }
            else if (filetype == "*.mid")
            {
                chartInfo = ChartInfoManager.AddSongInfoFromMidi(chartLocation);
                events = ChartEventManager.AddEventsFromMidi(chartLocation, chartInfo);
                BPMChanges = ChartBPMManager.AddBPMChangesFromMidi(chartLocation);
                noteCharts.Add(ChartNotechartManager.GenerateNotechartFromMidi("ExpertSingle", chartLocation));
                noteCharts[0] = ChartTimeValueManager.GenerateTimeValues(noteCharts[0], BPMChanges,
                                     events, chartInfo);
                noteCharts[0] = ChartHOPOManager.AssignHOPOS(noteCharts[0], chartInfo);

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
            foreach (Notechart curr_notechart in noteCharts)
            {
                curr_notechart.print_info();
            }
        }

        /// <summary>
        /// Stores metadata on the chart.  See the ChartInfo class for more.
        /// </summary>
        public ChartInfo chartInfo { get; set; }

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
        public List<Notechart> noteCharts { get; set; }
    }
}
