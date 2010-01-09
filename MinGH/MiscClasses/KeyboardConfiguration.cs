using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace MinGH.MiscClasses
{
    public class KeyboardConfiguration
    {
        public static Keys green = Keys.A;
        public static Keys red = Keys.S;
        public static Keys yellow = Keys.D;
        public static Keys blue = Keys.F;
        public static Keys orange = Keys.G;
        public static Keys upStrum = Keys.Up;
        public static Keys downStrum = Keys.Down;
        public static Keys starPower = Keys.RightShift;

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
