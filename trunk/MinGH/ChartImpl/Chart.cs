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
                StreamReader input_stream = new StreamReader(filename);
                string input_file = input_stream.ReadToEnd();

                // Add in all the various chart information
                chartInfo = ChartInfoManager.AddSongInfo(input_file);
                BPMChanges = ChartBPMManager.AddBPMChanges(input_file);
                events = ChartEventManager.AddEvents(input_file);

                // Adds just the expert notechart, can make a sneaky way of doing all avaliable charts later
                noteCharts.Add(ChartNotechartManager.GenerateNotechart("ExpertSingle", input_file));
                for (int i = 0; i < noteCharts.Count; i++)
                {
                    noteCharts[i] = ChartTimeValueManager.GenerateTimeValues(noteCharts[i], BPMChanges,
                                     ref events, chartInfo.offset, ref chartInfo.chartLengthMiliseconds);

                    noteCharts[i] = ChartHOPOManager.AssignHOPOS(noteCharts[i], chartInfo);
                    noteCharts[i] = ChartChordManager.AssignChords(noteCharts[i]);
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
        public ChartInfo chartInfo;

        /// <summary>
        /// A list of every BPM change in the chart.
        /// </summary>
        public List<BPMChange> BPMChanges;
		
		/// <summary>
		/// A list of every event in the chart.
		/// </summary>
        public List<Event> events;

        /// <summary>
        /// A list of every avaliable notechart (i.e. ExpertSingle, MediumDoubleGuitar).
        /// The string constructor, at the moment, does not intelligently pick out every
        /// valid chart within that particular file.  It only chooses ExpertSingle for now.
        /// </summary>
        public List<Notechart> noteCharts;
    }
}
