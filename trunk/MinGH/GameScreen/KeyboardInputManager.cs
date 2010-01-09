using Microsoft.Xna.Framework.Input;
using MinGH.MiscClasses;

namespace MinGH.GameScreen
{
    class KeyboardInputManager : IKeyboardInputManager
    {
        KeyboardState currentState, previousState;

        /// <summary>
        /// The manager needs to wait two frames before the keyboard states will
        /// be accurate (i.e. the currentState and previousState need to be filled
        /// out with real information).
        /// </summary>
        private int updatesWaited;

        public KeyboardInputManager()
        {
            currentState = new KeyboardState();
            previousState = new KeyboardState();
            updatesWaited = 0;
        }

        public void processKeyboardState(KeyboardState inputState)
        {
            previousState = currentState;
            currentState = inputState;
            if (updatesWaited < 2)
            {
                updatesWaited++;
            }
        }

        public KeyboardState getCurrentState()
        {
            return currentState;
        }

        public bool keyIsHit(Keys key)
        {
            if (updatesWaited >= 2)
            {
                return ((currentState.IsKeyDown(key) && previousState.IsKeyUp(key)));
            }
            else
            {
                return false;
            }
        }

        public bool keyIsHeld(Keys key)
        {
            if (updatesWaited >= 2)
            {
                // Check is the key was either already held down, or just hit
                return ((currentState.IsKeyDown(key) && previousState.IsKeyDown(key)) || keyIsHit(key));
            }
            else
            {
                return false;
            }
        }

        public bool keyWasHeld(Keys key)
        {
            if (updatesWaited <= 2)
            {
                return (currentState.IsKeyUp(key) && previousState.IsKeyDown(key));
            }
            else
            {
                return false;
            }
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
