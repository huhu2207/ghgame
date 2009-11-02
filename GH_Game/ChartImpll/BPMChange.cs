using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MinGH.ChartImpl
{
    // Contains information on a BPM Change
    class BPMChange : Entity
    {
        // Defailt Constructor
        public BPMChange()
        {
            TimeValue = 0;
            TickValue = 0;
            BPMValue = 0;
        }

        // Typical Constructor
        public BPMChange(uint in_location, long in_value)
        {
            TimeValue = 0;
            TickValue = in_location;
            BPMValue = in_value;
        }

        // Test function to view stored information
        public void print_info()
        {
            Console.WriteLine("L = '{0}' V = '{1}'", TickValue, BPMValue);
        }

        public long BPMValue;
    }
}
