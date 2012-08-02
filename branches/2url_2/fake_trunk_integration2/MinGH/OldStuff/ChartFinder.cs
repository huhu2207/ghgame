using System.Collections.Generic;
using System.IO;
using ChartEngine;

namespace MinGH.GameScreen.SongSelection
{
    /// <summary>
    /// Searches the song directory
    /// </summary>
    class ChartFinder
    {
        /// <summary>
        /// Generates a list of chartPaths which contains every chart within the
        /// song directory.
        /// OH NO TRUNK CHANGE!?!?!?
        /// </summary>
        /// <param name="songDirectory">The song directory to search.</param>
        /// <returns>A list of all found charts.</returns>
        public static List<ChartSelection> GenerateAllChartPaths(string songDirectory)
        {
            List<ChartSelection> listOfCharts = new List<ChartSelection>();

            try
            {
                // Sadly, I have to do two separate searches due to a Directory.GetFiles limitation
                SearchDirectoryForCharts(songDirectory, "*.chart", listOfCharts);
                SearchDirectoryForCharts(songDirectory, "*.mid", listOfCharts);
            }
            catch (IOException e)
            {
                e.ToString();
                //TODO: Add some logging for exceptions.
            }

            return listOfCharts;
        }

        /// <summary>
        /// Searches the song directory recursively for any chart files, and
        /// appends any found charts into a single list.
        /// </summary>
        /// <param name="directory">The directory to search.</param>
        /// <param name="extension">The </param>
        /// <param name="listOfCharts">The list to append charts to.</param>
        private static void SearchDirectoryForCharts(string directory, string extension, List<ChartSelection> listOfCharts)
        {
            foreach (string currDirectory in Directory.GetDirectories(directory))
            {
                foreach (string currFile in Directory.GetFiles(currDirectory, extension))
                {
                    listOfCharts.Add(new ChartSelection { chartPath = currFile, directory = currDirectory, chartType=extension });
                }
                SearchDirectoryForCharts(currDirectory, extension, listOfCharts);
            }
        }
    }
}
