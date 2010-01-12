using System;
using System.Xml;

namespace MinGH.Config
{
    /// <summary>
    /// Reads an XML file for the currently selected speed mod.
    /// The XML file have a special format simiar to the provided file.
    /// </summary>
    public class SpeedModReader
    {
        /// <summary>
        /// Read in the currently specified speed mod as per the configuration XML file.
        /// </summary>
        /// <param name="sourceXMLFile">Path to the configuration XML file.</param>
        /// <returns>A constructed SpeedModValue.</returns>
        public static SpeedModValue ReadInCurrentSpeedModFromXML(string sourceXMLFile)
        {
            bool speedModFound = false;
            double NVMultiplier = 0.0;
            int selectedSpeedMod = 0;
            int MSOffset = 0;
            XmlTextReader xmlReader = new XmlTextReader(sourceXMLFile);

            if (xmlReader.ReadToFollowing("selectedSpeedMod"))
            {
                selectedSpeedMod = xmlReader.ReadElementContentAsInt();
            }

            while (xmlReader.ReadToFollowing("speedMod") && !speedModFound)
            {
                if (selectedSpeedMod == Convert.ToInt32(xmlReader.GetAttribute("id")))
                {
                    xmlReader.ReadToFollowing("MSOffset");
                    MSOffset = xmlReader.ReadElementContentAsInt();

                    xmlReader.ReadToFollowing("NVMultiplier");
                    NVMultiplier = xmlReader.ReadElementContentAsDouble();

                    speedModFound = true;
                }
            }

            if (speedModFound)
            {
                return new SpeedModValue(MSOffset, NVMultiplier);
            }
            else
            {
                return new SpeedModValue();
            }
        }
    }
}
