using System;

namespace MinGH.ChartImpl
{
    /// <summary>
    /// Contains information on a BPM Change.  Is derived from the Entity class.
    /// </summary>
    class BPMChange : Entity
    {
        /// <summary>
        /// Default Constructor.
        /// </summary>
        public BPMChange()
        {
            TimeValue = 0;
            TickValue = 0;
            BPMValue = 0;
        }

        /// <summary>
        /// Creates a BPMChange with a valid location and value.
        /// </summary>
        /// <param name="inTickValue">
        /// The tick value (location) associated with the new BPM change.
        /// </param>
        /// <param name="inValue">
        /// The actual value of the BPM change.
        /// </param>
        public BPMChange(uint inTickValue, long inValue)
        {
            TimeValue = 0;
            TickValue = inTickValue;
            BPMValue = inValue;
        }

        /// <summary>
        /// Prints the TickValue and BPMValue of this BPM change.
        /// </summary>
        public void print_info()
        {
            Console.WriteLine("L = '{0}' V = '{1}'", TickValue, BPMValue);
        }

		/// <summary>
		/// The actual value of the BPM Change (i.e. 185 BPM).
		/// Note that this project follows the *.chart specification where all
		/// BPM change values must be 6 digits long (i.e. 185000 BPM).
		/// </summary>
        public long BPMValue { get; set; }
    }
}
