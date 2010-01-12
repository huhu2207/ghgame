using System.Collections.Generic;
using System.IO;

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
        /// </summary>
        /// <param name="songDirectory">The song directory to search.</param>
        /// <returns>A list of all found charts.</returns>
        public static List<ChartLocation> GenerateAllChartPaths(string songDirectory)
        {
            List<ChartLocation> listOfCharts = new List<ChartLocation>();

            try
            {
                SearchDirectoryForCharts(songDirectory, listOfCharts);
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
        /// <param name="listOfCharts">The list to append charts to.</param>
        /// <returns></returns>
        private static void SearchDirectoryForCharts(string directory, List<ChartLocation> listOfCharts)
        {
            foreach (string currDirectory in Directory.GetDirectories(directory))
            {
                foreach (string currFile in Directory.GetFiles(currDirectory, "*.chart"))
                {
                    listOfCharts.Add(new ChartLocation { chartPath = currFile, directory = currDirectory });
                }
                SearchDirectoryForCharts(currDirectory, listOfCharts);
            }
        }
    }
}
