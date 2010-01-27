using System;
using System.Xml;

namespace MinGH.Config
{
    /// <summary>
    /// Reads in the current themes settings from the config.xml file
    /// </summary>
    public class ThemeSettingReader
    {
        /// <summary>
        /// Scans through the config.xml file for the current theme settings.
        /// </summary>
        /// <param name="sourceXMLFile">The config file's path.</param>
        /// <returns>A fully filled out ThemeSetting</returns>
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
                    themeToReturn.noteSkinTexture = xmlReader.ReadElementContentAsString();

                    xmlReader.ReadToFollowing("fretboardTexture");
                    themeToReturn.fretboardTexture = xmlReader.ReadElementContentAsString();

                    xmlReader.ReadToFollowing("laneSeparatorTexture");
                    themeToReturn.laneSeparatorTexture = xmlReader.ReadElementContentAsString();

                    xmlReader.ReadToFollowing("fretboardBorders");
                    themeToReturn.laneSeparatorTexture = xmlReader.ReadElementContentAsString();

                    xmlReader.ReadToFollowing("hitMarkerTexture");
                    themeToReturn.hitMarkerTexture = xmlReader.ReadElementContentAsString();

                    xmlReader.ReadToFollowing("hitMarkerDepth");
                    themeToReturn.hitMarkerDepth = xmlReader.ReadElementContentAsInt();
                    xmlReader.ReadToFollowing("hitMarkerSize");
                    themeToReturn.hitMarkerSize = xmlReader.ReadElementContentAsInt();

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
