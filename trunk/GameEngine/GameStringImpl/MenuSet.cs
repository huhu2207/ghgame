using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.GameStringImpl
{
    /// <summary>
    /// Manages a set of menus and properly switches between them.
    /// </summary>
    public class MenuSet
    {
        /// <summary>
        /// The list of menus within this set.
        /// </summary>
        public List<Menu> menus { get; set; }

        /// <summary>
        /// The iterator pointing to the currently selected menu.
        /// </summary>
        public int currentlySelectedMenu { get; set; }

        /// <summary>
        /// Wether the set has menus or not.
        /// </summary>
        private bool emptySet { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public MenuSet()
        {
            currentlySelectedMenu = 0;
            menus = new List<Menu>();
            emptySet = true;
        }

        /// <summary>
        /// Adds a menu to the set.
        /// </summary>
        /// <param name="newMenu">The menu to add.</param>
        public void AddMenu(Menu newMenu)
        {
            menus.Add(newMenu);
            if (!emptySet)
            {
                menus[menus.Count - 1].visible = false;
            }
            emptySet = false;
        }

        /// <summary>
        /// Moves game focus onto the next menu in the list.  Nothing happens
        /// if this MenuSet is empty or there are no further menus to move to.
        /// </summary>
        public void SelectNextMenu()
        {
            if ((!emptySet) && (currentlySelectedMenu < menus.Count - 1))
            {
                menus[currentlySelectedMenu].visible = false;
                currentlySelectedMenu++;
                menus[currentlySelectedMenu].visible = true;
            }
        }

        /// <summary>
        /// Moves game focus onto the previous menu in the list.  Nothing happens
        /// if this MenuSet is empty or there are no previous menus to move to.
        /// </summary>
        public void SelectPreviousMenu()
        {
            if ((!emptySet) && (currentlySelectedMenu > 0))
            {
                menus[currentlySelectedMenu].visible = false;
                currentlySelectedMenu--;
                menus[currentlySelectedMenu].visible = true;
            }
        }

        /// <summary>
        /// Calls the currently focused menu to move its selection down one.
        /// </summary>
        /// <param name="windowHeight">The height of the game window.</param>
        public void SelectNextEntryInCurrentMenu(int windowHeight)
        {
                menus[currentlySelectedMenu].SelectNextEntry(windowHeight);
        }

        /// <summary>
        /// Calls the currently focused menu to move its selection up one.
        /// </summary>
        /// <param name="windowHeight">The height of the game window.</param>
        public void SelectPreviousEntryInCurrentMenu(int windowHeight)
        {
                menus[currentlySelectedMenu].SelectPreviousEntry(windowHeight);
        }

        public void draw(SpriteBatch spriteBatch, SpriteFont spriteFont, Rectangle viewport)
        {
            if (!emptySet)
            {
                foreach (Menu currMenu in menus)
                {
                    currMenu.draw(spriteBatch, spriteFont, viewport);
                }
            }
        }
    }
}
