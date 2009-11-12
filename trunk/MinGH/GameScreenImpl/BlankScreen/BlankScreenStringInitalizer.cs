using MinGH.GameStringImpl;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MinGH.GameScreenImpl.BlankScreen
{
    class BlankScreenStringInitalizer
    {
        // This is where the programmer will initialize all strings used in the program
        public static void initializeStrings(ref GameStringManager str_manager, int screenWidth,
                                              int screenHeight)
        {
            /* Current Organization of Strings (the value will be added when possible):
             *      [0] = Debug String 1 (used for various debugging values)
             */
            str_manager.Add(new GameString(new Vector2(screenWidth / 2, screenHeight / 2), Color.White));  // 0
        }
    }
}
