using Microsoft.Xna.Framework.Input;

namespace MinGH.GameScreenImpl
{
	/// <remarks>
	/// A wrapper that allows for easy checking for keyboard keys
	/// </remarks>
    public interface IKeyboardInputManager
    {
        /// <summary>
        /// Simply moves the currentState into the previousState and puts inputState
        /// into currentState.
        /// </summary>
        /// <param name="inputState">
        /// The new state for the keyboard.
        /// </param>
        void processKeyboardState(KeyboardState inputState);

        /// <summary>
        /// Checks if key was hit during the check.
        /// </summary>
        /// <param name="key">
        /// The key the user wishes to check for.
        /// </param>
        /// <returns>
        /// True or False.
        /// </returns>
        bool keyIsHit(Keys key);
        
        /// <summary>
        /// Checks if key was hit and held down during the check.
        /// </summary>
        /// <param name="key">
        /// The key the user wishes to check for.
        /// </param>
        /// <returns>
        /// True or False.
        /// </returns>
        bool keyIsHeld(Keys key);

        /// <summary>
        /// Checks if key was held down immediately before the check.
        /// </summary>
        /// <param name="key">
        /// The key the user wishes to check for.
        /// </param>
        /// <returns>
        /// True or False.
        /// </returns>
        bool keyWasHeld(Keys key);
    }
}
