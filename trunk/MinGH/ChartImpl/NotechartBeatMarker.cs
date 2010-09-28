using System;

namespace MinGH.ChartImpl
{
    public class NotechartBeatmarker
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public NotechartBeatmarker()
        {
            timeValue = 0;
            markerDuration = 4;
        }

        public NotechartBeatmarker(uint inTimeValue, int inDuration)
        {
            timeValue = inTimeValue;
            markerDuration = inDuration;
        }

        public uint timeValue { get; set; }
        public int markerDuration { get; set; }  // "4" = Quarter note, "8" = Eighth note, etc.
    }
}
