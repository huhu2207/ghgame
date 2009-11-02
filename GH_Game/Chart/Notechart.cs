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
            Green_Notes = new List<Note>();
            Red_Notes = new List<Note>();
            Yellow_Notes = new List<Note>();
            Blue_Notes = new List<Note>();
            Orange_Notes = new List<Note>();
            SP_Notes = new List<Note>();
        }

        // The typical constructor
        public Notechart(string in_name)
        {
            Chart_Name = in_name;
            Green_Notes = new List<Note>();
            Red_Notes = new List<Note>();
            Yellow_Notes = new List<Note>();
            Blue_Notes = new List<Note>();
            Orange_Notes = new List<Note>();
            SP_Notes = new List<Note>();
        }

        // Generate the milisecond time values for each note
        public void generateNotechartTimeValuesFromTickValues()
        {

        }

        // Test function to view stored information
        public void print_info()
        {
            foreach (Note curr_note in Green_Notes)
            {
                Console.Write("Green: - ");
                curr_note.print_info();
            }

            foreach (Note curr_note in Red_Notes)
            {
                Console.Write("Red: - ");
                curr_note.print_info();
            }

            foreach (Note curr_note in Yellow_Notes)
            {
                Console.Write("Yellow: - ");
                curr_note.print_info();
            }

            foreach (Note curr_note in Blue_Notes)
            {
                Console.Write("Blue: - ");
                curr_note.print_info();
            }

            foreach (Note curr_note in Orange_Notes)
            {
                Console.Write("Orange: - ");
                curr_note.print_info();
            }

            foreach (Note curr_note in SP_Notes)
            {
                Console.Write("SP: - ");
                curr_note.print_info();
            }
        }

        public string Chart_Name; // The name of the chart itself (i.e. ExpertSingle)

        // The note data lists
        public List<Note> Green_Notes;
        public List<Note> Red_Notes;
        public List<Note> Yellow_Notes;
        public List<Note> Blue_Notes;
        public List<Note> Orange_Notes;
        public List<Note> SP_Notes;
    }
}
