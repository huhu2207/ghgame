// Classes that store the various data from a *.chart file
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace GH_Game.Chart
{
    // The master chart class (where everything is ultimately kept)
    class Chart
    {
        // The default constructor
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
        }

        // The typical constructor
        public Chart(string filename)
        {
            BPM_Changes = new List<BPMChange>();
            Events = new List<Event>();
            Note_Charts = new List<Notechart>();
            chartBPMManager = new ChartBPMManager();
            chartInfoManager = new ChartInfoManager();
            chartEventManager = new ChartEventManager();
            chartNotechartManager = new ChartNotechartManager();

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
                    Note_Charts[i] = chartNotechartManager.GenerateTimeValues(Note_Charts[i], BPM_Changes, Events, chartInfo.offset);
                }

                // Close the input stream
                input_stream.Close();
            }
        }

        // Test function to view stored information
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

        // The basic information on the chart
        public ChartInfo chartInfo;

        // Various chart data lists
        public List<BPMChange> BPM_Changes;
        public List<Event> Events;

        // The list of possible charts (i.e. Easy Single Guitar, Expert Bass, Medium Lead)
        public List<Notechart> Note_Charts;

        // Input information managers
        private ChartBPMManager chartBPMManager;
        private ChartInfoManager chartInfoManager;
        private ChartEventManager chartEventManager;
        private ChartNotechartManager chartNotechartManager;
    }
}
