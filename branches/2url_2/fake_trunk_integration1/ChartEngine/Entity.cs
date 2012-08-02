namespace ChartEngine
{ 
	/// <summary>
	/// An abstract base class for the various chart elements (i.e. NotechartNote, Event)
	/// </summary>
    public abstract class Entity
    {
		/// <summary>
		/// The location value in ticks (a notation used in the MIDI arcetecture).
		/// </summary>
        public uint tickValue { get; set; }
		
		/// <summary>
		/// The time value of the particular entity (i.e. at what exact milisecond does it show up in the chart).
		/// </summary>
        public uint timeValue { get; set; }
    }
}
