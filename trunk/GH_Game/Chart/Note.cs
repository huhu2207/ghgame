using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GH_Game.Chart
{
    // Contains information on an individual note
    class Note : Entity
    {
        // Defailt Constructor
        public Note()
        {
            TimeValue = 0;
            TickValue = 0;
            Duration = 0;
        }

        // Typical Constructor
        public Note(uint in_location, int in_duration)
        {
            TimeValue = 0;
            TickValue = in_location;
            Duration = in_duration;
        }

        // Test function to view stored information
        public void print_info()
        {
            Console.WriteLine("L = '{0}' D = '{1}'", TickValue, Duration);
        }

        public int Duration;
    }
}
