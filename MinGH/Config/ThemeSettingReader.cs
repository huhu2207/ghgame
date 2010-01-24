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

                    xmlReader.ReadToFollowing("fretboard");
                    themeToReturn.fretboardTexture = xmlReader.ReadElementContentAsString();

                    xmlReader.ReadToFollowing("laneSeparators");
                    themeToReturn.laneSeparatorTexture = xmlReader.ReadElementContentAsString();

                    xmlReader.ReadToFollowing("hitMarkerTexture");
                    themeToReturn.hitMarkerTexture = xmlReader.ReadElementContentAsString();

                    xmlReader.ReadToFollowing("hitMarkerDepth");
                    themeToReturn.hitMarkerDepth = xmlReader.ReadElementContentAsInt();
                    xmlReader.ReadToFollowing("hitMarkerSize");
                    themeToReturn.hitMarkerSize = xmlReader.ReadElementContentAsInt();

                    xmlReader.ReadToFollowing("fretboardBorders");
                    themeToReturn.laneSeparatorTexture = xmlReader.ReadElementContentAsString();

                    xmlReader.ReadToFollowing("backgroundDirectory");
                    themeToReturn.backgroundDirectory = xmlReader.ReadElementContentAsString();

                    xmlReader.ReadToFollowing("laneSize");
                    xmlReader.ReadToFollowing("guitarSingle");
                    themeToReturn.laneSizeGuitar = xmlReader.ReadElementContentAsInt();
                    xmlReader.ReadToFollowing("drumSingle");
                    themeToReturn.laneSizeDrums = xmlReader.ReadElementContentAsInt();

                    xmlReader.ReadToFollowing("laneSeparatorSize");
                    themeToReturn.laneSeparatorSize = xmlReader.ReadElementContentAsInt();

                    xmlReader.ReadToFollowing("fretboardBorderSize");
                    themeToReturn.fretboardBorderSize = xmlReader.ReadElementContentAsInt();
                }
            }

            return themeToReturn;
        }
    }
}
