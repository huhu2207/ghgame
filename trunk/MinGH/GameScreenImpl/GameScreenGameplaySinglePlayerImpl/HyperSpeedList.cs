using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MinGH.GameScreenImpl.GameScreenGameplaySinglePlayerImpl
{
    /// <summary>
    /// A class containing a list of predefined hyperspeed values that worked on my system.
    /// This is a horrible approach, but I can't think up a proper function to find these
    /// values.
    /// </summary>
    public class HyperSpeedList
    {
        public HyperSpeedList()
        {
            theList.Add(new HyperSpeedValue(963, 0.5));
            theList.Add(new HyperSpeedValue(820, 0.6));
            theList.Add(new HyperSpeedValue(705, 0.7));
            theList.Add(new HyperSpeedValue(615, 0.8));
            theList.Add(new HyperSpeedValue(560, 0.9));
            theList.Add(new HyperSpeedValue(500, 1.0));
        }
        public List<HyperSpeedValue> theList = new List<HyperSpeedValue>();
    }
}
