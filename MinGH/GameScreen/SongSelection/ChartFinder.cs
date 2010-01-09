using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MinGH.GameScreen.MiscClasses;
using System.IO;

namespace MinGH.GameScreen.SongSelection
{
    class ChartFinder
    {
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

        private static List<ChartLocation> SearchDirectoryForCharts(string directory, List<ChartLocation> listOfCharts)
        {
            foreach (string currDirectory in Directory.GetDirectories(directory))
            {
                foreach (string currFile in Directory.GetFiles(currDirectory, "*.chart"))
                {
                    listOfCharts.Add(new ChartLocation { chartPath = currFile, directory = currDirectory });
                }
                SearchDirectoryForCharts(currDirectory, listOfCharts);
            }
            return listOfCharts;
        }
    }
}
