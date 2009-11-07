﻿
namespace MinGH.ChartImpl
{
    /// <remarks>
    /// A class that allows for easy checking for "end of notechart" when doing a time manager run through.
    /// </remarks>
    class EndofChartCondition
    {
		/// <summary>
		/// Various boolean values that get set off if no more values of a specific type are avaliable
		/// </summary>
        public bool noMoreEvents;
        public bool noMoreGreenNotes;
        public bool noMoreRedNotes;
        public bool noMoreYellowNotes;
        public bool noMoreBlueNotes;
        public bool noMoreOrangeNotes;
        public bool noMoreSPNotes;

		/// <summary>
		/// Default constructor
		/// </summary>
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

        /// <summary>
        /// The overloading of the true operator.  This class is true when all boolean member values are true.
        /// </summary>
        /// <param name="myCondition">
        /// The end of chart condition to be testing.
        /// </param>
        /// <returns>
        /// True or False.
        /// </returns>
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

        /// <summary>
        /// The overloading of the true operator.  This class is false when any boolean member value is false.
        /// </summary>
        /// <param name="myCondition">
        /// The end of chart condition to be testing.
        /// </param>
        /// <returns>
        /// True or False.
        /// </returns>
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
