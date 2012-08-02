using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.GameStringImpl
{
    /// <summary>
    /// Class designed to contain the creating, drawing and logic of a very simple menu system.
    /// NOTE: The 0th entry in the menu will always be the title which is unselectable, therefore
    /// the currentlySelectedEntry will never be less than 1.
    /// </summary>
    public class Menu
    {
        /// <summary>
        /// Whether the menu should be drawn or not.
        /// </summary>
        public bool visible { get; set; }

        /// <summary>
        /// Specifies the currently selected option
        /// </summary>
        public int currentlySelectedEntry { get; set; }

        /// <summary>
        /// The vertical pixel size of the font being used (do not apply scaling to this value)
        /// </summary>
        public int fontSize { get; set; }

        /// <summary>
        /// Allows the menu to scroll down if the currently selected option passes a certian
        /// point on the screen.
        /// </summary>
        public bool scrollable { get; set; }

        /// <summary>
        /// The top limit where the currently selected option can be before the menu will
        /// scroll upwards.  Is expected to be a percent (e.g. 0.20 for 20%)
        /// </summary>
        public double topScrollThreshold { get; set; }

        /// <summary>
        /// The bottom limit where the currently selected option can be before the menu will
        /// scroll downwards.  Is expected to be a percent (e.g. 0.80 for 80%)
        /// </summary>
        public double bottomScrollThreshold { get; set; }

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
        public int entryPadding { get; set; }

        /// <summary>
        /// The vertical pixel space between the menu and the first menu entry.
        /// </summary>
        public int titlePadding { get; set; }

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
                                                  ((i - 1) * (fontSize * entryScaling.Y + entryPadding));
                        float newEntryPositionX = _location.X;
                        stringManager.strings[i].position = new Vector2(newEntryPositionX, newEntryPositionY);
                    }
                }
            }
        }
        private Vector2 _location;

        /// <summary>
        /// The original position of the menu.
        /// </summary>
        public Vector2 defaultPosition { get; set; }

        /// <summary>
        /// The color of non-selected entries.
        /// </summary>
        public Color defaultEntryColor { get; set; }

        /// <summary>
        /// The color of the selected entry.
        /// </summary>
        public Color selectedEntryColor { get; set; }

        /// <summary>
        /// The string manager that holds all of the menu data.
        /// </summary>
        private GameStringManager stringManager { get; set; }

        /// <summary>
        /// Initializes a new menu in the middle
        /// </summary>
        /// <param name="titleValue">The title for the new menu.</param>
        /// <param name="initialLocation">The starting location of the menu.</param>
        public Menu(string titleValue, Vector2 initialLocation)
        {
            fontSize = 15;
            entryPadding = 5;
            titlePadding = 15;
            currentlySelectedEntry = 0;
            defaultEntryColor = Color.White;
            selectedEntryColor = Color.Yellow;

            stringManager = new GameStringManager();
            location = initialLocation;
            defaultPosition = initialLocation;
            GameString titleGameString = new GameString(location, defaultEntryColor, titleValue);
            stringManager.Add(titleGameString);
            titleScaling = new Vector2(1.0f, 1.0f);
            entryScaling = new Vector2(1.0f, 1.0f);
            topScrollThreshold = 0.2;
            bottomScrollThreshold = 0.8;
            visible = true;
        }

        /// <summary>
        /// Draw the menu on the screen
        /// </summary>
        /// <param name="spriteBatch">The sprite batch to use.</param>
        /// <param name="spriteFont">The sprite font to use.</param>
        public void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            if (visible)
            {
                stringManager.DrawStrings(spriteBatch, spriteFont);
            }
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

            SelectNthEntry(1, -1);  // If an entry is added, there is atleast one option to highlight
        }

        /// <summary>
        /// Selects the entry below the currently selected entry, or the first entry
        /// if the currently selected entry has nothing below it.
        /// </summary>
        public void SelectNextEntry(int windowHeight)
        {
            if (stringManager.strings.Count > 0)
            {
                if (currentlySelectedEntry == stringManager.strings.Count - 1)
                {
                    SelectNthEntry(1, windowHeight);
                }
                else
                {
                    SelectNthEntry(currentlySelectedEntry + 1, windowHeight);
                }
            }
        }

        /// <summary>
        /// Selects the entry above the currently selected entry, or the last entry
        /// if the currently selected entry has nothing above it.
        /// </summary>
        public void SelectPreviousEntry(int windowHeight)
        {
            if (stringManager.strings.Count > 0)
            {
                if (currentlySelectedEntry == 1)
                {
                    SelectNthEntry(stringManager.strings.Count - 1, windowHeight);
                }
                else
                {
                    SelectNthEntry(currentlySelectedEntry - 1, windowHeight);
                }
            }
        }

        /// <summary>
        /// Selects the nth entry in a menu.
        /// </summary>
        /// <param name="n">The entry number to select.</param>
        private void SelectNthEntry(int n, int windowHeight)
        {
            if ((stringManager.strings.Count > 1) && (n < stringManager.strings.Count))
            {
                if (currentlySelectedEntry > 0)
                {
                    stringManager.strings[currentlySelectedEntry].color = defaultEntryColor;
                }
                stringManager.strings[n].color = selectedEntryColor;
                currentlySelectedEntry = n;

                if (windowHeight > 0 && scrollable)
                {
                    if ((stringManager.strings[currentlySelectedEntry].position.Y < windowHeight * topScrollThreshold))
                    {
                        float newYValue = (int)(windowHeight * topScrollThreshold) - ((currentlySelectedEntry + 1) * (fontSize * entryScaling.Y + entryPadding));
                        location = new Vector2(location.X, newYValue);
                    }
                    else if (stringManager.strings[currentlySelectedEntry].position.Y > windowHeight * bottomScrollThreshold)
                    {
                        float newYValue = (int)(windowHeight * bottomScrollThreshold) - ((currentlySelectedEntry + 1) * (fontSize * entryScaling.Y + entryPadding));
                        location = new Vector2(location.X, newYValue);
                    }
                }
            }
        }
    }
}
