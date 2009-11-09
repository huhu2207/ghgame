
namespace MinGH.ChartImpl
{
	/// <remarks>
	/// Stores the various data on the chart that have single instances (only appear once).
	/// </remarks>
    class ChartInfo
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public ChartInfo()
        {
            songName = "default";
            artistName = "default";
            offset = 0.0f;
            chartLengthMiliseconds = 0;
            HOPOThreshold = 12;
            resolution = 192;
        }

        /// <summary>
        /// The name of the song associated with the chart (i.e. Freebird).
        /// </summary>
        public string songName;
		 
		/// <summary>
		/// The name of the associated artist (i.e. Lynyrd Skynyrd).
		/// </summary>
        public string artistName;
		
		/// <summary>
		/// Stores the offset of the chart in seconds (i.e. 1.42 seconds before the song starts).
		/// </summary>
        public float offset;
		
		/// <summary>
		/// The total length of the chart in miliseconds.  This value is currently calculated
		/// from the last event or note in the chart.
		/// </summary>
        public uint chartLengthMiliseconds;

        /// <summary>
        /// What speed of notes will be hammerons (i.e. eights notes, quarter notes, etc.)
        /// </summary>
        public int HOPOThreshold;

        /// <summary>
        /// The number of ticks between two quarter notes (is almost always 192)
        /// </summary>
        public int resolution;
    }
}
