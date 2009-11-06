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

        public void processKeyboardState(KeyboardState inputState)
        {
            previousState = currentState;
            currentState = inputState;
        }

        public bool keyWasHit(Keys key)
        {
            if ((currentState.IsKeyDown(key) && previousState.IsKeyUp(key)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool keyWasHeld(Keys key)
        {
            // Check is the key was either already held down, or just hit
            if ((currentState.IsKeyDown(key) && previousState.IsKeyDown(key)) || keyWasHit(key))
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
