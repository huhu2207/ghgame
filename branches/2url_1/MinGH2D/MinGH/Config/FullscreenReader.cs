using System.Xml;

namespace MinGH.Config
{
    /// <summary>
    /// Reads in the fullscreen selection from the configuration XML file.
    /// </summary>
    public class FullscreenReader
    {
        /// <summary>
        /// Read in the fullscreen selection from the configuration XML file.
        /// </summary>
        /// <param name="sourceXMLFile">The path to the configuration XML file.</param>
        /// <returns>Whether to use fullscreen or not.</returns>
        public static bool ReadInFullscreenSelectionFromXML(string sourceXMLFile)
        {
            XmlTextReader xmlReader = new XmlTextReader(sourceXMLFile);

            if (xmlReader.ReadToFollowing("fullscreen"))
            {
                return xmlReader.ReadElementContentAsBoolean();
            }
            else
            {
                return false;
            }
        }
    }
}
