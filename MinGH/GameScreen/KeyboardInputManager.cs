using Microsoft.Xna.Framework.Input;
using MinGH.MiscClasses;

namespace MinGH.GameScreen
{
    /// <summary>
    /// See IKeyboardInputManager.
    /// </summary>
    class KeyboardInputManager : IKeyboardInputManager
    {
        KeyboardState currentState, previousState;

        /// <summary>
        /// The manager needs to wait two frames before the keyboard states will
        /// be accurate (i.e. the currentState and previousState need to be filled
        /// out with real information).
        /// </summary>
        private int updatesWaited { get; set; }

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
            if (keyIsHeld(KeyboardConfiguration.orangeFret))
            {
                return KeyboardConfiguration.orangeFret;
            }
            else if (keyIsHeld(KeyboardConfiguration.blueFret))
            {
                return KeyboardConfiguration.blueFret;
            }
            else if (keyIsHeld(KeyboardConfiguration.yellowFret))
            {
                return KeyboardConfiguration.yellowFret;
            }
            else if (keyIsHeld(KeyboardConfiguration.redFret))
            {
                return KeyboardConfiguration.redFret;
            }
            else if (keyIsHeld(KeyboardConfiguration.greenFret))
            {
                return KeyboardConfiguration.greenFret;
            }
            else 
            {
                return Keys.None;
            }
        }

        public Keys[] getCurrentKeyArray()
        {
            return currentState.GetPressedKeys();
        }
    }
}
