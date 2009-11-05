using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MinGH.ChartImpl
{
    // Contains information on an individual note
    class ChartNote : Entity
    {
        // Defailt Constructor
        public ChartNote()
        {
            TimeValue = 0;
            TickValue = 0;
            Duration = 0;
        }

        // Typical Constructor
        public ChartNote(uint in_location, int in_duration)
        {
            TimeValue = 0;
            TickValue = in_location;
            Duration = in_duration;
        }

        // Test function to view stored information
        public void print_info()
        {
            Console.WriteLine("L = '{0}' D = '{1}' T = '{2}'", TickValue, Duration, TimeValue);
        }

        public int Duration;
    }
}
