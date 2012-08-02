using System.Xml;

namespace MinGH.Config
{
    /// <summary>
    /// Reads in the useDrumStyleInputForGuitarMode selection from the configuration XML file.
    /// </summary>
    public class DrumInputForGuitarReader
    {
        /// <summary>
        /// Read in the useDrumStyleInputForGuitarMode selection from the configuration XML file.
        /// </summary>
        /// <param name="sourceXMLFile">The path to the configuration XML file.</param>
        /// <returns>Whether to use useDrumStyleInputForGuitarMode or not.</returns>
        public static bool ReadInInputStyleFromXML(string sourceXMLFile)
        {
            XmlTextReader xmlReader = new XmlTextReader(sourceXMLFile);

            if (xmlReader.ReadToFollowing("useDrumStyleInputForGuitarMode"))
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
