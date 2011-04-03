using System;
using System.Xml;

namespace MinGH.Config
{
    class ConfigFileReader
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

                    xmlReader.ReadToFollowing("fretbooardDepth");
                    themeToReturn.fretboardDepth = xmlReader.ReadElementContentAsInt();

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

        /// <summary>
        /// Read in the songDirectory element from the configuration XML file.
        /// </summary>
        /// <param name="sourceXMLFile">The path to the configuration XML file.</param>
        /// <returns>The song directory to use.</returns>
        public static string ReadInCurrentSongDirectoryFromXML(string sourceXMLFile)
        {
            XmlTextReader xmlReader = new XmlTextReader(sourceXMLFile);

            if (xmlReader.ReadToFollowing("songDirectory"))
            {
                return xmlReader.ReadElementContentAsString();
            }
            else
            {
                return "./Songs";
            }
        }

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
