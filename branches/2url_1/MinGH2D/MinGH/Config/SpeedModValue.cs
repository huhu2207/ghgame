namespace MinGH.Config
{
    /// <summary>
    /// A simple class that encapsulates the values for a proper speed mod setting.
    /// </summary>
    public class SpeedModValue
    {
        /// <summary>
        /// The number of miliseconds a note must be drawn early to keep the notes
        /// in sync with the music.
        /// </summary>
        public int milisecondOffset { get; set; }

        /// <summary>
        /// The value that is multiplied with the current frame's elapsed milisecond
        /// value (which gets the actual speed).
        /// </summary>
        public double noteVelocityMultiplier { get; set; }

        /// <summary>
        /// Default constructor.  Uses pre-defined values.
        /// </summary>
        public SpeedModValue() :
            this(900, 0.5)
        {}

        /// <summary>
        /// Creates a new HyperSpeedValue with the provided data.
        /// </summary>
        /// <param name="MSOffset">The milisecond offset.</param>
        /// <param name="NVMult">The note velocity multiplier.</param>
        public SpeedModValue(int MSOffset, double NVMult)
        {
            milisecondOffset = MSOffset;
            noteVelocityMultiplier = NVMult;
        }
    }
}
