using System;
using System.Collections.Generic;
using System.IO;

namespace MinGH.ChartImpl
{
    /// <remarks>
    /// The master chart class.  All data pertaining to a particular chart is located here.
    /// </remarks>
    class Chart
    {
        /// <summary>
        /// Default Constructor.  Should never be used since no input function exists yet.
        /// </summary>
        public Chart()
        {
            chartInfo = new ChartInfo();
            BPM_Changes = new List<BPMChange>();
            Events = new List<Event>();
            Note_Charts = new List<Notechart>();
            chartBPMManager = new ChartBPMManager();
            chartInfoManager = new ChartInfoManager();
            chartEventManager = new ChartEventManager();
            chartNotechartManager = new ChartNotechartManager();
            chartTimeValueManager = new ChartTimeValueManager();
        }

        /// <summary>
        /// Creates a chart file from a specified input filename.
        /// </summary>
        /// <param name="filename">
        /// The path to a valid *.chart file.  The constructor creates the
        /// input stream using this string.
        /// </param>
        public Chart(string filename)
        {
            BPM_Changes = new List<BPMChange>();
            Events = new List<Event>();
            Note_Charts = new List<Notechart>();
            chartBPMManager = new ChartBPMManager();
            chartInfoManager = new ChartInfoManager();
            chartEventManager = new ChartEventManager();
            chartNotechartManager = new ChartNotechartManager();
            chartTimeValueManager = new ChartTimeValueManager();

            // First, check if the file even exists
            if (!File.Exists(filename))
            {
                Console.WriteLine("ERROR: File not found. Creating blank chart...");
                chartInfo = new ChartInfo();
            }
            else
            {
                // Read the whole file into a string
                StreamReader input_stream = new StreamReader(filename);
                string input_file = input_stream.ReadToEnd();

                // Add in all the various chart information
                chartInfo = chartInfoManager.Add_Song_Info(input_file);
                BPM_Changes = chartBPMManager.AddBPMChanges(input_file);
                Events = chartEventManager.AddEvents(input_file);

                // Adds just the expert notechart, can make a sneaky way of doing all avaliable charts later
                Note_Charts.Add(chartNotechartManager.GenerateNotechart("ExpertSingle", input_file));
                for (int i = 0; i < Note_Charts.Count; i++)
                {
                    Note_Charts[i] = chartTimeValueManager.GenerateTimeValues(Note_Charts[i], BPM_Changes,
                                     ref Events, chartInfo.offset, ref chartInfo.chartLengthMiliseconds);
                }

                // Close the input stream
                input_stream.Close();
            }
        }

        /// <summary>
        /// A debugging function that will print out alll relavent data from the chart
        /// onto the console.
        /// </summary>
        public void print_info()
        {
            Console.WriteLine("Song Name = {0}", chartInfo.songName);
            Console.WriteLine("Artist Name = {0}", chartInfo.artistName);
            Console.WriteLine("Offset = {0}", chartInfo.offset);
            Console.WriteLine("");

            Console.WriteLine("BPM Changes:");
            foreach (BPMChange curr_change in BPM_Changes)
            {
                curr_change.print_info();
            }
            Console.WriteLine("");

            Console.WriteLine("Events:");
            foreach (Event curr_event in Events)
            {
                curr_event.print_info();
            }
            Console.WriteLine("Press Enter to read in the notes...");
            //Console.ReadLine();

            Console.WriteLine("Notes:");
            foreach (Notechart curr_notechart in Note_Charts)
            {
                curr_notechart.print_info();
            }
        }

        /// <summary>
        /// Stores metadata on the chart.  See the ChartInfo class for more.
        /// </summary>
        public ChartInfo chartInfo;

        /// <summary>
        /// A list of every BPM change in the chart.
        /// </summary>
        public List<BPMChange> BPM_Changes;
		
		/// <summary>
		/// A list of every event in the chart.
		/// </summary>
        public List<Event> Events;

        /// <summary>
        /// A list of every avaliable notechart (i.e. ExpertSingle, MediumDoubleGuitar).
        /// The string constructor, at the moment, does not intelligently pick out every
        /// valid chart within that particular file.  It only chooses ExpertSingle for now.
        /// </summary>
        public List<Notechart> Note_Charts;

        /// <summary>
        /// The various manager classes that make filling out the chart information
        /// easier.  See thier respective class pages for more information.
        /// </summary>
        private ChartBPMManager chartBPMManager;
        private ChartInfoManager chartInfoManager;
        private ChartEventManager chartEventManager;
        private ChartNotechartManager chartNotechartManager;
        private ChartTimeValueManager chartTimeValueManager;
    }
}
