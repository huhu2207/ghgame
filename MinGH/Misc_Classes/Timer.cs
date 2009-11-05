
namespace MinGH
{
    // Timer class that counts via miliseconds
    class Timer
    {
        // Default constructor
        public Timer()
        {
            timer_name = "default";
            current_time = 0;
            end_value = 1;
            is_up = false;
        }

        // Typical constructor
        public Timer(string input_name)
        {
            timer_name = input_name;
            current_time = 0;
            end_value = 1;
            is_up = false;
        }

        // Updates the counter
        public void Update(double in_time)
        {
            current_time += in_time;

            // If the timer passes the end value, set the "is_up" value to true
            if (current_time >= end_value)
                is_up = true;
        }

        public string timer_name;  // The name/description of the timer
        public double current_time;  // The current value of the timer
        public double end_value;  // When the timer is considered "done"
        public bool is_up;  // Shows if the timer is done counting
    }
}
