
namespace MinGH
{
    /// <remarks>
    /// A basic timer class.
    /// </remarks>
    class Timer
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public Timer()
        {
            timer_name = "default";
            current_time = 0;
            end_value = 1;
            is_up = false;
        }

        /// <summary>
        /// A constructor that initializes the timer's name
        /// </summary>
        /// <param name="input_name">
        /// The name for the new timer.
        /// </param>
        public Timer(string input_name)
        {
            timer_name = input_name;
            current_time = 0;
            end_value = 1;
            is_up = false;
        }

        /// <summary>
        /// Updates the counter according to the input value.
        /// </summary>
        /// <param name="in_time">
        /// The value to be added to the current time.
        /// </param>
        public void Update(double in_time)
        {
            current_time += in_time;

            // If the timer passes the end value, set the "is_up" value to true
            if (current_time >= end_value)
                is_up = true;
        }

		/// <summary>
		/// The name/description of the timer.
		/// </summary>
        public string timer_name;
		
		/// <summary>
		/// The current value of the timer.
		/// </summary>
        public double current_time;
		
		/// <summary>
		/// The upper bound to the timer.
		/// </summary>
        public double end_value;
		
		/// <summary>
		/// Tells the programmer if the timer is up.
		/// </summary>
        public bool is_up;  
    }
}
