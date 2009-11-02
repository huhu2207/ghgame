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
            Location = 0;
            TickValue = 0;
            Duration = 0;
        }

        // Typical Constructor
        public Note(uint in_location, int in_duration)
        {
            Location = in_location;
            Duration = in_duration;
        }

        public Note(uint in_location, int in_duration, uint inTickValue)
        {
            Location = in_location;
            Duration = in_duration;
            TickValue = inTickValue;
        }

        // Test function to view stored information
        public void print_info()
        {
            Console.WriteLine("L = '{0}' D = '{1}'", Location, Duration);
        }

        public int Duration;
    }
}
