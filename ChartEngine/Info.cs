casey_test integration commit
namespace ChartEngine
{
	/// <summary>
	/// Stores the various data on the chart that have single instances (only appear once).
	/// </summary>
    public class Info
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public Info()
        {
            songName = "default";
            artistName = "default";
            offset = 0.0f;
            chartLengthMiliseconds = 0;

            // I really mean 12th notes (8th note triplets), but some midi files
            // have notes that are a few ticks away from being classified as a 12th note
            // due to the midi being a little TOO exact (i.e. 11.9999999th notes)...
            // Since I cant imagine anyone using 11th notes, I use them as the 
            // threshold to take care of these slight anomalies
            HOPOThreshold = 11;

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
