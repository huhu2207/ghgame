using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MinGH.GameScreenImpl.GameScreenGameplaySinglePlayerImpl
{
    // This hitbox is a thin rectangle spanning across the entire screen
    // from left to right.
    class HorizontalHitBox
    {
        Rectangle physicalHitbox;
        int centerLocation;  // We are only concerned with the y axis, so no need for a point structure
        const int greatThreshold = 25;  // How far away from the center a great hit will be
        int goodThreshold = 50;  // How far away from the center a good hit will be

        // Nearly pointless default constructor
        // NOTE: Do we need this?
        public HorizontalHitBox()
        {
            physicalHitbox = new Rectangle();
            centerLocation = 0;
        }

        // The proper constructor
        public HorizontalHitBox(Rectangle gameScreenRectangle)
        {
            physicalHitbox = gameScreenRectangle;
            // The hit box center is at 85% towards the bottom.
            // NOTE: we convert to int...this may introduce slight error on some resolutions
            centerLocation = (int)(gameScreenRectangle.Height * 0.85);
        }
    }
}
