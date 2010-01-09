using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MinGH.GameScreen.MiscClasses
{
    /// <summary>
    /// A simple string class that stores the full path to a chart in addition to 
    /// the path to the directory containing the chart.  Two strings are stored
    /// for ease of use in other aspects of the program.
    /// </summary>
    public class ChartLocation
    {
        public string chartPath;
        public string directory;

        public ChartLocation()
        {
            chartPath = null;
            directory = null;
        }
    }
}
