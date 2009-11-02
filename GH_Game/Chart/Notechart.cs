using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GH_Game.Chart
{
    // Stores information on a specific notechart (a full chart contains multiple notecharts)
    class Notechart
    {
        // The default constructor
        public Notechart()
        {
            Chart_Name = "default";
            greenNotes = new List<Note>();
            redNotes = new List<Note>();
            yellowNotes = new List<Note>();
            blueNotes = new List<Note>();
            orangeNotes = new List<Note>();
            SPNotes = new List<Note>();
        }

        // The typical constructor
        public Notechart(string in_name)
        {
            Chart_Name = in_name;
            greenNotes = new List<Note>();
            redNotes = new List<Note>();
            yellowNotes = new List<Note>();
            blueNotes = new List<Note>();
            orangeNotes = new List<Note>();
            SPNotes = new List<Note>();
        }

        // Generate the milisecond time values for each note
        public void generateNotechartTimeValuesFromTickValues()
        {

        }

        // Test function to view stored information
        public void print_info()
        {
            foreach (Note curr_note in greenNotes)
            {
                Console.Write("Green: - ");
                curr_note.print_info();
            }

            foreach (Note curr_note in redNotes)
            {
                Console.Write("Red: - ");
                curr_note.print_info();
            }

            foreach (Note curr_note in yellowNotes)
            {
                Console.Write("Yellow: - ");
                curr_note.print_info();
            }

            foreach (Note curr_note in blueNotes)
            {
                Console.Write("Blue: - ");
                curr_note.print_info();
            }

            foreach (Note curr_note in orangeNotes)
            {
                Console.Write("Orange: - ");
                curr_note.print_info();
            }

            foreach (Note curr_note in SPNotes)
            {
                Console.Write("SP: - ");
                curr_note.print_info();
            }
        }

        public string Chart_Name; // The name of the chart itself (i.e. ExpertSingle)

        // The note data lists
        public List<Note> greenNotes;
        public List<Note> redNotes;
        public List<Note> yellowNotes;
        public List<Note> blueNotes;
        public List<Note> orangeNotes;
        public List<Note> SPNotes;
    }
}
