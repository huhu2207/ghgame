namespace MinGH.Config
{
    /// <summary>
    /// Stores any information read in from the config.xml file (i.e. user settings).
    /// </summary>
    public class GameConfiguration
    {
        /// <summary>
        /// The currently used speed mod (i.e. how fast the notes come down and how
        /// spread apart they are).
        /// </summary>
        public SpeedModValue speedModValue { get; set; }

        /// <summary>
        /// The folder that contains all songs to be used in the game.
        /// </summary>
        public string songDirectory { get; set; }

        /// <summary>
        /// Constructs a GameConfiguration using a path to a XML file.
        /// </summary>
        /// <param name="sourceXMLFile">Path to the configuration XML file.</param>
        public GameConfiguration(string sourceXMLFile)
        {
            speedModValue = SpeedModReader.ReadInCurrentSpeedModFromXML(sourceXMLFile);
            songDirectory = SongDirectoryReader.ReadInCurrentSongDirectoryFromXML(sourceXMLFile);
        }
    }
}
