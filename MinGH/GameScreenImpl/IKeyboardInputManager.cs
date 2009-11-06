using Microsoft.Xna.Framework.Input;

namespace MinGH.GameScreenImpl
{
    public interface IKeyboardInputManager
    {
        // Copies currentState into the previousState
        // and sets inputState as currentState
        void processKeyboardState(KeyboardState inputState);

        // Returns true if a key was not down in the previous state,
        // but is down in the current state
        bool keyIsHit(Keys key);
        
        // Returns true if either KeyIsHit is true, or if a key is down in both
        // previousState and currentState
        bool keyIsHeld(Keys key);

        // Returns true if a key was down in the previous state, but is up in the
        // current state.  Can double as a keyWasHit function.
        bool keyWasHeld(Keys key);
    }
}
