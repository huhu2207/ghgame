using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using MinGH.Extensions;

namespace MinGH.ChartImpl
{
	/// <remarks>
	/// A manager class that will read and store all Events in a chart in an
	/// organized manner.
	/// </remarks>
    class ChartEventManager
    {
        /// <summary>
        /// Opens a specified chart and reads in all the valid events 
        /// (i.e 14208 = E "section Verse 1a") and returns a populated list.
        /// </summary>
        /// <param name="input_string">
        /// The whole *.chart file stored in one massive string.
        /// </param>
        /// <returns>
        /// A list containing every valid event from the chart.  Due to the nature
        /// of the *.chart specification, these events will be in proper order.
        /// </returns>
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
                                              properStringCreator.createProperString(parsed_line.SubArray(3, parsed_line.Length))));
                    }
                }
            }

            // Close the string stream
            pattern_stream.Close();

            return eventListToReturn;
        }
    }
}
