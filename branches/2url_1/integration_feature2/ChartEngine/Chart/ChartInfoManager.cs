using System.IO;
using System.Text.RegularExpressions;
using ChartEngine.Extensions;
using ChartEngine.Shared;

namespace ChartEngine.Chart
{
	/// <summary>
	/// A manager class that will read and store some of the chart information in an
	/// organized manner.
	/// </summary>
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
        /// A filled out ChartInfo instance.
        /// </returns>
        public static Info AddSongInfoFromChart(string inputFile)
        {
            Info chartInfoToReturn = new Info();

            // Single out the song section via regular expressions
            string pattern = Regex.Escape("[") + "Song]\\s*" + Regex.Escape("{") + "[^}]*";
            Match matched_section = Regex.Match(inputFile, pattern);

            // Create the stream from the singled out section of the input string
            StringReader stringReader = new StringReader(matched_section.ToString());
            string currentLine = "";
            string[] parsedLine;

            while ((currentLine = stringReader.ReadLine()) != null)
            {
                // Trim and split the line to retrieve information
                currentLine = currentLine.Trim();
                parsedLine = currentLine.Split(' ');

                // Check for various song infos and parse accordingly
                if (parsedLine[0] == "Name")
                {
                    chartInfoToReturn.songName = ProperStringCreator.createProperString(parsedLine.SubArray(2, parsedLine.Length));
                }

                else if (parsedLine[0] == "Artist")
                {
                    chartInfoToReturn.artistName = ProperStringCreator.createProperString(parsedLine.SubArray(2, parsedLine.Length));
                }

                else if (parsedLine[0] == "Offset")
                {
                    chartInfoToReturn.offset = float.Parse(parsedLine[2]);
                }

                else if (parsedLine[0] == "Resolution")
                {
                    chartInfoToReturn.resolution = int.Parse(parsedLine[2]);
                }

                else if (parsedLine[0] == "hopo_note")
                {
                    chartInfoToReturn.HOPOThreshold = int.Parse(parsedLine[2]);
                }

                else if (parsedLine[0] == "MusicStream")
                {
                    chartInfoToReturn.musicStream = ProperStringCreator.createProperString(parsedLine.SubArray(2, parsedLine.Length));
                }

                else if (parsedLine[0] == "GuitarStream")
                {
                    chartInfoToReturn.guitarStream = ProperStringCreator.createProperString(parsedLine.SubArray(2, parsedLine.Length));
                }

                else if (parsedLine[0] == "BassStream")
                {
                    chartInfoToReturn.bassStream = ProperStringCreator.createProperString(parsedLine.SubArray(2, parsedLine.Length));
                }
            }

            // Close the string stream
            stringReader.Close();

            return chartInfoToReturn;
        }

        /// <summary>
        /// Reads in the song information from the directory in which a midi file
        /// and its supporting files are stored.  Since this function never
        /// actually accesses the midi file itself (just song.ini), it does not
        /// need to be merged with the midi parsing function.
        /// </summary>
        /// <param name="inputPath">
        /// The path to the midi file (which also contains song.ini)
        /// </param>
        /// <returns>
        /// A filled out ChartInfo instance.
        /// </returns>
        public static Info AddSongInfoFromMidi(string inputPath)
        {
            Info chartInfoToReturn = new Info();
            chartInfoToReturn.offset = 0;

            if (File.Exists(inputPath + "\\song.ogg"))
            {
                chartInfoToReturn.musicStream = "song.ogg";
            }

            if (File.Exists(inputPath + "\\guitar.ogg"))
            {
                chartInfoToReturn.guitarStream = "guitar.ogg";
            }

            if (File.Exists(inputPath + "\\rhythm.ogg"))
            {
                chartInfoToReturn.bassStream = "rhythm.ogg";
            }

            if (File.Exists(inputPath + "\\drums.ogg"))
            {
                chartInfoToReturn.drumStream = "drums.ogg";
            }

            if (File.Exists(inputPath + "\\song.ini"))
            {
                StreamReader inputStream = new StreamReader(inputPath + "\\Song.ini");
                string inputFile = inputStream.ReadToEnd();
                StringReader stringReader = new StringReader(inputFile);
                string currentLine = "";
                string[] parsedLine;

                while ((currentLine = stringReader.ReadLine()) != null)
                {
                    // Trim and split the line to retrieve information
                    currentLine = currentLine.Trim();
                    parsedLine = currentLine.Split(' ');

                    if ((parsedLine[0] == "artist") || (parsedLine[0] == "Artist"))
                    {
                        chartInfoToReturn.artistName = ProperStringCreator.createProperString(parsedLine.SubArray(2, parsedLine.Length));
                    }
                    else if ((parsedLine[0] == "name") || (parsedLine[0] == "Name"))
                    {
                        chartInfoToReturn.songName = ProperStringCreator.createProperString(parsedLine.SubArray(2, parsedLine.Length));
                    }
                    else if (parsedLine[0] == "hopo_note")
                    {
                        chartInfoToReturn.HOPOThreshold = int.Parse(parsedLine[2]);
                    }
                }
            }

            return chartInfoToReturn;
        }
    }
}
