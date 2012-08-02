using System;
using System.Collections.Generic;

namespace ChartEngine
{
	/// <summary>
	/// Stores information on a specific notechart.
	/// A Chart can have multiple Notecharts (i.e. easy, medium).
	/// </summary>
    public class Notes
    {
        /// <summary>
        /// Default Constructor.
        /// </summary>
        public Notes()
        {
            difficulty = "default";
            instrument = "default";
            notes = new List<Note>();
            SPNotes = new List<Note>();
        }

        /// <summary>
        /// A constructor that takes in a specified chart name
        /// </summary>
        /// <param name="inDifficulty">
        /// The difficulty of the new notechart.
        /// </param>
        /// <param name="inInstrument">
        /// The instrument of the new notechart.
        /// </param>
        public Notes(string inDifficulty, string inInstrument)
        {
            difficulty = inDifficulty;
            instrument = inInstrument;
            notes = new List<Note>();
            SPNotes = new List<Note>();
        }

        /// <summary>
        /// Debug function that prints out the values for every single note within the notechart.
        /// </summary>
        public void print_info()
        {
            foreach (Note curr_note in notes)
            {
                Console.Write(curr_note.noteType.ToString() + ": - ");
            }

            foreach (Note curr_note in SPNotes)
            {
                Console.Write("SP: - ");
                curr_note.printInfo();
            }
        }

		/// <summary>
		/// The instrument this chart uses.
		/// </summary>
        public string instrument { get; set; }

        /// <summary>
        /// The difficulty this chart uses.
        /// </summary>
        public string difficulty { get; set; }

        /// <summary>
        /// List that contains every single note in the chart.
        /// </summary>
        public List<Note> notes { get; set; }

        /// <summary>
        /// A list containing every star power note in the chart.
        /// </summary>
        public List<Note> SPNotes { get; set; }
    }
}
