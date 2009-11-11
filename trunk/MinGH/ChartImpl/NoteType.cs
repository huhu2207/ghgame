namespace MinGH.ChartImpl
{
    public class NoteType
    {
        public bool Green;
        public bool Red;
        public bool Yellow;
        public bool Blue;
        public bool Orange;
        public bool SP;

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
