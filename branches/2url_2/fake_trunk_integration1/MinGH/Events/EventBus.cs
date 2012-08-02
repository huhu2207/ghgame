namespace MinGH.Events
{
    /// <summary>
    /// A simple eventbus implementation based on Perry Birch's blog post.
    /// With the current implementation, there will be a separate static
    /// instance of the event bus for each passed in type.
    /// <see cref="http://perrybirch.blogspot.com/2007/06/generic-message-bus.html"/>
    /// </summary>
    public class EventBus<T>
    {
        private EventBus() {}

        private static readonly object _lock = new object();
        
        /// <summary>
        /// Gets the instance of the event bus for the provided type
        /// OOGA BOOGA!!!!
        /// </summary>
        public static EventBus<T> instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                        _instance = new EventBus<T>();
                    return _instance;
                }
            }
        }
        private static EventBus<T> _instance = null;

        public delegate void EventHandler(object sender, T contents);
        public event EventHandler Event;

        /// <summary>
        /// Fires a new event containing the provided contents
        /// </summary>
        /// <param name="sender">A reference to the object firing the event</param>
        /// <param name="contents">Any contents needed for the event</param>
        public void fireEvent(object sender, T contents)
        {
            Event(sender, contents);
        }
    }
}
