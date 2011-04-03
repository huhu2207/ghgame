namespace MinGH.Config
{
    /// <summary>
    /// Stores any information read in from the config.xml file (i.e. user settings).
    /// </summary>
    public class GameConfiguration
    {
        /// <summary>
        /// Have the game play for you.
        /// </summary>
        public bool autoplay { get; set; }

        /// <summary>
        /// The currently used speed mod (i.e. how fast the notes come down and how
        /// spread apart they are).
        /// </summary>
        public int MSTillHit { get; set; }

        /// <summary>
        /// The current theme settings.
        /// </summary>
        public ThemeSetting themeSetting { get; set; }

        /// <summary>
        /// The folder that contains all songs to be used in the game.
        /// </summary>
        public string songDirectory { get; set; }

        /// <summary>
        /// Whether to display in fullscreen.
        /// </summary>
        public bool fullscreen { get; set; }

        /// <summary>
        /// Whether to user use drum input in guitar mode
        /// NOTE: See config.xml for more details
        /// </summary>
        public bool useDrumStyleInputForGuitarMode { get; set; }

        /// <summary>
        /// Fine tunes by the milisecond when the notes are actually hit
        /// </summary>
        public int MSOffset { get; set; }

        /// <summary>
        /// Constructs a GameConfiguration using a path to a XML file.
        /// </summary>
        /// <param name="sourceXMLFile">Path to the configuration XML file.</param>
        public GameConfiguration(string sourceXMLFile)
        {
            MSTillHit = ConfigFileReader.ReadInMSTillHitFromXML(sourceXMLFile);
            songDirectory = ConfigFileReader.ReadInCurrentSongDirectoryFromXML(sourceXMLFile);
            fullscreen = ConfigFileReader.ReadInFullscreenSelectionFromXML(sourceXMLFile);
            useDrumStyleInputForGuitarMode = ConfigFileReader.ReadInInputStyleFromXML(sourceXMLFile);
            themeSetting = ConfigFileReader.ReadInCurrentThemeFromXML(sourceXMLFile);
            autoplay = ConfigFileReader.ReadInAutoplaySelectionFromXML(sourceXMLFile);
            MSOffset = ConfigFileReader.ReadInMSOffsetSelectionFromXML(sourceXMLFile);
        }
    }
}
