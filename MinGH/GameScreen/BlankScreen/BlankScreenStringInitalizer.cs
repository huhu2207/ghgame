using MinGH.GameStringImpl;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MinGH.GameScreen.BlankScreen
{
    class BlankScreenStringInitalizer
    {
        // This is where the programmer will initialize all strings used in the program
        public static GameStringManager initializeStrings(int screenWidth,
                                                          int screenHeight)
        {
            /* Current Organization of Strings (the value will be added when possible):
             *      [0] = Debug String 1 (used for various debugging values)
             */
            GameStringManager stringManagerToReturn = new GameStringManager();

            stringManagerToReturn.Add(new GameString(new Vector2(screenWidth / 2, screenHeight / 2), Color.White));  // 0

            return stringManagerToReturn;
        }
    }
}
