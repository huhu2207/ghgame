using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace Chart_View
{
    // Timer class that counts via miliseconds
    class Timer
    {
        // Default constructor
        public Timer()
        {
            timer_name = "default";
            current_time = 0.0f;
            end_value = 1.0f;
        }

        // Typical constructor
        public Timer(string input_name)
        {
            timer_name = input_name;
            current_time = 0.0f;
            end_value = 1.0f;
        }

        // Updates the counter
        public void Update(double in_time)
        {
            current_time += in_time;
        }

        public string timer_name;
        public double current_time;
        public double end_value;
    }
}
