namespace MinGH.ChartImpl
{
    /// <summary>
    /// Class that designates what buttons are in a particular note (e.g. a GY chord would
    /// have green = true and yellow = true, while a single green note would just be
    /// green = true).
    /// </summary>
    public class NoteType
    {
        public bool Green { get; set; }
        public bool Red { get; set; }
        public bool Yellow { get; set; }
        public bool Blue { get; set; }
        public bool Orange { get; set; }
        public bool SP { get; set; }

        public NoteType()
        {
            Green = false;
            Red = false;
            Yellow = false;
            Blue = false;
            Orange = false;
            SP = false;
        }

        public bool isEqual(NoteType other)
        {
            if ((Green == other.Green) &&
                (Red == other.Red) &&
                (Yellow == other.Yellow) &&
                (Blue == other.Blue) &&
                (Orange == other.Orange))
            {
                return true;
            }
            else
            { 
                return false;
            }
        }
    }
}
