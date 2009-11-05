using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace MinGH.GameScreenImpl
{
    public interface IKeyboardInputManager
    {
        void processKeyboardState(KeyboardState inputState);
        bool keyWasHit(Keys key);
    }
}
