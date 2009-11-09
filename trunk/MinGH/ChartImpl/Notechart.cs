using System;
using System.Collections.Generic;

namespace MinGH.ChartImpl
{
	/// <remarks>
	/// Stores information on a specific notechart.
	/// A Chart can have multiple Notecharts (i.e. easy, medium).
	/// </remarks>
    class Notechart
    {
        /// <summary>
        /// Default Constructor.
        /// </summary>
        public Notechart()
        {
            Chart_Name = "default";
            //greenNotes = new List<ChartNote>();
            //redNotes = new List<ChartNote>();
            //yellowNotes = new List<ChartNote>();
            //blueNotes = new List<ChartNote>();
            //orangeNotes = new List<ChartNote>();
            notes = new List<ChartNote>();
            SPNotes = new List<ChartNote>();
        }

        /// <summary>
        /// A constructor that takes in a specified chart name
        /// </summary>
        /// <param name="in_name">
        /// The actual name of the chart (i.e. ExpertSingle)
        /// </param>
        public Notechart(string in_name)
        {
            Chart_Name = in_name;
            //greenNotes = new List<ChartNote>();
            //redNotes = new List<ChartNote>();
            //yellowNotes = new List<ChartNote>();
            //blueNotes = new List<ChartNote>();
            //orangeNotes = new List<ChartNote>();
            notes = new List<ChartNote>();
            SPNotes = new List<ChartNote>();
        }

        /// <summary>
        /// Debug function that prints out the values for every single note within the notechart.
        /// </summary>
        public void print_info()
        {
            //foreach (ChartNote curr_note in greenNotes)
            //{
            //    Console.Write("Green: - ");
            //    curr_note.print_info();
            //}

            //foreach (ChartNote curr_note in redNotes)
            //{
            //    Console.Write("Red: - ");
            //    curr_note.print_info();
            //}

            //foreach (ChartNote curr_note in yellowNotes)
            //{
            //    Console.Write("Yellow: - ");
            //    curr_note.print_info();
            //}

            //foreach (ChartNote curr_note in blueNotes)
            //{
            //    Console.Write("Blue: - ");
            //    curr_note.print_info();
            //}

            //foreach (ChartNote curr_note in orangeNotes)
            //{
            //    Console.Write("Orange: - ");
            //    curr_note.print_info();
            //}

            foreach (ChartNote curr_note in notes)
            {
                Console.Write(curr_note.noteType.ToString() + ": - ");
            }

            foreach (ChartNote curr_note in SPNotes)
            {
                Console.Write("SP: - ");
                curr_note.print_info();
            }
        }

		/// <summary>
		/// The actual name the of notechart (i.e. ExpertDoubleGuitar)
		/// </summary>
        public string Chart_Name;

        /// <summary>
        /// Lists that contain every single note in the chart.  There is also a list for every star power note.
        /// </summary>
        /*public List<ChartNote> greenNotes;
        public List<ChartNote> redNotes;
        public List<ChartNote> yellowNotes;
        public List<ChartNote> blueNotes;
        public List<ChartNote> orangeNotes;*/
        public List<ChartNote> notes;
        public List<ChartNote> SPNotes;
    }
}
