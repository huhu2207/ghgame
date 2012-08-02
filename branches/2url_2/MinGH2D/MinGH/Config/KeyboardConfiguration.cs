using Microsoft.Xna.Framework.Input;

namespace MinGH.Config
{
    /// <summary>
    /// Assigns the specified keys to a universal key notation for MinGH.
    /// </summary>
    public static class KeyboardConfiguration
    {
        // Guitar related buttons
        public const Keys greenFret = Keys.A;
        public const Keys redFret = Keys.S;
        public const Keys yellowFret = Keys.D;
        public const Keys blueFret = Keys.F;
        public const Keys orangeFret = Keys.G;
        public const Keys upStrum = Keys.Up;
        public const Keys downStrum = Keys.Down;
        public const Keys starPower = Keys.RightShift;

        // Drum related buttons
        public const Keys redDrum = Keys.X;
        public const Keys yellowDrum = Keys.C;
        public const Keys blueDrum = Keys.V;
        public const Keys greenDrum = Keys.B;
        public const Keys bassPedal = Keys.Z;

        /// <summary>
        /// Converts an integer into a designated key.
        /// </summary>
        /// <param name="num">The number to convert.</param>
        /// <returns>The key value for the inputted number.</returns>
        public static Keys getGuitarKeyFromNumber(int num)
        {
            if (num == 0)
                return greenFret;
            if (num == 1)
                return redFret;
            if (num == 2)
                return yellowFret;
            if (num == 3)
                return blueFret;
            if (num == 4)
                return orangeFret;

            return Keys.None;
        }

        /// <summary>
        /// Converts a key to the designated number.
        /// </summary>
        /// <param name="key">The key to convert.</param>
        /// <returns>The integer value for the inputted key.</returns>
        public static int getGuitarNumberFromKey(Keys key)
        {
            if (key == greenFret)
                return 0;
            if (key == redFret)
                return 1;
            if (key == yellowFret)
                return 2;
            if (key == blueFret)
                return 3;
            if (key == orangeFret)
                return 4;

            return -1;
        }

        /// <summary>
        /// Converts an integer into a designated key.
        /// </summary>
        /// <param name="num">The number to convert.</param>
        /// <returns>The key value for the inputted number.</returns>
        public static Keys getDrumKeyFromNumber(int num)
        {
            if (num == 0)
                return bassPedal;
            if (num == 1)
                return redDrum;
            if (num == 2)
                return yellowDrum;
            if (num == 3)
                return blueDrum;
            if (num == 4)
                return greenDrum;

            return Keys.None;
        }

        /// <summary>
        /// Converts a key to the designated number.
        /// </summary>
        /// <param name="key">The key to convert.</param>
        /// <returns>The integer value for the inputted key.</returns>
        public static int getDrumNumberFromKey(Keys key)
        {
            if (key == bassPedal)
                return 0;
            if (key == redDrum)
                return 1;
            if (key == yellowDrum)
                return 2;
            if (key == blueDrum)
                return 3;
            if (key == greenDrum)
                return 4;

            return -1;
        }

    }
}
