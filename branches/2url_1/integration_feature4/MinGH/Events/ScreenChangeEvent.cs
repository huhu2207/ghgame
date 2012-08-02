using System;
using MinGH.Enum;

namespace MinGH.Events
{
    public class ScreenChange : EventArgs
    {
        private readonly GameState _newState;

        public ScreenChange(GameState newState)
        {
            _newState = newState;
        }

        public GameState newState
        {
            get { return _newState; }
        }
    }
}
