namespace MinGH.ChartImpl
{
	/// <summary>
	/// Stores the various data on the chart that have single instances (only appear once).
	/// </summary>
    public class ChartInfo
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
            musicStream = null;
            guitarStream = null;
            bassStream = null;
            drumStream = null;
        }

        /// <summary>
        /// The name of the song associated with the chart (i.e. Freebird).
        /// </summary>
        public string songName { get; set; }
		 
		/// <summary>
		/// The name of the associated artist (i.e. Lynyrd Skynyrd).
		/// </summary>
        public string artistName { get; set; }
		
		/// <summary>
		/// Stores the offset of the chart in seconds (i.e. 1.42 seconds before the song starts).
		/// </summary>
        public float offset { get; set; }
		
		/// <summary>
		/// The total length of the chart in miliseconds.  This value is currently calculated
		/// from the last event or note in the chart.
		/// </summary>
        public uint chartLengthMiliseconds { get; set; }

        /// <summary>
        /// What speed of notes will be hammerons (i.e. eights notes, quarter notes, etc.)
        /// </summary>
        public int HOPOThreshold { get; set; }

        /// <summary>
        /// The number of ticks between two quarter notes (is almost always 192)
        /// </summary>
        public int resolution { get; set; }

        /// <summary>
        /// The background music file for the chart (used if no bass or guitar stream is present)
        /// </summary>
        public string musicStream { get; set; }

        /// <summary>
        /// The gutiar music file for the chart.
        /// </summary>
        public string guitarStream { get; set; }

        /// <summary>
        /// The bass/rhythm music file for the chart.
        /// </summary>
        public string bassStream { get; set; }

        /// <summary>
        /// The drum music file for the chart.
        /// </summary>
        public string drumStream { get; set; }
    }
}
