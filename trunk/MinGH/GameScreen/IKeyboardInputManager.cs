using Microsoft.Xna.Framework.Input;
using MinGH.MiscClasses;

namespace MinGH.GameScreen
{
	/// <summary>
	/// A wrapper that allows for easy checking for keyboard keys
	/// </summary>
    public interface IKeyboardInputManager
    {
        /// <summary>
        /// Simply moves the currentState into the previousState and puts inputState
        /// into currentState.
        /// </summary>
        /// <param name="inputState">The new state for the keyboard.</param>
        void processKeyboardState(KeyboardState inputState);

        /// <summary>
        /// Returns the current keyboard state.
        /// </summary>
        /// <returns>currentState.</returns>
        KeyboardState getCurrentState();

        /// <summary>
        /// Checks if key was hit during the check.
        /// </summary>
        /// <param name="key">The key the user wishes to check for.</param>
        /// <returns>True or False.</returns>
        bool keyIsHit(Keys key);
        
        /// <summary>
        /// Checks if key was hit and held down during the check.
        /// </summary>
        /// <param name="key">The key the user wishes to check for.</param>
        /// <returns>True or False.</returns>
        bool keyIsHeld(Keys key);

        /// <summary>
        /// Checks if key was held down immediately before the check.
        /// </summary>
        /// <param name="key">The key the user wishes to check for.</param>
        /// <returns>True or False.</returns>
        bool keyWasHeld(Keys key);

        /// <summary>
        /// Returns the highest held key in regards to the GH button precedence
        /// (e.g. red is less than blue)
        /// </summary>
        /// <returns>The key value.</returns>
        Keys getHighestHeldKey();

        /// <summary>
        /// Returns an array containing every key that is hit on a the current frame.
        /// </summary>
        /// <returns>An array of keys.</returns>
        Keys[] getCurrentKeyArray();
    }
}
