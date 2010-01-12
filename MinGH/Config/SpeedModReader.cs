using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace MinGH.Config
{
    public class SpeedModReader
    {
        public static HyperSpeedValue ReadInSpeedModsFromXML(string sourceXMLFile)
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
                return new HyperSpeedValue(MSOffset, NVMultiplier);
            }
            else
            {
                return new HyperSpeedValue();
            }
        }
    }
}
