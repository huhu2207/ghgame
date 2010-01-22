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
        public int distanceUntilLeftMostLaneGuitarSingle { get; set; }
        public int distanceUntilLeftMostLaneDrumSingle { get; set; }
        public int laneBorderSize { get; set; }
        public int laneSize { get; set; }

        public ThemeSetting()
        {
            noteSkinFile = ".\\Content\\Sprites\\DefaultSprites.png";
            backgroundDirectory = ".\\Content\\Backgrounds\\Default";
            distanceUntilLeftMostLaneGuitarSingle = 194;
            distanceUntilLeftMostLaneDrumSingle = 225;
            laneBorderSize = 3;
            laneSize = 83;
        }
    }
}
