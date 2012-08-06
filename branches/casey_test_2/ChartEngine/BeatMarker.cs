casey_test_2
using System;

namespace ChartEngine
{
    public class Beatmarker
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public Beatmarker()
        {
            timeValue = 0;
            markerDuration = 4;
        }

        public Beatmarker(uint inTimeValue, int inDuration)
        {
            timeValue = inTimeValue;
            markerDuration = inDuration;
        }

        public uint timeValue { get; set; }
        public int markerDuration { get; set; }  // "4" = Quarter note, "8" = Eighth note, etc.
    }
}
