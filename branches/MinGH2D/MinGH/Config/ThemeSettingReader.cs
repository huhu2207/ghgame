using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace MinGH.Config
{
    public class ThemeSettingReader
    {
        public static ThemeSetting ReadInCurrentThemeFromXML(string sourceXMLFile)
        {
            ThemeSetting themeToReturn = new ThemeSetting();
            int selectedTheme = 0;
            XmlTextReader xmlReader = new XmlTextReader(sourceXMLFile);

            if (xmlReader.ReadToFollowing("selectedTheme"))
            {
                selectedTheme = xmlReader.ReadElementContentAsInt();
            }

            while (xmlReader.ReadToFollowing("theme"))
            {
                if (selectedTheme == Convert.ToInt32(xmlReader.GetAttribute("id")))
                {
                    xmlReader.ReadToFollowing("noteSkinFile");
                    themeToReturn.noteSkinFile = xmlReader.ReadElementContentAsString();

                    xmlReader.ReadToFollowing("backgroundDirectory");
                    themeToReturn.backgroundDirectory = xmlReader.ReadElementContentAsString();

                    xmlReader.ReadToFollowing("distanceUntilLeftMostLane");
                    xmlReader.ReadToFollowing("guitarSingle");
                    themeToReturn.distanceUntilLeftMostLaneGuitarSingle = xmlReader.ReadElementContentAsInt();
                    xmlReader.ReadToFollowing("drumSingle");
                    themeToReturn.distanceUntilLeftMostLaneDrumSingle = xmlReader.ReadElementContentAsInt();

                    xmlReader.ReadToFollowing("laneBorderSize");
                    themeToReturn.laneBorderSize = xmlReader.ReadElementContentAsInt();

                    xmlReader.ReadToFollowing("laneSize");
                    themeToReturn.laneSize = xmlReader.ReadElementContentAsInt();
                }
            }

            return themeToReturn;
        }
    }
}
