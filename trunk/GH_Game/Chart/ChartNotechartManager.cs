using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace GH_Game.Chart
{
    class ChartNotechartManager
    {
        // Adds a notechart(from the specified chartname) to the chart from an input string.
        // Chartname must be a valid chart type (i.e. ExpertSingle)
        public Notechart GenerateNotechart(string chartname, string input_string)
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
                                    result_notechart.Green_Notes.Add(new Note(Convert.ToUInt32(parsed_line[0]),
                                                                              Convert.ToInt32(parsed_line[4])));
                                    break;

                                case 1:
                                    result_notechart.Red_Notes.Add(new Note(Convert.ToUInt32(parsed_line[0]),
                                                                            Convert.ToInt32(parsed_line[4])));
                                    break;

                                case 2:
                                    result_notechart.Yellow_Notes.Add(new Note(Convert.ToUInt32(parsed_line[0]),
                                                                               Convert.ToInt32(parsed_line[4])));
                                    break;

                                case 3:
                                    result_notechart.Blue_Notes.Add(new Note(Convert.ToUInt32(parsed_line[0]),
                                                                             Convert.ToInt32(parsed_line[4])));
                                    break;

                                case 4:
                                    result_notechart.Orange_Notes.Add(new Note(Convert.ToUInt32(parsed_line[0]),
                                                                               Convert.ToInt32(parsed_line[4])));
                                    break;

                                default:
                                    Console.WriteLine("ERROR: Invalid Note Detcted.  Skipping...");
                                    break;
                            }

                        }

                        // Also check for SP notes
                        if (parsed_line[2] == "S")
                            result_notechart.SP_Notes.Add(new Note(Convert.ToUInt32(parsed_line[0]),
                                                                   Convert.ToInt32(parsed_line[4])));
                    }
                }
            }

            // Close the string stream
            pattern_stream.Close();

            return result_notechart;
        }
    }
}
