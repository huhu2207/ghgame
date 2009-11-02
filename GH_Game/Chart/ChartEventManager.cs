using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

namespace GH_Game.Chart
{
    class ChartEventManager
    {
        // Reads in the events of a chart file (can pass either the whole input file
        // or the desired part)
        public List<Event> AddEvents(string input_string)
        {
            List<Event> eventListToReturn = new List<Event>();
            ProperStringCreator properStringCreator = new ProperStringCreator();

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
                    {
                        eventListToReturn.Add(new Event(Convert.ToUInt32(parsed_line[0]),
                                              properStringCreator.createProperString(parsed_line)));
                    }
                }
            }

            // Close the string stream
            pattern_stream.Close();

            return eventListToReturn;
        }
    }
}
