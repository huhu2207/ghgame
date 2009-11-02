using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GH_Game.Chart
{
    // Contains information on a BPM Change
    class BPM_Change : Entity
    {
        // Defailt Constructor
        public BPM_Change()
        {

            Location = 0;
            Value = 0;
        }

        // Typical Constructor
        public BPM_Change(uint in_location, long in_value)
        {
            Location = in_location;
            Value = in_value;
        }

        // Test function to view stored information
        public void print_info()
        {
            Console.WriteLine("L = '{0}' V = '{1}'", Location, Value);
        }

        public long Value;
    }
}
