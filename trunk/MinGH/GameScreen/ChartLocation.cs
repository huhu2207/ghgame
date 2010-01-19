namespace MinGH.GameScreen
{
    /// <summary>
    /// A simple string class that stores the full path to a chart in addition to 
    /// the path to the directory containing the chart.  Two strings are stored
    /// for ease of use in other aspects of the program.
    /// </summary>
    public class ChartLocation
    {
        public string chartPath { get; set; }
        public string directory { get; set; }
        public string chartType { get; set; }

        public ChartLocation()
        {
            chartPath = null;
            directory = null;
        }
    }
}
