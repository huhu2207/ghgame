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
        public Rectangle physicalHitbox;
        public int centerLocation;  // We are only concerned with the y axis, so no need for a point structure
        public const int greatThreshold = 50;  // How far away from the center a great hit will be

        // How far away from the center a good hit will be
        // NOTE: The goodThreshold * 2 is the total height of the hitbox
        public const int goodThreshold = 100;

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
            // The hit box center is at 85% towards the bottom.
            // NOTE: we convert to int...this may introduce slight error on some resolutions
            centerLocation = (int)(gameScreenRectangle.Height * 0.85);

            physicalHitbox = new Rectangle
            {
                Width = gameScreenRectangle.Width,
                Height = HorizontalHitBox.goodThreshold,
                Y = centerLocation - (HorizontalHitBox.goodThreshold / 2),
                X = 0
            };
        }
    }
}
