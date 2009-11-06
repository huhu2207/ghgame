using System.IO;
using System.Text.RegularExpressions;
using MinGH.Extensions;

namespace MinGH.ChartImpl
{
    class ChartInfoManager
    {
        // Adds the song info to the chart variable (can pass either the whole input file
        // or the desired part)
        public ChartInfo Add_Song_Info(string input_string)
        {
            ChartInfo chartInfoToReturn = new ChartInfo();
            ProperStringCreator properStringCreator = new ProperStringCreator();

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
                    chartInfoToReturn.songName = properStringCreator.createProperString(parsed_line.SubArray(2, parsed_line.Length));

                if (parsed_line[0] == "Artist")
                    chartInfoToReturn.artistName = properStringCreator.createProperString(parsed_line.SubArray(2, parsed_line.Length));

                if (parsed_line[0] == "Offset")
                    chartInfoToReturn.offset = float.Parse(parsed_line[2]);
            }

            // Close the string stream
            pattern_stream.Close();

            return chartInfoToReturn;
        }
    }
}
