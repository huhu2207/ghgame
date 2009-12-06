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
    /// NOTE: 0.5 is HS0 while 1.0 = HS5
    /// </summary>
    public class HyperSpeedList
    {
        public HyperSpeedList()
        {
            theList.Add(new HyperSpeedValue(900, 0.5));
            theList.Add(new HyperSpeedValue(750, 0.6));
            theList.Add(new HyperSpeedValue(630, 0.7));
            theList.Add(new HyperSpeedValue(550, 0.8));
            theList.Add(new HyperSpeedValue(500, 0.9));
            theList.Add(new HyperSpeedValue(460, 1.0));
        }
        public List<HyperSpeedValue> theList = new List<HyperSpeedValue>();
    }
}
