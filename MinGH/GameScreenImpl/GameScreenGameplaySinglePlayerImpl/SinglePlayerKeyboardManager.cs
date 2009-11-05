using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace MinGH.GameScreenImpl.GameScreenGameplaySinglePlayerImpl
{
    class SinglePlayerKeyboardManager : IKeyboardInputManager
    {
        KeyboardState currentState, previousState;

        public SinglePlayerKeyboardManager()
        {
            currentState = new KeyboardState();
            previousState = new KeyboardState();
        }

        void processInput(KeyboardState inputState)
        {
            previousState = currentState;
            currentState = inputState;
        }
    }
}
