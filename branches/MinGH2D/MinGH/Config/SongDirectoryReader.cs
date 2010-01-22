using System.Xml;

namespace MinGH.Config
{
    /// <summary>
    /// Reads the configuration XML file for the song directory to use.
    /// </summary>
    public class SongDirectoryReader
    {
        /// <summary>
        /// Read in the songDirectory element from the configuration XML file.
        /// </summary>
        /// <param name="sourceXMLFile">The path to the configuration XML file.</param>
        /// <returns>The song directory to use.</returns>
        public static string ReadInCurrentSongDirectoryFromXML(string sourceXMLFile)
        {
            XmlTextReader xmlReader = new XmlTextReader(sourceXMLFile);

            if (xmlReader.ReadToFollowing("songDirectory"))
            {
                return xmlReader.ReadElementContentAsString();
            }
            else
            {
                return "./Songs";
            }
        }
    }
}
