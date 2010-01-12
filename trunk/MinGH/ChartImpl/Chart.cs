using System;
using System.Collections.Generic;
using System.IO;

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
        /// input stream using this string.
        /// </param>
        public Chart(string filename)
        {
            BPMChanges = new List<BPMChange>();
            events = new List<Event>();
            noteCharts = new List<Notechart>();

            // First, check if the file even exists
            if (!File.Exists(filename))
            {
                Console.WriteLine("ERROR: File not found. Creating blank chart...");
                chartInfo = new ChartInfo();
            }
            else
            {
                // Read the whole file into a string
                StreamReader inputStream = new StreamReader(filename);
                string inputFile = inputStream.ReadToEnd();

                // Add in all the various chart information
                chartInfo = ChartInfoManager.AddSongInfo(inputFile);
                BPMChanges = ChartBPMManager.AddBPMChanges(inputFile);
                events = ChartEventManager.AddEvents(inputFile);

                // Adds just the expert notechart, can make a sneaky way of doing all avaliable charts later
                noteCharts.Add(ChartNotechartManager.GenerateNotechart("ExpertSingle", inputFile));
                for (int i = 0; i < noteCharts.Count; i++)
                {
                    noteCharts[i] = ChartTimeValueManager.GenerateTimeValues(noteCharts[i], BPMChanges,
                                     events, chartInfo);

                    noteCharts[i] = ChartHOPOManager.AssignHOPOS(noteCharts[i], chartInfo);
                }

                // Close the input stream
                inputStream.Close();
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
