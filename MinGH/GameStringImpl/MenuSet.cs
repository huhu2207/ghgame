using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MinGH.GameStringImpl
{
    class MenuSet
    {
        public List<Menu> menus { get; set; }
        public int currentlySelectedMenu { get; set; }
        private bool emptySet { get; set; }

        public MenuSet()
        {
            currentlySelectedMenu = 0;
            menus = new List<Menu>();
            emptySet = true;
        }

        public void AddMenu(Menu newMenu)
        {
            menus.Add(newMenu);
            if (!emptySet)
            {
                menus[menus.Count - 1].visible = false;
            }
            emptySet = false;
        }

        public void SelectNextMenu()
        {
            if ((!emptySet) && (currentlySelectedMenu < menus.Count - 1))
            {
                menus[currentlySelectedMenu].visible = false;
                currentlySelectedMenu++;
                menus[currentlySelectedMenu].visible = true;
            }
        }

        public void SelectPreviousMenu()
        {
            if ((!emptySet) && (currentlySelectedMenu > 0))
            {
                menus[currentlySelectedMenu].visible = false;
                currentlySelectedMenu--;
                menus[currentlySelectedMenu].visible = true;
            }
        }

        public void SelectNextEntryInCurrentMenu(int windowHeight)
        {
                menus[currentlySelectedMenu].SelectNextEntry(windowHeight);
        }

        public void SelectPreviousEntryInCurrentMenu(int windowHeight)
        {
                menus[currentlySelectedMenu].SelectPreviousEntry(windowHeight);
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            if (!emptySet)
            {
                foreach (Menu currMenu in menus)
                {
                    currMenu.Draw(spriteBatch, spriteFont);
                }
            }
        }
    }
}
