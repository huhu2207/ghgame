using Microsoft.Xna.Framework.Input;

namespace MinGH.GameScreenImpl
{
    public interface IKeyboardInputManager
    {
        void processKeyboardState(KeyboardState inputState);
        bool keyWasHit(Keys key);
        bool keyWasHeld(Keys key);
    }
}
