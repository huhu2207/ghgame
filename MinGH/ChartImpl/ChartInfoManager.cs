using System.IO;
using System.Text.RegularExpressions;
using MinGH.Extensions;

namespace MinGH.ChartImpl
{
	/// <remarks>
	/// A manager class that will read and store some of the chart information in an
	/// organized manner.
	/// </remarks>
    class ChartInfoManager
    {
        /// <summary>
        /// Opens a specified chart and reads in the usable chart information
        /// (i.e. Artist = "Bullet for my Valentine")
        /// </summary>
        /// <param name="inputFile">
        /// The whole *.chart file stored in one massive string.
        /// </param>
        /// <returns>
        /// A chart info class that has every field filled out with information from
        /// the input chart file.
        /// </returns>
        public static ChartInfo AddSongInfo(string inputFile)
        {
            ChartInfo chartInfoToReturn = new ChartInfo();
            ProperStringCreator properStringCreator = new ProperStringCreator();

            // Single out the song section via regular expressions
            string pattern = Regex.Escape("[") + "Song]\\s*" + Regex.Escape("{") + "[^}]*";
            Match matched_section = Regex.Match(inputFile, pattern);

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
                {
                    chartInfoToReturn.songName = properStringCreator.createProperString(parsed_line.SubArray(2, parsed_line.Length));
                }

                else if (parsed_line[0] == "Artist")
                {
                    chartInfoToReturn.artistName = properStringCreator.createProperString(parsed_line.SubArray(2, parsed_line.Length));
                }

                else if (parsed_line[0] == "Offset")
                {
                    chartInfoToReturn.offset = float.Parse(parsed_line[2]);
                }

                else if (parsed_line[0] == "Resolution")
                {
                    chartInfoToReturn.resolution = int.Parse(parsed_line[2]);
                }

                else if (parsed_line[0] == "hopo_note")
                {
                    chartInfoToReturn.HOPOThreshold = int.Parse(parsed_line[2]);
                }
            }

            // Close the string stream
            pattern_stream.Close();

            return chartInfoToReturn;
        }
    }
}
