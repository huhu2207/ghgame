using Microsoft.Xna.Framework.Input;

namespace MinGH.MiscClasses
{
    /// <summary>
    /// Assigns the specified keys to a universal key notation for MinGH.
    /// </summary>
    public static class KeyboardConfiguration
    {
        public const Keys green = Keys.A;
        public const Keys red = Keys.S;
        public const Keys yellow = Keys.D;
        public const Keys blue = Keys.F;
        public const Keys orange = Keys.G;
        public const Keys upStrum = Keys.Up;
        public const Keys downStrum = Keys.Down;
        public const Keys starPower = Keys.RightShift;

        /// <summary>
        /// Converts an integer into a designated key.
        /// </summary>
        /// <param name="num">The number to convert.</param>
        /// <returns>The key value for the inputted number.</returns>
        public static Keys getKey(int num)
        {
            if (num == 0)
                return green;
            if (num == 1)
                return red;
            if (num == 2)
                return yellow;
            if (num == 3)
                return blue;
            if (num == 4)
                return orange;

            return Keys.None;
        }
    }
}
