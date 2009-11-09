using System;

namespace MinGH.ChartImpl
{
    /// <remarks>
    /// The class that stores information.  Is derived from the Entity class.
    /// </remarks>
    class ChartNote : Entity
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public ChartNote()
        {
            TimeValue = 0;
            TickValue = 0;
            Duration = 0;
            noteType = NoteType.Green;
        }

        /// <summary>
        /// Creates a ChartNote with a valid location and duration.
        /// </summary>
        /// <param name="inTickValue">
        /// The tick value (location) associated with the new BPM change.
        /// </param>
        /// <param name="inDuration">
        /// The duration of the new note.
        /// </param>
        public ChartNote(uint inTickValue, int inDuration, NoteType inNoteType)
        {
            TimeValue = 0;
            TickValue = inTickValue;
            Duration = inDuration;
            noteType = inNoteType;
        }

        /// <summary>
        /// Prints the various information this note contains.
        /// </summary>
        public void print_info()
        {
            Console.WriteLine("L = '{0}' D = '{1}' T = '{2}'", TickValue, Duration, TimeValue);
        }

		/// <summary>
		/// The actual length of the note (i.e. how long the player will have to hold the proper button down).
		/// </summary>
        public int Duration;

        /// <summary>
        /// What kind of note this is (i.e. Red, Blue, SP, etc.).
        /// </summary>
        public NoteType noteType;
    }
}
