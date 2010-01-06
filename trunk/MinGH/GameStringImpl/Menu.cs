using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MinGH.GameStringImpl
{
    /// <summary>
    /// Class designed to contain the creating, drawing and logic of a very simple menu system
    /// </summary>
    class Menu
    {
        /// <summary>
        /// Specifies the currently selected option
        /// </summary>
        int currentlySelectedOption { get; set; }

        /// <summary>
        /// The vertical pixel size of the font being used (do not apply scaling to this value)
        /// </summary>
        int fontSize { get; set; }

        /// <summary>
        /// Sets and gets the scaling value for every entry in the menu (i.e. batch scaling).
        /// </summary>
        public Vector2 entryScaling
        {
            get { return _entryScaling; }
            set
            {
                _entryScaling = value;
                for (int i = 1; i < stringManager.strings.Count; i++)
                {
                    stringManager.strings[i].scale = _entryScaling;
                }
            }
        }
        private Vector2 _entryScaling;

        /// <summary>
        /// The vertical pixel space between each menu entry.
        /// </summary>
        int entryPadding { get; set; }

        /// <summary>
        /// The vertical pixel space between the menu and the first menu entry.
        /// </summary>
        int titlePadding { get; set; }

        /// <summary>
        /// Edits the scaling value of the menu title directly
        /// NOTE: Every menu must have a title as the first entry).
        /// </summary>
        public Vector2 titleScaling
        {
            get { return stringManager.strings[0].scale; }
            set { stringManager.strings[0].scale = value; }
        }

        /// <summary>
        /// Gets and sets the location of the menu.
        /// NOTE: The menu font is center justified.
        /// </summary>
        public Vector2 location
        {
            get
            {
                return _location;
            }
            set
            {
                _location = value;
                for (int i = 0; i < stringManager.strings.Count; i++)
                {
                    if (i == 0)
                    {
                        stringManager.strings[i].position = _location;
                    }
                    else
                    {
                        float newEntryPositionY = _location.Y + (fontSize * titleScaling.Y) + titlePadding +
                                                  ((i) * (fontSize * entryScaling.Y + entryPadding));
                        float newEntryPositionX = _location.X;
                        stringManager.strings[i].position = new Vector2(newEntryPositionX, newEntryPositionY);
                    }
                }
            }
        }
        private Vector2 _location;

        /// <summary>
        /// The string manager that holds all of the menu data.
        /// </summary>
        private GameStringManager stringManager { get; set; }

        /// <summary>
        /// Typical constructor.
        /// </summary>
        /// <param name="titleValue">The title for the new menu.</param>
        /// <param name="screenWidth">The width of the screen to draw the menu on.</param>
        /// <param name="screenHeight">The height of the screen to draw the menu on.</param>
        public Menu(string titleValue, int screenWidth, int screenHeight)
        {
            fontSize = 15;
            entryPadding = 5;
            titlePadding = 15;

            stringManager = new GameStringManager();
            location = new Vector2(screenWidth / 2f, screenHeight / 4f);
            GameString titleGameString = new GameString(location, Color.White, titleValue);
            stringManager.Add(titleGameString);
            titleScaling = new Vector2(1.0f, 1.0f);
            entryScaling = new Vector2(1.0f, 1.0f);
        }

        /// <summary>
        /// Draw the menu on the screen
        /// </summary>
        /// <param name="spriteBatch">The sprite batch to use.</param>
        /// <param name="spriteFont">The sprite font to use.</param>
        public void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            stringManager.DrawStrings(spriteBatch, spriteFont);
        }

        /// <summary>
        /// Add an entry to the menu, while keeping in consideration of the current scaling and
        /// height values.
        /// </summary>
        /// <param name="newEntryValue">The string value for the new menu entry.</param>
        public void AddEntry(string newEntryValue)
        {
            float newEntryPositionY = location.Y + (fontSize * titleScaling.Y) + titlePadding +
                                      ((stringManager.GetNumberOfStrings() - 1) * (fontSize * entryScaling.Y + entryPadding));
            float newEntryPositionX = location.X;
            GameString stringToAdd = new GameString(new Vector2(newEntryPositionX, newEntryPositionY), Color.White, newEntryValue);
            stringToAdd.scale = entryScaling;
            stringManager.Add(stringToAdd);
        }
    }
}
