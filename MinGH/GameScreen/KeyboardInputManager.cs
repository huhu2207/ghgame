using Microsoft.Xna.Framework.Input;
using MinGH.Misc_Classes;

namespace MinGH.GameScreen
{
    class KeyboardInputManager : IKeyboardInputManager
    {
        KeyboardState currentState, previousState;

        public KeyboardInputManager()
        {
            currentState = new KeyboardState();
            previousState = new KeyboardState();
        }

        public void processKeyboardState(KeyboardState inputState)
        {
            previousState = currentState;
            currentState = inputState;
        }

        public KeyboardState getCurrentState()
        {
            return currentState;
        }

        public bool keyIsHit(Keys key)
        {
            return ((currentState.IsKeyDown(key) && previousState.IsKeyUp(key)));
        }

        public bool keyIsHeld(Keys key)
        {
            // Check is the key was either already held down, or just hit
            return ((currentState.IsKeyDown(key) && previousState.IsKeyDown(key)) || keyIsHit(key));
        }

        public bool keyWasHeld(Keys key)
        {
            return (currentState.IsKeyUp(key) && previousState.IsKeyDown(key));
        }

        public Keys getHighestHeldKey()
        {
            if (keyIsHeld(KeyboardConfiguration.orange))
            {
                return KeyboardConfiguration.orange;
            }
            else if (keyIsHeld(KeyboardConfiguration.blue))
            {
                return KeyboardConfiguration.blue;
            }
            else if (keyIsHeld(KeyboardConfiguration.yellow))
            {
                return KeyboardConfiguration.yellow;
            }
            else if (keyIsHeld(KeyboardConfiguration.red))
            {
                return KeyboardConfiguration.red;
            }
            else if (keyIsHeld(KeyboardConfiguration.green))
            {
                return KeyboardConfiguration.green;
            }
            else 
            {
                return Keys.None;
            }
        }
    }
}
