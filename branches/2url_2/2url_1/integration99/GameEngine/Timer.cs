namespace GameEngine
{
    /// <summary>
    /// A basic timer class.
    /// </summary>
    class Timer
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public Timer()
        {
            timerName = "default";
            currentTime = 0;
            endValue = 1;
            isUp = false;
        }

        /// <summary>
        /// A constructor that initializes the timer's name
        /// </summary>
        /// <param name="input_name">
        /// The name for the new timer.
        /// </param>
        public Timer(string input_name)
        {
            timerName = input_name;
            currentTime = 0;
            endValue = 1;
            isUp = false;
        }

        /// <summary>
        /// Updates the counter according to the input value.
        /// </summary>
        /// <param name="in_time">
        /// The value to be added to the current time.
        /// </param>
        public void Update(double in_time)
        {
            currentTime += in_time;

            // If the timer passes the end value, set the "is_up" value to true
            if (currentTime >= endValue)
                isUp = true;
        }

		/// <summary>
		/// The name/description of the timer.
		/// </summary>
        public string timerName { get; set; }
		
		/// <summary>
		/// The current value of the timer.
		/// </summary>
        public double currentTime { get; set; }
		
		/// <summary>
		/// The upper bound to the timer.
		/// </summary>
        public double endValue { get; set; }
		
		/// <summary>
		/// Tells the programmer if the timer is up.
		/// </summary>
        public bool isUp { get; set; }  
    }
}
