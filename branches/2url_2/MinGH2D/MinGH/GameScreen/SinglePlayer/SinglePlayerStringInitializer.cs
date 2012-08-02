﻿using GameEngine.GameStringImpl;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MinGH.GameScreen.SinglePlayer
{
    /// <summary>
    /// Static class that initializes the strings on a single player screen.
    /// </summary>
    class SinglePlayerStringInitializer
    {
        /// <summary>
        /// This is where the programmer will initialize all strings used in a single player screen.
        /// </summary>
        /// <param name="screenWidth">The width of the game window.</param>
        /// <param name="screenHeight">The height of the game window.</param>
        /// <returns>A filled out game string manager.</returns>
        public static GameStringManager initializeStrings(int screenWidth,
                                                          int screenHeight)
        {
            /* Current Organization of Strings (the value will be added when possible):
             *      [0] = Debug String 1 (used for various debugging values)
             *      [1] = see [0]
             *      [2] = Song Information
             *      [3] = Player Score, multiplier and combo
             *      [4] = Player Health
             */
            GameStringManager stringManagerToReturn = new GameStringManager();

            stringManagerToReturn.Add(new GameString(new Vector2(80f, 30f), Color.White));  // 0
            stringManagerToReturn.Add(new GameString(new Vector2(52f, 92f), Color.White));  // 1
            stringManagerToReturn.Add(new GameString(new Vector2(screenWidth - 80f, 120f), Color.White));  // 2
            stringManagerToReturn.Add(new GameString(new Vector2(screenWidth - 80f, screenHeight - 100f), Color.White));  // 3
            stringManagerToReturn.Add(new GameString(new Vector2(80f, screenHeight - 100f), Color.White));  // 4

            return stringManagerToReturn;
        }
    }
}
