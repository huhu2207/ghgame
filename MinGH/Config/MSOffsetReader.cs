using System.Xml;

namespace MinGH.Config
{
    /// <summary>
    /// Reads in the fullscreen selection from the configuration XML file.
    /// </summary>
    public class MSOffsetReader
    {
        /// <summary>
        /// Read in the MSOffset selection from the configuration XML file.
        /// </summary>
        /// <param name="sourceXMLFile">The path to the configuration XML file.</param>
        /// <returns>The offset that the game will use.</returns>
        public static int ReadInMSOffsetSelectionFromXML(string sourceXMLFile)
        {
            XmlTextReader xmlReader = new XmlTextReader(sourceXMLFile);

            if (xmlReader.ReadToFollowing("MSOffset"))
            {
                return xmlReader.ReadElementContentAsInt();
            }
            else
            {
                return 0;
            }
        }
    }
}