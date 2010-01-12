using System;

namespace MinGH.ChartImpl
{
    /// <summary>
    /// The class that stores information.  Is derived from the Entity class.
    /// </summary>
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
            noteType = new NoteType();
            isHOPO = false;

            noteType.Green = false;
            noteType.Red = false;
            noteType.Yellow = false;
            noteType.Blue = false;
            noteType.Orange = false;
            noteType.SP = false;
        }

        /// <summary>
        /// Creates a ChartNote with a valid location, duration and note column.
        /// </summary>
        /// <param name="inTickValue">
        /// The tick value (location) associated with the new BPM change.
        /// </param>
        /// <param name="inDuration">
        /// The duration of the new note.
        /// </param>
        /// <param name="noteColumn">
        /// Note to add (0 = green, 4 = orange).
        /// </param>
        public ChartNote(uint inTickValue, int inDuration, int noteColumn)
        {
            TimeValue = 0;
            TickValue = inTickValue;
            Duration = inDuration;

            noteType = new NoteType();

            addNote(noteColumn);
        }

        /// <summary>
        /// Adds a note to this chart note (more than one note in a chartnote
        /// effectively makes this a chord).
        /// </summary>
        /// <param name="noteColumn">
        /// Note to add (0 = green, 4 = orange, 5 = SP).
        /// </param>
        public void addNote(int noteColumn)
        {
            if (noteColumn == 0)
                noteType.Green = true;
            if (noteColumn == 1)
                noteType.Red = true;
            if (noteColumn == 2)
                noteType.Yellow = true;
            if (noteColumn == 3)
                noteType.Blue = true;
            if (noteColumn == 4)
                noteType.Orange = true;
            if (noteColumn == 5)
                noteType.SP = true;

            if (getNoteCount() > 1)
                isChord = true;
        }

        /// <summary>
        /// Gets the number of notes in this current chartnote.
        /// </summary>
        /// <returns>
        /// How many notes in this chartnote.
        /// </returns>
        public int getNoteCount()
        {
            int numberCount = 0;

            if (noteType.Green)
                numberCount++;
            if (noteType.Red)
                numberCount++;
            if (noteType.Yellow)
                numberCount++;
            if (noteType.Blue)
                numberCount++;
            if (noteType.Orange)
                numberCount++;

            return numberCount;
        }

        /// <summary>
        /// Gets the nth active note in this chartnote (i.e. 2nd note in a GRB chord is R)
        /// NOTE: -1 is returned if no note is set, but should never happen
        /// </summary>
        /// <param name="n">
        /// What degree of note to get.
        /// </param>
        /// <returns>
        /// The note column on the nth note (0 = green, etc.)
        /// </returns>
        public int getNthNote(int n)
        {
            if (noteType.Green)
            {
                if (n == 0)
                    return 0;
                else
                    n--;
            }
            if (noteType.Red)
            {
                if (n == 0)
                    return 1;
                else
                    n--;
            }
            if (noteType.Yellow)
            {
                if (n == 0)
                    return 2;
                else
                    n--;
            }
            if (noteType.Blue)
            {
                if (n == 0)
                    return 3;
                else
                    n--;
            }
            if (noteType.Orange)
            {
                if (n == 0)
                    return 4;
                else
                    n--;
            }

            return -1;
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
        public int Duration { get; set; }

        /// <summary>
        /// What kind of note this is (i.e. Red, Blue, SP, etc.).
        /// </summary>
        public NoteType noteType { get; set; }

        /// <summary>
        /// Is this note a hammeron/pulloff?
        /// </summary>
        public bool isHOPO { get; set; }

        /// <summary>
        /// Is this note a part of a chord?
        /// </summary>
        public bool isChord { get; set; }
    }
}
