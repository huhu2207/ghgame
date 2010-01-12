using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MinGH.Config
{
    public class GameConfiguration
    {
        public HyperSpeedValue speedModValue { get; set; }

        public GameConfiguration(string sourceXMLFile)
        {
            speedModValue = SpeedModReader.ReadInSpeedModsFromXML(sourceXMLFile);
        }
    }
}
