using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MinGH.ChartImpl
{
    // Contains information on an individual event
    class Event : Entity
    {
        // Default Constructor
        public Event()
        {
            TimeValue = 0;
            TickValue = 0;
            Value = "default";
        }

        // typical Constructor
        public Event(uint in_location, string in_value)
        {
            TimeValue = 0;
            TickValue = in_location;
            Value = in_value;
        }

        // Test function to view stored information
        public void print_info()
        {
            Console.WriteLine("L = '{0}' V = '{1}'", TickValue, Value);
        }

        public string Value;
    }
}