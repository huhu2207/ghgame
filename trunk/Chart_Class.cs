// Classes that store the various data from a *.chart file
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Chart_View
{
    // The master chart class (where everything is ultimately kept)
    class Chart
    {
        // The default constructor
        public Chart()
        {
            Song_Name = "default";
            Artist_Name = "default";
            Offset = 0.0f;
            BPM_Changes = new List<BPM_Change>();
            Events = new List<Event>();
            Note_Charts = new List<Notechart>();
        }

        // The typical constructor
        public Chart(string filename)
        {
            BPM_Changes = new List<BPM_Change>();
            Events = new List<Event>();
            Note_Charts = new List<Notechart>();

            // First, check if the file even exists
            if (!File.Exists(filename))
            {
                Console.WriteLine("ERROR: File not found. Creating blank chart...");
                Song_Name = "default";
                Artist_Name = "default";
                Offset = 0.0f;
            }
            else
            {
                // Read the whole file into a string
                StreamReader input_stream = new StreamReader(filename);
                string input_file = input_stream.ReadToEnd();

                // Add in all the various chart information
                Add_Song_Info(input_file);
                Add_BPM_Changes(input_file);
                Add_Events(input_file);
                Add_Notechart("ExpertSingle", input_file);

                // Close the input stream
                input_stream.Close();
            }
        }

        // Adds the song info to the chart variable (can pass either the whole input file
        // or the desired part)
        public void Add_Song_Info(string input_string)
        {
            // Single out the song section via regular expressions
            string pattern = Regex.Escape("[") + "Song]\\s*" + Regex.Escape("{") + "[^}]*";
            Match matched_section = Regex.Match(input_string, pattern);

            // Create the stream from the singled out section of the input string
            StringReader pattern_stream = new StringReader(matched_section.ToString());
            string current_line = "";
            string[] parsed_line;

            while ((current_line = pattern_stream.ReadLine()) != null)
            {
                // Trim and split the line to retrieve information
                current_line = current_line.Trim();
                parsed_line = current_line.Split(' ');

                // Check for various song infos and parse accordingly
                if (parsed_line[0] == "Name")
                    Song_Name = create_proper_string(parsed_line);

                if (parsed_line[0] == "Artist")
                    Artist_Name = create_proper_string(parsed_line);

                if (parsed_line[0] == "Offset")
                    Offset = float.Parse(parsed_line[2]);
            }

            // Close the string stream
            pattern_stream.Close();
        }

        // Reads in the bpm changes of a chart file (can pass either the whole input file
        // or the desired part)
        public void Add_BPM_Changes(string input_string)
        {
            // Single out the BPM section via regular expressions
            string pattern = Regex.Escape("[") + "SyncTrack]\\s*" + Regex.Escape("{") + "[^}]*";
            Match matched_section = Regex.Match(input_string, pattern);

            // Create the stream from the singled out section of the input string
            StringReader pattern_stream = new StringReader(matched_section.ToString());
            string current_line = "";
            string[] parsed_line;

            while ((current_line = pattern_stream.ReadLine()) != null)
            {
                // Trim and split the line to retrieve information
                current_line = current_line.Trim();
                parsed_line = current_line.Split(' ');

                // If a valid change is found, add it to the list
                if (parsed_line.Length == 4)
                {
                    if (parsed_line[2] == "B")
                        BPM_Changes.Add(new BPM_Change(Convert.ToInt32(parsed_line[0]), Convert.ToInt64(parsed_line[3])));
                }
            }

            // Close the string stream
            pattern_stream.Close();
        }

        // Reads in the events of a chart file (can pass either the whole input file
        // or the desired part)
        public void Add_Events(string input_string)
        {
            // Single out the event section via regular expressions
            string pattern = Regex.Escape("[") + "Events]\\s*" + Regex.Escape("{") + "[^}]*";
            Match matched_section = Regex.Match(input_string, pattern);

            // Create the stream from the singled out section of the input string
            StringReader pattern_stream = new StringReader(matched_section.ToString());
            string current_line = "";
            string[] parsed_line;

            while ((current_line = pattern_stream.ReadLine()) != null)
            {
                // Trim and split the line to retrieve information
                current_line = current_line.Trim();
                parsed_line = current_line.Split(' ');

                // If a valid event is found, add it to the list
                if (parsed_line.Length >= 4)
                {
                    if (parsed_line[2] == "E")
                        Events.Add(new Event(Convert.ToInt32(parsed_line[0]), create_proper_string(parsed_line)));
                }
            }

            // Close the string stream
            pattern_stream.Close();
        }

        // Adds a notechart(from the specified chartname) to the chart from an input string.
        // Chartname must be a valid chart type (i.e. ExpertSingle)
        public void Add_Notechart(string chartname, string input_string)
        {
            // Single out the specified section via regular expressions
            string pattern = Regex.Escape("[") + chartname + "]\\s*" + Regex.Escape("{") + "[^}]*";
            Match matched_section = Regex.Match(input_string, pattern);

            // Create the stream from the singled out section of the input string
            StringReader pattern_stream = new StringReader(matched_section.ToString());
            string current_line = "";
            string[] parsed_line;

            // Create the resulting notechart and prep for input
            Notechart result_notechart = new Notechart(chartname);

            //If specific notechart is not found, return a generic one
            if (!(matched_section.Success))
            {
                result_notechart.Chart_Name = chartname;
                result_notechart.Green_Notes.Add(new Note());
            }
            
            // Else, read in all the chart information
            else
            {
                while ((current_line = pattern_stream.ReadLine()) != null)
                {
                    // Trim and split the line to retrieve information
                    current_line = current_line.Trim();
                    parsed_line = current_line.Split(' ');

                    // If a valid note is found, add it to the list
                    if (parsed_line.Length == 5)
                    {
                        if (parsed_line[2] == "N")
                        {
                            // Find out which note the current line is, and add it to the respective list
                            switch (Convert.ToInt32(parsed_line[3]))
                            {
                                case 0:
                                    result_notechart.Green_Notes.Add(new Note(Convert.ToInt32(parsed_line[0]),
                                                                              Convert.ToInt32(parsed_line[4])));
                                    break;

                                case 1:
                                    result_notechart.Red_Notes.Add(new Note(Convert.ToInt32(parsed_line[0]),
                                                                            Convert.ToInt32(parsed_line[4])));
                                    break;

                                case 2:
                                    result_notechart.Yellow_Notes.Add(new Note(Convert.ToInt32(parsed_line[0]),
                                                                               Convert.ToInt32(parsed_line[4])));
                                    break;

                                case 3:
                                    result_notechart.Blue_Notes.Add(new Note(Convert.ToInt32(parsed_line[0]),
                                                                             Convert.ToInt32(parsed_line[4])));
                                    break;

                                case 4:
                                    result_notechart.Orange_Notes.Add(new Note(Convert.ToInt32(parsed_line[0]),
                                                                               Convert.ToInt32(parsed_line[4])));
                                    break;

                                default:
                                    Console.WriteLine("ERROR: Invalid Note Detcted.  Skipping...");
                                    break;
                            }

                        }

                        // Also check for SP notes
                        if (parsed_line[2] == "S")
                            result_notechart.SP_Notes.Add(new Note(Convert.ToInt32(parsed_line[0]),
                                                                   Convert.ToInt32(parsed_line[4])));
                    }
                }
            }

            // Close the string stream
            pattern_stream.Close();

            // Finally, add the parsed notechart to the notechart list
            Note_Charts.Add(result_notechart);
        }

        // Creates a proper string from a split array (used with the name and artist like cases)
        // -This private function should only be used by other methods in this class
        private string create_proper_string(string[] input)
        {
            string result = "";  // Create the return string

            // Concenate the result string together
            for (int i = 2; i < input.Length; i++)
            {
                result = result + " " + input[i];
            }

            // Remove any leading or following quotes and return
            result = result.Trim(' ').Trim('"');
            return result;
        }

        // Test function to view stored information
        public void print_info()
        {
            Console.WriteLine("Song Name = {0}", Song_Name);
            Console.WriteLine("Artist Name = {0}", Artist_Name);
            Console.WriteLine("Offset = {0}", Offset);
            Console.WriteLine("");

            Console.WriteLine("BPM Changes:");
            foreach (BPM_Change curr_change in BPM_Changes)
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
            Console.ReadLine();

            Console.WriteLine("Notes:");
            foreach (Notechart curr_notechart in Note_Charts)
            {
                curr_notechart.print_info();
            }
        }

        // Various chart info variables
        public string Song_Name;  // The name of the associated song
        public string Artist_Name;  // The name of the assiciated artist
        public float Offset;  // Stores the offset of the chart

        // Various chart data lists
        public List<BPM_Change> BPM_Changes;  // Every BPM change
        public List<Event> Events;  // Every Event

        // The list of possible charts (i.e. Easy Single Guitar, Expert Bass, Medium Lead)
        public List<Notechart> Note_Charts;
    }

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

    // Base class for Note and Event
    class Entity
    {
        public long Location;
    }

    // Contains information on an individual note
    class Note : Entity
    {
        // Defailt Constructor
        public Note()
        {
            Location = 0;
            Duration = 0;
        }

        // Typical Constructor
        public Note(int in_location, int in_duration)
        {
            Location = in_location;
            Duration = in_duration;
        }

        // Test function to view stored information
        public void print_info()
        {
            Console.WriteLine("L = '{0}' D = '{1}'", Location, Duration);
        }

        public int Duration;
    }

    // Contains information on a BPM Change
    class BPM_Change : Entity
    {
        // Defailt Constructor
        public BPM_Change()
        {
            Location = 0;
            Value = 0;
        }

        // Typical Constructor
        public BPM_Change(int in_location, long in_value)
        {
            Location = in_location;
            Value = in_value;
        }

        // Test function to view stored information
        public void print_info()
        {
            Console.WriteLine("L = '{0}' V = '{1}'", Location, Value);
        }

        public long Value;
    }

    // Contains information on an individual event
    class Event : Entity
    {
        // Default Constructor
        public Event()
        {
            Location = 0;
            Value = "default";
        }

        // typical Constructor
        public Event(int in_location, string in_value)
        {
            Location = in_location;
            Value = in_value;
        }

        // Test function to view stored information
        public void print_info()
        {
            Console.WriteLine("L = '{0}' V = '{1}'", Location, Value);
        }

        public string Value;
    }
}
