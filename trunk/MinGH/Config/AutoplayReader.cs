using System.Xml;

namespace MinGH.Config
{
    /// <summary>
    /// Reads in the fullscreen selection from the configuration XML file.
    /// </summary>
    public class AutoplayReader
    {
        /// <summary>
        /// Read in the autoplay selection from the configuration XML file.
        /// </summary>
        /// <param name="sourceXMLFile">The path to the configuration XML file.</param>
        /// <returns>Whether to use autoplay or not.</returns>
        public static bool ReadInAutoplaySelectionFromXML(string sourceXMLFile)
        {
            XmlTextReader xmlReader = new XmlTextReader(sourceXMLFile);

            if (xmlReader.ReadToFollowing("autoplay"))
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
