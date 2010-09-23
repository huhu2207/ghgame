using System;
using System.Xml;

namespace MinGH.Config
{
    /// <summary>
    /// Reads an XML file for the currently selected speed mod.
    /// The XML file have a special format simiar to the provided file.
    /// </summary>
    public class MSTillHitReader
    {
        /// <summary>
        /// Read in the currently specified speed mod as per the configuration XML file.
        /// </summary>
        /// <param name="sourceXMLFile">Path to the configuration XML file.</param>
        /// <returns>Number of milisezonds a note will take to travel the fretboard.</returns>
        public static int ReadInMSTillHitFromXML(string sourceXMLFile)
        {
            bool speedModFound = false;
            int selectedSpeedMod = 0;
            int MSTillHit = 0;
            XmlTextReader xmlReader = new XmlTextReader(sourceXMLFile);

            if (xmlReader.ReadToFollowing("selectedSpeedMod"))
            {
                selectedSpeedMod = xmlReader.ReadElementContentAsInt();
            }

            while (xmlReader.ReadToFollowing("speedMod") && !speedModFound)
            {
                if (selectedSpeedMod == Convert.ToInt32(xmlReader.GetAttribute("id")))
                {
                    xmlReader.ReadToFollowing("MSTillHit");
                    MSTillHit = xmlReader.ReadElementContentAsInt();

                    speedModFound = true;
                }
            }

            if (speedModFound)
            {
                return MSTillHit;
            }
            else
            {
                return 2000;
            }
        }
    }
}
