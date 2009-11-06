using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MinGH.GameStringImpl;

namespace MinGH.GameScreenImpl.GameScreenGameplaySinglePlayerImpl
{
    class SinglePlayerStringInitializer
    {
        // This is where the programmer will initialize all strings used in the program
        public static void initializeStrings(ref GameStringManager str_manager, int screenWidth,
                                              int screenHeight)
        {
            /* Current Organization of Strings (the value will be added when possible):
             *      [0] = Debug String 1 (used for various debugging values)
             *      [1] = see [0]
             *      [2] = Song Title
             *      [3] = Artist Title
             *      [4] = Player Score, multiplier and combo
             *      [5] = Player Health
             */
            str_manager.Add(new GameString(new Vector2(80f, 30f), Color.White));  // 0
            str_manager.Add(new GameString(new Vector2(52f, 92f), Color.White));  // 1
            str_manager.Add(new GameString(new Vector2(screenWidth - 90f, 30f), Color.White));  // 2
            str_manager.Add(new GameString(new Vector2(screenWidth - 80f, 90f), Color.White));  // 3
            str_manager.Add(new GameString(new Vector2(screenWidth - 80f, screenHeight - 100f), Color.White));  // 4
            str_manager.Add(new GameString(new Vector2(80f, screenHeight - 100f), Color.White));  // 5

        }
    }
}
