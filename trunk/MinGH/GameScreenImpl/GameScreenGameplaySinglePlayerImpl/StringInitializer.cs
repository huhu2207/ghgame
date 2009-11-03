using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MinGH.GameStringImpl;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MinGH.GameScreenImpl.GameScreenGameplaySinglePlayerImpl
{
    class StringInitializer
    {
        // This is where the programmer will initialize all strings used in the program
        public static void initializeStrings(ref GameStringManager str_manager, int screen_width,
                                              int screen_height)
        {
            /* Current Organization of Strings (the value will be added when possible):
             *      [0] = Debug String 1 (used for various debugging values)
             *      [1] = see [0]
             *      [2] = Song Title
             *      [3] = Artist Title
             */
            str_manager.Add(new GameString(new Vector2(80f, 30f), Color.White));  // 0
            str_manager.Add(new GameString(new Vector2(52f, 92f), Color.White));  // 1
            str_manager.Add(new GameString(new Vector2((float)screen_width - 90f,
                                           30f), Color.White));  // 2
            str_manager.Add(new GameString(new Vector2((float)screen_width - 80f,
                                           90f), Color.White));  // 3
        }
    }
}
