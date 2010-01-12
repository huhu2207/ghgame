using System;

namespace MinGH.ChartImpl
{
    /// <summary>
    /// Contains information of an event from a chart.
    /// </summary>
    class Event : Entity
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public Event()
        {
            TimeValue = 0;
            TickValue = 0;
            Value = "default";
        }

        /// <summary>
        /// The genral constructor using a given tick and string value.
        /// </summary>
        /// <param name="inTickValue">
        /// The location value in ticks (a notation used in the MIDI arcetecture).
        /// </param>
        /// <param name="inValue">
        /// The string value that details the specific event.
        /// </param>
        public Event(uint inTickValue, string inValue)
        {
            TimeValue = 0;
            TickValue = inTickValue;
            Value = inValue;
        }

        /// <summary>
        /// A debugging function that prints out the various values of the event.
        /// </summary>
        public void print_info()
        {
            Console.WriteLine("L = '{0}' V = '{1}'", TickValue, Value);
        }

		/// <summary>
		/// The details pertaining to this event (i.e. 131904 = E "end").
		/// </summary>
        public string Value { get; set; }
    }
}