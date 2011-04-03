namespace MinGH.GameScreen
{
    /// <summary>
    /// A simple string class that stores all relavent information for picking
    /// a particular chart from a source file.
    /// 
    /// Current possible chartTypes:
    ///      SingleGuitar
    ///      DoubleGuitar
    ///      DoubleBass
    ///      Drums
    /// </summary>
    public class ChartSelection
    {
        /// <summary>
        /// The full path to the chart file being used.
        /// </summary>
        public string chartPath { get; set; }

        /// <summary>
        /// The path to the directory in which the chart and audio files
        /// are stored.
        /// </summary>
        public string directory { get; set; }

        /// <summary>
        /// The filetype of the chart (e.g. *.chart or *.mid)
        /// </summary>
        public string chartType { get; set; }

        /// <summary>
        /// The selected difficulty.
        /// </summary>
        public string difficulty { get; set; }

        /// <summary>
        /// The selected instrument.
        /// </summary>
        public string instrument { get; set; }

        public ChartSelection()
        {
            chartPath = null;
            directory = null;
            chartType = null;
            difficulty = null;
            instrument = null;
        }

        public ChartSelection(ChartSelection other)
        {
            chartPath = other.chartPath;
            directory = other.directory;
            chartType = other.chartType;
            difficulty = other.difficulty;
            instrument = other.instrument;
        }
    }
}
