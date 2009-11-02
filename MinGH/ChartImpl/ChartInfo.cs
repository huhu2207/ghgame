using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MinGH.ChartImpl
{
    class ChartInfo
    {
        // Default Constructor
        public ChartInfo()
        {
            songName = "default";
            artistName = "default";
            offset = 0.0f;
            chartLengthMiliseconds = 0;
        }

        // Various chart info variables
        public string songName;  // The name of the associated song
        public string artistName;  // The name of the assiciated artist
        public float offset;  // Stores the offset of the chart
        public uint chartLengthMiliseconds;  // How long the chart is (miliseconds)
    }
}
