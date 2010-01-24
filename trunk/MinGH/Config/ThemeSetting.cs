using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MinGH.Config
{
    public class ThemeSetting
    {
        public string noteSkinFile { get; set; }
        public string backgroundDirectory { get; set; }
        public string fretboardTexture { get; set; }
        public string laneSeparatorTexture { get; set; }
        public string fretboardBorderTexture { get; set; }
        public string hitMarkerTexture { get; set; }
        public int laneSeparatorSize { get; set; }
        public int laneSizeGuitar { get; set; }
        public int laneSizeDrums { get; set; }
        public int fretboardBorderSize { get; set; }
        public int hitMarkerDepth { get; set; }
        public int hitMarkerSize { get; set; }

        public ThemeSetting()
        {
            noteSkinFile = ".\\Content\\Sprites\\DefaultSprites.png";
            backgroundDirectory = ".\\Content\\Backgrounds\\Default";
            fretboardTexture = ".\\Content\\FretboardDefault.png";
            laneSeparatorTexture = ".\\Content\\LaneSeparatorDefault.png";
            fretboardBorderTexture = ".\\Content\\LaneSeparatorDefault.png";
            hitMarkerTexture = ".\\Content\\Fretboards\\HitmarkerDefault.png";
            laneSeparatorSize = 3;
            laneSizeGuitar = 35;
            laneSizeDrums = 55;
            fretboardBorderSize = 6;
            hitMarkerDepth = 300;
            hitMarkerSize = 6;
        }
    }
}
