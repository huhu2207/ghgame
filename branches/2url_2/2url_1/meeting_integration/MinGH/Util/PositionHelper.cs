using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using FlatRedBall;
using FlatRedBall.Gui;

namespace MinGH.Util
{
    /// <summary>
    /// A set of helper functions for placing objects onto the screen
    /// </summary>
    class PositionHelper
    {
        /// <summary>
        /// Returns the desired coordinates according to the input.  The two
        /// values are expected to be percent values.  Due to FRB's unique
        /// coordinate system for normal objects (center of screen at start
        /// is 0,0), the percentage values provided may not perform as initally
        /// expected.  An X value of 50 will actually put the object approx
        /// 25% from the top of the screen and 75% from the bottom.
        /// </summary>
        /// <param name="x">The distance from the center to the edge of the
        /// screen in percentage form (e.g. 50 = half way to the right)</param>
        /// <param name="y">The distance from the center to the edge of the
        /// screen in percentage form (e.g. 50 = half way to the top)</param>
        /// <returns></returns>
        public static Vector2 percentToCoordSprite(float x, float y)
        {
            Camera camera = SpriteManager.Camera;
            float right = camera.AbsoluteRightXEdgeAt(0);
            float left = camera.AbsoluteLeftXEdgeAt(0);
            float top = camera.AbsoluteTopYEdgeAt(0);
            float bottom = camera.AbsoluteBottomYEdgeAt(0);
            float width = Math.Abs(right - left);
            float height = Math.Abs(bottom - top);

            Vector2 retVal = new Vector2();
            retVal.X = camera.X + (width / 2 * x / 100);
            retVal.Y = camera.Y + (height / 2 * y / 100);
            return retVal;
        }

        /// <summary>
        /// Places GUI elements at an arbitrary position on the screen determined
        /// by percentages.  Note that FRB's GUI coordinate system is different than the
        /// sprite coordinate system.  The GUI system is more in line with the
        /// traditional "0,0 is the top left and right and down is positive"
        /// mentality.
        /// </summary>
        /// <param name="x">The distance from the left to the right edge of the
        /// screen in percentage form (e.g. 50 = right in the middle)</param>
        /// <param name="y">The distance from the top to the bottom of the
        /// screen in percentage form (e.g. 50 = right in the middle)</param>
        /// <returns></returns>
        public static Vector2 percentToCoordGUI(float x, float y)
        {

            float width = GuiManager.XEdge * 2;
            float height = GuiManager.YEdge * 2;

            Vector2 retVal = new Vector2();
            retVal.X = width * x / 100;
            retVal.Y = height * y / 100;
            return retVal;
        }
    }
}
