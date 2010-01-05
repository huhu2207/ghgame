using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MinGH.GameScreen.SinglePlayer
{
    /// <summary>
    /// A simple class that encapsulates the values for a proper hyper speed setting.
    /// </summary>
    public class HyperSpeedValue
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

        public HyperSpeedValue(int MSOffset, double NVMult)
        {
            milisecondOffset = MSOffset;
            noteVelocityMultiplier = NVMult;
        }
    }
}
