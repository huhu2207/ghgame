using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MinGH.ChartImpl
{
    // This class allows me to put the numerous conditions for finding the end of a chart into
    // one easy to edit place.
    class EndofChartCondition
    {
        public bool noMoreEvents;
        public bool noMoreGreenNotes;
        public bool noMoreRedNotes;
        public bool noMoreYellowNotes;
        public bool noMoreBlueNotes;
        public bool noMoreOrangeNotes;
        public bool noMoreSPNotes;

        public EndofChartCondition()
        {
            noMoreEvents = false;
            noMoreGreenNotes = false;
            noMoreRedNotes = false;
            noMoreYellowNotes = false;
            noMoreBlueNotes = false;
            noMoreOrangeNotes = false;
            noMoreSPNotes = false;
        }

        // This object is false (aka done with) when all its members are true
        public static bool operator true(EndofChartCondition myCondition)
        {
            if ((myCondition.noMoreEvents == true) &&
                (myCondition.noMoreGreenNotes == true) &&
                (myCondition.noMoreRedNotes == true) &&
                (myCondition.noMoreYellowNotes == true) &&
                (myCondition.noMoreBlueNotes == true) &&
                (myCondition.noMoreOrangeNotes == true) &&
                (myCondition.noMoreSPNotes == true))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        // This object is false (aka still active) when any of its members are false
        public static bool operator false(EndofChartCondition myCondition)
        {
            if ((myCondition.noMoreEvents == true) &&
                (myCondition.noMoreGreenNotes == true) &&
                (myCondition.noMoreRedNotes == true) &&
                (myCondition.noMoreYellowNotes == true) &&
                (myCondition.noMoreBlueNotes == true) &&
                (myCondition.noMoreOrangeNotes == true) &&
                (myCondition.noMoreSPNotes == true))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
